import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CustomersOrdersVM } from 'src/app/Shared/Models/llm-ecommerce-efdb-vm';
import { AuthService } from 'src/app/Shared/Services/auth.service';
import { CrudService } from 'src/app/Shared/Services/crud.service';

@Component({
  selector: 'app-customers-order-history',
  templateUrl: './customers-order-history.component.html',
  styleUrls: ['./customers-order-history.component.css']
})
export class CustomersOrderHistoryComponent implements OnInit {
  customersOrders: CustomersOrdersVM = { myOrdersVMList: [], customersOrderDetailsVMList: []};

  constructor(
    private authService: AuthService,
    private router: Router,
    private crudService: CrudService
  ) {}

  ngOnInit() {
    this.crudService.getMyOrders().subscribe((data) => {
      this.customersOrders.myOrdersVMList = data;
    });

    this.crudService.myOrderDetails().subscribe((data) => {
      this.customersOrders.customersOrderDetailsVMList = data;
    });
  }

  public getOrderDetail(id: number): any {
    let cus = this.customersOrders.customersOrderDetailsVMList.filter((p) => p.orderId === id);
    return cus.reduce((sum, r) => sum + r.quantity, 0);
  }

  Logout() {
    this.authService.logout();

    this.router.navigate(['/home']);
  }

}
