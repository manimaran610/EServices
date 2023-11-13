import { Observable } from 'rxjs';
import { BaseHttpClientServiceService } from './Shared/base-http-client-service.service';
import { Injectable } from '@angular/core';
import { BaseResponse } from 'src/Models/response-models/base-response';
import { RequestParameter } from 'src/Models/request-parameter';
import { Room } from 'src/Models/room.Model';

@Injectable({
  providedIn: 'root'
})
export class RoomService {

  constructor(private httpService: BaseHttpClientServiceService) {
    httpService.mapURLPath('Room')
  }

  postRoom(data: Room): Observable<BaseResponse<number>> {
    return this.httpService.post<Room, number>(data)
  }

  getRoomById(id: string): Observable<BaseResponse<Room>> {
    return this.httpService.getById<Room>(id);
  }

  getAllPagedResponse(reqParams?: RequestParameter): Observable<BaseResponse<Room[]>> {
    reqParams = reqParams !== undefined ? reqParams! : new RequestParameter();
    return this.httpService.getAll<Room[]>(reqParams);
  }

  deleteRoomById(id: string): Observable<BaseResponse<number>> {
    return this.httpService.deleteById<number>(id);
  }

  updateRoom(id: string, data: Room): Observable<BaseResponse<number>> {
    return this.httpService.put<Room, number>(id, data)
  }
}
