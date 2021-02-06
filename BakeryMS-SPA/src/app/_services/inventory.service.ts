import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class InventoryService {
  baseUrl = environment.apiUrl + '';

  constructor(private http: HttpClient) { }

}
