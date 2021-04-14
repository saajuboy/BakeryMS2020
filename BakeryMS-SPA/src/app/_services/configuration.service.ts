import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from '../../environments/environment';
import { Configuration, ConfigurationList, Notification } from '../_models/configuration';

@Injectable({
  providedIn: 'root'
})
export class ConfigurationService {
  baseUrl = environment.apiUrl + '';

  constructor(private http: HttpClient) { }

  getConfigurations(): Observable<Configuration[]> {
    return this.http.get<Configuration[]>(this.baseUrl + 'configurations').pipe(
      map((response: Configuration[]) => {
        const configs = response;
        if (configs) {
          localStorage.setItem('BusinessPlaceId', configs.find(x => x.description === 'BusinessPlace').value);
          // this.decodedToken = this.jwtHelper.decodeToken(user.token);
          // console.log(this.decodedToken);
        }
        return response;
      })
    );
  }
  updateConfigurations(configs: ConfigurationList) {
    return this.http.post(this.baseUrl + 'configurations/updateConfig', configs);
  }

  getNotifications(): Observable<Notification[]> {
    return this.http.get<Notification[]>(this.baseUrl + 'notifications');
  }
  getNotification(id): Observable<Notification> {
    return this.http.get<Notification>(this.baseUrl + 'notifications/' + id);
  }
  getRecentNotification(): Observable<Notification[]> {
    return this.http.get<Notification[]>(this.baseUrl + 'notifications/GetRecentNotifications');
  }
}
