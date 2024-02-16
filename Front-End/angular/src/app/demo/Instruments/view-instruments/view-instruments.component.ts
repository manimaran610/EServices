import { GridColumnOptions } from '../../../../Models/grid-column-options';
import { FileProcessingService } from './../../../../Services/Shared/file-processing.service';
/* eslint-disable @angular-eslint/no-empty-lifecycle-method */
import { CUSTOM_ELEMENTS_SCHEMA, Component, OnInit } from '@angular/core';
import { ConfirmationService, MessageService } from 'primeng/api';
import { MessagesModule } from 'primeng/messages';
import { HttpClientModule } from '@angular/common/http';
import { ToastModule } from 'primeng/toast';
import { Router } from '@angular/router';
import { BaseResponse } from '../../../../Models/response-models/base-response';
import { InstrumentService } from '../../../../Services/Instrument.service';
import { BaseHttpClientServiceService } from '../../../../Services/Shared/base-http-client-service.service';
import { Instrument } from '../../../../Models/instrument.Model';
import { SharedModule } from '../../../theme/shared/shared.module';
import { CommonModule, DatePipe } from '@angular/common';
import { RequestParameter } from '../../../../Models/request-parameter';


@Component({
  selector: 'app-view-instruments',
  standalone: true,
  imports: [CommonModule, SharedModule, MessagesModule, HttpClientModule, ToastModule],
  providers: [ConfirmationService, InstrumentService, BaseHttpClientServiceService, MessageService, FileProcessingService, DatePipe],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  templateUrl: './view-instruments.component.html',
  styleUrls: ['./view-instruments.component.css']
})
export class ViewInstrumentsComponent implements OnInit {

  listOfInstrumentDetails: any[] = [];
  isDownloading:boolean=false;
  constructor(
    private instrumentService: InstrumentService,
    private messageService: MessageService,
    private fileProcessingService: FileProcessingService,
    public router: Router,
    private datePipe: DatePipe
  ) { }

  ngOnInit() {
    this.getInstrumentsFromServer();
  }

  navigateToUrl = (url: string | undefined) => this.router.navigateByUrl(url!);


  options: GridColumnOptions[] = [
    { field: 'serialNo', header: 'Serial No', isSortable: true, hasFilter: true, hasTableValue: true },
    { field: 'make', header: 'Make', isSortable: true, hasFilter: true, hasTableValue: true },
    { field: 'model', header: 'Model', isSortable: true, hasFilter: true, hasTableValue: true },
    { field: 'type', header: 'Type', isSortable: true, hasFilter: true, hasTableValue: true },
    { field: 'calibratedOnStr', header: 'calibratedOn', isSortable: true, hasFilter: true, hasTableValue: true },
    { field: 'calibratedDueOnStr', header: 'calibrated Due', isSortable: true, hasFilter: true, hasTableValue: true },
    { field: 'certificateName', header: 'certificateName', isSortable: true, hasFilter: true, hasTableValue: true }

  ]

  //#region  API Server calls
  getInstrumentsFromServer() {
    const reqParam = new RequestParameter();
    reqParam.count = 5000;
    this.instrumentService.getAllPagedResponse(reqParam).subscribe({
      next: (response: BaseResponse<Instrument[]>) => {
        if (response.succeeded) {
          this.listOfInstrumentDetails = response.data;
          this.listOfInstrumentDetails.forEach(e => {
            e.calibratedOnStr = this.datePipe.transform(e.calibratedOn, 'dd-MM-yyyy');
            e.calibratedDueOnStr = this.datePipe.transform(e.calibratedDueOn, 'dd-MM-yyyy');

          });
        }
      },
      error: (e) => {
        this.messageService.add({
          key: 'tc', severity: 'error', summary: 'Failed',
          detail: e.status == 0 ? 'Server connection error' : e.error.Message !== undefined ? e.error.Message : e.error.title, life: 4000
        });
      },
      complete: () => { }
    });
  }

  deleteInstrumentFromServer(id:string) {
    this.instrumentService.deleteInstrumentById(id).subscribe({
      next: (response: BaseResponse<number>) => {
        if (response.succeeded) {
          this.messageService.add({ key: 'tc', severity: 'success', summary: 'Deleted', detail: "Room deleted", life: 4000 });
        }
      },
      error: (e) => {
        this.messageService.add({
          key: 'tc', severity: 'error', summary: 'Failed',
          detail: e.status ==0? 'Server connection error': e.error.Message !== undefined ? e.error.Message : e.error.title, life: 4000
        });
      },
      complete: () => { },
    });
  }

  downloadInstrumentCertificate(id:string) {
    this.instrumentService.getInstrumentById(id).subscribe({
      next: (response: BaseResponse<Instrument>) => {
        if (response.succeeded) {
          const file = this.fileProcessingService.base64ToFile(response.data.certificateFile,response.data.certificateName!,'application/pdf')
          this.fileProcessingService.downloadFile(file);
        }
      },
      error: (e) => {
        this.messageService.add({
          key: 'tc', severity: 'error', summary: 'Failed',
          detail: e.status ==0? 'Server connection error': e.error.Message !== undefined ? e.error.Message : e.error.title, life: 4000
        });
        this.isDownloading=false;
      },
      complete: () => {   this.isDownloading=false;
      },
    });
  }


 onDelete(event:Instrument){
    this.deleteInstrumentFromServer(event.id.toString());
  }

onDownload(event:Instrument){
  this.isDownloading=true;
  this.downloadInstrumentCertificate(event.id.toString());
}
onPreview(event: Instrument){
  this.navigateToUrl(`Instruments/Update/${event.id}`)
}

}
