/* eslint-disable @angular-eslint/no-empty-lifecycle-method */
import { CUSTOM_ELEMENTS_SCHEMA, Component, OnInit } from '@angular/core';
import { CustomerDetailsComponent } from '../shared/customer-details/customer-details.component';
import { CommonModule } from '@angular/common';
import { SharedModule } from 'src/app/theme/shared/shared.module';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { DialogService, DynamicDialogModule,DynamicDialogRef  } from 'primeng/dynamicdialog';
import { MessagesModule } from 'primeng/messages';
import { HttpClientModule } from '@angular/common/http';
import { ToastModule } from 'primeng/toast';
import { BaseHttpClientServiceService } from 'src/Services/Shared/base-http-client-service.service';
import { ConfirmationService, MessageService } from 'primeng/api';
import { Router } from '@angular/router';

@Component({
  selector: 'app-acph',
  standalone: true,
  imports: [CommonModule, SharedModule, ConfirmDialogModule, MessagesModule, HttpClientModule, ToastModule, CustomerDetailsComponent,DynamicDialogModule],
  providers: [ConfirmationService, BaseHttpClientServiceService, MessageService,DialogService],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  templateUrl: './acph.component.html',
  styleUrls: ['./acph.component.css']
})
export class AcphComponent implements OnInit {
  ref: DynamicDialogRef | undefined;

  constructor(private router:Router,public dialogService: DialogService) { }

  show() {
    this.ref = this.dialogService.open(CustomerDetailsComponent, {
      header: 'Select a Product',
      width: '70%',
      contentStyle: { overflow: 'auto' },
      baseZIndex: 10000,
      maximizable: false
  });
  }

  ngOnInit() {
  }
  navigateToUrl = (url: string | undefined) => {
     this.router.navigateByUrl(url!);
  }
  

}
