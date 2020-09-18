import { Component, OnInit } from '@angular/core';
import { Router, NavigationEnd } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';
import { AuthService } from './_services/auth.service';

@Component({
  // tslint:disable-next-line
  selector: 'body',
  template: '<router-outlet></router-outlet>'
})
export class AppComponent implements OnInit {
  jwtHelper = new JwtHelperService();

  constructor(private router: Router, private authService: AuthService) { }

  ngOnInit() {
    const token = localStorage.getItem('token');
    // const user = JSON.parse(localStorage.getItem('user'));
    if (token) {
      this.authService.decodedToken = this.jwtHelper.decodeToken(token);
    }

    this.router.events.subscribe((evt) => {
      if (!(evt instanceof NavigationEnd)) {
        return;
      }
      window.scrollTo(0, 0);
    });
  }
}
