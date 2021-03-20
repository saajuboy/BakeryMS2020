import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from '../../environments/environment';
import { PurchaseOrderDetail, PurchaseOrderHeader } from '../_models/purchaseOrder';

@Injectable({
  providedIn: 'root'
})
export class InventoryService {
  baseUrl = environment.apiUrl + '';

  constructor(private http: HttpClient) { }

  getPurchaseOrders(isFromOutlet?: boolean): Observable<PurchaseOrderHeader[]> {
    return this.http.get<PurchaseOrderHeader[]>(this.baseUrl + 'purchaseOrder').pipe(
      map(response => {
        if (isFromOutlet != null) {
          return response.filter(a => a.isFromOutlet === isFromOutlet);
        }
        return response;
      }));

  }
  getPurchaseOrder(id): Observable<PurchaseOrderHeader> {
    return this.http.get<PurchaseOrderHeader>(this.baseUrl + 'purchaseOrder/' + id);
  }
  getPurchaseOrderDetailsOfHeader(id): Observable<PurchaseOrderDetail[]> {
    return this.http.get<PurchaseOrderHeader>(this.baseUrl + 'purchaseOrder/' + id).pipe(
      map(response => {
        return response.poDetail;
      }));
  }
  createPurchaseOrder(purchaseOrder: PurchaseOrderHeader) {
    let params = new HttpParams();
    params = params.append('isForSending', 'false');
    return this.http.post(this.baseUrl + 'purchaseOrder', purchaseOrder);
  }
  createPurchaseOrderAndSend(purchaseOrder: PurchaseOrderHeader) {
    let params = new HttpParams();
    params = params.append('isForSending', 'true');
    return this.http.post(this.baseUrl + 'purchaseOrder', purchaseOrder, { params });
  }
  updatePurchaseOrder(id: number, purchaseOrder: PurchaseOrderHeader) {
    let params = new HttpParams();
    params = params.append('isForSending', 'false');
    return this.http.put(this.baseUrl + 'purchaseOrder/' + id, purchaseOrder);
  }
  updatePurchaseOrderAndSend(id: number, purchaseOrder: PurchaseOrderHeader) {
    let params = new HttpParams();
    params = params.append('isForSending', 'true');
    return this.http.put(this.baseUrl + 'purchaseOrder/' + id, purchaseOrder, { params });
  }
  // patchItem(id: number, item: any) {
  //   return this.http.patch(this.baseUrl + 'items/' + id, item);
  // }
  deletePurchaseOrder(id: number) {
    return this.http.delete(this.baseUrl + 'purchaseOrder/' + id);
  }

}
