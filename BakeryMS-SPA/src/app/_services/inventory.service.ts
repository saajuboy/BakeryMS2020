import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from '../../environments/environment';
import { AvailableItemForList } from '../_models/availableItems';
import { GRNHeader, PurchaseOrderDetail, PurchaseOrderHeader } from '../_models/purchaseOrder';

@Injectable({
  providedIn: 'root'
})
export class InventoryService {
  baseUrl = environment.apiUrl + '';

  constructor(private http: HttpClient) { }

  getPurchaseOrders(isFromOutlet?: boolean, status?: number): Observable<PurchaseOrderHeader[]> {
    return this.http.get<PurchaseOrderHeader[]>(this.baseUrl + 'purchaseOrder').pipe(
      map(response => {
        let responseToReturn = response;
        if (isFromOutlet != null) {
          responseToReturn = responseToReturn.filter(a => a.isFromOutlet === isFromOutlet);
        }
        if (status != null) {
          responseToReturn = responseToReturn.filter(a => a.status === status);
        }
        return responseToReturn;
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
  getReorderPurchaseOrder(placeId, type): Observable<PurchaseOrderHeader> {
    let params = new HttpParams();
    params = params.append('placeId', placeId.toString());
    params = params.append('type', type.toString());
    return this.http.get<PurchaseOrderHeader>(this.baseUrl + 'purchaseOrder/GetReorderPurchaseOrder', { params });
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

  getGRN(id): Observable<GRNHeader> {
    return this.http.get<GRNHeader>(this.baseUrl + 'GRN/' + id);
  }
  createGRN(grn: GRNHeader) {
    return this.http.post(this.baseUrl + 'GRN', grn);
  }
  payGRN(grn: GRNHeader) {
    return this.http.post(this.baseUrl + 'GRN/PayDueGRNAmount', grn);
  }


  getAvailableItem(id: number, itemType: number): Observable<AvailableItemForList> {
    let params = new HttpParams();
    params = params.append('itemType', itemType.toString());
    return this.http.get<AvailableItemForList>(this.baseUrl + 'availableItems/' + id, { params });
  }
  getAvailableItems(placeId: number, itemType: number): Observable<AvailableItemForList[]> {
    let params = new HttpParams();
    params = params.append('placeId', placeId.toString());
    params = params.append('itemType', itemType.toString());
    return this.http.get<AvailableItemForList[]>(this.baseUrl + 'availableItems', { params });
  }
  getAvailableItemsForPOS(placeId: number, filter: number): Observable<AvailableItemForList[]> {
    let params = new HttpParams();
    params = params.append('placeId', placeId.toString());
    params = params.append('filter', filter.toString());
    return this.http.get<AvailableItemForList[]>(this.baseUrl + 'availableItems/GetAvailableItemsForPOS', { params });
  }
  getReorderItems(placeId: number, itemType: number): Observable<AvailableItemForList[]> {
    let params = new HttpParams();
    params = params.append('placeId', placeId.toString());
    params = params.append('itemType', itemType.toString());
    return this.http.get<AvailableItemForList[]>(this.baseUrl + 'availableItems/getReorderItems', { params });
  }

}
