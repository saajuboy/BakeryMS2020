import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from '../../environments/environment';
import { BusinessPlace } from '../_models/businessPlace';
import { Item, ItemCategory, ItemForDropdown, Unit } from '../_models/item';
import { Supplier, SupplierForDropdown } from '../_models/supplier';

@Injectable({
  providedIn: 'root'
})
export class MasterService {
  baseUrl = environment.apiUrl + '';

  constructor(private http: HttpClient) { }

  getSuppliers(isForDropdown: boolean, type?: number): Observable<Supplier[]> | Observable<SupplierForDropdown[]> {

    if (isForDropdown === true) {
      return this.http.get<SupplierForDropdown[]>(this.baseUrl + 'suppliers')
        .pipe(
          map(response => {
            if (type != null) {
              return response.filter(a => a.type === type);
            }
            return response;
          })
        );
    } else {
      return this.http.get<Supplier[]>(this.baseUrl + 'suppliers');
    }
  }
  getSupplier(id): Observable<Supplier> {
    return this.http.get<Supplier>(this.baseUrl + 'suppliers/' + id);
  }
  CreateSupplier(supplier: Supplier) {
    return this.http.post(this.baseUrl + 'suppliers', supplier);
  }
  updateSupplier(id: number, supplier: Supplier) {
    return this.http.put(this.baseUrl + 'suppliers/' + id, supplier);
  }
  deleteSupplier(id: number) {
    return this.http.delete(this.baseUrl + 'suppliers/' + id);
  }

  getItems(isForDropdown: boolean, type?: number): Observable<Item[]> | Observable<ItemForDropdown[]> {
    if (isForDropdown === true) {
      return this.http.get<ItemForDropdown[]>(this.baseUrl + 'items').pipe(
        map(response => {
          if (type != null) {
            return response.filter(a => a.type === type);
          }
          return response;
        })
      );
    } else {
      return this.http.get<Item[]>(this.baseUrl + 'items');
    }
  }
  CreateItem(item: Item) {
    return this.http.post(this.baseUrl + 'items', item);
  }
  getItem(id): Observable<Item> {
    return this.http.get<Item>(this.baseUrl + 'items/' + id);
  }
  updateItem(id: number, item: Item) {
    return this.http.put(this.baseUrl + 'items/' + id, item);
  }
  patchItem(id: number, item: any) {
    return this.http.patch(this.baseUrl + 'items/' + id, item);
  }
  deleteItem(id: number) {
    return this.http.delete(this.baseUrl + 'items/' + id);
  }


  getItemCode(id: number) {
    return this.http.get(this.baseUrl + 'itemCategory/getcode/' + id);
  }


  getItemCategories(): Observable<ItemCategory[]> {
    return this.http.get<ItemCategory[]>(this.baseUrl + 'itemCategory');
  }
  getItemCategory(id): Observable<ItemCategory> {
    return this.http.get<ItemCategory>(this.baseUrl + 'itemCategory/' + id);
  }
  CreateItemCategory(itemCategory: ItemCategory) {
    return this.http.post(this.baseUrl + 'itemCategory', itemCategory);
  }
  updateItemCategory(id: number, itemCategory: ItemCategory) {
    return this.http.put(this.baseUrl + 'itemCategory/' + id, itemCategory);
  }
  deleteItemCategory(id: number) {
    return this.http.delete(this.baseUrl + 'itemCategory/' + id);
  }


  getUnits(): Observable<Unit[]> {
    return this.http.get<Unit[]>(this.baseUrl + 'units');
  }
  getUnit(id): Observable<Unit> {
    return this.http.get<Unit>(this.baseUrl + 'units/' + id);
  }
  CreateUnit(unit: Unit) {
    return this.http.post(this.baseUrl + 'units', unit);
  }
  updateUnit(id: number, unit: Unit) {
    return this.http.put(this.baseUrl + 'units/' + id, unit);
  }
  deleteUnit(id: number) {
    return this.http.delete(this.baseUrl + 'units/' + id);
  }


  getBusinessPlaces(): Observable<BusinessPlace[]> {
    return this.http.get<BusinessPlace[]>(this.baseUrl + 'businessPlaces');
  }
  getBusinessPlace(id): Observable<BusinessPlace> {
    return this.http.get<BusinessPlace>(this.baseUrl + 'businessPlaces/' + id);
  }
}
