/* eslint-disable @angular-eslint/no-empty-lifecycle-method */
import { CUSTOM_ELEMENTS_SCHEMA, Component, OnDestroy, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from 'src/app/theme/shared/shared.module';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { DialogService, DynamicDialogModule, DynamicDialogRef } from 'primeng/dynamicdialog';
import { MessagesModule } from 'primeng/messages';
import { HttpClientModule } from '@angular/common/http';
import { ToastModule } from 'primeng/toast';
import { BaseHttpClientServiceService } from 'src/Services/Shared/base-http-client-service.service';
import { ConfirmationService, MessageService } from 'primeng/api';
import { BaseResponse } from 'src/Models/response-models/base-response';
import { GridComponent } from 'src/app/theme/shared/components/grid/grid.component';
import { Room } from 'src/Models/room.Model';
import { GridColumnOptions } from 'src/Models/grid-column-options';
import { RoomService } from 'src/Services/room.service';
import { RequestParameter } from 'src/Models/request-parameter';
import { CustomerDetailsComponent } from '../../../shared/Components/customer-details/customer-details.component';
import { ActivatedRoute, Router } from '@angular/router';
import { ParticleRoomLocationsThreeCycleComponent } from '../particle-room-locations-three-cycle/particle-room-locations-three-cycle.component';
import { CustomerDetailService } from 'src/Services/cutomer-detail.service';
import { BusinessConstants } from '../../../shared/Constants/business-constants';
import { FileProcessingService } from 'src/Services/Shared/file-processing.service';

@Component({
  selector: 'app-particle-count-three-cycle',
  standalone: true,
  imports: [CommonModule, SharedModule, ConfirmDialogModule, MessagesModule,
    HttpClientModule, ToastModule, CustomerDetailsComponent, ParticleRoomLocationsThreeCycleComponent,
    DynamicDialogModule, GridComponent],
  providers: [ConfirmationService, BaseHttpClientServiceService,CustomerDetailService,FileProcessingService, MessageService, DialogService, RoomService],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  templateUrl: './particle-count-three-cycle.component.html',
  styleUrls: ['./particle-count-three-cycle.component.css']
})
export class ParticleCountThreeCycleComponent implements OnDestroy, OnInit {
  ref: DynamicDialogRef | undefined;
  instance?: ParticleRoomLocationsThreeCycleComponent;
  customerDetailId: number = 0;
  roomId?: number;
  formType:number =4;
  isReportDownloading:boolean = false;
  listOfRooms: Room[] = [];

  constructor(private router: Router,private customerDetailService: CustomerDetailService, private fileService: FileProcessingService,private route: ActivatedRoute, public dialogService: DialogService, private messageService: MessageService, private roomService: RoomService) { }

  gridColumnOptions: GridColumnOptions[] = [
    { field: 'name', header: 'Room Name', isSortable: true, hasTableValue: true, isStandalone: false },
    { field: 'areaM2', header: 'Area M2', hasTableValue: true, isStandalone: false },
    { field: 'noOfLocations', header: 'No. of Locations', hasTableValue: true, isStandalone: false },
    { field: 'classType', header: 'Classification', hasTableValue: true, isStandalone: false },
    { field: '', header: '', hasTableValue: false, isStandalone: false }

  ]

  showDynamicPopup(roomId: number) {
    if (this.customerDetailId > 0) {
      this.ref = this.dialogService.open(ParticleRoomLocationsThreeCycleComponent, {
        header: 'Rooms and its Locations',
        width: '70%',
        contentStyle: { overflow: 'auto' },
        baseZIndex: 10000,
        maximizable: true,
        data: { customerDetailId: this.customerDetailId, roomId: roomId }
      });
      const dialogRef = this.dialogService.dialogComponentRefMap.get(this.ref);
      dialogRef!.changeDetectorRef.detectChanges();
      this.instance = dialogRef!.instance.componentRef!.instance as ParticleRoomLocationsThreeCycleComponent;
      this.instance.onCloseEventFire.subscribe((e) => {
        this.ref?.close(e);
        this.messageService.add({ key: 'tc', severity: 'success', summary: 'Success', detail: 'Room along with Locations saved', life: 4000 });

      });

      this.ref.onClose.subscribe((input: any) => {
        this.getRoomsFromServer();
      });
    } else {
      this.messageService.add({ key: 'tc', severity: 'warn', summary: 'Warning', detail: 'Please save the Customer First!', life: 4000 });

    }
  }

  ngOnInit() {
    this.route.params.subscribe(params => {
      if (params['id'] !== undefined && params['id'] > 0) {
        this.customerDetailId = params['id'];
        if (this.customerDetailId > 0) {
          this.getRoomsFromServer();
        }
      }
    });
  }

  navigateToUrl = (url: string | undefined) => {
    this.router.navigateByUrl(url!);
  }

  //#region  Event handlers
  onCustomerSave(response: BaseResponse<number>) {
    if (response.succeeded) {
      this.customerDetailId = response.data;
      this.messageService.add({ key: 'tc', severity: 'success', summary: 'Success', detail: 'Customer Details saved', life: 4000 });
    }
  }

  onCustomerError(message: string) {
    this.messageService.add({ key: 'tc', severity: 'error', summary: 'Failed', detail: message, life: 4000 });
  }

  onCustomerInfo(message: string) {
    this.messageService.add({ key: 'tc', severity: 'info', summary: 'Information', detail: message, life: 4000 });
  }

  onRoomPreview(event: Room) {
    this.showDynamicPopup(event.id);
  }

  onRoomDelete(event: Room) {
    this.roomId = event.id;
    this.deleteRoomFromServer();
  }
  onRoomDownload(event: Room) {
    this.roomId = event.id;
    this.getReportFromServer();
  }
  //#endregion

  getRoomsFromServer() {
    const reqparam = new RequestParameter();
    reqparam.filter = `customerDetailId:eq:${this.customerDetailId}`
    this.roomService.getAllPagedResponse(reqparam).subscribe({
      next: (response: BaseResponse<Room[]>) => {
        if (response.succeeded) {
          this.listOfRooms = response.data;
          console.log(this.listOfRooms)
        }
      },
      error: (e) => {
        this.messageService.add({
          key: 'tc', severity: 'error', summary: 'Failed',
          detail: e.status == 0 ? 'Server connection error' : e.error.Message !== undefined ? e.error.Message : e.error.title, life: 4000
        });
      },
      complete: () => { },
    });
  }

  deleteRoomFromServer() {
    this.roomService.deleteRoomById(this.roomId!.toString()).subscribe({
      next: (response: BaseResponse<number>) => {
        if (response.succeeded) {
          this.messageService.add({ key: 'tc', severity: 'success', summary: 'Deleted', detail: "Room deleted", life: 4000 });
        }
      },
      error: (e) => {
        this.messageService.add({
          key: 'tc', severity: 'error', summary: 'Failed',
          detail: e.status == 0 ? 'Server connection error' : e.error.Message !== undefined ? e.error.Message : e.error.title, life: 4000
        });
      },
      complete: () => { },
    });
  }

  getReportFromServer() {
    this.isReportDownloading = true;
    if (this.customerDetailId > 0) {
      this.customerDetailService.getReport(this.customerDetailId.toString(),this.roomId).subscribe({
        next: (response: BaseResponse<any>) => {
          if (response.succeeded) {
          
              const documentFile = this.fileService.base64ToFile(
                response.data.file,
                response.data.fileName,
                'application/vnd.openxmlformats-officedocument.wordprocessingml.document'
              );
              this.fileService.downloadFile(documentFile!);
            }
          },   
        error: (e) => {
          this.messageService.add({
            key: 'tc', severity: 'error', summary: 'Failed',
            detail: e.status == 0 ? 'Server connection error' : e.error.Message !== undefined ? e.error.Message : e.error.title, life: 4000
          });
          this.isReportDownloading = false;
        },
        complete: () => {  this.isReportDownloading = false;}
      });
    }
  }

    ngOnDestroy() {
      if (this.ref) {
        this.ref.close();
      }
      this.instance?.onCloseEventFire.unsubscribe();
    }

  }