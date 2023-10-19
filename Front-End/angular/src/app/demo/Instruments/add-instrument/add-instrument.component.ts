/* eslint-disable @angular-eslint/no-empty-lifecycle-method */
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { CommonModule } from '@angular/common';
import { CUSTOM_ELEMENTS_SCHEMA, Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ConfirmationService, Message } from 'primeng/api';
import { MessagesModule } from 'primeng/messages';
import { SharedModule } from 'src/app/theme/shared/shared.module';
import { Instrument } from 'src/Models/instrument.Model';


@Component({
  selector: 'app-add-instrument',
  standalone: true,
  imports: [CommonModule, SharedModule, ConfirmDialogModule, MessagesModule],
  providers: [ConfirmationService],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  templateUrl: './add-instrument.component.html',
  styleUrls: ['./add-instrument.component.css']
})
export class AddInstrumentComponent implements OnInit {

  instrumentFormGroup: FormGroup;
  msgs: Message[] = [];
  isAddInstrument: boolean = true;
  instrumentModel: Instrument = new Instrument();

  constructor(private confirmationService: ConfirmationService) {

    this.instrumentFormGroup = new FormGroup(
      {
        serialNo: new FormControl(),
        make: new FormControl(),
        model: new FormControl(),
        calibratedOn: new FormControl(),
        calibratedDueOn: new FormControl(),
        certificate: new FormControl(),
      }
    );
    this.addFormControlValidators();
  }

  ngOnInit() { }

  onSubmit() {
    console.log("Form submitted")
    this.instrumentModel = this.instrumentFormGroup.value;
    console.log(this.instrumentModel)
    this.confirmDialog();
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
        this.msgs = [{ severity: 'success', summary: 'Success', detail: 'Instrument added', life: 3000 }];
        alert('Following Data will be posted to Server Database\n' + JSON.stringify(this.instrumentModel));
      },
      reject: () => {
        this.msgs = [{ severity: 'error', summary: 'Failed', detail: 'Instrument not added', life: 3000 }];
      }
    });
  }

  addFormControlValidators(){
    this.instrumentFormGroup.controls['serialNo'].addValidators([Validators.required])
  }

  toggle() { this.isAddInstrument = !this.isAddInstrument; }
}
