import { Observable } from 'rxjs';
import { Instrument } from 'src/Models/instrument.Model';
import { BaseHttpClientServiceService } from './Shared/base-http-client-service.service';
import { Injectable } from '@angular/core';
import { BaseResponse } from 'src/Models/response-models/base-response';
import { RequestParameter } from 'src/Models/request-parameter';

@Injectable({
  providedIn: 'root'
})
export class InstrumentService {

  constructor(private httpService: BaseHttpClientServiceService) {
    httpService.mapURLPath('Instrument')
  }

  postInstrument(data: any): Observable<BaseResponse<number>> {
    console.warn(("postInstrument services method started"));

    return this.httpService.post<any, number>(data)
  }

  getInstrumentById(id: string): Observable<BaseResponse<Instrument>> {
    return this.httpService.getById<Instrument>(id);
  }

  getAllPagedResponse(reqParams?: RequestParameter): Observable<BaseResponse<Instrument[]>> {
    reqParams = reqParams !== undefined ? reqParams! : new RequestParameter();
    return this.httpService.getAll<Instrument[]>(reqParams);
  }

  deleteInstrumentById(id: string): Observable<BaseResponse<number>> {
    return this.httpService.deleteById<number>(id);
  }

  updateInstrument(id: string, data: Instrument): Observable<BaseResponse<number>> {
    return this.httpService.put<Instrument, number>(id, data)
  }
}
