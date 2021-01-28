import { Component, OnInit } from '@angular/core';
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

  constructor(private userService: UserService, private alertify: AlertifyService) { }

  ngOnInit() {

    this.userService.getUsers().subscribe(result => {
      this.users = result;
    }, error => {
      this.alertify.error(error);
    });

  }
}
