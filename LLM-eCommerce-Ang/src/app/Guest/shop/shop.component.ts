import { AfterViewChecked, Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CartItem, Category, CustomersWishlistsVM, Product, Review, WishlistsVM } from 'src/app/Shared/Models/llm-ecommerce-efdb-vm';
import { AuthService } from 'src/app/Shared/Services/auth.service';
import { CartService } from 'src/app/Shared/Services/cart.service';
import { CrudService } from 'src/app/Shared/Services/crud.service';
import { HomeService } from 'src/app/Shared/Services/home.service';

@Component({
  selector: 'app-shop',
  templateUrl: './shop.component.html',
  styleUrls: ['./shop.component.css']
})
export class ShopComponent implements OnInit, AfterViewChecked {
  products: Product[] = [];
  reviews: Review[] = [];
  categories: Category[] = [];
  errorMessage: string | null = null;

  constructor(private homeService: HomeService, private cartService: CartService, private crudService: CrudService, private authService: AuthService, private router: Router) {}

  addToCart(product: any, qty: number): void {
    const cartItem: CartItem = {
      id: product.productId,
      title: product.name,
      image: product.imageUrl,
      price: product.price,
      qty: qty
    };
    this.cartService.addToCart(cartItem);
  }

  ngOnInit(): void {
    this.fetchData();
  }

  
  ngAfterViewChecked(): void {
    this.cartService.initializeAddToCartButtons();
    this.cartService.initializeAddToWishButtons();
  }

  fetchData(): void {
    this.homeService.getCategories().subscribe(
      (data: Category[]) => {
        this.categories = data;
        console.log('Categories:', data);
        console.log("list", this.categories);
      },
      error => {
        this.errorMessage = 'Error fetching categories';
        console.error(error);
      });
      this.homeService.getProducts().subscribe((data: Product[]) => {
        this.products = data;
        console.log('Products:', data);
        console.log("list", this.products);
      },
      error => {
        this.errorMessage = 'Error fetching products';
        console.error(error);
      });
      this.homeService.getReviews().subscribe((data: Review[]) => {
        this.reviews = data;
        console.log('Reviews:', data);
        console.log("list", this.reviews);
      },
      error => {
        this.errorMessage = 'Error fetching reviews';
        console.error(error);
      });
  }
  getCategories(){
    return this.categories.length;
  }

  getProductCountByCategory(categoryId: number): number {
    return this.products.filter(p => p.categoryId === categoryId).length;
  }

  getAverageRating(productId: number): number {
    const productReviews = this.reviews.filter(r => r.productId === productId);
    return productReviews.length > 0 ? productReviews.reduce((sum, r) => sum + r.rating, 0) / productReviews.length : 0;
  }

  getReviewCount(productId: number): number {
    return this.reviews.filter(r => r.productId === productId).length;
  }

  addWishlistItem(itemId: number): void {
    let wish: CustomersWishlistsVM[] = [];
    let isLoggedIn = this.authService.isLoggedIn();
    let isCustomer = this.authService.isCustomer();
    if(!isLoggedIn && !isCustomer){
      this.router.navigate(['/login']);
    }
    this.homeService.getMyWishlists().subscribe(
      (data) => {
        wish = data.filter((p) => p.productId == itemId);
        if (wish.length == 0) {
          let wishlist: WishlistsVM = { productId: itemId };
          this.crudService.addWishlistItem(wishlist).subscribe(
            (data) => {
            },
            (error) => {
              this.errorMessage = 'Error deleting wishlist item';
              console.error(error);
            }
          );
        }
      },
      (error) => {
        this.errorMessage = 'Error deleting wishlist item';
        console.error(error);
      }
    );
    
  }
}
