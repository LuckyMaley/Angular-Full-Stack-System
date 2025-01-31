import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CustomersOrderDetailsVM, UsersProductsOrdersVM } from 'src/app/Shared/Models/llm-ecommerce-efdb-vm';
import { AuthService } from 'src/app/Shared/Services/auth.service';
import { CrudService } from 'src/app/Shared/Services/crud.service';

@Component({
  selector: 'app-sellers-order-history',
  templateUrl: './sellers-order-history.component.html',
  styleUrls: ['./sellers-order-history.component.css']
})
export class SellersOrderHistoryComponent implements OnInit {
  customersOrders: UsersProductsOrdersVM[] = [];
  cusOrderDetails: CustomersOrderDetailsVM[] = [];

  constructor(
    private authService: AuthService,
    private router: Router,
    private crudService: CrudService
  ) {}

  ngOnInit() {
    this.crudService.sellerUserProductsOrders().subscribe((data) => {
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
