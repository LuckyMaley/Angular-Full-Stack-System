import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CustomersOrdersPaymentsVM } from 'src/app/Shared/Models/llm-ecommerce-efdb-vm';
import { AuthService } from 'src/app/Shared/Services/auth.service';
import { HomeService } from 'src/app/Shared/Services/home.service';

@Component({
  selector: 'app-admins-payments',
  templateUrl: './admins-payments.component.html',
  styleUrls: ['./admins-payments.component.css']
})
export class AdminsPaymentsComponent implements OnInit {
  payments: CustomersOrdersPaymentsVM[] = [];

  constructor(
    private authService: AuthService,
    private router: Router,
    private homeService: HomeService
  ) {}

  ngOnInit() {
    this.homeService.getAllCustomerPayments().subscribe((data: CustomersOrdersPaymentsVM[]) => {
      this.payments = data;
    });
  }

  Logout() {
    this.authService.logout();

    this.router.navigate(['/home']);
  }


}
