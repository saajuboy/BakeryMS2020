import { HttpClient } from '@angular/common/http';
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

  getPurchaseOrders(status?: boolean): Observable<PurchaseOrderHeader[]> {
    return this.http.get<PurchaseOrderHeader[]>(this.baseUrl + 'purchaseOrder').pipe(
      map(response => {
        if (status != null) {
          return response.filter(a => a.status === status);
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
  CreatePurchaseOrder(purchaseOrder: PurchaseOrderHeader) {
    return this.http.post(this.baseUrl + 'purchaseOrder', purchaseOrder);
  }
  // updateItem(id: number, item: Item) {
  //   return this.http.put(this.baseUrl + 'items/' + id, item);
  // }
  // patchItem(id: number, item: any) {
  //   return this.http.patch(this.baseUrl + 'items/' + id, item);
  // }
  // deleteItem(id: number) {
  //   return this.http.delete(this.baseUrl + 'items/' + id);
  // }

}
