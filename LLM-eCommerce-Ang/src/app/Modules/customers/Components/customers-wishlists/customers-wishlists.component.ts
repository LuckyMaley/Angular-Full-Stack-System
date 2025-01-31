import { AfterViewChecked, AfterViewInit, Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CartItem, CustomersWishlistsVM, Product } from 'src/app/Shared/Models/llm-ecommerce-efdb-vm';
import { AuthService } from 'src/app/Shared/Services/auth.service';
import { CartService } from 'src/app/Shared/Services/cart.service';
import { CrudService } from 'src/app/Shared/Services/crud.service';
import { HomeService } from 'src/app/Shared/Services/home.service';

@Component({
  selector: 'app-customers-wishlists',
  templateUrl: './customers-wishlists.component.html',
  styleUrls: ['./customers-wishlists.component.css']
})
export class CustomersWishlistsComponent implements OnInit, AfterViewChecked {
  wishlists: CustomersWishlistsVM[] = [];
  products: Product[] = [];
  errorMessage: string | null = null;

  constructor(
    private authService: AuthService,
    private router: Router,
    private homeService: HomeService, private cartService: CartService, private crudService: CrudService
  ) {}

  ngOnInit() {
    this.fetchData();
  }


  ngAfterViewChecked(): void {
      this.cartService.initializeAddToCartButtons();
  }

  fetchData(){
    this.homeService.getMyWishlists().subscribe((data: CustomersWishlistsVM[]) => {
      this.wishlists = data;
    });
    this.homeService.getProducts().subscribe(data => {
      this.products = data;
   },
   error => {
     this.errorMessage = 'Error fetching products';
     console.error(error);
   });
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

  Logout() {
    this.authService.logout();

    this.router.navigate(['/home']);
  }


}
