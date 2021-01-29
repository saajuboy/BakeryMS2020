import { Component, OnInit } from '@angular/core';
import { Router, RouteReuseStrategy } from '@angular/router';
import { routes } from '../../../app.routing';
import { User } from '../../../_models/User';
import { AlertifyService } from '../../../_services/alertify.service';
import { UserService } from '../../../_services/user.service';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.scss']
})
export class UserListComponent implements OnInit {

  users: User[];
  search: string = '';

  constructor(private userService: UserService, private alertify: AlertifyService, private router: Router) { }

  ngOnInit() {

    this.userService.getUsers().subscribe(result => {
      this.users = result;
    }, error => {
      this.alertify.error(error);
    });

  }
  addUser() {
    this.router.navigateByUrl('/user/register');
  }

  deactivate(index: number) {
    this.alertify.confirm('Are you sure?',
      'Are you sure you want to deactivate user? ',
      () => { this.alertify.success('User deactivated succesfully'); },
      () => { });
  }
  delete(index: number) {
    this.alertify.confirm('Are you sure?',
      'Are you sure you want to delete user? This action cannot be undone',
      () => { this.alertify.success('User deleted succesfully'); },
      () => { });
  }
}
