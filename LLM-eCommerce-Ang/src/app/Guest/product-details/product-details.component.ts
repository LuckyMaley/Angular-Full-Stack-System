import { AfterViewChecked, Component, ElementRef, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import {
  CartItem,
  Category,
  CustomersReviewsVM,
  CustomersWishlistsVM,
  Product,
  Review,
  ReviewsVM,
  WishlistsVM,
} from 'src/app/Shared/Models/llm-ecommerce-efdb-vm';
import { HomeService } from 'src/app/Shared/Services/home.service';
import { filter } from 'rxjs';
import { CartService } from 'src/app/Shared/Services/cart.service';
import { InputNumService } from 'src/app/Shared/Services/input-num.service';
import { AuthService } from 'src/app/Shared/Services/auth.service';
import { CrudService } from 'src/app/Shared/Services/crud.service';
import { Roles, UserProfileVM } from 'src/app/Shared/Models/user-auth-vm';

@Component({
  selector: 'app-product-details',
  templateUrl: './product-details.component.html',
  styleUrls: ['./product-details.component.css'],
})
export class ProductDetailsComponent implements OnInit, AfterViewChecked {
  products: Product[] = [];
  product: Product = {
    $id: '',
    productId: 0,
    name: '',
    brand: '',
    description: '',
    type: '',
    price: 0,
    categoryId: 0,
    stockQuantity: 0,
    modifiedDate: '',
    imageUrl: '',
    category: {
      $id: '',
      categoryId: 0,
      name: '',
      products: {
        $id: '',
        $values: [],
      },
    },
    efUserProducts: {
      $id: '',
      $values: [],
    },
    orderDetails: {
      $id: '',
      $values: [],
    },
    reviews: {
      $id: '',
      $values: [],
    },
    wishlists: {
      $id: '',
      $values: [],
    },
  };
  userRoles: Roles = { $id: '', $values: [] };
  userProfileData: UserProfileVM = {
    firstName: '',
    lastName: '',
    userName: '',
    email: '',
    address: '',
    phoneNumber: '',
    userRoles: this.userRoles,
  };
  reviews: Review[] = [];
  cusReviews: CustomersReviewsVM[] = [];
  categories: Category[] = [];
  categoryItem: Category = {
    $id: '1',
    categoryId: 0,
    name: 'test',
    products: { $id: '', $values: this.products },
  };
  errorMessage: string | null = null;
  count: number = 0;
  inputQty: any = 1;
  idPassed: number = 0;
  public isAdminUser: boolean = false;
  public isSellerUser: boolean = false;
  public isCustomerUser: boolean = false;
  public isLoggedIn: boolean = false;
  public loggedInUsername: string | undefined = 'Langelihle';
  review: ReviewsVM = { rating: 5, title: '', comment: '', productId: 0 };
  existingReview: Review | null = null;
  hasReviewed = false;

  constructor(
    private homeService: HomeService,
    private router: Router,
    private auth: AuthService,
    private crudService: CrudService,
    private route: ActivatedRoute,
    private cartService: CartService,
    private elementRef: ElementRef,
    private inputValidationService: InputNumService
  ) {
    this.isCustomerUser = this.auth.isCustomer();
    this.isSellerUser = this.auth.isSeller();
    this.isAdminUser = this.auth.isAdmin();
    this.isLoggedIn = this.auth.isLoggedIn();
    this.loggedInUsername = this.auth.getLoggedInUserName();
  }

  addToCart(product: any, qty: number): void {
    const cartItem: CartItem = {
      id: product.productId,
      title: product.name,
      image: product.imageUrl,
      price: product.price,
      qty: qty,
    };
    this.cartService.addToCart(cartItem);
  }

  ngOnInit(): void {
    if (this.isLoggedIn) {
      this.auth.getUserProfile().subscribe((data) => {
        this.userProfileData = data;
      });
    }
    this.fetchData();

    this.route.paramMap.subscribe((paramMap) => {
      let stringId: any = paramMap.get('id');
      let id = Number.parseInt(stringId);
      this.idPassed = id;
      this.fetchDataWithId(id);
    });
    const inputs = this.elementRef.nativeElement.querySelectorAll('.num-input');
    inputs.forEach((input: HTMLInputElement) => {
      input.addEventListener('input', (event) => {
        this.inputValidationService.validateNumberInput(
          event.target as HTMLInputElement
        );
      });
    });
  }

  
  ngAfterViewChecked(): void {
    this.cartService.initializeAddToCartButtons();
    this.cartService.initializeAddToWishButtons();
   
  }

  fetchData(): void {
    this.homeService.getCategories().subscribe(
      (data: Category[]) => {
        this.categories = data;
      },
      (error) => {
        this.errorMessage = 'Error fetching categories';
        console.error(error);
      }
    );
    this.homeService.getProducts().subscribe(
      (data: Product[]) => {
        this.products = data;
      },
      (error) => {
        this.errorMessage = 'Error fetching products';
        console.error(error);
      }
    );
    this.homeService.getReviews().subscribe(
      (data: Review[]) => {
        this.reviews = data;
      },
      (error) => {
        this.errorMessage = 'Error fetching reviews';
        console.error(error);
      }
    );
  }

  fetchDataWithId(id: number): void {
    this.homeService.getProductsById(id).subscribe(
      (data: Product) => {
        this.product = data;
        this.count = data.stockQuantity;
      },
      (error) => {
        this.errorMessage = 'Error fetching products';
        console.error(error);
      }
    );
    this.homeService.getAllCustomerReviews().subscribe(
      (data: CustomersReviewsVM[]) => {
        this.cusReviews = data.filter((p) => p.productId === id);
        if (this.isLoggedIn) {
          const lengthReviews = this.cusReviews.find(
            (p) =>
              p.identityUsername.trim().toLowerCase() ===
              this.userProfileData.userName.trim().toLowerCase()
          )?.efUserId;
          const containsId = this.reviews.filter(
            (c) => c.efUserId === lengthReviews
          ).length;
          if (containsId > 0) {
            this.hasReviewed = true;
          }
        }
      },
      (error) => {
        this.errorMessage = 'Error fetching reviews';
        console.error(error);
      }
    );
  }

  getCategories() {
    return this.categories.length;
  }

  getProduct(prodId: number): Product {
    const product = this.products.find((p) => p.productId === prodId);
    if (!product) {
      const defaultProduct: Product = {
        productId: 0,
        name: 'Default Product',
        description: 'This is a default product',
        price: 0,
        stockQuantity: 0,
        categoryId: 0,
        $id: '',
        brand: null,
        type: null,
        modifiedDate: '',
        imageUrl: '',
        category: this.categoryItem,
        efUserProducts: {
          $id: '',
          $values: [],
        },
        orderDetails: {
          $id: '',
          $values: [],
        },
        reviews: {
          $id: '',
          $values: [],
        },
        wishlists: {
          $id: '',
          $values: [],
        },
      };
      return defaultProduct;
    }
    return product;
  }

  getProductCount(prodId: number): number {
    return this.products.filter((p) => p.productId === prodId).length;
  }

  getProductCountByCategory(categoryId: number): number {
    return this.products.filter((p) => p.categoryId === categoryId).length;
  }

  getAverageRating(productId: number): number {
    const productReviews = this.reviews.filter(
      (r) => r.productId === productId
    );
    return productReviews.length > 0
      ? productReviews.reduce((sum, r) => sum + r.rating, 0) /
          productReviews.length
      : 0;
  }

  getReviewCount(productId: number): number {
    return this.reviews.filter((r) => r.productId === productId).length;
  }

  getProductReviews(productId: number) {
    return this.reviews.filter((r) => r.productId === productId);
  }

  updateRating(event: any) {
    const value = event.target.value;
    this.review.rating = parseInt(value);
  }

  onSubmit() {
    this.review.productId = this.idPassed;
    if (
      this.review.title &&
      this.review.comment &&
      this.review.productId &&
      this.review.rating
    ) {
      this.crudService.postReview(this.review).subscribe(
        (response) => {
          console.log('Review posted successfully');
          this.hasReviewed = true;
          this.fetchData();
          this.fetchDataWithId(this.idPassed);
        },
        (error) => {
          console.error('Error posting review', error);
          this.errorMessage = 'Error posting review';
          return;
        }
      );
    } else {
      this.errorMessage =
        'Please make sure that review has a title and a comment';
    }
  }

  addWishlistItem(itemId: number): void {
    let wish: CustomersWishlistsVM[] = [];
    if(!this.isLoggedIn && !this.isCustomerUser){
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
