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
import { GridColumnOptions } from 'src/Models/grid-column-options';
import { BaseResponse } from 'src/Models/response-models/base-response';
import { RoomGrill } from 'src/Models/room-grill.Model';
import { Room } from 'src/Models/room.Model';
import { BaseHttpClientServiceService } from 'src/Services/Shared/base-http-client-service.service';
import { RoomService } from 'src/Services/room.service';
import { GridComponent } from 'src/app/theme/shared/components/grid/grid.component';
import { SharedModule } from 'src/app/theme/shared/shared.module';

@Component({
  selector: 'app-filter-integrity-room-grills',
  standalone: true,
  imports: [
    CommonModule,
    SharedModule,
    ConfirmDialogModule,
    HttpClientModule,
    ReactiveFormsModule,
    GridComponent,
    ToastModule,
    DynamicDialogModule
  ],
  providers: [ConfirmationService, RoomService, BaseHttpClientServiceService, MessageService],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  templateUrl: './filter-integrity-room-grills.component.html',
  styleUrls: ['./filter-integrity-room-grills.component.css']
})
export class FilterIntegrityRoomGrillsComponent implements OnInit {
  acphRoomsFormGroup: FormGroup;
  roomModel?: Room;
  tempGrillsList: any[] = [];
  listOfGrills: any[] = [];
  roomId: number = 0;
  isSaveLoading: boolean = false;

  onCloseEventFire: EventEmitter<any> = new EventEmitter<any>();
  addNewRowEvent: EventEmitter<any> = new EventEmitter<any>();

  constructor(
    private messageService: MessageService,
    public ref: DynamicDialogConfig,
    private changeRef: ChangeDetectorRef,
    private roomService: RoomService
  ) {
    this.acphRoomsFormGroup = new FormGroup({
      roomName: new FormControl(),
      noOfGrills: new FormControl(),
    });
    this.addFormControlValidators();
  }

  gridColumnOptions: GridColumnOptions[] = [

    { field: 'grillNo', header: 'Terminal Filter ID', isEditable: true, isSortable: true, hasTableValue: true, isStandalone: false, orderNo: 1 },
    { field: 'size', header: 'Size',isEditable: true, hasTableValue: true, isStandalone: false },
    { field: 'upStreamConcat', header: 'Up Stream Concatenation %',isEditable: true,  hasTableValue: true, isStandalone: false },
    { field: 'penetration', header: 'Penetration %',isEditable: true,  hasTableValue: true, isStandalone: false },
    { field: 'effective', header: 'Effective %',isEditable: true,  hasTableValue: true, isStandalone: false },
    { field: 'result', header: 'Result', isEditable: false, hasTableValue: true, isStandalone: false, },
    { field: '', header: 'Action', isEditable: false, hasTableValue: false, isStandalone: false }
  ]
  
  ngOnInit() {
    if (this.ref.data!.roomId !== undefined && this.ref.data!.roomId > 0) {
      console.log(this.ref.data);
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
      this.messageService.add({ key: 'tc', severity: 'warn', summary: 'Warning', detail: 'Please Add the Grills', life: 4000 });
    } else {
      this.messageService.add({ key: 'tc', severity: 'warn', summary: 'Warning', detail: 'Room Details Invalid', life: 4000 });
    }
  }

  //#region  API Server
  postRoomToAPIServer() {
    this.isSaveLoading = true;
    this.roomService.postRoom(this.roomModel!).subscribe({
      next: (response: BaseResponse<number>) => {
        if (response.succeeded)
          this.messageService.add({
            key: 'tc',
            severity: 'success',
            summary: 'Success',
            detail: 'Room along with Grills saved',
            life: 4000
          });
        this.onCloseEventFire.emit(true);
      },
      error: (e) => {
        this.messageService.add({
          key: 'tc',
          severity: 'error',
          summary: 'Failed',
          detail: e.status == 0 ? 'Server connection error' : e.error.Message !== undefined ? e.error.Message : e.error.title,
          life: 4000
        });
        this.isSaveLoading = false;
      },
      complete: () => {
        this.isSaveLoading = false;
      }
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
          key: 'tc',
          severity: 'error',
          summary: 'Failed',
          detail: e.status == 0 ? 'Server connection error' : e.error.Message !== undefined ? e.error.Message : e.error.title,
          life: 4000
        });
        this.isSaveLoading = false;
      },
      complete: () => {
        this.isSaveLoading = false;
      }
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
      error: (e) => {
        this.messageService.add({
          key: 'tc',
          severity: 'error',
          summary: 'Failed',
          detail: e.status == 0 ? 'Server connection error' : e.error.Message !== undefined ? e.error.Message : e.error.title,
          life: 4000
        });
      },
      complete: () => {}
    });
  }
  //#endregion

  //#region Grill Rows
  addGrills() {
    this.acphRoomsFormGroup.valid
      ? this.addNewRowEvent.emit(true)
      : this.messageService.add({ key: 'tc', severity: 'warn', summary: 'Warning', detail: 'Room Details Invalid', life: 4000 });
  }

  onGrillSave(event: any) {
    // event = this.performGrillCalculations(event);

    if (this.listOfGrills.find((e) => e.id === event.id) === undefined) {
      this.listOfGrills.push(event);
    } else {
      const index = this.listOfGrills.findIndex((e) => e.id === event.id);
      this.listOfGrills[index] = event;
    }
    this.acphRoomsFormGroup.controls['noOfGrills'].patchValue(this.listOfGrills.length);
    // this.updateCalculationChanges();
    this.changeRef.detectChanges();
  }

  onGrillDelete(event: any) {
    if (this.listOfGrills.find((e) => e.id === event.id) !== undefined) {
      this.listOfGrills = this.listOfGrills.filter((e) => e.id !== event.id);
    }
    this.acphRoomsFormGroup.controls['noOfGrills'].patchValue(this.listOfGrills.length);
    // this.updateCalculationChanges();
    this.changeRef.detectChanges();
  }

 

  //#region Forms controls
  onClear() {
    this.acphRoomsFormGroup.reset();
    this.listOfGrills = [];
  }
  addFormControlValidators() {
    this.acphRoomsFormGroup.controls['roomName'].addValidators([Validators.required]);
  }
  //#endregion

  //#region mappers
  mapFormToRoomObject() {
    this.roomModel = new Room();
    this.roomModel.name = this.acphRoomsFormGroup.controls['roomName'].value;
    this.roomModel.noOfGrills = this.acphRoomsFormGroup.controls['noOfGrills'].value;
    this.roomModel.customerDetailId = this.ref.data.customerDetailId;
    this.listOfGrills.forEach((e) => this.roomModel?.roomGrills.push(this.MapToRoomGrill(e)));
  }

  MapToRoomGrill(grill: any): RoomGrill {
    const result = new RoomGrill();
    result.size = grill.size;
    result.upStreamConcat = grill.upStreamConcat;
    result.penetration = grill.penetration;
    result.referenceNumber = grill.grillNo;
    result.effective=grill.effective;
    result.result=grill.result;
    return result;
  }
  reverseMapRoomObjectToForm() {
    this.acphRoomsFormGroup.controls['roomName'].patchValue(this.roomModel!.name);
    this.acphRoomsFormGroup.controls['noOfGrills'].patchValue(this.roomModel!.noOfGrills);
    this.roomModel!.roomGrills.forEach((e) => this.reverseMapRoomGrillToGrid(e));
  }

  reverseMapRoomGrillToGrid(roomGrill: RoomGrill) {
    const gridResult: any = {
      id: 0,
      size: 0,
      upStreamConcat: '',
      result:'',
      penetration: 0,
      grillNo: '',
      effective: 0,
    };
    gridResult.size = roomGrill.size;
    gridResult.upStreamConcat = roomGrill.upStreamConcat;
    gridResult.penetration = roomGrill.penetration;
    gridResult.referenceNumber = roomGrill.referenceNumber;
    gridResult.effective=roomGrill.effective;
    
    this.listOfGrills = [...this.listOfGrills, gridResult];
    this.changeRef.detectChanges();
  }

  //#endregion
}

