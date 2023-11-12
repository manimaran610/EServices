import { RequestParameter } from './../Models/request-parameter';
import { CustomerDetail } from './../Models/customer-detail.Model';
import { Observable } from 'rxjs';
import { BaseHttpClientServiceService } from './Shared/base-http-client-service.service';
import { Injectable } from '@angular/core';
import { BaseResponse } from '../Models/response-models/base-response';


@Injectable({
  providedIn: 'root'
})
export class CustomerDetailService {

  constructor(private httpService: BaseHttpClientServiceService) {
    // httpService.mapURLPath('CustomerDetail')
  }

  mapURLpath(){
    this.httpService.mapURLPath('CustomerDetail')
  }
  postCustomerDetail(data: CustomerDetail): Observable<BaseResponse<number>> {
    this.mapURLpath();
    return this.httpService.post<CustomerDetail, number>(data)
  }

  getCustomerDetailById(id: string): Observable<BaseResponse<CustomerDetail>> {
    this.mapURLpath();

    return this.httpService.getById<CustomerDetail>(id);
  }

  getAllPagedResponse(reqParams?: RequestParameter): Observable<BaseResponse<CustomerDetail[]>> {
    this.mapURLpath();

    reqParams = reqParams !== undefined ? reqParams! : new RequestParameter();
    return this.httpService.getAll<CustomerDetail[]>(reqParams);
  }

  deleteCustomerDetailById(id: string): Observable<BaseResponse<number>> {
    this.mapURLpath();

    return this.httpService.deleteById<number>(id);
  }

  updateCustomerDetail(id: string, data: CustomerDetail): Observable<BaseResponse<number>> {
    this.mapURLpath();

    return this.httpService.put<CustomerDetail, number>(id, data)
  }
}
