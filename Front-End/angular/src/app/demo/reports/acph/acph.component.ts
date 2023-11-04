/* eslint-disable @angular-eslint/no-empty-lifecycle-method */
import { CUSTOM_ELEMENTS_SCHEMA, Component, OnInit } from '@angular/core';
import { CustomerDetailsComponent } from '../shared/customer-details/customer-details.component';
import { CommonModule } from '@angular/common';
import { SharedModule } from 'src/app/theme/shared/shared.module';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { MessagesModule } from 'primeng/messages';
import { HttpClientModule } from '@angular/common/http';
import { ToastModule } from 'primeng/toast';
import { BaseHttpClientServiceService } from 'src/Services/Shared/base-http-client-service.service';
import { ConfirmationService, MessageService } from 'primeng/api';
import { Router } from '@angular/router';

@Component({
  selector: 'app-acph',
  standalone: true,
  imports: [CommonModule, SharedModule, ConfirmDialogModule, MessagesModule, HttpClientModule, ToastModule, CustomerDetailsComponent],
  providers: [ConfirmationService, BaseHttpClientServiceService, MessageService],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  templateUrl: './acph.component.html',
  styleUrls: ['./acph.component.css']
})
export class AcphComponent implements OnInit {

  constructor(private router:Router) { }

  ngOnInit() {
  }
  navigateToUrl = (url: string | undefined) => {
     this.router.navigateByUrl(url!);
  }

}
