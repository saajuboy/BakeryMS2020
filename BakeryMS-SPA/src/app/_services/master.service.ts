import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Item, ItemForDropdown } from '../_models/item';
import { Supplier, SupplierForDropdown } from '../_models/supplier';

@Injectable({
  providedIn: 'root'
})
export class MasterService {
  baseUrl = environment.apiUrl + '';

  constructor(private http: HttpClient) { }

  getSuppliers(isForDropdown: boolean): Observable<Supplier[]> | Observable<SupplierForDropdown[]> {

    if (isForDropdown === true) {
      return this.http.get<SupplierForDropdown[]>(this.baseUrl + 'suppliers');
    } else {
      return this.http.get<Supplier[]>(this.baseUrl + 'suppliers');
    }


  }
  getSupplier(id): Observable<Supplier> {
    return this.http.get<Supplier>(this.baseUrl + 'users/' + id);
  }

  getItems(isForDropdown: boolean): Observable<Item[]> | Observable<ItemForDropdown[]> {

    if (isForDropdown === true) {
      return this.http.get<ItemForDropdown[]>(this.baseUrl + 'items');
    } else {
      return this.http.get<Item[]>(this.baseUrl + 'items');
    }


  }
  getItem(id): Observable<Item> {
    return this.http.get<Item>(this.baseUrl + 'items/' + id);
  }

}
