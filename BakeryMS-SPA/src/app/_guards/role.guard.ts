import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';
import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable({
    providedIn: 'root'
})
export class RoleGuard implements CanActivate {

    jwtHelper = new JwtHelperService;

    constructor(private authService: AuthService, private router: Router, private alertify: AlertifyService) { }

    canActivate(route: ActivatedRouteSnapshot): boolean {

        const allowedRoles = route.data['allowedRoles'];

        let activate = false;

        for (const role of allowedRoles) {
            switch (role) {
                case 'Admin':
                    activate = this.authService.isUserAdmin();
                    break;
                case 'OutletManager':
                    activate = this.authService.isUserOutletManager();
                    break;
                case 'BakeryManager':
                    activate = this.authService.isUserBakeryManager();
                    break;
                case 'Cashier':
                    activate = this.authService.isUserCashier();
                    break;
                case 'Baker':
                    activate = this.authService.isUserBaker();
                    break;
                default:
                    activate = false;

            }

            if (activate) {
                break;
            }

        }

        if (activate) {
            return true;
        }

        this.alertify.error('UnAuthorized');
        this.router.navigate(['/dashboard']);
        return false;
    }

}
