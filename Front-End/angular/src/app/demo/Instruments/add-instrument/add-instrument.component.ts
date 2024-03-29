import { data } from './../../../fack-db/series-data';
import { FileProcessingService } from './../../../../Services/Shared/file-processing.service';
/* eslint-disable @angular-eslint/no-empty-lifecycle-method */
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { CommonModule } from '@angular/common';
import { CUSTOM_ELEMENTS_SCHEMA, Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ConfirmationService, Message, MessageService } from 'primeng/api';
import { MessagesModule } from 'primeng/messages';
import { SharedModule } from 'src/app/theme/shared/shared.module';
import { HttpClientModule } from '@angular/common/http';
import { ToastModule } from 'primeng/toast';
import { BaseResponse } from 'src/Models/response-models/base-response';
import {  ActivatedRoute, Router } from '@angular/router';
import { BaseHttpClientServiceService } from 'src/Services/Shared/base-http-client-service.service';
import { BusinessConstants } from '../../reports/shared/Constants/business-constants';
import { InstrumentService } from '../../../../Services/Instrument.service';
import { Instrument } from '../../../../Models/instrument.Model';


@Component({
    selector: 'app-add-instrument',
    standalone: true,
    imports: [CommonModule, SharedModule, ConfirmDialogModule, MessagesModule, HttpClientModule, ToastModule],
    providers: [ConfirmationService, InstrumentService, BaseHttpClientServiceService, MessageService,FileProcessingService],
    schemas: [CUSTOM_ELEMENTS_SCHEMA],
    templateUrl: './add-instrument.component.html',
    styleUrls: ['./add-instrument.component.css']
})
export class AddInstrumentComponent implements OnInit {
    @ViewChild('fileInput') fileInput?: ElementRef<HTMLInputElement>;
    instrumentFormGroup: FormGroup;
    msgs: Message[] = [];
    instrumentTypes :string[]=BusinessConstants.InstrumentTypes;
    isAddInstrument: boolean = true;
    isSaveLoading:boolean=false;
    isSaved: boolean = false;
    instrumentId:number = 0;

    tempFileName?: string;
    tempFile?: string;
    instrumentModel: Instrument = new Instrument();

    constructor(
        private confirmationService: ConfirmationService,
        private instrumentService: InstrumentService,
        private messageService: MessageService,
        private fileProcessingService:FileProcessingService,
        public router: Router,
        private route: ActivatedRoute
    ) {

        this.instrumentFormGroup = new FormGroup(
            {
                serialNo: new FormControl(),
                make: new FormControl(),
                model: new FormControl(),
                type: new FormControl('0'),
                calibratedOn: new FormControl(),
                calibratedDueOn: new FormControl(),
                certificate: new FormControl(),
            }
        );
        this.addFormControlValidators();
    }

    ngOnInit() {
        this.route.params.subscribe(params => {
            if (params['id'] !== undefined && params['id'] > 0) {
              this.instrumentId = params['id'];
              if (this.instrumentId > 0) {
                this.getInstrumentById(this.instrumentId.toString());
              }
            }
          });
     }
    navigateToUrl = (url: string | undefined) => this.router.navigateByUrl(url!);


    async onSubmit() {
        if (this.instrumentFormGroup.valid) {
            console.log("Form submitted")
                this.instrumentModel = this.instrumentFormGroup.value;
                this.instrumentModel.certificateName = this.tempFileName;
                this.instrumentModel.certificateFile = this.tempFile;
                this.confirmDialog();
        }else {
            this.messageService.add({ key: 'tc', severity: 'warn', summary: 'Warning', detail: "Instrument details Invalid", life: 4000 });
        }
    }

    onClear() { this.instrumentFormGroup.reset() }

    confirmDialog() {
        this.confirmationService.confirm({
            message: 'Are you sure that you want to save instrument?',
            header: 'Confirmation',
            icon: 'pi pi-exclamation-triangle',
            acceptButtonStyleClass: 'btn btn-success m-2',
            rejectButtonStyleClass: 'btn btn-danger',
            accept: () => {
              this.instrumentId > 0 ? this.UpdateInstrument(): this.postInstrumentToAPIServer() 
            },
            reject: () => { }
        });
    }

    postInstrumentToAPIServer() {
        this.isSaveLoading =true;
        this.instrumentService.postInstrument(this.instrumentModel).subscribe({
            next: (response: BaseResponse<number>) => {
                if (response.succeeded) this.messageService.add({ key: 'tc', severity: 'success', summary: 'Success', detail: 'Instrument details saved', life: 4000 }); 
                this.isSaved=true;      
            },
            error: (e) => {
                console.log(e)
                this.messageService.add({ key: 'tc', severity: 'error', summary: 'Failed',
                detail: e.status ==0? 'Server connection error': e.error.Message !== undefined ? e.error.Message : e.error.title, life: 4000 });
                this.isSaveLoading =false;

            },
            complete: () => {
                this.isSaveLoading =false;
                // this.router.navigateByUrl("/Home-page")
            },
        });
    }

    UpdateInstrument() {
        this.isSaveLoading =true;
        this.instrumentModel.id=this.instrumentId;
        this.instrumentService.updateInstrument(this.instrumentId.toString(),this.instrumentModel).subscribe({
            next: (response: BaseResponse<number>) => {
                if (response.succeeded) this.messageService.add({ key: 'tc', severity: 'success', summary: 'Success', detail: 'Instrument details updated', life: 4000 }); 
                this.isSaved=true;      
            },
            error: (e) => {
                console.log(e)
                this.messageService.add({ key: 'tc', severity: 'error', summary: 'Failed',
                detail: e.status ==0? 'Server connection error': e.error.Message !== undefined ? e.error.Message : e.error.title, life: 4000 });
                this.isSaveLoading =false;

            },
            complete: () => {
                this.isSaveLoading =false;
                // this.router.navigateByUrl("/Home-page")
            },
        });
    }
    
    getInstrumentById(id:string) {
        this.instrumentService.getInstrumentById(id).subscribe({
          next: (response: BaseResponse<Instrument>) => {
            if (response.succeeded) {
                  this.instrumentModel=response.data;  
                  this.mapInstrumentDetailToForm();   
            }
          },
          error: (e) => {
            this.messageService.add({
              key: 'tc', severity: 'error', summary: 'Failed',
              detail: e.status ==0? 'Server connection error': e.error.Message !== undefined ? e.error.Message : e.error.title, life: 4000
            });
          },
          complete: () => {  
          },
        });
      }
    async uploadFile(event: any) {

        if (event.files.length > 0 && event.files[0].size < 5120000) {
            this.tempFileName = event.files[0].name;
          if(event.files[0].name.split('?')[0].split('.').pop() !== 'pdf') {
            this.instrumentFormGroup.controls['certificate'].setErrors({min:true});
          //  this.messageService.add({ key: 'tc', severity: 'warn', summary: 'Warning', detail: "Please choose PDF Files", life: 4000 });
          }
            this.tempFile = await this.fileProcessingService.fileToBase64(event.files[0]);
           
        }
        else if (event.files.length > 0 && event.files[0].size > 5120000) {
            this.instrumentFormGroup.controls['certificate'].setErrors({max:true} )
        }
        else {
            this.tempFile = undefined;
            this.tempFileName = undefined;
        }
    }

    addFormControlValidators() {
        this.instrumentFormGroup.controls['serialNo'].addValidators([Validators.required])
        this.instrumentFormGroup.controls['make'].addValidators([Validators.required])
        this.instrumentFormGroup.controls['model'].addValidators([Validators.required])
        this.instrumentFormGroup.controls['type'].addValidators([Validators.required])
        this.instrumentFormGroup.controls['calibratedOn'].addValidators([Validators.required])
        this.instrumentFormGroup.controls['calibratedDueOn'].addValidators([Validators.required])
        this.instrumentFormGroup.controls['certificate'].addValidators([Validators.required])

    }
    

    mapInstrumentDetailToForm() {
        this.instrumentFormGroup.controls['serialNo'].patchValue(this.instrumentModel.serialNumber);
        this.instrumentFormGroup.controls['make'].patchValue(this.instrumentModel.make);
        this.instrumentFormGroup.controls['model'].patchValue(this.instrumentModel.model);
        this.instrumentFormGroup.controls['type'].patchValue(this.instrumentModel.type);
        this.instrumentFormGroup.controls['calibratedOn'].patchValue(this.formatDate(this.instrumentModel.calibratedOn!));
        this.instrumentFormGroup.controls['calibratedDueOn'].patchValue(this.formatDate(this.instrumentModel.calibratedDueOn!));
        const file = this.fileProcessingService.base64ToFile(this.instrumentModel.certificateFile,this.instrumentModel.certificateName!,'application/pdf');
        console.log(file);



        

}

private formatDate(date: Date) {
    const d = new Date(date);
    let month = '' + (d.getMonth() + 1);
    let day = '' + d.getDate();
    const year = d.getFullYear();
    if (month.length < 2) month = '0' + month;
    if (day.length < 2) day = '0' + day;
    return [year, month, day].join('-');
  }
}

  