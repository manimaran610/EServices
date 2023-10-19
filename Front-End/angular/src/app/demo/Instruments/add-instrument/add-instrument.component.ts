// import { ConfirmDialogModule } from 'primeng/confirmdialog';
/* eslint-disable @angular-eslint/no-empty-lifecycle-method */
import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { ConfirmationService } from 'primeng/api/confirmationservice';
import { SharedModule } from 'src/app/theme/shared/shared.module';

@Component({
  selector: 'app-add-instrument',
  standalone:true,
  imports:[CommonModule,SharedModule],
  templateUrl: './add-instrument.component.html',
  styleUrls: ['./add-instrument.component.css']
})
export class AddInstrumentComponent implements OnInit {

  instrumentFormGroup:FormGroup;
  constructor() { 
    this.instrumentFormGroup = new FormGroup(
      {
        serialNo: new FormControl(),
        make: new FormControl(),
        model: new FormControl(),
        calibratedOn: new FormControl(),
        calibratedDueOn: new FormControl(),
        certificate: new FormControl(),
   

      }
    )
  }

  ngOnInit() {
  }

  onSubmit(){
    // this.confirmationService.confirm({
    //   message: 'Are you sure that you want to proceed?',
    //   header: 'Confirmation',
    //   icon: 'pi pi-exclamation-triangle',
    //   accept: () =>    console.log(this.instrumentFormGroup.value)      ,
    //   reject: () =>console.log("Reject")
    // });
    console.log("on submit")

    console.log(this.instrumentFormGroup.value)
  }

  public onClear(){
    this.instrumentFormGroup.reset()
    console.log("on clear")
    console.log(this.instrumentFormGroup.value)

  }
}
