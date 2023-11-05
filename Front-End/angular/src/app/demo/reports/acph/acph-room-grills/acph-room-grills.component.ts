import { GroupedColumnOptions } from './../../../../../Models/grouped-column-options';
/* eslint-disable @angular-eslint/no-empty-lifecycle-method */
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { CUSTOM_ELEMENTS_SCHEMA, Component, OnInit, EventEmitter } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ConfirmationService, MessageService } from 'primeng/api';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { DynamicDialogConfig } from 'primeng/dynamicdialog';
import { ToastModule } from 'primeng/toast';
import { InstrumentService } from 'src/Services/Instrument.service';
import { BaseHttpClientServiceService } from 'src/Services/Shared/base-http-client-service.service';
import { GridComponent } from 'src/app/theme/shared/components/grid/grid.component';
import { SharedModule } from 'src/app/theme/shared/shared.module';

@Component({
  selector: 'app-acph-room-grills',
  standalone: true,
  imports: [CommonModule, SharedModule, ConfirmDialogModule, HttpClientModule, ReactiveFormsModule, GridComponent, ToastModule],
  providers: [ConfirmationService, InstrumentService, BaseHttpClientServiceService, DynamicDialogConfig, MessageService],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  templateUrl: './acph-room-grills.component.html',
  styleUrls: ['./acph-room-grills.component.css']
})
export class AcphRoomGrillsComponent implements OnInit {

  acphRoomsFormGroup: FormGroup;
  // instance:GridComponent;
  list: any[] = []
  addNewRowEvent: EventEmitter<any> = new EventEmitter<any>();

  groupedColumnOptions: GroupedColumnOptions[] = [
    {
      gridColumnOptions: [
        { field: 'GrillNo', header: 'Grill/Filter Reference No.', rowspan: '2', hasTableValue: true, isStandalone: false },
        { field: 'FilterArea', header: 'Filter Area', colspan: '1', hasTableValue: false, isStandalone: false },
        { field: 'AirVelocityInFPM', header: 'Air Velociy Reading in FPM', colspan: '5', hasTableValue: false, isStandalone: false },
        { field: 'AvgVelocityInFPM', header: 'Avg Velocity FPM', rowspan: '2', hasTableValue: true, isStandalone: false },
        { field: 'AirFlowCFM', header: 'Air Flow CFM', rowspan: '2', hasTableValue: true, isStandalone: false },
        { field: 'TotalAirFlowCFM', header: 'Total Air Flow CFM', rowspan: '2', hasTableValue: true, isStandalone: true },
        { field: 'RoomVolCFM', header: 'Room Volume CFT', rowspan: '2', hasTableValue: true, isStandalone: true },
        { field: 'AirChanges', header: 'Air Changes per hour', rowspan: '2', hasTableValue: true, isStandalone: true }
      ]
    },
    {
      gridColumnOptions: [
        { field: 'sqrt', header: 'Sqrt', hasTableValue: true, isStandalone: false },
        { field: '1', header: '1', hasTableValue: true, isStandalone: false },
        { field: '2', header: '2', hasTableValue: true, isStandalone: false },
        { field: '3', header: '3', hasTableValue: true, isStandalone: false },
        { field: '4', header: '4', hasTableValue: true, isStandalone: false },
        { field: '5', header: '5', hasTableValue: true, isStandalone: false }
      ]
    }
  ]

  constructor(private messageService: MessageService) {
    this.acphRoomsFormGroup = new FormGroup(
      {
        roomName: new FormControl(),
        design: new FormControl(),
        noOfGrills: new FormControl(),
        roomVolume: new FormControl(),
      });
    this.addFormControlValidators();

  }
  ngOnInit() { }

  onSubmit() {
    console.log(this.acphRoomsFormGroup.value);

  }


  addGrills() {
    if (this.acphRoomsFormGroup.controls['noOfGrills'].valid && this.acphRoomsFormGroup.controls['noOfGrills'].value > 0) {
      for (let index = 0; index < this.acphRoomsFormGroup.controls['noOfGrills'].value; index++) {
        this.addNewRowEvent.emit(true);

      }
    }else {
      this.messageService.add({ key: 'tc', severity: 'warn', summary: 'Warning', detail: "Please enter valid No. of Grills to add!", life: 4000 });

    }
  }



  onClear() { this.acphRoomsFormGroup.reset() }
  addFormControlValidators() {
    this.acphRoomsFormGroup.controls['roomName'].addValidators([Validators.required])
    this.acphRoomsFormGroup.controls['design'].addValidators([Validators.required])
    this.acphRoomsFormGroup.controls['noOfGrills'].addValidators([Validators.required, Validators.max(30)])
    this.acphRoomsFormGroup.controls['roomVolume'].addValidators([Validators.required])
  }
}