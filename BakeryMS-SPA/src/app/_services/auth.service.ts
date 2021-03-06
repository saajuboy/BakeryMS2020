import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { JwtHelperService } from '@auth0/angular-jwt';
import { map } from 'rxjs/operators';

const httpOptions = {
  headers: new HttpHeaders({
    'Authorization': 'Bearer ' + localStorage.getItem('token')
  })
};

@Injectable({
  providedIn: 'root'
})


export class AuthService {

  baseUrl = environment.apiUrl + 'auth/';
  jwtHelper = new JwtHelperService;
  decodedToken: any;

  constructor(private http: HttpClient) { }

  login(model: any) {
    return this.http.post(this.baseUrl + 'login', model).pipe(
      map((response: any) => {
        const user = response;
        if (user) {
          localStorage.setItem('token', user.token);
          this.decodedToken = this.jwtHelper.decodeToken(user.token);
          console.log(this.decodedToken);
        }
      })
    );
  }

  register(model: any) {
    return this.http.post(this.baseUrl + 'register', model, httpOptions);
  }


  loggedIn() {
    const token = localStorage.getItem('token');
    return !this.jwtHelper.isTokenExpired(token);
  }

  // roleMethods

  isUserAdmin() {
    let roles: string[];
    // const token = localStorage.getItem('token');
    // const decodeToken = this.jwtHelper.decodeToken(token);
    roles = this.decodedToken.role;
    return roles.includes('Admin');
  }
  isUserOutletManager() {
    let roles: string[];
    const token = localStorage.getItem('token');
    const decodeToken = this.jwtHelper.decodeToken(token);
    roles = decodeToken.role;
    return roles.includes('OutletManager');
  }
  isUserBakeryManager() {
    let roles: string[];
    const token = localStorage.getItem('token');
    const decodeToken = this.jwtHelper.decodeToken(token);
    roles = decodeToken.role;
    return roles.includes('BakeryManager');
  }
  isUserCashier() {
    let roles: string[];
    const token = localStorage.getItem('token');
    const decodeToken = this.jwtHelper.decodeToken(token);
    roles = decodeToken.role;
    return roles.includes('Cashier');
  }
  isUserBaker() {
    let roles: string[];
    const token = localStorage.getItem('token');
    const decodeToken = this.jwtHelper.decodeToken(token);
    roles = decodeToken.role;
    return roles.includes('Baker');
  }
  getuserId(): number {
    const id = this.decodedToken.nameid;
    return +id;
  }
  getuserName(): string {
    const name = this.decodedToken.unique_name;
    return name;
  }

}
