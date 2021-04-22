import { Component, ViewChild } from '@angular/core';
import { navItems as navAdmin } from '../../_NavObjects/_nav';
import { navItems as navBakeryManager } from '../../_NavObjects/_navBM';
import { navItems as navOutletManager } from '../../_NavObjects/_navOM';
import { navItems as navCashier } from '../../_NavObjects/_navCash';
import { navItems as navUser } from '../../_NavObjects/_navUser';
import { AlertifyService } from '../../_services/alertify.service';
import { Router } from '@angular/router';
import { AuthService } from '../../_services/auth.service';
import { JwtHelperService } from '@auth0/angular-jwt';
import { ConfigurationService } from '../../_services/configuration.service';
import { Notification } from '../../_models/configuration';
import { interval, Subscription } from 'rxjs';
import { ModalDirective } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-dashboard',
  templateUrl: './default-layout.component.html'
})
export class DefaultLayoutComponent {
  @ViewChild('infoModal') public infoModal: ModalDirective;

  public sidebarMinimized = false;
  public navItems;
  jwtHelper = new JwtHelperService;
  isAsideDisplay: boolean = false;
  notifications: Notification[] = [];
  notiCount: number = 0;
  notiNotReadCount: number = 0;
  notification: Notification = <Notification>{};
  userId: number;
  userName:string;

  subscription: Subscription;

  constructor(private alertify: AlertifyService,
    private router: Router,
    private auth: AuthService,
    private configSvc: ConfigurationService) {

    this.setNavItem(auth);
    this.getNotifications();
    this.userId = auth.getuserId();
    this.userName =auth.getuserName();
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
    } else if (auth.isUserCashier()) {
      this.navItems = navCashier;
    } else {
      this.navItems = navUser;
    }
  }


  logout() {
    localStorage.removeItem('token');
    localStorage.removeItem('BusinessPlaceId');
    this.alertify.message('logged out');
    this.router.navigate(['/login']);
  }

  getNotifications() {
    const source = interval(10000);
    this.subscription = source.subscribe(val => {
      this.configSvc.getRecentNotification().subscribe((res) => {
        this.notifications = res;
        this.notiCount = this.notifications.length;
        this.notiNotReadCount = this.notifications.filter(x => x.isRead === false).length;
      });
    });
  }

  getNoti(id: number) {
    this.configSvc.getNotification(id).subscribe((res) => {
      this.notification = res;
      this.infoModal.show();
    });
  }
}
