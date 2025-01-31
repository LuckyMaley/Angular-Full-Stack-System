import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { AdminsRoutingModule } from './admins-routing.module';
import { AdminsHomeComponent } from './Components/admins-home/admins-home.component';
import { AdminsOrderDetailsComponent } from './Components/admins-order-details/admins-order-details.component';
import { AdminsOrderHistoryComponent } from './Components/admins-order-history/admins-order-history.component';
import { AdminsProductsComponent } from './Components/admins-products/admins-products.component';
import { AdminsProfileComponent } from './Components/admins-profile/admins-profile.component';
import { AdminsUsersComponent } from './Components/admins-users/admins-users.component';
import { AdminsUsersUpdateComponent } from './Components/admins-users/CrudAdminUsers/admins-users-update/admins-users-update.component';
import { AdminsUsersDeleteComponent } from './Components/admins-users/CrudAdminUsers/admins-users-delete/admins-users-delete.component';
import { AdminsUsersCreateComponent } from './Components/admins-users/CrudAdminUsers/admins-users-create/admins-users-create.component';
import { AdminsProductsCreateComponent } from './Components/admins-products/CrudAdminProducts/admins-products-create/admins-products-create.component';
import { AdminsProductsDeleteComponent } from './Components/admins-products/CrudAdminProducts/admins-products-delete/admins-products-delete.component';
import { AdminsProductsUpdateComponent } from './Components/admins-products/CrudAdminProducts/admins-products-update/admins-products-update.component';
import { AdminsDashboardComponent } from './Components/admins-dashboard/admins-dashboard.component';
import { AdminsCategoriesComponent } from './Components/admins-categories/admins-categories.component';
import { AdminsCategoriesCreateComponent } from './Components/admins-categories/CrudAdminCategories/admins-categories-create/admins-categories-create.component';
import { AdminsCategoriesDeleteComponent } from './Components/admins-categories/CrudAdminCategories/admins-categories-delete/admins-categories-delete.component';
import { AdminsCategoriesUpdateComponent } from './Components/admins-categories/CrudAdminCategories/admins-categories-update/admins-categories-update.component';
import { AdminsShippingComponent } from './Components/admins-shipping/admins-shipping.component';
import { AdminsShippingUpdateComponent } from './Components/admins-shipping/CrudAdminShipping/admins-shipping-update/admins-shipping-update.component';
import { AdminsShippingPendingComponent } from './Components/admins-shipping/admins-shipping-pending/admins-shipping-pending.component';
import { AdminsReviewsComponent } from './Components/admins-reviews/admins-reviews.component';
import { AdminsWishlistsComponent } from './Components/admins-wishlists/admins-wishlists.component';
import { AdminsPaymentsComponent } from './Components/admins-payments/admins-payments.component';
import { AdminsPaymentsPendingComponent } from './Components/admins-payments/admins-payments-pending/admins-payments-pending.component';


@NgModule({
  declarations: [
    AdminsHomeComponent,
    AdminsOrderDetailsComponent,
    AdminsOrderHistoryComponent,
    AdminsProductsComponent,
    AdminsProfileComponent,
    AdminsUsersComponent,
    AdminsUsersUpdateComponent,
    AdminsUsersDeleteComponent,
    AdminsUsersCreateComponent,
    AdminsProductsCreateComponent,
    AdminsProductsDeleteComponent,
    AdminsProductsUpdateComponent,
    AdminsDashboardComponent,
    AdminsCategoriesComponent,
    AdminsCategoriesCreateComponent,
    AdminsCategoriesDeleteComponent,
    AdminsCategoriesUpdateComponent,
    AdminsShippingComponent,
    AdminsShippingUpdateComponent,
    AdminsShippingPendingComponent,
    AdminsReviewsComponent,
    AdminsWishlistsComponent,
    AdminsPaymentsComponent,
    AdminsPaymentsPendingComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    AdminsRoutingModule
  ]
})
export class AdminsModule { }
