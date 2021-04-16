import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ReportingService {
  baseUrl = environment.apiUrl + '';

  constructor(private http: HttpClient) {

  }

  // master
  getMasterReports(reportType: number, itemType?: number, wildCard?: string) {
    // return this.http.get<User[]>(this.baseUrl + 'users', httpOptions);
    // return this.http.get<User[]>(this.baseUrl + 'users');
    let headers = new HttpHeaders();
    let params = new HttpParams();
    headers = headers.set('Accept', 'application/pdf');

    params = params.append('reportType', reportType.toString());
    if (itemType && itemType != 4) {
      params = params.append('itemType', itemType.toString());
    }
    if (wildCard && wildCard != '') {
      params = params.append('wildCard', wildCard);
    }

    return this.http.get(this.baseUrl + 'Reports/GetMasterReport', { headers: headers, responseType: 'blob', params: params });
  }

}
