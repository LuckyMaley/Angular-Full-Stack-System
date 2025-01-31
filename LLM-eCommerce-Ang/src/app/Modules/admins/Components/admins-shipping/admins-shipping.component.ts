import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Shipping } from 'src/app/Shared/Models/llm-ecommerce-efdb-vm';
import { AuthService } from 'src/app/Shared/Services/auth.service';
import { CrudService } from 'src/app/Shared/Services/crud.service';
import { HomeService } from 'src/app/Shared/Services/home.service';

@Component({
  selector: 'app-admins-shipping',
  templateUrl: './admins-shipping.component.html',
  styleUrls: ['./admins-shipping.component.css']
})
export class AdminsShippingComponent implements OnInit {
  shippings: Shipping[] = [];

  constructor(
    private authService: AuthService,
    private router: Router,
    private homeService: HomeService
  ) {}

  ngOnInit() {
    this.homeService.getShippings().subscribe((data: Shipping[]) => {
      this.shippings = data;
    });
  }

  Logout() {
    this.authService.logout();

    this.router.navigate(['/home']);
  }
}
