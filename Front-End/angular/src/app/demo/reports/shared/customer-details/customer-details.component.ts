import { BaseResponse } from './../../../../../Models/response-models/base-response';
/* eslint-disable @angular-eslint/no-empty-lifecycle-method */
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { CUSTOM_ELEMENTS_SCHEMA, Component, EventEmitter, OnInit } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ConfirmationService } from 'primeng/api';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { DynamicDialogConfig, DynamicDialogModule } from 'primeng/dynamicdialog';
import { Instrument } from 'src/Models/instrument.Model';
import { InstrumentService } from 'src/Services/Instrument.service';
import { BaseHttpClientServiceService } from 'src/Services/Shared/base-http-client-service.service';
import { SharedModule } from 'src/app/theme/shared/shared.module';

@Component({
  selector: 'app-customer-details',
  standalone: true,
  imports: [CommonModule, SharedModule, ConfirmDialogModule, HttpClientModule, ReactiveFormsModule,DynamicDialogModule],
  providers: [ConfirmationService, InstrumentService, BaseHttpClientServiceService ,DynamicDialogConfig],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  templateUrl: './customer-details.component.html',
  styleUrls: ['./customer-details.component.css']
})
export class CustomerDetailsComponent implements OnInit {

  customerDetailsFormGroup: FormGroup;
  
  instrumentList: Instrument[] = [];
  filterInstBySNo: Instrument[] = [];
  filterInstByType: Instrument[] = [];  
  oneventFire : EventEmitter<number> = new EventEmitter<number>();


  constructor(private instrumentService: InstrumentService, public ref: DynamicDialogConfig) {

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
    console.log('Form submitted')
    this.oneventFire.emit(10)   
  }
  onClear() { this.customerDetailsFormGroup.reset() }

  addFormControlValidators() {
    this.customerDetailsFormGroup.controls['client'].addValidators([Validators.required])
    this.customerDetailsFormGroup.controls['dateOfTest'].addValidators([Validators.required])
    this.customerDetailsFormGroup.controls['plant'].addValidators([Validators.required])
    this.customerDetailsFormGroup.controls['equipmentId'].addValidators([Validators.required])
    this.customerDetailsFormGroup.controls['areaOfTest'].addValidators([Validators.required])

  }

  getInstrumentsFromServer() {
    this.instrumentService.getAllPagedResponse().subscribe({
      next: (response: BaseResponse<Instrument[]>) => {
        if (response.succeeded) {
          this.instrumentList = response.data;
          this.filterInstByType = this.removeDuplicates(this.instrumentList,'type')
              
        }
      },
      error: (e) => { console.error(e.error) },
      complete: () => { },
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
}
