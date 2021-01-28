import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { User } from '../../../_models/User';
import { AlertifyService } from '../../../_services/alertify.service';
import { AuthService } from '../../../_services/auth.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: 'register.component.html'
})
export class RegisterComponent implements OnInit {

  user: User;
  registerForm: FormGroup;

  get r() { return this.registerForm; }

  constructor(private authService: AuthService,
    private alertify: AlertifyService,
    private fb: FormBuilder,
    private router: Router) { }

  ngOnInit() {
    this.createRegisterForm();
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
      this.authService.register(this.user).subscribe(() => {
        this.alertify.success('Registration Successful ');
      }, error => {
        this.alertify.error(error);
      }, () => {
        this.router.navigate(['/user/list']);
      });

      console.log(this.user);
      console.log(Object.assign({}, this.registerForm.value));


    }
  }
}
