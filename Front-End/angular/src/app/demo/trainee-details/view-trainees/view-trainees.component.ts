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
import { BaseHttpClientServiceService } from '../../../../Services/Shared/base-http-client-service.service';
import { SharedModule } from '../../../theme/shared/shared.module';
import { CommonModule, DatePipe } from '@angular/common';
import { RequestParameter } from '../../../../Models/request-parameter';
import { TraineeService } from 'src/Services/trainee.service';
import { Trainee } from 'src/Models/trainee.Model';

@Component({
  selector: 'app-view-trainees',
  standalone: true,
  imports: [CommonModule, SharedModule, MessagesModule, HttpClientModule, ToastModule],
  providers: [ConfirmationService, TraineeService, BaseHttpClientServiceService, MessageService, FileProcessingService, DatePipe],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  templateUrl: './view-trainees.component.html',
  styleUrls: ['./view-trainees.component.css']
})
export class ViewTraineesComponent implements OnInit {
  listOfemployees: any[] = [];
  isDownloading:boolean=false;
  constructor(
    private traineeService: TraineeService,
    private messageService: MessageService,
    private fileProcessingService: FileProcessingService,
    public router: Router,
    private datePipe: DatePipe
  ) { }

  ngOnInit() {
    this.getTraineessFromServer();
  }

  navigateToUrl = (url: string | undefined) => this.router.navigateByUrl(url!);


  options: GridColumnOptions[] = [
    { field: 'employeeId', header: 'Employee Id', isSortable: true, hasFilter: true, hasTableValue: true ,width:'30%'},
    { field: 'name', header: 'Name', isSortable: true, hasFilter: true, hasTableValue: true ,width:'30%'},
    { field: 'certificateName', header: 'certificateName', isSortable: true, hasFilter: true, hasTableValue: true,width:'30%' }

  

  ]

  //#region  API Server calls
  getTraineessFromServer() {
    const reqParam = new RequestParameter();
    reqParam.count = 5000;
    this.traineeService.getAllPagedResponse(reqParam).subscribe({
      next: (response: BaseResponse<Trainee[]>) => {
        if (response.succeeded) {
          this.listOfemployees = response.data;
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

  deleteTraineeFromServer(id:string) {
    this.traineeService.deleteTraineeById(id).subscribe({
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

  downloadEmployeeCertificate(id:string) {
    this.traineeService.getTraineeById(id).subscribe({
      next: (response: BaseResponse<Trainee>) => {
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


 onDelete(event:Trainee){
    this.deleteTraineeFromServer(event.id.toString());
  }

onDownload(event:Trainee){
  this.isDownloading=true;
  this.downloadEmployeeCertificate(event.id.toString());
}
onPreview(event: Trainee){
  this.navigateToUrl(`Employees/Update/${event.id}`)
}

}

