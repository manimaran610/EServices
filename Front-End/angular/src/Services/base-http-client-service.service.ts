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
    });
  }

  public setHttpParams(reqParam: RequestParameter) {
    return new HttpParams()
      .set('PageNumber', reqParam.pageNumber!)
      .set('PageSize', reqParam.pageSize!)
      .set('Sort', reqParam.sort!)
      .set('Filter', reqParam.filter!)
  }


  public get<T>(headerParam: RequestParameter): Observable<T> {
    const headers = this.populateHttpHeaders;
    const params = this.setHttpParams(headerParam);
    if (headerParam.filter !== undefined && headerParam.filter !== '' && headerParam.filter != null) {
      params.set('filter', headerParam.filter!);
    }
    return this.http.get<T>(`${this.url}`, {
      headers: headers,
      params: params
    });
  }

  public getById<T>(id?: string): Observable<T> {
    const headers = this.populateHttpHeaders;
    return this.http.get<T>(`${this.url}/${id}`, {
      headers: headers,
    });
  }

  public post<Req, Res>(request: Req): Observable<Res> {
    const headers = this.populateHttpHeaders;
    return this.http.post<Res>(`${this.url}`, request, {
      headers: headers,
    });
  }

  public put<Req, Res>(id?: string, request?: Req): Observable<Res> {
    const headers = this.populateHttpHeaders;
    return this.http.put<Res>(`${this.url}/${id}`, request, {
      headers: headers
    });
  }

  public getpagedSearch<Res>(httpParams: HttpParams): Observable<Res> {
    const headers = this.populateHttpHeaders;
    headers.set('Content-Type', 'application/json');
    return this.http.get<Res>(`${this.url}`, {
      headers: headers,
      params: httpParams,
    });
  }

  public delete<Response>(id?: string,): Observable<Response> {
    const headers = this.populateHttpHeaders;
    return this.http.delete<Response>(`${this.url}/${id}`, { headers: headers });
  }
}

