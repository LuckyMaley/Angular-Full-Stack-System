import { Component, OnInit } from '@angular/core';
import { CartItem } from '../../Shared/Models/llm-ecommerce-efdb-vm'
import { Route, Router } from '@angular/router';
import { AuthService } from 'src/app/Shared/Services/auth.service';
import { CartService } from 'src/app/Shared/Services/cart.service';

@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.css']
})
export class CartComponent implements OnInit{
    cartItems: CartItem[] = [];
    price: number = 0;
  
    constructor(private router: Router, private authService: AuthService, private cartService: CartService){}

  ngOnInit(): void {
    this.router.events.subscribe((events) =>{
      this.loadCartItems();
    
    });
    
  }

  private loadCartItems(): void {
    const items = JSON.parse(localStorage.getItem('cart') || '[]');
    this.cartItems = items || [];
    this.price = this.cartItems.reduce((sum, r) => sum + (r.price * r.qty), 0)
  }

  deleteCartItem(itemId: string): void {
    this.cartService.deleteCartItem(itemId);
    this.loadCartItems();
  }

  
  navigateToLogin(): void {
    this.router.navigate(['/login']);
  }

  navigateToCheckout(): void {
    this.router.navigate(['/checkout']);
  }

  proceedToCheckout(): void {
    if (this.authService.isLoggedIn()) {
      this.navigateToCheckout();
    } else {
      this.navigateToLogin();
    }
  }
}
