import { Injectable } from '@angular/core';
declare let alertify: any;

@Injectable({
  providedIn: 'root'
})
export class AlertifyService {

  constructor() { }

  confirm(title: string, message: string, okCallback: () => any, cancelCallback: () => any) {
    // tslint:disable-next-line: only-arrow-functions
    alertify.confirm(title, message, function (e) {
      if (e) {
        okCallback();
      } else { }
    }, function (e) {
      if (e) {
        cancelCallback();
      }
    });


  }

  success(message: string) {
    alertify.success(message);
  }

  error(message: string) {
    alertify.error(message);
  }

  warning(message: string) {
    alertify.warning(message);
  }

  message(message: string) {
    alertify.message(message);
  }

}
