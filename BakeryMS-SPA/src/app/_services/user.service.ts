import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Role, RoleList, User } from '../_models/User';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  baseUrl = environment.apiUrl + '';

  constructor(private http: HttpClient) { }

  getUsers(): Observable<User[]> {
    // return this.http.get<User[]>(this.baseUrl + 'users', httpOptions);
    return this.http.get<User[]>(this.baseUrl + 'users');
  }

  getUser(id): Observable<User> {
    return this.http.get<User>(this.baseUrl + 'users/' + id);
    // return this.http.get<User>(this.baseUrl + 'users/' + id, httpOptions);
  }

  updateUser(id: number, user: User) {
    return this.http.put(this.baseUrl + 'users/' + id, user);
  }
  patchUser(id: number, user: any) {
    return this.http.patch(this.baseUrl + 'users/' + id, user);
  }
  deleteUser(id: number) {
    return this.http.delete(this.baseUrl + 'users/' + id);
  }

  getAvailableRoles(): Observable<Role[]> {
    return this.http.get<Role[]>(this.baseUrl + 'users/GetAvailableRoles');
  }
  getRoles(id: number): Observable<Role[]> {
    let params = new HttpParams();
    params = params.append('userId', id.toString());
    return this.http.get<Role[]>(this.baseUrl + 'users/GetRoles', { params });
  }
  updateRoles(id: number, roleLists: number[]) {
    let params = new HttpParams();
    params = params.append('userId', id.toString());
    const roleList = <RoleList>{ roles: [] };
    roleLists.forEach(x => roleList.roles.push(<Role>{ id: +x, roleName: '' }));
    return this.http.post(this.baseUrl + 'users/UpdateRoles', roleList, { params });
  }

}
