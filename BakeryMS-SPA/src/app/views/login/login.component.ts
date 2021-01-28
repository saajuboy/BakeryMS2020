import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../_services/auth.service';
import { AlertifyService } from '../../_services/alertify.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-dashboard',
  templateUrl: 'login.component.html'
})
export class LoginComponent implements OnInit {

  model: any = {};

  constructor(public authService: AuthService, private alertify: AlertifyService, private router: Router) { }

  ngOnInit() {
  }

  login() {
    this.authService.login(this.model).subscribe(next => {
      this.alertify.success('logged in succesfully');
      this.alertify.message('Welcome ' + this.authService.decodedToken.unique_name + '!');
    }, error => {
      this.alertify.error(error);
    }, () => {
      this.router.navigate(['']);
    });
  }

  loggedin() {
    return this.authService.loggedIn();
  }

  logout() {
    localStorage.removeItem('token');
    this.alertify.message('logged out');
    this.router.navigate(['/login']);
  }

  styleObject(): Object {
    return {
      background: 'url(assets/img/Background/Login1.jpg)',
      'background-repeat': 'no-repeat',
      'background-size': '1600px 900px'
      // filter: 'blur(0px)',
      // '-moz-filter': 'blur(1px)'
    };
  }

  // styleObjectForLoginForm(): object {
  //   return {
  //     filter: 'blur(0px)',
  //     '-webkit-filter': 'blur(0px)'
  //   };
  // }
}
