import { Event } from '@angular/router';
/* eslint-disable @typescript-eslint/no-explicit-any */
import { GroupedColumnOptions } from './../../../../../Models/grouped-column-options';
/* eslint-disable @angular-eslint/no-empty-lifecycle-method */
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { CUSTOM_ELEMENTS_SCHEMA, Component, OnInit, EventEmitter, ChangeDetectorRef } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ConfirmationService, MessageService } from 'primeng/api';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { DynamicDialogConfig, DynamicDialogModule } from 'primeng/dynamicdialog';
import { ToastModule } from 'primeng/toast';
import { RoomGrill } from 'src/Models/room-grill.Model';
import { Room } from 'src/Models/room.Model';
import { InstrumentService } from 'src/Services/Instrument.service';
import { BaseHttpClientServiceService } from 'src/Services/Shared/base-http-client-service.service';
import { GridComponent } from 'src/app/theme/shared/components/grid/grid.component';
import { SharedModule } from 'src/app/theme/shared/shared.module';

@Component({
  selector: 'app-acph-room-grills',
  standalone: true,
  imports: [CommonModule, SharedModule, ConfirmDialogModule, HttpClientModule, ReactiveFormsModule, GridComponent, ToastModule, DynamicDialogModule],
  providers: [ConfirmationService, InstrumentService, BaseHttpClientServiceService, DynamicDialogConfig, MessageService],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  templateUrl: './acph-room-grills.component.html',
  styleUrls: ['./acph-room-grills.component.css']
})
export class AcphRoomGrillsComponent implements OnInit {

  acphRoomsFormGroup: FormGroup;
  roomModel?: Room;
  onCloseEventFire: EventEmitter<any> = new EventEmitter<any>();
  tempGrillsList: any[] = []
  list: any[] = []
  addNewRowEvent: EventEmitter<any> = new EventEmitter<any>();

  groupedColumnOptions: GroupedColumnOptions[] = [
    {
      gridColumnOptions: [
        { field: 'grillNo', header: 'Grill/Filter Reference No.', rowspan: '2', isEditable: true, hasTableValue: true, isStandalone: false, orderNo: 1 },
        { field: 'filterArea', header: 'Filter Area', colspan: '1', hasTableValue: false, isStandalone: false },
        { field: 'airVelocityreadingInFPM', header: 'Air Velociy Reading in FPM', colspan: '5', hasTableValue: false, isStandalone: false },
        { field: 'avgVelocityInFPM', header: 'Avg Velocity FPM', rowspan: '2', hasTableValue: true, isStandalone: false, orderNo: 8 },
        { field: 'airFlowCFM', header: 'Air Flow CFM', rowspan: '2', hasTableValue: true, isStandalone: false, orderNo: 9 },
        { field: 'totalAirFlowCFM', header: 'Total Air Flow CFM', rowspan: '2', hasTableValue: true, isStandalone: true, orderNo: 10 },
        { field: 'toomVolCFM', header: 'Room Volume CFT', rowspan: '2', hasTableValue: true, isStandalone: true, orderNo: 11 },
        { field: 'airChanges', header: 'Air Changes per hour', rowspan: '2', hasTableValue: true, isStandalone: true, orderNo: 12 }
      ]
    },
    {
      gridColumnOptions: [
        { field: 'sqrt', header: 'Sqrt', isEditable: true, hasTableValue: true, isStandalone: false, orderNo: 2 },
        { field: 'one', header: '1', isEditable: true, hasTableValue: true, isStandalone: false, orderNo: 3 },
        { field: 'two', header: '2', isEditable: true, hasTableValue: true, isStandalone: false, orderNo: 4 },
        { field: 'three', header: '3', isEditable: true, hasTableValue: true, isStandalone: false, orderNo: 5 },
        { field: 'four', header: '4', isEditable: true, hasTableValue: true, isStandalone: false, orderNo: 6 },
        { field: 'five', header: '5', isEditable: true, hasTableValue: true, isStandalone: false, orderNo: 7 }
      ]
    }
  ]

  constructor(private messageService: MessageService, public ref: DynamicDialogConfig, private changeRef: ChangeDetectorRef) {
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
    console.warn(this.list);


    this.mapFormToRoomObject();
    console.log(this.roomModel);

  }


  addGrills() { this.addNewRowEvent.emit(true); }

  onGrillSave(event: any) {

    event = this.performGrillCalculations(event);

    if (this.list.find(e => e.id === event.id) === undefined) {
      this.list.push(event);
    } else {
      const index = this.list.findIndex(e => e.id === event.id);
      this.list[index] = event;
    }
    this.acphRoomsFormGroup.controls['noOfGrills'].patchValue(this.list.length)
    this.changeRef.detectChanges();

  }


  onGrillDelete(event: any) {
    if (this.list.find(e => e.id === event.id) !== undefined) {
      this.list = this.list.filter(e => e.id !== event.id);
    }
    this.acphRoomsFormGroup.controls['noOfGrills'].patchValue(this.list.length)
    this.changeRef.detectChanges();


  }

  performGrillCalculations(grill: any) {
    const avgVelocityInFPM = parseInt(grill.one) + parseInt(grill.two) + parseInt(grill.three) + parseInt(grill.four) + parseInt(grill.five);
    grill.avgVelocityInFPM = avgVelocityInFPM / 5;
    grill.airFlowCFM = Math.round(parseFloat(grill.sqrt) * grill.avgVelocityInFPM);
    grill.roomVolume = parseInt(this.acphRoomsFormGroup.controls['roomVolume'].value)
    grill.airChanges =  (grill.airFlowCFM / grill.roomVolume) * 60;
    console.log(grill);
    return grill;
  }

  onClear() {
    this.acphRoomsFormGroup.reset()
    this.list = [];
  }
  addFormControlValidators() {
    this.acphRoomsFormGroup.controls['roomName'].addValidators([Validators.required])
    this.acphRoomsFormGroup.controls['design'].addValidators([Validators.required])
    this.acphRoomsFormGroup.controls['noOfGrills'].addValidators([Validators.required])
    this.acphRoomsFormGroup.controls['roomVolume'].addValidators([Validators.required])
  }

  mapFormToRoomObject() {
    this.roomModel = new Room();
    this.roomModel.name = this.acphRoomsFormGroup.controls['roomName'].value;
    this.roomModel.designACPH = this.acphRoomsFormGroup.controls['design'].value;
    this.roomModel.noOfGrills = this.acphRoomsFormGroup.controls['noOfGrills'].value;
    this.roomModel.roomVolume = this.acphRoomsFormGroup.controls['roomVolume'].value;
    this.list.forEach(e => this.roomModel?.grills.push(this.MapToRoomGrill(e)))
  }

  MapToRoomGrill(grill: any): RoomGrill {
    const result = new RoomGrill();
    result.airFlowCFM = grill.airFlowCFM;
    result.avgVelocityFPM = grill.avgVelocityInFPM;
    result.filterAreaSqft = grill.sqrt;
    result.referenceNumber = grill.grillNo;
    result.airVelocityReadingInFPMO = grill.one + ',' + grill.two + ',' + grill.three + ',' + grill.four + ',' + grill.five
    return result;

  }
}