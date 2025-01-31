import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { map, Observable, of } from 'rxjs';
import {
  CategoriesResponse,
  Category,
  CustomersOrdersPaymentsVM,
  CustomersOrdersPaymentsVMResponse,
  CustomersReviewsVM,
  CustomersReviewsVMObjectResponse,
  CustomersReviewsVMResponse,
  CustomersWishlistsVM,
  CustomersWishlistsVMObjectResponse,
  CustomersWishlistsVMResponse,
  Product,
  ProductResponse,
  Review,
  ReviewResponse,
  Shipping,
  ShippingResponse,
} from '../Models/llm-ecommerce-efdb-vm';
import { GlobalConstants } from 'src/app/global-constants';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root',
})
export class HomeService {
  private apiUrl = GlobalConstants._RestBaseURL;

  constructor(private http: HttpClient, private authService: AuthService) {}

  getCategories(): Observable<Category[]> {
    return this.http
      .get<CategoriesResponse>(`${this.apiUrl}/api/Categories`)
      .pipe(map((response) => response.$values));
  }

  getCategoriesById(id: number): Observable<Category> {
    return this.http
      .get<Category>(`${this.apiUrl}/api/Categories/${id}`)
      .pipe(map((response) => response));
  }

  getProducts(): Observable<Product[]> {
    return this.http
      .get<ProductResponse>(`${this.apiUrl}/api/Products`)
      .pipe(map((response) => response.$values));
  }

  getProductsById(id: number): Observable<Product> {
    return this.http
      .get<Product>(`${this.apiUrl}/api/Products/${id}`)
      .pipe(map((response) => response));
  }

  getReviews(): Observable<Review[]> {
    return this.http
      .get<ReviewResponse>(`${this.apiUrl}/api/Reviews`)
      .pipe(map((response) => response.$values));
  }

  getReviewById(id:number): Observable<Review> {
    return this.http
      .get<Review>(`${this.apiUrl}/api/Reviews/${id}`)
      .pipe(map((response) => response));
  }

  getAllCustomerReviews(): Observable<CustomersReviewsVM[]> {
    return this.http
      .get<CustomersReviewsVMResponse>(`${this.apiUrl}/cusrev/allreviews`)
      .pipe(map((response) => response.$values));
  }

  getMyReviews(): Observable<CustomersReviewsVM[]> {
    let UserToken: string = this.authService.getUserToken();
    if (this.authService.isLoggedIn() && UserToken) {
      let reqHeader = new HttpHeaders().set(
        'Authorization',
        'bearer ' + UserToken
      );
      return this.http
        .get<CustomersReviewsVMObjectResponse>(`${this.apiUrl}/cusrev/MyReviews`, {
          headers: reqHeader,
        })
        .pipe(map((response) => response.customersReviews.$values));
    } else {
      let result: any = 'error, please login';
      return of(result);
    }
  }

  getMyWishlists(): Observable<CustomersWishlistsVM[]> {
    let UserToken: string = this.authService.getUserToken();
    if (this.authService.isLoggedIn() && UserToken) {
      let reqHeader = new HttpHeaders().set(
        'Authorization',
        'bearer ' + UserToken
      );
      return this.http
        .get<CustomersWishlistsVMObjectResponse>(`${this.apiUrl}/cuswish/MyWishlists`, {
          headers: reqHeader,
        })
        .pipe(map((response) => response.customersWishlists.$values));
    } else {
      let result: any = 'error, please login';
      return of(result);
    }
  }

  getAllCustomerWishlists(): Observable<CustomersWishlistsVM[]> {
    let UserToken: string = this.authService.getUserToken();
    if (this.authService.isLoggedIn() && UserToken) {
      let reqHeader = new HttpHeaders().set(
        'Authorization',
        'bearer ' + UserToken
      );
      return this.http
        .get<CustomersWishlistsVMResponse>(`${this.apiUrl}/cuswish`, {
          headers: reqHeader,
        })
        .pipe(map((response) => response.$values));
    } else {
      let result: any = 'error, please login';
      return of(result);
    }
  }

  getAllCustomerPayments(): Observable<CustomersOrdersPaymentsVM[]> {
    let UserToken: string = this.authService.getUserToken();
    if (this.authService.isLoggedIn() && UserToken) {
      let reqHeader = new HttpHeaders().set(
        'Authorization',
        'bearer ' + UserToken
      );
      return this.http
        .get<CustomersOrdersPaymentsVMResponse>(`${this.apiUrl}/cusordPay`, {
          headers: reqHeader,
        })
        .pipe(map((response) => response.$values));
    } else {
      let result: any = 'error, please login';
      return of(result);
    }
  }

  getShippings(): Observable<Shipping[]> {
    return this.http
      .get<ShippingResponse>(`${this.apiUrl}/api/Shippings`)
      .pipe(map((response) => response.$values));
  }

  getShippingsById(id: number): Observable<Shipping> {
    return this.http
      .get<Shipping>(`${this.apiUrl}/api/Shippings/${id}`)
      .pipe(map((response) => response));
  }
}
