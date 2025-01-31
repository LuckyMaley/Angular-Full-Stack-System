import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Shipping } from 'src/app/Shared/Models/llm-ecommerce-efdb-vm';
import { AuthService } from 'src/app/Shared/Services/auth.service';
import { HomeService } from 'src/app/Shared/Services/home.service';

@Component({
  selector: 'app-admins-shipping-pending',
  templateUrl: './admins-shipping-pending.component.html',
  styleUrls: ['./admins-shipping-pending.component.css']
})
export class AdminsShippingPendingComponent implements OnInit {
  shippings: Shipping[] = [];

  constructor(
    private authService: AuthService,
    private router: Router,
    private homeService: HomeService
  ) {}

  ngOnInit() {
    this.homeService.getShippings().subscribe((data: Shipping[]) => {
      this.shippings = data.filter(p => p.deliveryStatus === 'Pending');
    });
  }

  Logout() {
    this.authService.logout();

    this.router.navigate(['/home']);
  }

}
