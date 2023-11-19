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
import { BaseResponse } from 'src/Models/response-models/base-response';
import { RoomGrill } from 'src/Models/room-grill.Model';
import { Room } from 'src/Models/room.Model';
import { BaseHttpClientServiceService } from 'src/Services/Shared/base-http-client-service.service';
import { RoomService } from 'src/Services/room.service';
import { GridComponent } from 'src/app/theme/shared/components/grid/grid.component';
import { SharedModule } from 'src/app/theme/shared/shared.module';

@Component({
  selector: 'app-acph-room-grills',
  standalone: true,
  imports: [CommonModule, SharedModule, ConfirmDialogModule, HttpClientModule, ReactiveFormsModule, GridComponent, ToastModule, DynamicDialogModule],
  providers: [ConfirmationService, RoomService, BaseHttpClientServiceService, MessageService],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  templateUrl: './acph-room-grills.component.html',
  styleUrls: ['./acph-room-grills.component.css']
})
export class AcphRoomGrillsComponent implements OnInit {

  acphRoomsFormGroup: FormGroup;
  roomModel?: Room;
  tempGrillsList: any[] = []
  listOfGrills: any[] = [];
  roomId: number = 0;
  onCloseEventFire: EventEmitter<any> = new EventEmitter<any>();
  addNewRowEvent: EventEmitter<any> = new EventEmitter<any>();

  groupedColumnOptions: GroupedColumnOptions[] = [
    {
      gridColumnOptions: [
        { field: 'grillNo', header: 'Grill/Filter Reference No.', rowspan: '2', isEditable: true, isSortable: true, hasTableValue: true, isStandalone: false, orderNo: 1 },
        { field: 'filterArea', header: 'Filter Area', colspan: '1', hasTableValue: false, isStandalone: false },
        { field: 'airVelocityreadingInFPM', header: 'Air Velociy Reading in FPM', colspan: '5', hasTableValue: false, isStandalone: false },
        { field: 'avgVelocityInFPM', header: 'Avg Velocity FPM', rowspan: '2', hasTableValue: true, isStandalone: false, orderNo: 8 },
        { field: 'airFlowCFM', header: 'Air Flow CFM', rowspan: '2', hasTableValue: true, isStandalone: false, orderNo: 9 },
        { field: 'totalAirFlowCFM', header: 'Total Air Flow CFM', rowspan: '2', hasTableValue: true, isStandalone: true, orderNo: 10 },
        { field: 'roomVolCFM', header: 'Room Volume CFT', rowspan: '2', hasTableValue: true, isStandalone: true, orderNo: 11 },
        { field: 'airChanges', header: 'Air Changes per hour', rowspan: '2', hasTableValue: true, isStandalone: true, orderNo: 12 },
        { field: '', header: 'Action', rowspan: '2', hasTableValue: false, isStandalone: false }

      ]
    },
    {
      gridColumnOptions: [
        { field: 'sqrt', width: '15%', header: 'Sqrt', isEditable: true, hasTableValue: true, isStandalone: false, orderNo: 2 },
        { field: 'one', width: '15%', header: '1', isEditable: true, hasTableValue: true, isStandalone: false, orderNo: 3 },
        { field: 'two', width: '15%', header: '2', isEditable: true, hasTableValue: true, isStandalone: false, orderNo: 4 },
        { field: 'three', width: '15%', header: '3', isEditable: true, hasTableValue: true, isStandalone: false, orderNo: 5 },
        { field: 'four', width: '15%', header: '4', isEditable: true, hasTableValue: true, isStandalone: false, orderNo: 6 },
        { field: 'five', width: '15%', header: '5', isEditable: true, hasTableValue: true, isStandalone: false, orderNo: 7 }
      ]
    }
  ]

  constructor(
    private messageService: MessageService,
    public ref: DynamicDialogConfig,
    private changeRef: ChangeDetectorRef,
    private roomService: RoomService
  ) {
    this.acphRoomsFormGroup = new FormGroup(
      {
        roomName: new FormControl(),
        design: new FormControl(),
        noOfGrills: new FormControl(),
        roomVolume: new FormControl(),
      });
    this.addFormControlValidators();

  }
  ngOnInit() {
    if (this.ref.data!.roomId !== undefined && this.ref.data!.roomId > 0) {
      console.log(this.ref.data)
      this.roomId = this.ref.data.roomId;
      this.getRoomByIdFromServer();
    }
  }

  onSubmit() {
    if (this.acphRoomsFormGroup.valid && this.listOfGrills.length > 0) {
      this.mapFormToRoomObject();
      console.log(this.roomModel);
      this.roomId > 0 ? this.updateRoomToAPIServer() : this.postRoomToAPIServer();
    } else if (this.listOfGrills.length <= 0) {
      this.messageService.add({ key: 'tc', severity: 'warn', summary: 'Warning', detail: "Please Add the Grills", life: 4000 });
    }
    else {
      this.messageService.add({ key: 'tc', severity: 'warn', summary: 'Warning', detail: "Room Details Invalid", life: 4000 });
    }

  }

  //#region  API Server
  postRoomToAPIServer() {
    this.roomService.postRoom(this.roomModel!).subscribe({
      next: (response: BaseResponse<number>) => {
        if (response.succeeded) this.messageService.add({ key: 'tc', severity: 'success', summary: 'Success', detail: 'Room along with Grills saved', life: 4000 });
        this.onCloseEventFire.emit(true);
      },
      error: (e) => {
        this.messageService.add({ key: 'tc', severity: 'error', summary: 'Failed', detail: e.error.title, life: 4000 });
      },
      complete: () => { },
    });
  }

  updateRoomToAPIServer() {
    this.roomModel!.id =this.roomId;
    this.roomService.updateRoom(this.roomId.toString(), this.roomModel!).subscribe({
      next: (response: BaseResponse<number>) => {
        console.log(response);
        this.onCloseEventFire.emit(true);
      },
      error: (e) => {
        console.error(e.error);
        this.messageService.add({ key: 'tc', severity: 'error', summary: 'Failed', 
        detail: e.error.Message !== undefined ?e.error.Message :e.error.title, life: 4000 });

      },
      complete: () => { },
    });
  }

  getRoomByIdFromServer() {
    this.roomService.getRoomById(this.roomId.toString()).subscribe({
      next: (response: BaseResponse<Room>) => {
        if (response.succeeded) {
          this.roomModel = response.data;
          this.reverseMapRoomObjectToForm();
        }
      },
      error: (e) => { console.error(e.error) },
      complete: () => { },
    });
  }
  //#endregion

  //#region Grill Rows
  addGrills() {
    this.acphRoomsFormGroup.valid ? this.addNewRowEvent.emit(true) :
      this.messageService.add({ key: 'tc', severity: 'warn', summary: 'Warning', detail: "Room Details Invalid", life: 4000 });
  }


  onGrillSave(event: any) {

    event = this.performGrillCalculations(event);

    if (this.listOfGrills.find(e => e.id === event.id) === undefined) {
      this.listOfGrills.push(event);
    } else {
      const index = this.listOfGrills.findIndex(e => e.id === event.id);
      this.listOfGrills[index] = event;
    }
    this.acphRoomsFormGroup.controls['noOfGrills'].patchValue(this.listOfGrills.length)
    this.updateCalculationChanges();
    this.changeRef.detectChanges();
  }


  onGrillDelete(event: any) {
    if (this.listOfGrills.find(e => e.id === event.id) !== undefined) {
      this.listOfGrills = this.listOfGrills.filter(e => e.id !== event.id);
    }
    this.acphRoomsFormGroup.controls['noOfGrills'].patchValue(this.listOfGrills.length);
    this.updateCalculationChanges();
    this.changeRef.detectChanges();

  }

  performGrillCalculations(grill: any) {
    const avgVelocityInFPM = parseInt(grill.one) + parseInt(grill.two) + parseInt(grill.three) + parseInt(grill.four) + parseInt(grill.five);
    grill.avgVelocityInFPM = Math.round(avgVelocityInFPM / 5);
    grill.airFlowCFM = Math.round(parseFloat(grill.sqrt) * grill.avgVelocityInFPM);
    let totalAirFlowCFM = grill.airFlowCFM;
    this.listOfGrills.forEach(e => totalAirFlowCFM += parseInt(e.airFlowCFM));
    grill.totalAirFlowCFM = totalAirFlowCFM;
    grill.roomVolCFM = parseInt(this.acphRoomsFormGroup.controls['roomVolume'].value)
    grill.airChanges = Math.round((grill.totalAirFlowCFM / grill.roomVolCFM) * 60);
    console.log(grill);
    return grill;
  }

  updateCalculationChanges() {
    let totalAirFlowCFM = 0;
    const roomVolume = parseInt(this.acphRoomsFormGroup.controls['roomVolume'].value);
    this.listOfGrills.forEach(e => totalAirFlowCFM += parseInt(e.airFlowCFM));
    this.listOfGrills.forEach(e => {
      e.totalAirFlowCFM = totalAirFlowCFM;
      e.roomVolCFM = roomVolume;
      e.airChanges = Math.round((e.totalAirFlowCFM / e.roomVolCFM) * 60);

    })

  }
  //#endregion

  //#region Forms controls
  onClear() {
    this.acphRoomsFormGroup.reset()
    this.listOfGrills = [];
  }
  addFormControlValidators() {
    this.acphRoomsFormGroup.controls['roomName'].addValidators([Validators.required])
    this.acphRoomsFormGroup.controls['design'].addValidators([Validators.required])
    this.acphRoomsFormGroup.controls['roomVolume'].addValidators([Validators.required, Validators.min(1)])
  }
  //#endregion

  //#region mappers
  mapFormToRoomObject() {
    this.roomModel = new Room();
    this.roomModel.name = this.acphRoomsFormGroup.controls['roomName'].value;
    this.roomModel.designACPH = this.acphRoomsFormGroup.controls['design'].value;
    this.roomModel.noOfGrills = this.acphRoomsFormGroup.controls['noOfGrills'].value;
    this.roomModel.roomVolume = this.acphRoomsFormGroup.controls['roomVolume'].value;
    this.roomModel.totalAirFlowCFM = this.listOfGrills[0].totalAirFlowCFM !== undefined ? this.listOfGrills[0].totalAirFlowCFM : 0;
    this.roomModel.airChangesPerHour = this.listOfGrills[0].airChanges !== undefined ? this.listOfGrills[0].airChanges : 0
    this.roomModel.customerDetailId = this.ref.data.customerDetailId;
    this.listOfGrills.forEach(e => this.roomModel?.roomGrills.push(this.MapToRoomGrill(e)))
  }

  MapToRoomGrill(grill: any): RoomGrill {
    const result = new RoomGrill();
    result.airFlowCFM = grill.airFlowCFM;
    result.avgVelocityFPM = grill.avgVelocityInFPM;
    result.filterAreaSqft = parseFloat(grill.sqrt);
    result.referenceNumber = grill.grillNo;
    result.airVelocityReadingInFPMO = grill.one + ',' + grill.two + ',' + grill.three + ',' + grill.four + ',' + grill.five
    return result;

  }
  reverseMapRoomObjectToForm() {
    this.acphRoomsFormGroup.controls['roomName'].patchValue(this.roomModel!.name);
    this.acphRoomsFormGroup.controls['design'].patchValue(this.roomModel!.designACPH);
    this.acphRoomsFormGroup.controls['noOfGrills'].patchValue(this.roomModel!.noOfGrills);
    this.acphRoomsFormGroup.controls['roomVolume'].patchValue(this.roomModel!.roomVolume);
    this.roomModel!.roomGrills.forEach(e => this.reverseMapRoomGrillToGrid(e));
    this.updateCalculationChanges();
    console.log(this.acphRoomsFormGroup.value)
    console.log(this.listOfGrills)
  }

  reverseMapRoomGrillToGrid(roomGrill: RoomGrill) {
    const result: any = { 
      id:0,airFlowCFM: 0, avgVelocityInFPM: 0, sqrt: 0, grillNo: '', one: 0, two: 0, three: 0, four: 0, five: 0 };
    result.airFlowCFM = roomGrill.airFlowCFM;
    result.id =roomGrill.id;
    result.avgVelocityInFPM = roomGrill.avgVelocityFPM;
    result.sqrt = roomGrill.filterAreaSqft;
    result.grillNo = roomGrill.referenceNumber;
    result.one = roomGrill.airVelocityReadingInFPMO.split(',')[0],
      result.two = roomGrill.airVelocityReadingInFPMO.split(',')[1],
      result.three = roomGrill.airVelocityReadingInFPMO.split(',')[2],
      result.four = roomGrill.airVelocityReadingInFPMO.split(',')[3],
      result.five = roomGrill.airVelocityReadingInFPMO.split(',')[4]
    this.listOfGrills = [...this.listOfGrills, result];
    this.changeRef.detectChanges();
  }



  //#endregion
}