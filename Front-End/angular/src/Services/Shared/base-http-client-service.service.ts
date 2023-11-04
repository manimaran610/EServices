import { BaseResponse } from './../../Models/response-models/base-response';
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { RequestParameter } from 'src/Models/request-parameter';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class BaseHttpClientServiceService {

  private url?: string;

  constructor(private http: HttpClient) {
    this.url = environment.apiUrl + '/' + environment.appVersion + '/'
  }

  mapURLPath(path: string) {
    this.url += path;
  }

  public get populateHttpHeaders(): HttpHeaders {
    return new HttpHeaders({
      // Authorization: `Bearer ${this.authentication.getAccessToken()}`,
      'Content-Type': 'application/json',
      
    });
  }

  public setHttpParams(reqParam: RequestParameter) {
    console.log(reqParam)
    return new HttpParams()
      .set('offset', reqParam.offset!)
      .set('count', reqParam.count!)
      .set('Sort', reqParam.sort!)
      .set('Filter', reqParam.filter!)
  }


  public getAll<T>(headerParam: RequestParameter): Observable<BaseResponse<T>> {
    const headers = this.populateHttpHeaders;
    const params = this.setHttpParams(headerParam);
    if (headerParam.filter !== undefined && headerParam.filter !== '' && headerParam.filter != null) {
      params.set('filter', headerParam.filter!);
    }
    return this.http.get<BaseResponse<T>>(`${this.url}`, {
      headers: headers,
      params: params
    });
  }

  public getById<T>(id?: string): Observable<BaseResponse<T>> {
    const headers = this.populateHttpHeaders;
    return this.http.get<BaseResponse<T>>(`${this.url}/${id}`, {
      headers: headers,
    });
  }

  public post<Req, Res>(request: Req): Observable<BaseResponse<Res>> {
    console.log("base service post started")
    const headers = this.populateHttpHeaders;
    return this.http.post<BaseResponse<Res>>(`${this.url}`, request, {
      headers: headers,
    });
  }

  public put<Req, Res>(id?: string, request?: Req): Observable<BaseResponse<Res>> {
    const headers = this.populateHttpHeaders;
    return this.http.put<BaseResponse<Res>>(`${this.url}/${id}`, request, {
      headers: headers
    });
  }

  public searchByHttpParams<Res>(httpParams: HttpParams): Observable<BaseResponse<Res>> {
    const headers = this.populateHttpHeaders;
    headers.set('Content-Type', 'application/json');
    return this.http.get<BaseResponse<Res>>(`${this.url}`, {
      headers: headers,
      params: httpParams,
    });
  }

  public deleteById<Res>(id?: string,): Observable<BaseResponse<Res>> {
    const headers = this.populateHttpHeaders;
    return this.http.delete<BaseResponse<Res>>(`${this.url}/${id}`, { headers: headers });
  }
}

