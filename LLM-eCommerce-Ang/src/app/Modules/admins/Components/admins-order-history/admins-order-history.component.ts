import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import {
  AdminCustomersOrdersVM,
  CustomersOrderDetailsVM,
} from 'src/app/Shared/Models/llm-ecommerce-efdb-vm';
import { AuthService } from 'src/app/Shared/Services/auth.service';
import { CrudService } from 'src/app/Shared/Services/crud.service';

@Component({
  selector: 'app-admins-order-history',
  templateUrl: './admins-order-history.component.html',
  styleUrls: ['./admins-order-history.component.css'],
})
export class AdminsOrderHistoryComponent implements OnInit {
  customersOrders: AdminCustomersOrdersVM[] = [];
  cusOrderDetails: CustomersOrderDetailsVM[] = [];

  constructor(
    private authService: AuthService,
    private router: Router,
    private crudService: CrudService
  ) {}

  ngOnInit() {
    this.crudService.customerOrders().subscribe((data) => {
      this.customersOrders = data;
    });

    this.crudService.customerOrderDetails().subscribe((data) => {
      this.cusOrderDetails = data;
    });
  }

  public getOrderDetail(id: number): any {
    let cus = this.cusOrderDetails.filter((p) => p.orderId === id);
    return cus.reduce((sum, r) => sum + r.quantity, 0);
  }

  Logout() {
    this.authService.logout();

    this.router.navigate(['/home']);
  }
}
