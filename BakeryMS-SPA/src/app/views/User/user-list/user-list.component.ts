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
  sortOrder = { one: false, two: false, three: false, four: false };

  constructor(private userService: UserService, private alertify: AlertifyService, private router: Router) { }

  ngOnInit() {

    this.userService.getUsers().subscribe(result => {
      this.users = result;
      this.users.sort((a, b) => b.id - a.id);

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

  sort(propertyNumber: number) {

    switch (propertyNumber) {
      case 1:
        this.users.sort((a, b) =>
          this.sortOrder.one === false ? a.username.localeCompare(b.username) : b.username.localeCompare(a.username));
        this.sortOrder.one = !this.sortOrder.one;
        break;
      case 2:
        this.users.sort((a, b) => this.sortOrder.two === false ? a.gender.localeCompare(b.gender) : b.gender.localeCompare(a.gender));
        this.sortOrder.two = !this.sortOrder.two;
        break;
      case 3:
        this.users.sort((a, b) => {
          return this.sortOrder.three === false ?
            <any>new Date(b.lastActive) - <any>new Date(a.lastActive) :
            <any>new Date(a.lastActive) - <any>new Date(b.lastActive);
        });
        this.sortOrder.three = !this.sortOrder.three;
        break;
      case 4:
        this.users.sort((a, b) => this.sortOrder.four === false ? +a.status - +b.status : +b.status - +a.status);
        this.sortOrder.four = !this.sortOrder.four;
        break;
      default:
        this.users.sort((a, b) =>
          this.sortOrder.one === false ? a.username.localeCompare(b.username) : b.username.localeCompare(a.username));
        this.sortOrder.one = !this.sortOrder.one;
        break;
    }
  }
}
