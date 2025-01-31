import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { map, catchError } from 'rxjs/operators';

import { GlobalConstants } from 'src/app/global-constants';
import {
  UserProfileVM,
  UserRegToApiVM,
  UserLoginFormVM,
  LoginResponseVM,
  LoginResponseMessageVM,
  UserProfileVMResponse,
} from '../Models/user-auth-vm';
import { CartService } from './cart.service';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private _baseurl: string = GlobalConstants._RestBaseURL;

  public currentUserIsAdmin: boolean = false;
  

  public userProfileInfo: UserProfileVM = {
    firstName: '',
    lastName: '',
    userName: '',
    email: '',
    address:'',
    phoneNumber:'',
    userRoles: {$id:'',$values:[]},
  };

  constructor(private http: HttpClient, private cart: CartService) {}

  getUserToken() {
    let currUserToken: string = localStorage.getItem('usertoken') || 'N/A';
    //console.log('getUserToken: currUserToken' + currUserToken);

    if (this.isLoggedIn() && currUserToken && currUserToken !== 'N/A') {
      return currUserToken;
    } else {
      return '';
    }
  }

  getUserProfile(): Observable<UserProfileVM> {
    let UserToken: string = this.getUserToken();
    //console.log(this.isLoggedIn());
    //console.log(UserToken);
    if (this.isLoggedIn() && UserToken) {
      let reqHeader = new HttpHeaders().set(
        'Authorization',
        'bearer ' + UserToken
      );
      return this.http.get<UserProfileVM>(this._baseurl + '/api/UserProfile', {
        headers: reqHeader,
      });
    } else {
      return of(this.userProfileInfo);
    }
  }


  getAdminAllUserProfile(): Observable<UserProfileVM[]> {
    let UserToken: string = this.getUserToken();
    //console.log(this.isLoggedIn());
    //console.log(UserToken);
    if (this.isLoggedIn() && UserToken) {
      let reqHeader = new HttpHeaders().set(
        'Authorization',
        'bearer ' + UserToken
      );
      return  this.http.get<UserProfileVMResponse>(`${this._baseurl}/api/UserProfile/AllUsers`, {
        headers: reqHeader
      }).pipe(map(response => response.$values));
    } else {
      return of([this.userProfileInfo]);
    }
  }

  userRegistration(newUserRegData: UserRegToApiVM): Observable<any> {
    let regUrl: string = this._baseurl + '/api/ApplicationUser/Register';
    let tempRegResponseData: any | undefined;

    console.log(
      'AuthService userRegistration data logObj.username:' +
        newUserRegData.username
    );

    return this.http.post(regUrl, newUserRegData).pipe(
      map(async (data) => {
        console.log('tempRegResponseData :' + data);

        tempRegResponseData = await data;
        console.log(
          'AuthService userRegistration data this.custModel:' +
            tempRegResponseData
        );

        return of(tempRegResponseData);
      }),
      catchError((err) => {
        console.error(err);
        tempRegResponseData = {
          userRegistrationErrorMessage:
            'Error: Unable to Register user: ' +
            newUserRegData.username +
            ' Server Error: ' +
            err,
        };
        localStorage.setItem(
          'userRegistrationErrorMessage',
          'Error: Unable to Register user: ' +
            newUserRegData.username +
            ' Server Error: ' +
            err
        );

        return of(tempRegResponseData);
      })
    );
  }

  adminUserCreate(newUserRegData: UserRegToApiVM): Observable<any> {
    let regUrl: string = this._baseurl + '/api/ApplicationUser/Register';
    let tempRegResponseData: any | undefined;

    console.log(
      'AuthService userRegistration data logObj.username:' +
        newUserRegData.username
    );

    return this.http.post(regUrl, newUserRegData).pipe(
      map(async (data) => {
        console.log('tempRegResponseData :' + data);

        tempRegResponseData = await data;
        console.log(
          'AuthService userRegistration data this.custModel:' +
            tempRegResponseData
        );

        return of(tempRegResponseData);
      }),
      catchError((err) => {
        console.error(err);
        tempRegResponseData = {
          userRegistrationErrorMessage:
            'Error: Unable to Register user: ' +
            newUserRegData.username +
            ' Server Error: ' +
            err,
        };
        localStorage.setItem(
          'userRegistrationErrorMessage',
          'Error: Unable to Register user: ' +
            newUserRegData.username +
            ' Server Error: ' +
            err
        );

        return of(tempRegResponseData);
      })
    );
  }

  userLogin(userName: string, userPw: string): Observable<any> {
    let loginUrl: string = this._baseurl + '/api/ApplicationUser/Login';

    let logObj: UserLoginFormVM = {
      userName: userName,
      password: userPw,
    };

    let tempLoginResponseData: any | undefined;
    let loggedInUser!: LoginResponseVM;
    let adminUsr: string = 'no';
    let currentUserLoginResponse: LoginResponseMessageVM = {
      userName: '',
      userRole: '',
      firstName: '',
      lastName: '',
      fullName: '',
      isAdminUserRole: false,
      message: '',
    };

    localStorage.clear();

    console.log(
      'AuthService userLogin data logObj.username:' + logObj.userName
    );
    localStorage.setItem('AuthService', logObj.userName);

    return this.http.post(loginUrl, logObj).pipe(
      map(async (data) => {
        tempLoginResponseData = await data;

        console.log('this.cusModel data:' + data);

        if (
          typeof tempLoginResponseData !== 'undefined' &&
          tempLoginResponseData !== null &&
          tempLoginResponseData
        ) {
          localStorage.setItem('user', JSON.stringify(tempLoginResponseData));

          const tmpResponseObj: LoginResponseVM = JSON.parse(
            localStorage.getItem('user') || '{}'
          );
          loggedInUser = tmpResponseObj;

          if (
            loggedInUser.roles.$values.find((role: string) => role === 'Administrator')
          ) {
            adminUsr = 'TRUE';
            currentUserLoginResponse.isAdminUserRole = true;
          } else {
            currentUserLoginResponse.isAdminUserRole = false;
          }

          currentUserLoginResponse.message = 'Success';
          currentUserLoginResponse.userName = loggedInUser.userName;
          currentUserLoginResponse.firstName = loggedInUser.firstName;
          currentUserLoginResponse.lastName = loggedInUser.lastName;
          currentUserLoginResponse.fullName = loggedInUser.firstName + ' ' + loggedInUser.lastName;

          currentUserLoginResponse.userRole = loggedInUser.roles.$values
              .filter((role: string) => typeof role !== undefined)
              .shift() || '';

          localStorage.setItem('isAdminUser', adminUsr);
          localStorage.setItem('usertoken', loggedInUser.token);
          localStorage.setItem('userRole', currentUserLoginResponse.userRole);
          localStorage.setItem('userName', currentUserLoginResponse.userName);
          localStorage.setItem('firstName', currentUserLoginResponse.firstName);
          localStorage.setItem('lastName', currentUserLoginResponse.lastName);
          localStorage.setItem('fullName', currentUserLoginResponse.fullName);
          localStorage.setItem('cart', JSON.stringify(this.cart.getCartItems()));
          
          console.log('currentUserLoginResponse :');
          console.log({ currentUserLoginResponse });

          return of(currentUserLoginResponse);
        } else {
          localStorage.setItem(
            'loginErrorMessage',
            'Unable to login user: ' + logObj.userName
          );
          currentUserLoginResponse.message =
            'Unable to login user: ' + logObj.userName;
          console.log(
            'currentUserLoginResponse data:' + currentUserLoginResponse.message
          );

          return of(currentUserLoginResponse);
        }
      }),
      catchError((err) => {
        console.error(err);
        localStorage.setItem(
          'loginErrorMessage',
          'Error: Unable to login user: ' +
            logObj.userName +
            ' Server Error: ' +
            err
        );
        currentUserLoginResponse.message =
          'Error: Unable to login user: ' +
          logObj.userName +
          ' Server Error: ' +
          err;
        console.log(
          'currentUserLoginResponse data:' + currentUserLoginResponse.message
        );

        return of(currentUserLoginResponse);
      })
    );
  }

  isLoggedIn() {
    let loggedInUser = localStorage.getItem('user');
    let currUser: string = localStorage.getItem('userName') || '';
    //console.log('isLoggedIn - loggedInUser: ' + loggedInUser);
    //console.log('isLoggedIn - currUser: ' + currUser);

    if (loggedInUser && currUser) {
      return true;
    } else {
      return false;
    }
  }


  getLoggedInUserName() {
    let currUser: string = localStorage.getItem('fullName') || '';
    return currUser;
  }

  

  isAdmin2() {
    let adminUser: string = localStorage.getItem('userRole') || 'NA';
    //console.log('adminUser: ' + adminUser);

    if (this.isLoggedIn() && adminUser && adminUser == 'Administrator') {
      return true;
    } else {
      return false;
    }
  }

  isAdmin() {
    let adminUser: string = localStorage.getItem('isAdminUser') || 'NA';
    //console.log('isAdmin - adminUser: ' + adminUser);

    if (this.isLoggedIn() && adminUser && adminUser == 'TRUE') {
      return true;
    } else {
      return false;
    }
  }

  isSeller() {
    let adminUser: string = localStorage.getItem('userRole') || 'NA';
    //console.log('adminUser: ' + adminUser);

    if (this.isLoggedIn() && adminUser && adminUser == 'Seller') {
      return true;
    } else {
      return false;
    }
  }

  isCustomer() {
    let adminUser: string = localStorage.getItem('userRole') || 'NA';
    //console.log('adminUser: ' + adminUser);

    if (this.isLoggedIn() && adminUser && adminUser == 'Customer') {
      return true;
    } else {
      return false;
    }
  }

  logout() {
    this.cart.clearCart();
    return localStorage.clear();
  }
}
