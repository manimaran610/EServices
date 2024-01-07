import { Observable } from 'rxjs';
import { BaseHttpClientServiceService } from './Shared/base-http-client-service.service';
import { Injectable } from '@angular/core';
import { BaseResponse } from 'src/Models/response-models/base-response';
import { RequestParameter } from 'src/Models/request-parameter';
import { Trainee } from 'src/Models/trainee.Model';

@Injectable({
  providedIn: 'root'
})
export class TraineeService {
  constructor(private httpService: BaseHttpClientServiceService) {}

  postTrainee(data: Trainee): Observable<BaseResponse<number>> {
    this.httpService.mapURLPath('Trainee');
    return this.httpService.post<Trainee, number>(data);
  }

  getTraineeById(id: string): Observable<BaseResponse<Trainee>> {
    this.httpService.mapURLPath('Trainee');
    return this.httpService.getById<Trainee>(id);
  }

  getAllPagedResponse(reqParams?: RequestParameter): Observable<BaseResponse<Trainee[]>> {
    this.httpService.mapURLPath('Trainee');

    reqParams = reqParams !== undefined ? reqParams! : new RequestParameter();
    return this.httpService.getAll<Trainee[]>(reqParams);
  }

  deleteTraineeById(id: string): Observable<BaseResponse<number>> {
    this.httpService.mapURLPath('Trainee');
    return this.httpService.deleteById<number>(id);
  }

  updateTrainee(id: string, data: Trainee): Observable<BaseResponse<number>> {
    this.httpService.mapURLPath('Trainee');
    return this.httpService.put<Trainee, number>(id, data);
  }
}
