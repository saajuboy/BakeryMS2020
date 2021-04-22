import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Role, User } from '../../../_models/User';
import { AlertifyService } from '../../../_services/alertify.service';
import { AuthService } from '../../../_services/auth.service';
import { UserService } from '../../../_services/user.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: 'register.component.html'
})
export class RegisterComponent implements OnInit {

  user: User;
  registerForm: FormGroup;
  isEditForm: boolean = false;
  isSelfEdit: boolean = false;
  userID: number;
  get r() { return this.registerForm; }

  roles: number[] = [];
  availableroles: Role[] = [];

  constructor(private authService: AuthService,
    private alertify: AlertifyService,
    private fb: FormBuilder,
    private router: Router,
    private route: ActivatedRoute,
    private userService: UserService) { }

  ngOnInit() {
    this.createRegisterForm();

    //for edit Form
    this.route.paramMap.subscribe(params => {
      const usrId = +params.get('id');

      if (usrId) {
        const curUSerId = this.authService.getuserId();
        if (this.authService.isUserAdmin || curUSerId === usrId) {
          this.getUser(usrId);
          this.isEditForm = true;
          this.userID = usrId;
          this.getAvailableRoles();

          this.isSelfEdit = curUSerId === usrId ? true : false;
        }
        else {
          this.alertify.error("Un authorized");
          this.router.navigate(['/dashboard']);
        }
      }
    });

  }
  createRegisterForm() {
    this.registerForm = this.fb.group({
      username: ['', [Validators.required, Validators.pattern('^[a-zA-Z0-9_@$]+')]],
      firstname: ['', [Validators.required, Validators.pattern('^[a-zA-Z]+')]],
      lastname: ['', [Validators.required, Validators.pattern('^[a-zA-Z]+')]],
      password: ['', [Validators.minLength(4), Validators.maxLength(20), Validators.required]],
      confirmPassword: ['', Validators.required],
      gender: ['male']
    }, { validators: this.passwordMatchValidator });
  }
  passwordMatchValidator(g: FormGroup) {
    return g.get('password').value === g.get('confirmPassword').value ? null : { mismatch: true };
  }

  register() {
    if (this.registerForm.valid) {
      this.user = Object.assign({}, this.registerForm.value);
      if (this.isEditForm === false) {
        this.authService.register(this.user).subscribe(() => {
          this.alertify.success('Registration Successful ');
        }, error => {
          this.alertify.error(error);
        }, () => {
          this.router.navigate(['/user/list']);
        });
      } else {
        this.userService.updateUser(this.userID, this.user).subscribe(() => {
          this.alertify.success('user updated successfully');
          if (this.isSelfEdit == false) {
            this.userService.updateRoles(this.userID, this.roles).subscribe(
              () => { this.alertify.success('Roles updated successfully'); },
              (error: any) => { this.alertify.warning(error.error.message) }
            )
          }
        }, error => {
          this.alertify.error(error.error);
        },
          () => {
            this.router.navigate(['/user/list']);
          });
        // this.alertify.success('updated Succes');
      }




      console.log(this.isEditForm);
      console.log(this.user);
      console.log(Object.assign({}, this.registerForm.value));


    }
  }

  //for editForm
  getUser(id: number) {
    this.userService.getUser(id).subscribe(
      (user: User) => this.createEditUserForm(user),
      (error: any) => { console.log(error); }
    );
  }

  createEditUserForm(user: User) {
    this.registerForm.patchValue({
      username: user.username,
      firstname: user.firstName,
      lastname: user.lastName,
      gender: user.gender
    });
  }
  getAvailableRoles() {
    this.userService.getAvailableRoles().subscribe((result) => {
      this.availableroles = result;
    });

  }
  backToList() {
    this.router.navigate(['/user/list']);
  }
}
