import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { OrderDetailsSellerVM } from 'src/app/Shared/Models/llm-ecommerce-efdb-vm';
import { AuthService } from 'src/app/Shared/Services/auth.service';
import { CrudService } from 'src/app/Shared/Services/crud.service';

@Component({
  selector: 'app-sellers-order-details',
  templateUrl: './sellers-order-details.component.html',
  styleUrls: ['./sellers-order-details.component.css']
})
export class SellersOrderDetailsComponent implements OnInit {
  sellerOrderHistoryVM: OrderDetailsSellerVM = {customersOrderDetailsVMList:[], usersProductsOrdersVM:{
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
  totalAmount: 0
  }}

  constructor(private crudService: CrudService, private authService: AuthService, private router: Router, private route: ActivatedRoute){}

  ngOnInit(): void {
    this.route.paramMap.subscribe(
      paramMap =>{
        let stringId:any = paramMap.get('id');
        console.log(stringId);
        let id = Number.parseInt(stringId);
      this.crudService.sellerUserProductsOrders().subscribe((data) => {
      const order =  data.find(p => p.orderId === id);
      if(order){
        this.sellerOrderHistoryVM.usersProductsOrdersVM = order;
        
      }
      
    });
    this.crudService.customerOrderDetailsById(id).subscribe((data) => {
      this.sellerOrderHistoryVM.customersOrderDetailsVMList = data;
    });
      }
    );
    
}

public getTotal(){
  return this.sellerOrderHistoryVM.customersOrderDetailsVMList.reduce((sum, r) => sum + r.unitPrice, 0);
}

Logout() {
this.authService.logout();

this.router.navigate(['/home']);
}

}
