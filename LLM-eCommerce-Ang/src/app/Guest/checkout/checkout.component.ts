import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CartItem, FullOrderVM, OrderDetailsTwoVM } from 'src/app/Shared/Models/llm-ecommerce-efdb-vm';
import { Roles, UserProfileVM } from 'src/app/Shared/Models/user-auth-vm';
import { AuthService } from 'src/app/Shared/Services/auth.service';
import { CartService } from 'src/app/Shared/Services/cart.service';
import { CrudService } from 'src/app/Shared/Services/crud.service';
import { OrderDetail } from '../../Shared/Models/llm-ecommerce-efdb-vm';

@Component({
  selector: 'app-checkout',
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.component.css'],
})
export class CheckoutComponent implements OnInit {
  cartItems: CartItem[] = [];
  price: number = 0;
  selectedPaymentMethod: string = '';
  paypalEmail: string = '';
  directCheckBankName: string = '';
  directCheckAccountNumber: string = '';
  bankTransferBankName: string = '';
  bankTransferAccountNumber: string = '';
  showShippingAddress:boolean = false;
  shippingAddressLine1: string = '';
  shippingAddressToStore: string = '';
  errorMessage: string = '';
  userRoles: Roles = {$id:'', $values:['']};
  
  userProfileData: UserProfileVM = {
    firstName: '',
    lastName: '',
    userName: '',
    email: '',
    address:'',
    phoneNumber:'',
    userRoles: this.userRoles
  }

  constructor(
    private router: Router,
    private authService: AuthService,
    private cartService: CartService,
    private crudService: CrudService
  ) {}

  ngOnInit(): void {
    this.router.events.subscribe((events) => {
      this.loadCartItems();
      this.authService
      .getUserProfile()
      .subscribe(( data) => {
        this.userProfileData =  data;
      });

    });
    
  }

  private loadCartItems(): void {
    const items = JSON.parse(localStorage.getItem('cart') || '[]');
    this.cartItems = items || [];
    this.price = this.cartItems.reduce((sum, r) => sum + r.price * r.qty, 0);
  }

  toggleShippingAddress() {
    this.showShippingAddress = !this.showShippingAddress;
  }

  validateAndSubmit() {
    this.errorMessage = '';
    if (this.userProfileData.userRoles.$values.includes('Administrator') || this.userProfileData.userRoles.$values.includes('Seller') || this.authService.getLoggedInUserName().trim() == '') {
      this.errorMessage = 'Please login as a customer to checkout.';
      return;
    }


    if (!this.selectedPaymentMethod) {
      this.errorMessage = 'Please select a payment method.';
      return;
    }

    if (this.selectedPaymentMethod === 'Paypal'){
      if(this.paypalEmail.trim() == '') {
      this.errorMessage = 'Please enter your PayPal email.';
      return;
    }
  }

    if (this.selectedPaymentMethod === 'Direct Check'){
       if((!this.directCheckBankName || !this.directCheckAccountNumber) || (this.directCheckBankName.trim() == '' || this.directCheckAccountNumber.trim() == '')) {
      this.errorMessage = 'Please enter your bank name and account number for direct check.';
      return;
    }
  }
   
    if (this.selectedPaymentMethod === 'Bank Transfer'){
      if((!this.bankTransferBankName || !this.bankTransferAccountNumber) || (this.bankTransferBankName.trim() === '' || this.bankTransferAccountNumber.trim() === '')) {
      this.errorMessage = 'Please enter your bank name and account number for bank transfer.';
      return;
    }
  }

    if (this.cartItems.length == 0) {
      this.errorMessage = 'Please enter products in your cart to continue.';
      return;
    }

    if (this.showShippingAddress) {
      if (!this.shippingAddressLine1) {
        this.errorMessage = 'Please fill in all the shipping address fields.';
        return;
      }
      else{
        this.shippingAddressToStore = this.shippingAddressLine1;
      }
    }
    else{
      this.shippingAddressToStore = this.userProfileData.address;
    }
    
    let orderDetailsItems: OrderDetailsTwoVM[] = [];
    for(let item of this.cartItems){
      let orderDetail : OrderDetailsTwoVM = {
        productId: Number.parseInt(item.id),
        quantity: item.qty,
        unitPrice: item.price
      };
      orderDetailsItems.push(orderDetail);
    };
    let orderId: number = 0
    const orderData: FullOrderVM = {
      shippingAddress: this.shippingAddressToStore,
      shippingMethod: 'Courier',
      totalAmount: this.price,
      paymentMethod: this.selectedPaymentMethod,
      status: 'Paid',
      orderDetails: orderDetailsItems 
    };

    this.crudService.makeOrder(orderData).subscribe(
      response => {
        this.cartService.clearCart();
        this.router.navigate(['/success/'+ response]);
      },
      error => {
        console.error('Order submission failed:', error);
        this.errorMessage = 'Order submission failed. Please try again.';
      }
    );
  }
}
