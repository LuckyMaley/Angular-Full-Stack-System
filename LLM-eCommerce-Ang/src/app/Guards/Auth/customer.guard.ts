import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot,  Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from 'src/app/Shared/Services/auth.service';

@Injectable({
  providedIn: 'root'
})
export class CustomerGuard  {
  constructor(private auth: AuthService, private router: Router) { }
   canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): boolean {
    if (!this.auth.isCustomer()) {
      this.router.navigate(['/login']);
      return false;
    }

    return this.auth.isCustomer();
  }

}