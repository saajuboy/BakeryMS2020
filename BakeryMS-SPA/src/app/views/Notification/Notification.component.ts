import { Component, OnInit, ViewChild } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { Notification } from '../../_models/configuration';
import { AlertifyService } from '../../_services/alertify.service';
import { ConfigurationService } from '../../_services/configuration.service';

@Component({
  selector: 'app-Notification',
  templateUrl: './Notification.component.html',
  styleUrls: ['./Notification.component.scss']
})
export class NotificationComponent implements OnInit {
  @ViewChild('infoModal') public infoModal: ModalDirective;

  notifications: Notification[] = [];
  search: string = '';
  notification: Notification = <Notification>{};
  sortOrder = { one: false, two: false, three: false, four: false, five: false };

  constructor(private configSvc: ConfigurationService,
    private alertify: AlertifyService) { }

  ngOnInit() {
    this.getList();
  }

  getList() {
    this.configSvc.getNotifications().subscribe(result => {
      this.notifications = result;
      this.notifications.sort((a, b) => b.id - a.id);
    }, error => {
      this.alertify.error(error);
    });
  }

  ShowNotification(id: number) {
    this.configSvc.getNotification(id).subscribe((cus) => {

      this.notification = cus;
      this.infoModal.show();
    });
  }

  sort(propertyNumber: number) {

    switch (propertyNumber) {
      case 1:
        this.notifications.sort(
          (a, b) => this.sortOrder.one === false ?
            a.title.localeCompare(b.title) :
            b.title.localeCompare(a.title)
        );
        this.sortOrder.one = !this.sortOrder.one;
        break;
      case 2:
        this.notifications.sort(
          (a, b) => this.sortOrder.two === false ?
            a.date.localeCompare(b.date) :
            b.date.localeCompare(a.date)
        );
        this.sortOrder.two = !this.sortOrder.two;
        break;
      case 3:
        this.notifications.sort(
          (a, b) => this.sortOrder.three === false ?
            a.time.localeCompare(b.time) :
            b.time.localeCompare(a.time)
        );
        this.sortOrder.three = !this.sortOrder.three;
        break;
      case 4:
        this.notifications.sort(
          (a, b) => this.sortOrder.four === false ?
            +a.isRead - +b.isRead : +b.isRead - +a.isRead
        );
        this.sortOrder.four = !this.sortOrder.four;
        break;
      case 5:
        this.notifications.sort(
          (a, b) => this.sortOrder.five === false ?
            +a.status - +b.status : +b.status - +a.status
        );
        this.sortOrder.five = !this.sortOrder.five;
        break;
      default:
        this.notifications.sort(
          (a, b) => this.sortOrder.one === false ?
            a.title.localeCompare(b.title) :
            b.title.localeCompare(a.title)
        );
        this.sortOrder.one = !this.sortOrder.one;
        break;
    }
  }

}
