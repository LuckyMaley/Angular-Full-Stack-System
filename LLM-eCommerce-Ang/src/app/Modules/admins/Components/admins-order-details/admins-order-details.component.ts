import { Component, OnInit } from '@angular/core';
import { AdminOrderHistoryVM } from '../../../../Shared/Models/llm-ecommerce-efdb-vm';
import { CrudService } from 'src/app/Shared/Services/crud.service';
import { AuthService } from 'src/app/Shared/Services/auth.service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-admins-order-details',
  templateUrl: './admins-order-details.component.html',
  styleUrls: ['./admins-order-details.component.css'],
})
export class AdminsOrderDetailsComponent implements OnInit {
  adminOrderHistoryVM: AdminOrderHistoryVM = {
    customersOrderDetailsVMList: [],
    usersProductsOrdersVMList: {
      $id: '',
      efUserId: 0,
      firstName: '',
      lastName: '',
      email: '',
      address: '',
      phoneNumber: '',
      identityUsername: '',
      role: '',
      orderId: 0,
      shippingId: 0,
      shippingDate: new Date().toISOString(),
      shippingAddress: '',
      shippingMethod: '',
      trackingNumber: '',
      deliveryStatus: '',
      orderDate: '',
      totalAmount: 0,
    },
  };

  constructor(
    private crudService: CrudService,
    private authService: AuthService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.route.paramMap.subscribe((paramMap) => {
      let stringId: any = paramMap.get('id');
      console.log(stringId);
      let id = Number.parseInt(stringId);
      this.crudService.customerOrders().subscribe((data) => {
        const order = data.find((p) => p.orderId === id);
        if (order) {
          this.adminOrderHistoryVM.usersProductsOrdersVMList = order;
        }
      });
      this.crudService.customerOrderDetailsById(id).subscribe((data) => {
        this.adminOrderHistoryVM.customersOrderDetailsVMList = data;
      });
    });
  }

  public getTotal() {
    return this.adminOrderHistoryVM.customersOrderDetailsVMList.reduce(
      (sum, r) => sum + r.unitPrice,
      0
    );
  }

  Logout() {
    this.authService.logout();

    this.router.navigate(['/home']);
  }
}
