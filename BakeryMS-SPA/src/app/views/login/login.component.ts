import { Component } from '@angular/core';

@Component({
  selector: 'app-dashboard',
  templateUrl: 'login.component.html'
})
export class LoginComponent {

  styleObject(): Object {
    return {
      background: 'url(assets/img/Background/Login1.jpg)',
      'background-repeat': 'no-repeat',
      'background-size': '1600px 900px'
    };
  }
}
