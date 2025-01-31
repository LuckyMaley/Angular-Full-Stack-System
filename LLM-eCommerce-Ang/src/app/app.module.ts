import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { PageNotFoundComponent } from './Shared/Layout/page-not-found/page-not-found.component';
import { DefaultFooterComponent } from './Shared/Layout/default-footer/default-footer.component';
import { AboutUsComponent } from './Shared/Layout/about-us/about-us.component';
import { PrivacyInfoComponent } from './Shared/Layout/privacy-info/privacy-info.component';
import { DefaultHomeComponent } from './Guest/default-home/default-home.component';
import { MainNavigationComponent } from './Shared/Layout/main-navigation/main-navigation.component';
import { UserLoginComponent } from './Auth/user-login/user-login.component';
import { UserRegistrationComponent } from './Auth/user-registration/user-registration.component';
import { ShopComponent } from './Guest/shop/shop.component';
import { MensComponent } from './Guest/mens/mens.component';
import { WomensComponent } from './Guest/womens/womens.component';
import { ProductDetailsComponent } from './Guest/product-details/product-details.component';
import { WishlistComponent } from './Guest/wishlist/wishlist.component';
import { CartComponent } from './Guest/cart/cart.component';
import { UserProfileComponent } from './Auth/user-profile/user-profile.component';
import { SpecificUserFooterComponent } from './Shared/Layout/specific-user-footer/specific-user-footer.component';
import { CheckoutComponent } from './Guest/checkout/checkout.component';
import { OrderSuccessComponent } from './Guest/checkout/order-success/order-success.component';
import { ContactUsComponent } from './Shared/Layout/contact-us/contact-us.component';

@NgModule({
  declarations: [
    AppComponent,
    PageNotFoundComponent,
    DefaultFooterComponent,
    AboutUsComponent,
    PrivacyInfoComponent,
    DefaultHomeComponent,
    MainNavigationComponent,
    UserLoginComponent,
    UserRegistrationComponent,
    ShopComponent,
    MensComponent,
    WomensComponent,
    ProductDetailsComponent,
    WishlistComponent,
    CartComponent,
    UserProfileComponent,
    SpecificUserFooterComponent,
    CheckoutComponent,
    OrderSuccessComponent,
    ContactUsComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpClientModule,
    AppRoutingModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
