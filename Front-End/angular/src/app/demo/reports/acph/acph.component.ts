/* eslint-disable @angular-eslint/no-empty-lifecycle-method */
import { CUSTOM_ELEMENTS_SCHEMA, Component, OnDestroy, OnInit } from '@angular/core';
import { CustomerDetailsComponent } from '../shared/customer-details/customer-details.component';
import { CommonModule } from '@angular/common';
import { SharedModule } from 'src/app/theme/shared/shared.module';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { DialogService, DynamicDialogModule, DynamicDialogRef } from 'primeng/dynamicdialog';
import { MessagesModule } from 'primeng/messages';
import { HttpClientModule } from '@angular/common/http';
import { ToastModule } from 'primeng/toast';
import { BaseHttpClientServiceService } from 'src/Services/Shared/base-http-client-service.service';
import { ConfirmationService, MessageService } from 'primeng/api';
import { Router } from '@angular/router';
import { AcphRoomGrillsComponent } from './acph-room-grills/acph-room-grills.component';
import { BaseResponse } from 'src/Models/response-models/base-response';
import { GridComponent } from 'src/app/theme/shared/components/grid/grid.component';
import { Room } from 'src/Models/room.Model';
import { GridColumnOptions } from 'src/Models/grid-column-options';
import { RoomService } from 'src/Services/room.service';
import { RequestParameter } from 'src/Models/request-parameter';

@Component({
  selector: 'app-acph',
  standalone: true,
  imports: [CommonModule, SharedModule, ConfirmDialogModule, MessagesModule,
    HttpClientModule, ToastModule, CustomerDetailsComponent, AcphRoomGrillsComponent,
    DynamicDialogModule, GridComponent],
  providers: [ConfirmationService, BaseHttpClientServiceService, MessageService, DialogService, RoomService],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  templateUrl: './acph.component.html',
  styleUrls: ['./acph.component.css']
})
export class AcphComponent implements OnDestroy, OnInit {
  ref: DynamicDialogRef | undefined;
  instance?: AcphRoomGrillsComponent;
  customerDetailId: number = 0;
  roomId?: number;
  listOfRooms: Room[] = [];

  constructor(private router: Router, public dialogService: DialogService, private messageService: MessageService, private roomService: RoomService) { }

  gridColumnOptions: GridColumnOptions[] = [
    { field: 'name', header: 'Room Name', isSortable: true, hasTableValue: true, isStandalone: false },
    { field: 'designACPH', header: 'Design ACPH', hasTableValue: true, isStandalone: false },
    { field: 'noOfGrills', header: 'No. of Grills', hasTableValue: true, isStandalone: false },
    { field: 'roomVolume', header: 'Room Volume', hasTableValue: true, isStandalone: false },
    { field: 'totalAirFlowCFM', header: 'Total AirFlow CFM', hasTableValue: true, isStandalone: false },
    { field: 'airChangesPerHour', header: 'Air Changes per hour', hasTableValue: true, isStandalone: false }
  ]

  show() {
    if (this.customerDetailId > 0) {
      this.ref = this.dialogService.open(AcphRoomGrillsComponent, {
        header: 'Rooms and Grills',
        width: '70%',
        contentStyle: { overflow: 'auto' },
        baseZIndex: 10000,
        maximizable: true,
        data: { customerDetailId: this.customerDetailId, roomId: this.roomId }
      });
      const dialogRef = this.dialogService.dialogComponentRefMap.get(this.ref);
      dialogRef!.changeDetectorRef.detectChanges();
      this.instance = dialogRef!.instance.componentRef!.instance as AcphRoomGrillsComponent;
      this.instance.onCloseEventFire.subscribe((e) => {
        this.ref?.close(e);
        this.messageService.add({ key: 'tc', severity: 'success', summary: 'Success', detail: 'Room along with Grills saved', life: 4000 });

      });

      this.ref.onClose.subscribe((input: any) => {
        this.getRoomsFromServer();
      });
    } else {
      this.messageService.add({ key: 'tc', severity: 'warn', summary: 'Warning', detail: 'Please save the Customer First!', life: 4000 });

    }
  }

  ngOnInit() { }

  navigateToUrl = (url: string | undefined) => {
    this.router.navigateByUrl(url!);
  }
  onCustomerSave(response: BaseResponse<number>) {
    if (response.succeeded) {
      this.customerDetailId = response.data;
      this.messageService.add({ key: 'tc', severity: 'success', summary: 'Success', detail: 'Customer Details saved', life: 4000 });
    }

  }

  onCustomerError(message: string) {
    this.customerDetailId = 0;
    this.messageService.add({ key: 'tc', severity: 'error', summary: 'Failed', detail: message, life: 4000 });
  }

  getRoomsFromServer() {
    let reqparam = new RequestParameter();
    reqparam.filter = `customerDetailId:eq:${this.customerDetailId}`
    this.roomService.getAllPagedResponse(reqparam).subscribe({
      next: (response: BaseResponse<Room[]>) => {
        if (response.succeeded) { this.listOfRooms = response.data; }
      },
      error: (e) => { console.error(e.error) },
      complete: () => { },
    });
  }


  ngOnDestroy() {
    if (this.ref) {
      this.ref.close();
    }
    this.instance?.onCloseEventFire.unsubscribe();
  }

}
