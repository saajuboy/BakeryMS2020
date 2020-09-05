import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';
import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable({
    providedIn: 'root'
})
export class AdminGuard implements CanActivate {

    jwtHelper = new JwtHelperService;

    constructor(private authService: AuthService, private router: Router, private alertify: AlertifyService) { }

    canActivate(): boolean {
        if (this.authService.isUserAdmin()) {
            return true;
        }

        this.alertify.error('UnAuthorized');
        this.router.navigate(['/dashboard']);
        return false;
    }

}
