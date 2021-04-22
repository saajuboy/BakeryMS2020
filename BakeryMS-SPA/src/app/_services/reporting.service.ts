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
  getSalesReports(reportType: number, range?: number, date?: string, month?: number, year?: number, wildCard?: string) {
    let headers = new HttpHeaders();
    let params = new HttpParams();
    headers = headers.set('Accept', 'application/pdf');

    params = params.append('reportType', reportType.toString());

    if (range) {
      params = params.append('range', range.toString());
    }
    if (date && date != '') {
      params = params.append('date', date);
    }
    if (month) {
      params = params.append('month', month.toString());
    }
    if (year) {
      params = params.append('year', year.toString());
    }
    if (wildCard && wildCard != '') {
      params = params.append('wildCard', wildCard);
    }

    return this.http.get(this.baseUrl + 'Reports/GetSalesReport', { headers: headers, responseType: 'blob', params: params });
  }
  getInventoryReports(reportType: number, range?: number, date?: string, month?: number, year?: number, wildCard?: string) {
    let headers = new HttpHeaders();
    let params = new HttpParams();
    headers = headers.set('Accept', 'application/pdf');

    params = params.append('reportType', reportType.toString());

    if (range) {
      params = params.append('range', range.toString());
    }
    if (date && date != '') {
      params = params.append('date', date);
    }
    if (month) {
      params = params.append('month', month.toString());
    }
    if (year) {
      params = params.append('year', year.toString());
    }
    if (wildCard && wildCard != '') {
      params = params.append('wildCard', wildCard);
    }

    return this.http.get(this.baseUrl + 'Reports/GetInventoryReport', { headers: headers, responseType: 'blob', params: params });
  }

  getManufacturingReports(reportType: number, itemId?: number, wildCard?: string) {
    let headers = new HttpHeaders();
    let params = new HttpParams();
    headers = headers.set('Accept', 'application/pdf');

    params = params.append('reportType', reportType.toString());

    if (itemId) {
      params = params.append('itemId', itemId.toString());
    }
    // if (date && date != '') {
    //   params = params.append('date', date);
    // }
    // if (month) {
    //   params = params.append('month', month.toString());
    // }
    // if (year) {
    //   params = params.append('year', year.toString());
    // }
    if (wildCard && wildCard != '') {
      params = params.append('wildCard', wildCard);
    }

    return this.http.get(this.baseUrl + 'Reports/GetManufacturingReport', { headers: headers, responseType: 'blob', params: params });
  }

}
