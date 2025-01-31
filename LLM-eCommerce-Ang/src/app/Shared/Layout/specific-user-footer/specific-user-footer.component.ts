import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../Services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-specific-user-footer',
  templateUrl: './specific-user-footer.component.html',
  styleUrls: ['./specific-user-footer.component.css']
})
export class SpecificUserFooterComponent implements OnInit {
  public isAdminUser: boolean = false;
  public isSellerUser: boolean = false;
  public isCustomerUser: boolean = false;
  public isLoggedIn: boolean = false;
  public loggedInUsername: string | undefined = 'Langelihle';
  today: Date = new Date();
  ngOnInit(): void {
      
  }
  constructor(private auth: AuthService, private router: Router) {
    this.isCustomerUser = this.auth.isCustomer();
    this.isSellerUser = this.auth.isSeller();
    this.isAdminUser = this.auth.isAdmin();
    this.isLoggedIn = this.auth.isLoggedIn();
    this.loggedInUsername = this.auth.getLoggedInUserName();
  }

}
