import { Component, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { User } from '../../../_models/User';
import { AlertifyService } from '../../../_services/alertify.service';
import { UserService } from '../../../_services/user.service';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.scss']
})
export class UserListComponent implements OnInit {
  @ViewChild('infoModal') public infoModal: ModalDirective;

  users: User[];
  search: string = '';
  userInfo: any = {};

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

  deactivate(id: number) {
    const usr = this.users.find(a => a.id === id);
    const user: any = {};
    user.status = usr.status === false ? true : false;
    this.alertify.confirm('Are you sure?',
      'Are you sure you want to ' + (usr.status === false ? 'Activate' : 'Deactivate') + ' user? ',
      () => {
        this.userService.patchUser(id, user).subscribe(() => {
          this.alertify.success('User deactivated succesfully');
          this.users.find(a => a.id === id).status = user.status;
        }, () => {
          this.alertify.error('Failed To Deactivate User');
        });

      },
      () => { });

    // this.alertify.success('User deactivated succesfully');
  }
  delete(id: number) {
    this.alertify.confirm('Are you sure?',
      'Are you sure you want to delete user? This action cannot be undone',
      () => {
        this.userService.deleteUser(id).subscribe((next) => {
          this.alertify.success('User deleted succesfully');
          this.users = this.users.filter(function (obj) {
            return obj.id !== id;
          });
        }, () => {
          this.alertify.error('Failed to Delete User');
        });
      },
      () => { });

  }
  editUser(id: number) {
    this.router.navigate(['user/edit', id]);
  }
  ShowUserInfo(id: number) {
    this.userInfo = this.users.find(a => a.id === id);
    console.log(this.userInfo);

    this.infoModal.show();
  }
}
