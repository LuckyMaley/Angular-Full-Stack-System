import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DefaultHomeComponent } from './Guest/default-home/default-home.component';
import { ShopComponent } from './Guest/shop/shop.component';
import { MensComponent } from './Guest/mens/mens.component';
import { WomensComponent } from './Guest/womens/womens.component';
import { ProductDetailsComponent } from './Guest/product-details/product-details.component';
import { WishlistComponent } from './Guest/wishlist/wishlist.component';
import { CartComponent } from './Guest/cart/cart.component';
import { FaqComponent } from './Shared/Layout/faq/faq.component';
import { AboutUsComponent } from './Shared/Layout/about-us/about-us.component';
import { PrivacyInfoComponent } from './Shared/Layout/privacy-info/privacy-info.component';
import { UserLoginComponent } from './Auth/user-login/user-login.component';
import { UserRegistrationComponent } from './Auth/user-registration/user-registration.component';
import { UserProfileComponent } from './Auth/user-profile/user-profile.component';
import { AdminGuard } from './Guards/Auth/admin.guard';
import { SellerGuard } from './Guards/Auth/seller.guard';
import { CustomerGuard } from './Guards/Auth/customer.guard';
import { PageNotFoundComponent } from './Shared/Layout/page-not-found/page-not-found.component';
import { CheckoutComponent } from './Guest/checkout/checkout.component';
import { OrderSuccessComponent } from './Guest/checkout/order-success/order-success.component';
import { ContactUsComponent } from './Shared/Layout/contact-us/contact-us.component';

const routes: Routes = [
  { path: 'home', component: DefaultHomeComponent },
  { path: 'shop', component: ShopComponent },
  { path: 'mens', component: MensComponent },
  { path: 'womens', component: WomensComponent },
  { path: 'product/:id', component: ProductDetailsComponent },
  { path: 'wishlist', component: WishlistComponent },
  { path: 'cart', component: CartComponent },
  { path: 'checkout', component: CheckoutComponent },
  { path: 'success/:id', component: OrderSuccessComponent },
  { path: 'faq', component: FaqComponent },
  { path: 'about', component: AboutUsComponent },
  { path: 'contact', component: ContactUsComponent },
  { path: 'privacy', component: PrivacyInfoComponent },



  { path: 'login', component: UserLoginComponent },
  { path: 'register', component: UserRegistrationComponent },
  { path: 'userprofile', component: UserProfileComponent },

  
  {
    path: 'admin',
    canActivate: [AdminGuard],
    loadChildren: () =>
      import('./Modules/admins/admins.module').then((m) => m.AdminsModule),
  },

  {
    path: 'seller',
    canActivate: [SellerGuard],
    loadChildren: () =>
      import('./Modules/sellers/sellers.module').then((m) => m.SellersModule),
  },

  {
    path: 'customer',
    canActivate: [CustomerGuard],
    loadChildren: () =>
      import('./Modules/customers/customers.module').then((m) => m.CustomersModule),
  }, 


  { path: '', redirectTo: '/home', pathMatch: 'full' },

  { path: '**', component: PageNotFoundComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
