import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot,  CanActivate,  Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from 'src/app/Shared/Services/auth.service';

@Injectable({
  providedIn: 'root'
})

export class AdminGuard implements CanActivate {
  constructor(private auth: AuthService, private router: Router) { }
   canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): boolean {
    if (!this.auth.isAdmin()) {
      this.router.navigate(['/login']);
      return false;
    }

    return this.auth.isAdmin();
  }

}