import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot,  Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from 'src/app/Shared/Services/auth.service';

@Injectable({
  providedIn: 'root'
})
export class SellerGuard  {
  constructor(private auth: AuthService, private router: Router) { }
   canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): boolean {
    if (!this.auth.isSeller()) {
      this.router.navigate(['/login']);
      return false;
    }

    return this.auth.isSeller();
  }

}