import { AfterViewInit, Component, OnInit } from '@angular/core';
import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
import { AuthService } from './Shared/Services/auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'Instant Order E-Commerce Website';
  isMainNavigationVisible: boolean = true;
  isUserNavigationVisible: boolean = false;
  public isAdminUser: boolean = false;
  public isSellerUser: boolean = false;
  public isCustomerUser: boolean = false;
  public isLoggedIn: boolean = false;
  public loggedInUsername: string | undefined = 'User';

  constructor(private router: Router, private activatedRoute: ActivatedRoute, private auth: AuthService) {
    
  }

  ngOnInit(): void {
    this.router.events.subscribe(event => {
      if (event instanceof NavigationEnd) {
        this.updateNavigationVisibility();
      }
      this.isCustomerUser = this.auth.isCustomer();
      this.isSellerUser = this.auth.isSeller();
      this.isAdminUser = this.auth.isAdmin();
      this.isLoggedIn = this.auth.isLoggedIn();
      this.loggedInUsername = this.auth.getLoggedInUserName();
    });
  }


  private updateNavigationVisibility(): void {
    const currentRoute = this.activatedRoute.root;
    const routePath = this.extractRoutePath(currentRoute);
    this.isUserNavigationVisible = routePath.includes('admin') || routePath.includes('seller');
    this.isMainNavigationVisible = !this.isUserNavigationVisible;
  
  }
  
  private extractRoutePath(route: ActivatedRoute): string {
    let routePath = '';
    while (route.firstChild) {
      route = route.firstChild;
      routePath += `/${route.snapshot.url.map(segment => segment.path).join('/')}`;
    }
    return routePath;
  }
}
