import { AfterViewChecked, Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CartItem, CustomersWishlistsVM, Product } from 'src/app/Shared/Models/llm-ecommerce-efdb-vm';
import { AuthService } from 'src/app/Shared/Services/auth.service';
import { CartService } from 'src/app/Shared/Services/cart.service';
import { CrudService } from 'src/app/Shared/Services/crud.service';
import { HomeService } from 'src/app/Shared/Services/home.service';

@Component({
  selector: 'app-wishlist',
  templateUrl: './wishlist.component.html',
  styleUrls: ['./wishlist.component.css']
})
export class WishlistComponent implements OnInit, AfterViewChecked {
  wishlists: CustomersWishlistsVM[] = [];
  products: Product[] = [];
  errorMessage: string | null = null;
  public isAdminUser: boolean = false;
  public isSellerUser: boolean = false;
  public isCustomerUser: boolean = false;
  public isLoggedIn: boolean = false;
  public loggedInUsername: string | undefined = 'Langelihle';

  constructor(
    private authService: AuthService,
    private router: Router,
    private homeService: HomeService, private cartService: CartService, private crudService: CrudService
  ) {
    this.isCustomerUser = this.authService.isCustomer();
    this.isSellerUser = this.authService.isSeller();
    this.isAdminUser = this.authService.isAdmin();
    this.isLoggedIn = this.authService.isLoggedIn();
    this.loggedInUsername = this.authService.getLoggedInUserName();
  }

  ngOnInit() {
    this.fetchData();
  }

  fetchData(){
    this.homeService.getMyWishlists().subscribe((data: CustomersWishlistsVM[]) => {
      this.wishlists = data;
      console.log()
    });
    this.homeService.getProducts().subscribe(data => {
      this.products = data;
   },
   error => {
     this.errorMessage = 'Error fetching products';
     console.error(error);
   });
  }

  ngAfterViewChecked(): void {
      this.cartService.initializeAddToCartButtons();
  }

  
  getProductImg(prodId:number){
    return this.products.find(product => product.productId == prodId)?.imageUrl;
  }

  addToCart(productId: number): void {
    let product = this.products.find(p => p.productId == productId);
    if(product){
    const cartItem: CartItem = {
      id: product.productId.toString(),
      title: product.name ? product.name : '',
      image: product.imageUrl,
      price: product.price,
      qty: 1,
    };
  
    this.cartService.addToCart(cartItem);
    }
  }

  deleteWishlistItem(itemId: number): void {
    this.crudService.removeWishlistItem(itemId).subscribe(data => {
      this.fetchData();
   },
   error => {
     this.errorMessage = 'Error deleting wishlist item';
     console.error(error);
   });
  }

}
