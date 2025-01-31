import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CustomersWishlistsVM } from 'src/app/Shared/Models/llm-ecommerce-efdb-vm';
import { AuthService } from 'src/app/Shared/Services/auth.service';
import { HomeService } from 'src/app/Shared/Services/home.service';

@Component({
  selector: 'app-admins-wishlists',
  templateUrl: './admins-wishlists.component.html',
  styleUrls: ['./admins-wishlists.component.css']
})
export class AdminsWishlistsComponent implements OnInit {
  wishlists: CustomersWishlistsVM[] = [];

  constructor(
    private authService: AuthService,
    private router: Router,
    private homeService: HomeService
  ) {}

  ngOnInit() {
    this.homeService.getAllCustomerWishlists().subscribe((data: CustomersWishlistsVM[]) => {
      this.wishlists = data;
    });
  }

  Logout() {
    this.authService.logout();

    this.router.navigate(['/home']);
  }

}
