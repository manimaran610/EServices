/* eslint-disable @typescript-eslint/no-explicit-any */
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
import { RoomLocation } from 'src/Models/room-Location.Model';
import { Room } from 'src/Models/room.Model';
import { BaseHttpClientServiceService } from 'src/Services/Shared/base-http-client-service.service';
import { RoomService } from 'src/Services/room.service';
import { GridComponent } from 'src/app/theme/shared/components/grid/grid.component';
import { SharedModule } from 'src/app/theme/shared/shared.module';
import { BusinessConstants } from '../../../shared/Constants/business-constants';
import { GridColumnOptions } from 'src/Models/grid-column-options';

@Component({
  selector: 'app-particle-room-locations-single-cycle',
  standalone: true,
  imports: [CommonModule, SharedModule, ConfirmDialogModule, HttpClientModule, ReactiveFormsModule, GridComponent, ToastModule, DynamicDialogModule],
  providers: [ConfirmationService, RoomService, BaseHttpClientServiceService, MessageService],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  templateUrl: './particle-room-locations-single-cycle.component.html',
  styleUrls: ['./particle-room-locations-single-cycle.component.css']
})
export class ParticleRoomLocationsSingleCycleComponent implements OnInit {
  roomsFormGroup: FormGroup;
  roomModel?: Room;
  tempGrillsList: any[] = []
  classficationList: any[] = BusinessConstants.ClassificationTypes;
  listOflocations: any[] = [];
  roomId: number = 0;
  isSaveLoading: boolean = false;

  onCloseEventFire: EventEmitter<any> = new EventEmitter<any>();
  addNewRowEvent: EventEmitter<any> = new EventEmitter<any>();

  gridColumnOptions: GridColumnOptions[] = [

    { field: 'locationNo', header: 'Location No.', isEditable: true, isSortable: true, hasTableValue: true, isStandalone: false, orderNo: 1 },
    { field: 'ptAverage', header: '0.5 Micron and above',isEditable: true, hasTableValue: true,inputType: 'number', isStandalone: false },
    { field: 'oneAverage', header: '1 Micron and above',isEditable: true,  hasTableValue: true,inputType: 'number', isStandalone: false },
    { field: 'fiveAverage', header: '5 Micron and above',isEditable: true,  hasTableValue: true,inputType: 'number', isStandalone: false },
    { field: 'result', header: 'Result', isEditable: false, hasTableValue: true, isStandalone: false, },
    { field: '', header: 'Action', isEditable: false, hasTableValue: false, isStandalone: false }
  ]

  constructor(
    private messageService: MessageService,
    public ref: DynamicDialogConfig,
    private changeRef: ChangeDetectorRef,
    private roomService: RoomService
  ) {
    this.roomsFormGroup = new FormGroup(
      {
        roomName: new FormControl(),
        areaM2: new FormControl(),
        noOfLocations: new FormControl(),
        classType: new FormControl('0'),
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

  //#region Forms controls
  onSubmit() {
    if (this.roomsFormGroup.valid && this.listOflocations.length > 0) {
      if (this.hasMinimumCleanRoomSampling()) {
        this.mapFormToRoomObject();
        console.log(this.roomModel);
        this.roomId > 0 ? this.updateRoomToAPIServer() : this.postRoomToAPIServer();
      }
    } else if (this.listOflocations.length <= 0) {
      this.messageService.add({ key: 'tc', severity: 'warn', summary: 'Warning', detail: "Please Add the Locations", life: 4000 });
    }
    else {
      this.messageService.add({ key: 'tc', severity: 'warn', summary: 'Warning', detail: "Room Details Invalid", life: 4000 });
    }

  }

  onClear() {
    this.roomsFormGroup.reset()
    this.listOflocations = [];
  }
  addFormControlValidators() {
    this.roomsFormGroup.controls['roomName'].addValidators([Validators.required])
    this.roomsFormGroup.controls['areaM2'].addValidators([Validators.required, Validators.min(1)])
    this.roomsFormGroup.controls['classType'].addValidators([Validators.required,])
  }
  reEvaluateCalcResults() {
    console.log(this.listOflocations)
    this.listOflocations.forEach(e => e = this.evaluateFinalResult(e));
  }

  hasMinimumCleanRoomSampling(): boolean {
    console.log('hasMinimumCleanRoomSampling')
    let result: boolean = false;
    const areaM2 = parseInt(this.roomsFormGroup.controls['areaM2'].value);
    const noOfLocation = parseInt(this.roomsFormGroup.controls['noOfLocations'].value);

    let cleanRoomSample = BusinessConstants.ISO14644_Clean_Room_Samples.find(e => e.cleanroomArea == areaM2);
    if (cleanRoomSample !== undefined) {
      const minimumSample = parseInt(cleanRoomSample.minimumSample);
      if (minimumSample > noOfLocation) {
        this.messageService.add({ key: 'tc', severity: 'warn', summary: 'Warning', detail: `${minimumSample} Minimum samples required for Area ${areaM2}`, life: 4000 });
      } else result = true;
    }
    else {
      cleanRoomSample = BusinessConstants.ISO14644_Clean_Room_Samples.filter(e => parseInt(e.cleanroomArea) > areaM2)[0];
      if (cleanRoomSample !== undefined) {
        const minimumSample = parseInt(cleanRoomSample.minimumSample);
        if (minimumSample > noOfLocation) {
          this.messageService.add({ key: 'tc', severity: 'warn', summary: 'Warning', detail: `${minimumSample} Minimum samples required for Area ${areaM2}`, life: 4000 });
        } else result = true;
      }
      else {
        const minimumSample = 27 * Math.round(areaM2 / 1000);
        if (minimumSample > noOfLocation) {
          this.messageService.add({ key: 'tc', severity: 'warn', summary: 'Warning', detail: `${minimumSample} Minimum samples required for Area ${areaM2}`, life: 4000 });
        } else result = true;
      }
    }
    return result;
  }
  //#endregion

  //#region Grill Rows
  addGridRow() {
    this.roomsFormGroup.valid && this.roomsFormGroup.controls['classType'].value !== '0' ? this.addNewRowEvent.emit(true) :
      this.messageService.add({ key: 'tc', severity: 'warn', summary: 'Warning', detail: "Room Details Invalid", life: 4000 });
  }


  onGridRowSave(event: any) {

    this.evaluateFinalResult(event);
    if (this.listOflocations.find(e => e.id === event.id) === undefined) {
      this.listOflocations.push(event);
    } else {
      const index = this.listOflocations.findIndex(e => e.id === event.id);
      this.listOflocations[index] = event;
    }

    this.roomsFormGroup.controls['noOfLocations'].patchValue(this.listOflocations.length)
    this.changeRef.detectChanges();
  }


  onGridRowDelete(event: any) {
    if (this.listOflocations.find(e => e.id === event.id) !== undefined) {
      this.listOflocations = this.listOflocations.filter(e => e.id !== event.id);
    }
    this.roomsFormGroup.controls['noOfLocations'].patchValue(this.listOflocations.length);
    this.changeRef.detectChanges();

  }


  evaluateFinalResult(rowData: any) {
    const className = this.roomsFormGroup.controls['classType'].value;
    const classType = this.classficationList.find(e => e.name == className);
    const isPassed = rowData.ptAverage <= classType.pointFiveMicron && rowData.oneAverage <= classType.oneMicron && rowData.fiveAverage <= classType.fiveMicron;
    rowData.result = isPassed ? 'Complies' : 'Non Complies';
    rowData.resultClass = isPassed ? 'text-c-green' : 'text-c-red';

  }


  //#endregion

  //#region  API Server
  postRoomToAPIServer() {
    this.isSaveLoading = true;
    this.roomService.postRoom(this.roomModel!).subscribe({
      next: (response: BaseResponse<number>) => {
        if (response.succeeded) this.messageService.add({ key: 'tc', severity: 'success', summary: 'Success', detail: 'Room along with Grills saved', life: 4000 });
        this.onCloseEventFire.emit(true);
      },
      error: (e) => {
        this.messageService.add({
          key: 'tc', severity: 'error', summary: 'Failed',
          detail: e.status ==0? 'Server connection error': e.error.Message !== undefined ? e.error.Message : e.error.title, life: 4000      
        });
        this.isSaveLoading = false;
      },
      complete: () => {
        this.isSaveLoading = false;
      },
    });
  }

  updateRoomToAPIServer() {
    this.isSaveLoading = true;
    this.roomModel!.id = this.roomId;
    this.roomService.updateRoom(this.roomId.toString(), this.roomModel!).subscribe({
      next: (response: BaseResponse<number>) => {
        console.log(response);
        this.onCloseEventFire.emit(true);
      },
      error: (e) => {
        console.error(e.error);
        this.messageService.add({
          key: 'tc', severity: 'error', summary: 'Failed',
          detail: e.status ==0? 'Server connection error': e.error.Message !== undefined ? e.error.Message : e.error.title, life: 4000      
        });
        this.isSaveLoading = false;
      },
      complete: () => {
        this.isSaveLoading = false;
      },
    });
  }

  getRoomByIdFromServer() {
    this.roomService.getRoomById(this.roomId.toString()).subscribe({
      next: (response: BaseResponse<Room>) => {
        if (response.succeeded) {
          this.roomModel = response.data;
          console.log(response)
          console.log(this.roomModel)
          this.reverseMapRoomObjectToForm();
        }
      },
      error: (e) => {
        this.messageService.add({
          key: 'tc', severity: 'error', summary: 'Failed',
          detail: e.status ==0? 'Server connection error': e.error.Message !== undefined ? e.error.Message : e.error.title, life: 4000      
        });         },
      complete: () => { },
    });
  }
  //#endregion

  //#region mappers
  mapFormToRoomObject() {
    this.roomModel = new Room();
    this.roomModel.name = this.roomsFormGroup.controls['roomName'].value;
    this.roomModel.areaM2 = this.roomsFormGroup.controls['areaM2'].value;
    this.roomModel.noOfLocations = this.roomsFormGroup.controls['noOfLocations'].value;
    this.roomModel.classType = this.roomsFormGroup.controls['classType'].value;
    this.roomModel.customerDetailId = this.ref.data.customerDetailId;
    this.listOflocations.forEach(e => this.roomModel?.roomLocations.push(this.MapToRoomLocation(e)))
  }

  MapToRoomLocation(location: any): RoomLocation {
    const result = new RoomLocation();
    result.referenceNumber = location.locationNo;
    result.averagePointFiveMicron = location.ptAverage
    result.averageFiveMicron = location.fiveAverage
    result.averageOneMicron = location.oneAverage
    result.result = location.result;
    return result;

  }
  reverseMapRoomObjectToForm() {
    this.roomsFormGroup.controls['roomName'].patchValue(this.roomModel!.name);
    this.roomsFormGroup.controls['areaM2'].patchValue(this.roomModel!.areaM2);
    this.roomsFormGroup.controls['noOfLocations'].patchValue(this.roomModel!.noOfLocations);
    this.roomsFormGroup.controls['classType'].patchValue(this.roomModel!.classType);
    this.roomModel!.roomLocations.forEach(e => this.reverseMapRoomLcationToGrid(e));
    this.reEvaluateCalcResults();

    console.log(this.roomsFormGroup.value)
    console.log(this.listOflocations)
  }

  reverseMapRoomLcationToGrid(location: RoomLocation) {
    const result: any = {
      id: 0, locationNo: 0, ptOne: 0, ptTwo: 0, ptThree: 0, ptAverage: 0, one: 0, two: 0, three: 0, Average: 0, result: ''
    };
    result.id = location.id;
    result.locationNo = location.referenceNumber;
    result.ptAverage = location.averagePointFiveMicron;
    result.oneAverage = location.averageOneMicron;
    result.fiveAverage = location.averageFiveMicron;
    result.result = location.result;

    this.listOflocations = [...this.listOflocations, result];
    this.changeRef.detectChanges();
  }

  //#endregion
}
