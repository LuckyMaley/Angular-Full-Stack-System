import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../Services/auth.service';
import { NavigationEnd, Router } from '@angular/router';

@Component({
  selector: 'app-default-footer',
  templateUrl: './default-footer.component.html',
  styleUrls: ['./default-footer.component.css']
})
export class DefaultFooterComponent implements OnInit {
  public isAdminUser: boolean = false;
  public isSellerUser: boolean = false;
  public isCustomerUser: boolean = false;
  public isLoggedIn: boolean = false;
  public loggedInUsername: string | undefined = 'User';
  today: Date = new Date();
  ngOnInit(): void {
    this.router.events.subscribe(event => {

        this.isCustomerUser = this.auth.isCustomer();
        this.isSellerUser = this.auth.isSeller();
        this.isAdminUser = this.auth.isAdmin();
        this.isLoggedIn = this.auth.isLoggedIn();
        this.loggedInUsername = this.auth.getLoggedInUserName();
      
    });
  }
  constructor(private auth: AuthService, private router: Router) {
    
  }
}
