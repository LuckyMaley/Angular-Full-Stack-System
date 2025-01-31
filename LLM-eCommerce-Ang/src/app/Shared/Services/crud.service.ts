import { Injectable } from '@angular/core';
import { GlobalConstants } from 'src/app/global-constants';
import { AuthService } from './auth.service';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import {
  AdminCustomersOrdersVM,
  AdminCustomersOrdersVMResponse,
  AdminOrderHistoryVM,
  AdminUserProductsVM,
  AdminUsersProductsVMResponse,
  CategoryVM,
  CustomerDashboardVM,
  CustomersOrderDetailsVM,
  CustomersOrderDetailsVMResponse,
  FullOrderVM,
  MyCustomersOrdersDetailsVMResponse,
  MyCustomersOrdersVMResponse,
  MyOrdersVM,
  MyOrdersVMResponse,
  Order,
  OrderResponseTwo,
  Payment,
  PaymentResponse,
  Product,
  ProductResponse,
  ProductsVM,
  ReviewsVM,
  Shipping,
  ShippingResponse,
  ShippingsVM,
  UsersProductsOrdersVM,
  UsersProductsOrdersVMObjectResponse,
  UsersProductsVM,
  UsersProductsVMObjectResponse,
  WishlistsVM,
} from '../Models/llm-ecommerce-efdb-vm';
import { Router } from '@angular/router';
import { catchError, map, Observable, of } from 'rxjs';
import { NgForm } from '@angular/forms';
import {
  UserProfileVMResponse,
  UserProfileVM,
  UserAccountVM,
  UserAccountVMResponse,
  Roles,
  UserAccountVMAdminSubmit,
  UserAccountVMSubmit,
} from '../Models/user-auth-vm';

@Injectable({
  providedIn: 'root',
})
export class CrudService {
  private _baseurl: string = GlobalConstants._RestBaseURL;

  constructor(
    private http: HttpClient,
    private authService: AuthService,
    private router: Router
  ) {}

  makeOrder(order: FullOrderVM): Observable<OrderResponseTwo> {
    let UserToken: string = this.authService.getUserToken();
    if (this.authService.isLoggedIn() && UserToken) {
      let reqHeader = new HttpHeaders().set(
        'Authorization',
        'bearer ' + UserToken
      );
      return this.http
        .post<OrderResponseTwo>(
          `${this._baseurl}/api/Orders/OrdersPay`,
          order,
          {
            headers: reqHeader,
          }
        )
        .pipe(catchError(this.handleError<OrderResponseTwo>('makeOrder')));
    } else {
      let result: any = 'error, please login';
      return of(result);
    }
  }

  getOrders(): Observable<Order[]> {
    return this.http
      .get<Order[]>(`${this._baseurl}/api/Orders`)
      .pipe(map((response) => response));
  }

  getShippings(): Observable<Shipping[]> {
    return this.http
      .get<ShippingResponse>(`${this._baseurl}/api/Shippings`)
      .pipe(map((response) => response.$values));
  }

  getPayments(): Observable<Payment[]> {
    return this.http
      .get<PaymentResponse>(`${this._baseurl}/api/Payments`)
      .pipe(map((response) => response.$values));
  }

  getOrderById(id: number) {
    return this.http
      .get<Order>(`${this._baseurl}/api/Orders/${id}`)
      .pipe(map((response) => response));
  }

  getShippingById(id: number) {
    return this.http
      .get<Shipping>(`${this._baseurl}/api/Shippings/${id}`)
      .pipe(map((response) => response));
  }

  getPaymentById(id: number) {
    return this.http
      .get<Payment>(`${this._baseurl}/api/Payments/${id}`)
      .pipe(map((response) => response));
  }

  private handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {
      console.error(error);
      return of(result as T);
    };
  }

  customerOrders(): Observable<AdminCustomersOrdersVM[]> {
    let UserToken: string = this.authService.getUserToken();
    if (this.authService.isLoggedIn() && UserToken) {
      let reqHeader = new HttpHeaders().set(
        'Authorization',
        'bearer ' + UserToken
      );
      return this.http
        .get<AdminCustomersOrdersVMResponse>(`${this._baseurl}/cusord`, {
          headers: reqHeader,
        })
        .pipe(map((response) => response.$values));
    } else {
      let result: AdminCustomersOrdersVM[] = [];
      return of(result);
    }
  }

  getMyOrders(): Observable<MyOrdersVM[]> {
    let UserToken: string = this.authService.getUserToken();
    if (this.authService.isLoggedIn() && UserToken) {
      let reqHeader = new HttpHeaders().set(
        'Authorization',
        'bearer ' + UserToken
      );
      return this.http
        .get<MyCustomersOrdersVMResponse>(`${this._baseurl}/cusord/MyOrders`, {
          headers: reqHeader,
        })
        .pipe(map((response) => response.customersOrders.$values));
    } else {
      let result: MyOrdersVM[] = [];
      return of(result);
    }
  }

  customerOrderDetails(): Observable<CustomersOrderDetailsVM[]> {
    let UserToken: string = this.authService.getUserToken();
    if (this.authService.isLoggedIn() && UserToken) {
      let reqHeader = new HttpHeaders().set(
        'Authorization',
        'bearer ' + UserToken
      );
      return this.http
        .get<CustomersOrderDetailsVMResponse>(`${this._baseurl}/cusordd`, {
          headers: reqHeader,
        })
        .pipe(map((response) => response.$values));
    } else {
      let result: CustomersOrderDetailsVM[] = [];
      return of(result);
    }
  }

  myOrderDetails(): Observable<CustomersOrderDetailsVM[]> {
    let UserToken: string = this.authService.getUserToken();
    if (this.authService.isLoggedIn() && UserToken) {
      let reqHeader = new HttpHeaders().set(
        'Authorization',
        'bearer ' + UserToken
      );
      return this.http
        .get<MyCustomersOrdersDetailsVMResponse>(`${this._baseurl}/cusordd/MyOrders`, {
          headers: reqHeader,
        })
        .pipe(map((response) => response.customersOrders.$values));
    } else {
      let result: CustomersOrderDetailsVM[] = [];
      return of(result);
    }
  }

  customerOrderDetailsById(id: number): Observable<CustomersOrderDetailsVM[]> {
    let UserToken: string = this.authService.getUserToken();
    if (this.authService.isLoggedIn() && UserToken) {
      let reqHeader = new HttpHeaders().set(
        'Authorization',
        'bearer ' + UserToken
      );
      return this.http
        .get<CustomersOrderDetailsVMResponse>(
          `${this._baseurl}/cusordd/customersByOrdersId/${id}`,
          {
            headers: reqHeader,
          }
        )
        .pipe(map((response) => response.$values));
    } else {
      let result: any = 'error, please login';
      return of(result);
    }
  }

  adminsProducts(): Observable<AdminUserProductsVM[]> {
    let UserToken: string = this.authService.getUserToken();
    if (this.authService.isLoggedIn() && UserToken) {
      let reqHeader = new HttpHeaders().set(
        'Authorization',
        'bearer ' + UserToken
      );
      return this.http
        .get<AdminUsersProductsVMResponse>(`${this._baseurl}/usrsprds`, {
          headers: reqHeader,
        })
        .pipe(map((response) => response.$values));
    } else {
      let result: AdminUserProductsVM[] = [];
      return of(result);
    }
  }

  sellersProducts(): Observable<UsersProductsVM[]> {
    let UserToken: string = this.authService.getUserToken();
    if (this.authService.isLoggedIn() && UserToken) {
      let reqHeader = new HttpHeaders().set(
        'Authorization',
        'bearer ' + UserToken
      );
      return this.http
        .get<UsersProductsVMObjectResponse>(`${this._baseurl}/usrsprds/MyProducts`, {
          headers: reqHeader,
        })
        .pipe(map((response) => response.usersProducts.$values));
    } else {
      let result: UsersProductsVM[] = [];
      return of(result);
    }
  }

  addProducts(form: ProductsVM): Observable<any> {
    let UserToken: string = this.authService.getUserToken();
    if (this.authService.isLoggedIn() && UserToken) {
      let reqHeader = new HttpHeaders().set(
        'Authorization',
        'bearer ' + UserToken
      );
      return this.http
        .post(`${this._baseurl}/api/Products`, form, {
          headers: reqHeader,
        })
        .pipe(
          map(async (data) => {
            console.log('tempResponseData :' + data);
          })
        );
    } else {
      let result: any = 'error, please login';
      return of(result);
    }
  }

  uploadImage(file: File): Observable<any> {
    const formData = new FormData();
    formData.append('file', file);
    let UserToken: string = this.authService.getUserToken();
    if (this.authService.isLoggedIn() && UserToken) {
      let reqHeader = new HttpHeaders().set(
        'Authorization',
        'bearer ' + UserToken
      );
      return this.http.post(`${this._baseurl}/api/Uploads`, formData, {
        headers: reqHeader,
      });
    } else {
      let result: any = 'error, please login';
      return of(result);
    }
  }

  deleteProducts(id: number): Observable<any> {
    let UserToken: string = this.authService.getUserToken();
    if (this.authService.isLoggedIn() && UserToken) {
      let reqHeader = new HttpHeaders().set(
        'Authorization',
        'bearer ' + UserToken
      );
      return this.http
        .delete(`${this._baseurl}/api/Products/${id}`, {
          headers: reqHeader,
        })
        .pipe(
          map(async (data) => {
            console.log('tempResponseData :' + data);
          })
        );
    } else {
      let result: any = 'error, please login';
      return of(result);
    }
  }

  updateProducts(id: number, form: ProductsVM): Observable<any> {
    let UserToken: string = this.authService.getUserToken();
    if (this.authService.isLoggedIn() && UserToken) {
      let reqHeader = new HttpHeaders().set(
        'Authorization',
        'bearer ' + UserToken
      );
      return this.http
        .put(`${this._baseurl}/api/Products/${id}`, form, {
          headers: reqHeader,
        })
        .pipe(
          map(async (data) => {
            console.log('tempResponseData :' + data);
          })
        );
    } else {
      let result: any = 'error, please login';
      return of(result);
    }
  }

  getAllUserProfile(): Observable<UserAccountVM[]> {
    let UserToken: string = this.authService.getUserToken();
    //console.log(this.isLoggedIn());
    //console.log(UserToken);
    if (this.authService.isLoggedIn() && UserToken) {
      let reqHeader = new HttpHeaders().set(
        'Authorization',
        'bearer ' + UserToken
      );
      return this.http
        .get<UserAccountVMResponse>(
          `${this._baseurl}/api/UserProfile/AllUsers`,
          {
            headers: reqHeader,
          }
        )
        .pipe(map((response) => response.$values));
    } else {
      let result: UserAccountVM[] = [];
      return of(result);
    }
  }

  getSpecificUserProfile(username: string): Observable<UserAccountVM> {
    let UserToken: string = this.authService.getUserToken();
    //console.log(this.isLoggedIn());
    //console.log(UserToken);
    if (this.authService.isLoggedIn() && UserToken) {
      let reqHeader = new HttpHeaders().set(
        'Authorization',
        'bearer ' + UserToken
      );
      return this.http.get<UserAccountVM>(
        `${this._baseurl}/api/UserProfile/specificUser/${username}`,
        {
          headers: reqHeader,
        }
      );
    } else {
      let userRoles: Roles = { $id: '', $values: [''] };

      let results: UserAccountVM = {
        firstName: '',
        lastName: '',
        userName: '',
        email: '',
        address: '',
        phoneNumber: '',
        rolesHeld: userRoles,
      };
      return of(results);
    }
  }

  getUserRegProfile(username: string): Observable<UserAccountVM> {
      return this.http.get<UserAccountVM>(
        `${this._baseurl}/api/UserProfile/UserRegCheck/${username}`);
  }

  deleteUserProfileById(id: string): Observable<UserProfileVM> {
    let UserToken: string = this.authService.getUserToken();
    //console.log(this.isLoggedIn());
    //console.log(UserToken);
    if (this.authService.isLoggedIn() && UserToken) {
      let reqHeader = new HttpHeaders().set(
        'Authorization',
        'bearer ' + UserToken
      );
      return this.http.delete<UserProfileVM>(
        `${this._baseurl}/api/UserProfile/DeleteUser/${id}`,
        {
          headers: reqHeader,
        }
      );
    } else {
      let userRoles: Roles = { $id: '', $values: [''] };

      let results: UserProfileVM = {
        firstName: '',
        lastName: '',
        userName: '',
        email: '',
        address: '',
        phoneNumber: '',
        userRoles: userRoles,
      };
      return of(results);
    }
  }

  updateUserProfile(
    formData: UserAccountVMAdminSubmit
  ): Observable<UserAccountVMAdminSubmit> {
    let UserToken: string = this.authService.getUserToken();
    //console.log(this.isLoggedIn());
    //console.log(UserToken);
    if (this.authService.isLoggedIn() && UserToken) {
      let reqHeader = new HttpHeaders().set(
        'Authorization',
        'bearer ' + UserToken
      );
      return this.http.put<UserAccountVMAdminSubmit>(
        `${this._baseurl}/api/UserProfile/Admin`,
        formData,
        {
          headers: reqHeader,
        }
      );
    } else {
      let userRoles: Roles = { $id: '', $values: [''] };

      let results: UserAccountVMAdminSubmit = {
        firstName: '',
        lastName: '',
        userName: '',
        email: '',
        address: '',
        phoneNumber: '',
        role: '',
      };
      return of(results);
    }
  }

  updateMyProfile(
    formData: UserAccountVMAdminSubmit
  ): Observable<UserAccountVMAdminSubmit> {
    let UserToken: string = this.authService.getUserToken();
    //console.log(this.isLoggedIn());
    //console.log(UserToken);
    if (this.authService.isLoggedIn() && UserToken) {
      let reqHeader = new HttpHeaders().set(
        'Authorization',
        'bearer ' + UserToken
      );
      return this.http.put<UserAccountVMAdminSubmit>(
        `${this._baseurl}/api/UserProfile`,
        formData,
        {
          headers: reqHeader,
        }
      );
    } else {
      let userRoles: Roles = { $id: '', $values: [''] };

      let results: UserAccountVMAdminSubmit = {
        firstName: '',
        lastName: '',
        userName: '',
        email: '',
        address: '',
        phoneNumber: '',
        role: '',
      };
      return of(results);
    }
  }

  updateMyUserProfile(
    formData: UserAccountVMSubmit
  ): Observable<UserAccountVMSubmit> {
    let UserToken: string = this.authService.getUserToken();
    //console.log(this.isLoggedIn());
    //console.log(UserToken);
    if (this.authService.isLoggedIn() && UserToken) {
      let reqHeader = new HttpHeaders().set(
        'Authorization',
        'bearer ' + UserToken
      );
      return this.http.put<UserAccountVMSubmit>(
        `${this._baseurl}/api/UserProfile`,
        formData,
        {
          headers: reqHeader,
        }
      );
    } else {
      let results: UserAccountVMSubmit = {
        firstName: '',
        lastName: '',
        userName: '',
        email: '',
        address: '',
        phoneNumber: '',
      };
      return of(results);
    }
  }

  addCategories(form: CategoryVM): Observable<any> {
    let UserToken: string = this.authService.getUserToken();
    if (this.authService.isLoggedIn() && UserToken) {
      let reqHeader = new HttpHeaders().set(
        'Authorization',
        'bearer ' + UserToken
      );
      return this.http
        .post(`${this._baseurl}/api/Categories`, form, {
          headers: reqHeader,
        })
        .pipe(
          map(async (data) => {
            console.log('tempResponseData :' + data);
          })
        );
    } else {
      let result: any = 'error, please login';
      return of(result);
    }
  }

  updateCategories(id: number, form: CategoryVM): Observable<any> {
    let UserToken: string = this.authService.getUserToken();
    if (this.authService.isLoggedIn() && UserToken) {
      let reqHeader = new HttpHeaders().set(
        'Authorization',
        'bearer ' + UserToken
      );
      return this.http
        .put(`${this._baseurl}/api/Categories/${id}`, form, {
          headers: reqHeader,
        })
        .pipe(
          map(async (data) => {
            console.log('tempResponseData :' + data);
          })
        );
    } else {
      let result: any = 'error, please login';
      return of(result);
    }
  }

  deleteCategories(id: number): Observable<any> {
    let UserToken: string = this.authService.getUserToken();
    if (this.authService.isLoggedIn() && UserToken) {
      let reqHeader = new HttpHeaders().set(
        'Authorization',
        'bearer ' + UserToken
      );
      return this.http
        .delete(`${this._baseurl}/api/Categories/${id}`, {
          headers: reqHeader,
        })
        .pipe(
          map(async (data) => {
            console.log('tempResponseData :' + data);
          })
        );
    } else {
      let result: any = 'error, please login';
      return of(result);
    }
  }

  updateShippings(id: number, form: ShippingsVM): Observable<any> {
    let UserToken: string = this.authService.getUserToken();
    if (this.authService.isLoggedIn() && UserToken) {
      let reqHeader = new HttpHeaders().set(
        'Authorization',
        'bearer ' + UserToken
      );
      return this.http
        .put(`${this._baseurl}/api/Shippings/${id}`, form, {
          headers: reqHeader,
        })
        .pipe(
          map(async (data) => {
            console.log('tempResponseData :' + data);
          })
        );
    } else {
      let result: any = 'error, please login';
      return of(result);
    }
  }

  postReview(review: ReviewsVM): Observable<any> {
    let UserToken: string = this.authService.getUserToken();
    if (this.authService.isLoggedIn() && UserToken) {
      let reqHeader = new HttpHeaders().set(
        'Authorization',
        'bearer ' + UserToken
      );
      return this.http
        .post<ReviewsVM>(`${this._baseurl}/api/Reviews`, review, {
          headers: reqHeader,
        })
        .pipe(
          map((data) => {
            console.log('tempResponseData :' + data);
          })
        );
    } else {
      let result: any = 'error, please login';
      return of(result);
    }
  }

  updateReview(id:number,review: ReviewsVM): Observable<any> {
    let UserToken: string = this.authService.getUserToken();
    if (this.authService.isLoggedIn() && UserToken) {
      let reqHeader = new HttpHeaders().set(
        'Authorization',
        'bearer ' + UserToken
      );
      return this.http
        .put<ReviewsVM>(`${this._baseurl}/api/Reviews/${id}`, review, {
          headers: reqHeader,
        })
        .pipe(
          map((data) => {
            console.log('tempResponseData :' + data);
          })
        );
    } else {
      let result: any = 'error, please login';
      return of(result);
    }
  }

  deleteReviews(id: number): Observable<any> {
    let UserToken: string = this.authService.getUserToken();
    if (this.authService.isLoggedIn() && UserToken) {
      let reqHeader = new HttpHeaders().set(
        'Authorization',
        'bearer ' + UserToken
      );
      return this.http
        .delete(`${this._baseurl}/api/Reviews/${id}`, {
          headers: reqHeader,
        })
        .pipe(
          map(async (data) => {
            console.log('tempResponseData :' + data);
          })
        );
    } else {
      let result: any = 'error, please login';
      return of(result);
    }
  }

  addWishlistItem(wishlist: WishlistsVM): Observable<any> {
    let UserToken: string = this.authService.getUserToken();
    if (this.authService.isLoggedIn() && UserToken) {
      let reqHeader = new HttpHeaders().set(
        'Authorization',
        'bearer ' + UserToken
      );
      return this.http
        .post<WishlistsVM>(`${this._baseurl}/api/Wishlists`, wishlist, {
          headers: reqHeader,
        })
        .pipe(
          map((data) => {
            console.log('tempResponseData :' + data);
          })
        );
    } else {
      let result: any = 'error, please login';
      return of(result);
    }
  }

  removeWishlistItem(id:number): Observable<any> {
    let UserToken: string = this.authService.getUserToken();
    if (this.authService.isLoggedIn() && UserToken) {
      let reqHeader = new HttpHeaders().set(
        'Authorization',
        'bearer ' + UserToken
      );
      return this.http
        .delete<WishlistsVM>(`${this._baseurl}/api/Wishlists/${id}`,  {
          headers: reqHeader,
        })
        .pipe(
          map((data) => {
            console.log('tempResponseData :' + data);
          })
        );
    } else {
      let result: any = 'error, please login';
      return of(result);
    }
  }

  sellerUserProductsOrders(): Observable<UsersProductsOrdersVM[]> {
    let UserToken: string = this.authService.getUserToken();
    if (this.authService.isLoggedIn() && UserToken) {
      let reqHeader = new HttpHeaders().set(
        'Authorization',
        'bearer ' + UserToken
      );
      return this.http
        .get<UsersProductsOrdersVMObjectResponse>(`${this._baseurl}/usrsprds/MyProductsOrders`, {
          headers: reqHeader,
        })
        .pipe(map((response) => response.usersProductsOrders.$values));
    } else {
      let result: UsersProductsOrdersVM[] = [];
      return of(result);
    }
  }
}


