import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { SalesHeader, SalesHeaderForPos, Transaction } from '../_models/availableItems';

@Injectable({
  providedIn: 'root'
})
export class PosService {
  baseUrl = environment.apiUrl + '';

  constructor(private http: HttpClient) { }

  getSale(id): Observable<SalesHeader> {
    return this.http.get<SalesHeader>(this.baseUrl + 'pointOfSales/' + id);
  }
  getSales(placeId, fromDate, toDate): Observable<SalesHeader[]> {
    let params = new HttpParams();
    params = params.append('placeId', placeId.toString());
    params = params.append('fromDate', fromDate.toString());
    params = params.append('toDate', toDate.toString());
    return this.http.get<SalesHeader[]>(this.baseUrl + 'pointOfSales', { params });
  }
  createSales(sales: SalesHeader): Observable<SalesHeader> {
    // let params = new HttpParams();
    // params = params.append('isForSending', 'false');
    return this.http.post<SalesHeader>(this.baseUrl + 'pointOfSales', sales);
  }

  getTransaction(id): Observable<Transaction> {
    return this.http.get<Transaction>(this.baseUrl + 'transactions/' + id);
  }
  getTransactions(placeId, fromDate, toDate): Observable<Transaction[]> {
    let params = new HttpParams();
    params = params.append('placeId', placeId.toString());
    params = params.append('fromDate', fromDate.toString());
    params = params.append('toDate', toDate.toString());
    return this.http.get<Transaction[]>(this.baseUrl + 'transactions', { params });
  }
  createTransaction(trans: Transaction): Observable<Transaction> {
    // let params = new HttpParams();
    // params = params.append('isForSending', 'false');
    return this.http.post<Transaction>(this.baseUrl + 'transactions', trans);
  }

}
