import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CustomersOrdersPaymentsVM } from 'src/app/Shared/Models/llm-ecommerce-efdb-vm';
import { AuthService } from 'src/app/Shared/Services/auth.service';
import { HomeService } from 'src/app/Shared/Services/home.service';

@Component({
  selector: 'app-admins-payments-pending',
  templateUrl: './admins-payments-pending.component.html',
  styleUrls: ['./admins-payments-pending.component.css']
})
export class AdminsPaymentsPendingComponent implements OnInit {
  payments: CustomersOrdersPaymentsVM[] = [];

  constructor(
    private authService: AuthService,
    private router: Router,
    private homeService: HomeService
  ) {}

  ngOnInit() {
    this.homeService.getAllCustomerPayments().subscribe((data: CustomersOrdersPaymentsVM[]) => {
      this.payments = data.filter(p => p.paymentStatus === 'Pending');
    });
  }

  Logout() {
    this.authService.logout();

    this.router.navigate(['/home']);
  }

}
