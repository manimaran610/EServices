/* eslint-disable @angular-eslint/no-empty-lifecycle-method */
import { CUSTOM_ELEMENTS_SCHEMA, Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from 'src/app/theme/shared/shared.module';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { DialogService, DynamicDialogModule } from 'primeng/dynamicdialog';
import { MessagesModule } from 'primeng/messages';
import { HttpClientModule } from '@angular/common/http';
import { ToastModule } from 'primeng/toast';
import { BaseHttpClientServiceService } from 'src/Services/Shared/base-http-client-service.service';
import { ConfirmationService, MessageService } from 'primeng/api';
import { BaseResponse } from 'src/Models/response-models/base-response';
import { GridComponent } from 'src/app/theme/shared/components/grid/grid.component';
import { Room } from 'src/Models/room.Model';
import { RoomService } from 'src/Services/room.service';
import { ActivatedRoute, Router } from '@angular/router';
import { CustomerDetailsComponent } from '../shared/Components/customer-details/customer-details.component';

@Component({
  selector: 'app-temp-mapping',
  standalone: true,
  imports: [CommonModule, SharedModule, ConfirmDialogModule, MessagesModule,
    HttpClientModule, ToastModule, CustomerDetailsComponent,
    DynamicDialogModule, GridComponent],
  providers: [ConfirmationService, BaseHttpClientServiceService, MessageService, DialogService, RoomService],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  templateUrl: './temp-mapping.component.html',
  styleUrls: ['./temp-mapping.component.css']
})
export class TempMappingComponent implements OnInit {

  customerDetailId: number = 0;
  roomId?: number;
  listOfRooms: Room[] = [];

  constructor(private router: Router, private route: ActivatedRoute, public dialogService: DialogService, private messageService: MessageService, private roomService: RoomService) { }


  ngOnInit() {
    this.route.params.subscribe(params => {
      if (params['id'] !== undefined && params['id'] > 0) {
        this.customerDetailId = params['id'];
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
  data: any[] = [
    // Your dynamic data goes here
    { name: 'John', age: 30, city: 'New York' },
    { name: 'Jane', age: 25, city: 'San Francisco' },
    // Add more rows as needed
  ];



}
