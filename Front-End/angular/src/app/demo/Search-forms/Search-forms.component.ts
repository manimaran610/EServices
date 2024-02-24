import { MessagesModule } from 'primeng/messages';
import { RequestParameter } from 'src/Models/request-parameter';
import { CustomerDetail } from './../../../Models/customer-detail.Model';
import { BaseHttpClientServiceService } from 'src/Services/Shared/base-http-client-service.service';
import { CustomerDetailService } from 'src/Services/cutomer-detail.service';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { GridColumnOptions } from 'src/Models/grid-column-options';
import { SharedModule } from 'src/app/theme/shared/shared.module';
import { BaseResponse } from 'src/Models/response-models/base-response';
import { MessageService } from 'primeng/api';
import { Router } from '@angular/router';
import { FormControl, FormGroup } from '@angular/forms';

@Component({
  selector: 'app-search-forms',
  standalone: true,
  imports: [CommonModule, SharedModule,MessagesModule,HttpClientModule],
  providers: [
    CustomerDetailService,
    BaseHttpClientServiceService,
    MessageService
  ],
  templateUrl: './Search-forms.component.html',
  styleUrls: ['./Search-forms.component.css']
})
export class SearchFormsComponent implements OnInit {
  listOfCustomerDetails:CustomerDetail[] =[];
  totalCount:number=0
  isSearching:boolean = false;
 searchFormGroup: FormGroup;
  constructor(  
    private customerDetailService: CustomerDetailService,
    private messageService: MessageService,
    private router: Router) { 
      this.searchFormGroup = new FormGroup({
        customerNo: new FormControl(),
        client: new FormControl(),
        fromDate: new FormControl(),
        toDate: new FormControl()
      });

    }

  ngOnInit() {
    console.log("search-forms-componenet");
    let reqParam =new RequestParameter();
    reqParam.count=100;
    this.getCustomerDetailFromServer(reqParam)
  }
 options:GridColumnOptions[]=[
  {field: 'customerNo', header: 'Customer No', isSortable: true, hasFilter: true,hasTableValue:true},
    {field: 'client', header: 'Client Name', isSortable: true, hasFilter: true,hasTableValue:true},
    {field: 'formTypeName', header: 'Report Type',  isSortable: true,hasFilter: true,hasTableValue:true},
    {field: 'dateOfTest', header: 'Date Tested', isSortable: true, hasFilter: true,hasTableValue:true}

 ]


onSubmit(){
  this.isSearching=true;
  const filterStrings : string[]= [];
  if(this.searchFormGroup.value.customerNo !== null){
      filterStrings.push('CustomerNo:con:' + this.searchFormGroup.value.customerNo);
  }
  if(this.searchFormGroup.value.client !== null){
    filterStrings.push('Client:con:' + this.searchFormGroup.value.client);
}
if(this.searchFormGroup.value.fromDate !== null){
  filterStrings.push( 'DateOfTest:gte:' + this.searchFormGroup.value.fromDate);
}
if(this.searchFormGroup.value.toDate !== null){
  filterStrings.push('DateOfTest:lte:' + this.searchFormGroup.value.toDate);
}

  const reqParam =new RequestParameter();
  reqParam.filter=filterStrings.join(',');
  reqParam.count = 5000;
  this.getCustomerDetailFromServer(reqParam)
}

onClear() { this.searchFormGroup.reset() }

  getCustomerDetailFromServer(reqParam?:RequestParameter) {
    this.customerDetailService.getAllPagedResponse(reqParam).subscribe({
      next: (response: BaseResponse<CustomerDetail[]>) => {
        if (response.succeeded) {
          this.listOfCustomerDetails = response.data;
          this.totalCount=response.totalCount;
        }
      },
      error: (e) => {
        this.messageService.add({ key: 'tc', severity: 'error', summary: 'Failed',
        detail: e.status ==0? 'Server connection error': e.error.Message !== undefined ? e.error.Message : e.error.title, life: 4000 });
      this.isSearching=false;
      },
      complete: () => { this.isSearching=false },
    });
  }

  onLazyLoad(event:any){
    console.log('on-Lazy-load-method-started')
    console.log(event);
    let reqParam =new RequestParameter();
    reqParam.offset=event.offset;
    reqParam.count=event.count;
    if(event.filter !== undefined) reqParam.filter=event.filter;
    if(event.sort !== undefined) reqParam.sort=event.sort;
    this.getCustomerDetailFromServer(reqParam);
  }

  onCustomerPreview(event:CustomerDetail){
     const formRoutePath = this.getFormRoutePathByFormTypeId(event.formType,event.id);
     this.navigateToUrl(formRoutePath) 
  }

  navigateToUrl = (url: string | undefined) => this.router.navigateByUrl(url!);

   getFormRoutePathByFormTypeId(typeId: number,customerDetailId:number): string {
    switch (typeId) {
      case 1:
        return `/Reports/ACPH/${customerDetailId}`;
      case 2:
        return `/Reports/FilterIntegrity/${customerDetailId}`;
      case 3:
        return `/Reports/ParticleCount/SingleCycle/${customerDetailId}`;
      case 4:
        return `/Reports/ParticleCount/ThreeCycle/${customerDetailId}`;
      case 5:
        return 'TempMapping';
      case 6:
        return `/Reports/ParticleCount/RecvCycle/${customerDetailId}`;
      case 7:
          return `/Reports/ACPH-Hoot/${customerDetailId}`;
      default:
        return 'NotSupported';
    }
  }
}
