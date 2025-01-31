import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../Services/auth.service';
import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';

@Component({
  selector: 'app-main-navigation',
  templateUrl: './main-navigation.component.html',
  styleUrls: ['./main-navigation.component.css']
})
export class MainNavigationComponent implements OnInit{
  public isAdminUser: boolean = false;
  public isSellerUser: boolean = false;
  public isCustomerUser: boolean = false;
  public isLoggedIn: boolean = false;
  public loggedInUsername: string | undefined = 'Langelihle';
  ngOnInit(): void {
    
    this.router.events.subscribe(event => {
      if (event instanceof NavigationEnd) {
      this.isCustomerUser = this.auth.isCustomer();
      this.isSellerUser = this.auth.isSeller();
      this.isAdminUser = this.auth.isAdmin();
      this.isLoggedIn = this.auth.isLoggedIn();
      this.loggedInUsername = this.auth.getLoggedInUserName();
      console.log("loggedInUsername: " + this.loggedInUsername);
      
      }
    }
    );
  }
  constructor(private auth: AuthService, private router: Router, private route: ActivatedRoute) {
    
  }

  Logout() {
    
    this.auth.logout();
    this.isLoggedIn = false;
    this.isAdminUser = false;
    this.isSellerUser = false;
    this.isCustomerUser = false;
    this.loggedInUsername = '';

    this.router.navigate(['/home']);
    
  }

}
