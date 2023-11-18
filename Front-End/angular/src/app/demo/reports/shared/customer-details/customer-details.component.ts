import { Instrument } from './../../../../../Models/instrument.Model';
import { SharedModule } from './../../../../theme/shared/shared.module';
import { InstrumentService } from './../../../../../Services/Instrument.service';
import { BaseHttpClientServiceService } from './../../../../../Services/Shared/base-http-client-service.service';
import { CustomerDetail } from './../../../../../Models/customer-detail.Model';
import { CustomerDetailService } from './../../../../../Services/cutomer-detail.service';
import { BaseResponse } from './../../../../../Models/response-models/base-response';
/* eslint-disable @angular-eslint/no-empty-lifecycle-method */
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { CUSTOM_ELEMENTS_SCHEMA, Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ConfirmationService } from 'primeng/api';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { DynamicDialogConfig, DynamicDialogModule } from 'primeng/dynamicdialog';
import { ProgressSpinnerModule } from 'primeng/progressspinner';
import { FileProcessingService } from 'src/Services/Shared/file-processing.service';


@Component({
  selector: 'app-customer-details',
  standalone: true,
  imports: [CommonModule, SharedModule, ConfirmDialogModule, HttpClientModule, ReactiveFormsModule, DynamicDialogModule, ProgressSpinnerModule],
  providers: [ConfirmationService, InstrumentService, CustomerDetailService, BaseHttpClientServiceService, FileProcessingService, DynamicDialogConfig],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  templateUrl: './customer-details.component.html',
  styleUrls: ['./customer-details.component.css']
})
export class CustomerDetailsComponent implements OnInit {
@Input() formType:number =0;
@Input() customerDetailId:number =0;

 
customerDetailsFormGroup: FormGroup;

  instrumentList: Instrument[] = [];
  filterInstBySNo: Instrument[] = [];
  filterInstByType: Instrument[] = [];
  customerDetailModel: CustomerDetail = new CustomerDetail();
  isSaved: boolean = false;
  isLoading: boolean = false;
  @Output() customerSave: EventEmitter<BaseResponse<number>> = new EventEmitter<BaseResponse<number>>();
  @Output() customerError: EventEmitter<string> = new EventEmitter<string>();
  @Output() customerInfo: EventEmitter<string> = new EventEmitter<string>();




  constructor(private instrumentService: InstrumentService, private customerDetailService: CustomerDetailService, private fileService: FileProcessingService, public ref: DynamicDialogConfig) {

    this.customerDetailsFormGroup = new FormGroup(
      {
        client: new FormControl(),
        dateOfTest: new FormControl(),
        plant: new FormControl(),
        equipmentId: new FormControl(),
        areaOfTest: new FormControl(),
        instrumentType: new FormControl(['0']),
        instrumentSerialNo: new FormControl(['0'])

      });
    this.addFormControlValidators();

  }

  ngOnInit() {
    this.getInstrumentsFromServer();
    if(this.customerDetailId > 0){
       this.GetCustomerDetailFromAPIServer();
    }
  }

  onSubmit() {
    console.log(this.customerDetailsFormGroup.value)
    console.log(this.customerDetailsFormGroup.valid)

    if (this.customerDetailsFormGroup.valid) {
      console.log('Form submitted')
      this.customerDetailModel = this.customerDetailsFormGroup.value;
      this.customerDetailModel.instrumentId = this.customerDetailsFormGroup.controls['instrumentSerialNo'].value;
      this.customerDetailModel.formType=this.formType;
      this.customerDetailId > 0 ? this.updateCustomerToAPIServer(): this.postCustomerToAPIServer();
    } else {
      this.customerError.emit("Customer Details Invalid")
    }

  }

  onClear() {
    this.customerDetailsFormGroup.reset()
  }

//#region  API Server calls
  getInstrumentsFromServer() {
    this.instrumentService.getAllPagedResponse().subscribe({
      next: (response: BaseResponse<Instrument[]>) => {
        if (response.succeeded) {
          this.instrumentList = response.data;
          this.filterInstByType = this.removeDuplicates(this.instrumentList, 'type');
          if(this.customerDetailId > 0) this.mapCustomerDetailToForm();

        }
      },
      error: (e) => { console.error(e.error) },
      complete: () => { },
    });
  }

  getReportFromServer() {
    if (this.customerDetailId > 0) {
      this.isLoading = true;
      this.customerInfo.emit('File started Processing')
      // this.customerDetailService.getReport(this.customerDetailResponse.data.toString()).subscribe({
      //   next: (response: BaseResponse<string>) => {
      //     if (response.succeeded) {
      //       console.log(response)
      //       const file = this.fileService.base64ToFile(response.data, "ACPH.docx",'application/vnd.openxmlformats-officedocument.wordprocessingml.document');
      //       this.fileService.downloadFile(file);
      //     }
      //   },
      //   error: (e) => { 
      //     console.error(e.error);
      //     this.isLoading = false; },
      //   complete: () => {  this.isLoading = false;},
      // });
      this.isLoading=false;
    }
  }
  GetCustomerDetailFromAPIServer() {
    this.customerDetailService.getCustomerDetailById(this.customerDetailId.toString()).subscribe({
      next: (response: BaseResponse<CustomerDetail>) => {
        this.customerDetailModel = response.data;  
        this.mapCustomerDetailToForm();  
      },
      error: (e) => {
        console.error(e)
        e.error.Message !== undefined ? this.customerError.emit(e.error.Message) : this.customerError.emit(e.error.title)

      },
      complete: () => { },
    });
  }
  postCustomerToAPIServer() {
    console.log(this.customerDetailModel)
    this.customerDetailService.postCustomerDetail(this.customerDetailModel).subscribe({
      next: (response: BaseResponse<number>) => {
        console.log(response)
        this.customerSave.emit(response);
        this.customerDetailId=response.data;
        this.isSaved = true;
      },
      error: (e) => {
        console.error(e)
        e.error.Message !== undefined ? this.customerError.emit(e.error.Message) : this.customerError.emit(e.error.title)

      },
      complete: () => { },
    });
  }

  updateCustomerToAPIServer() {
    console.log(this.customerDetailModel)
    this.customerDetailModel.id = this.customerDetailId;
    this.customerDetailService.updateCustomerDetail(this.customerDetailId.toString(),this.customerDetailModel).subscribe({
      next: (response: BaseResponse<number>) => {
        console.log(response)
        this.customerSave.emit(response);
        this.isSaved = true;
      },
      error: (e) => {
        console.error(e)
        e.error.Message !== undefined ? this.customerError.emit(e.error.Message) : this.customerError.emit(e.error.title)

      },
      complete: () => { },
    });
  }
//#endregion

  onSelectOptionChange() {
    const type = this.customerDetailsFormGroup.controls['instrumentType'].value;
    this.filterInstBySNo = this.instrumentList.filter(e => e.type == type);
  }

  removeDuplicates(data: any[], key: string): any[] {
    return data.filter((item, index, self) => self.findIndex((i) => i[key] === item[key]) === index);
  }

  addFormControlValidators() {
    this.customerDetailsFormGroup.controls['client'].addValidators([Validators.required])
    this.customerDetailsFormGroup.controls['dateOfTest'].addValidators([Validators.required])
    this.customerDetailsFormGroup.controls['plant'].addValidators([Validators.required])
    this.customerDetailsFormGroup.controls['equipmentId'].addValidators([Validators.required])
    this.customerDetailsFormGroup.controls['areaOfTest'].addValidators([Validators.required])
    this.customerDetailsFormGroup.controls['instrumentSerialNo'].addValidators([Validators.min(1)])

  }

  mapCustomerDetailToForm(){
    this.customerDetailsFormGroup.controls['client'].patchValue(this.customerDetailModel.client);
    this.customerDetailsFormGroup.controls['dateOfTest'].patchValue(this.formatDate(this.customerDetailModel.dateOfTest!));
    this.customerDetailsFormGroup.controls['plant'].patchValue(this.customerDetailModel.plant);
    this.customerDetailsFormGroup.controls['equipmentId'].patchValue(this.customerDetailModel.equipmentId);
    this.customerDetailsFormGroup.controls['areaOfTest'].patchValue(this.customerDetailModel.areaOfTest);
    this.customerDetailsFormGroup.controls['instrumentType'].patchValue(this.instrumentList.find(e=>e.id== this.customerDetailModel.instrumentId)?.type);
    this.onSelectOptionChange();
    this.customerDetailsFormGroup.controls['instrumentSerialNo'].patchValue(this.customerDetailModel.instrumentId);


  }

  private formatDate(date:Date) {
    const d = new Date(date);
    let month = '' + (d.getMonth() + 1);
    let day = '' + d.getDate();
    const year = d.getFullYear();
    if (month.length < 2) month = '0' + month;
    if (day.length < 2) day = '0' + day;
    return [year, month, day].join('-');
  }

}
