import { Component } from '@angular/core';
import { navItems as navAdmin } from '../../_NavObjects/_nav';
import { navItems as navBakeryManager } from '../../_NavObjects/_navBM';
import { navItems as navOutletManager } from '../../_NavObjects/_navOM';
import { AlertifyService } from '../../_services/alertify.service';
import { Router } from '@angular/router';
import { AuthService } from '../../_services/auth.service';
import { JwtHelperService } from '@auth0/angular-jwt';

@Component({
  selector: 'app-dashboard',
  templateUrl: './default-layout.component.html'
})
export class DefaultLayoutComponent {
  public sidebarMinimized = false;
  public navItems;
  jwtHelper = new JwtHelperService;

  constructor(private alertify: AlertifyService, private router: Router, private auth: AuthService) {

    this.setNavItem(auth);

  }
  toggleMinimize(e) {
    this.sidebarMinimized = e;
  }

  setNavItem(auth: AuthService) {
    if (auth.isUserAdmin()) {
      this.navItems = navAdmin;
    } else if (auth.isUserOutletManager()) {
      this.navItems = navOutletManager;
    } else if (auth.isUserBakeryManager()) {
      this.navItems = navBakeryManager;
    }
  }


  logout() {
    localStorage.removeItem('token');
    localStorage.removeItem('BusinessPlaceId');
    this.alertify.message('logged out');
    this.router.navigate(['/login']);
  }
}
