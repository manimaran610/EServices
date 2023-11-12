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
import { CUSTOM_ELEMENTS_SCHEMA, Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ConfirmationService, Message } from 'primeng/api';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { DynamicDialogConfig, DynamicDialogModule } from 'primeng/dynamicdialog';


@Component({
  selector: 'app-customer-details',
  standalone: true,
  imports: [CommonModule, SharedModule, ConfirmDialogModule, HttpClientModule, ReactiveFormsModule, DynamicDialogModule],
  providers: [ConfirmationService, InstrumentService, CustomerDetailService, BaseHttpClientServiceService, DynamicDialogConfig],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  templateUrl: './customer-details.component.html',
  styleUrls: ['./customer-details.component.css']
})
export class CustomerDetailsComponent implements OnInit {

  customerDetailsFormGroup: FormGroup;

  instrumentList: Instrument[] = [];
  filterInstBySNo: Instrument[] = [];
  filterInstByType: Instrument[] = [];
  customerDetailModel: CustomerDetail = new CustomerDetail();
  @Output() customerSave: EventEmitter<BaseResponse<number>> = new EventEmitter<BaseResponse<number>>();
  @Output() customerError: EventEmitter<string> = new EventEmitter<string>();



  constructor(private instrumentService: InstrumentService, private customerDetailService: CustomerDetailService, public ref: DynamicDialogConfig) {

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
  }

  onSubmit() {
    if (this.customerDetailsFormGroup.valid && this.customerDetailsFormGroup.controls['instrumentSerialNo'].value[0] > 0) {
      console.log('Form submitted')
      this.customerDetailModel = this.customerDetailsFormGroup.value;
      this.customerDetailModel.instrumentId = this.customerDetailsFormGroup.controls['instrumentSerialNo'].value[0];
      this.postCustomerToAPIServer();
    } else {
      this.customerError.emit("Customer Details Invalid")
    }

  }

  onClear() {
    this.customerDetailsFormGroup.reset()
  }


  getInstrumentsFromServer() {
    this.instrumentService.getAllPagedResponse().subscribe({
      next: (response: BaseResponse<Instrument[]>) => {
        if (response.succeeded) {
          this.instrumentList = response.data;
          this.filterInstByType = this.removeDuplicates(this.instrumentList, 'type')

        }
      },
      error: (e) => { console.error(e.error) },
      complete: () => { },
    });
  }
  postCustomerToAPIServer() {
    console.log(this.customerDetailModel)
    this.customerDetailService.postCustomerDetail(this.customerDetailModel).subscribe({
      next: (response: BaseResponse<number>) => {
        console.log(response)
        this.customerSave.emit(response);
      },
      error: (e) => {
        console.error(e)
        e.error.Message !== undefined ? this.customerError.emit(e.error.Message) : this.customerError.emit(e.error.title)

      },
      complete: () => {},
    });
  }

  onSelectOptionChange() {
    const id = this.customerDetailsFormGroup.controls['instrumentType'].value;
    const instrument = this.instrumentList.find(e => e.id == id);
    this.filterInstBySNo = this.instrumentList.filter(e => e.type == instrument?.type);
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

  }

}
