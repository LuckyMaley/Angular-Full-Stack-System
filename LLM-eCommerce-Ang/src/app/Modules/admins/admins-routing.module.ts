import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminsHomeComponent } from './Components/admins-home/admins-home.component';
import { AdminsProductsComponent } from './Components/admins-products/admins-products.component';
import { AdminsProductsCreateComponent } from './Components/admins-products/CrudAdminProducts/admins-products-create/admins-products-create.component';
import { AdminsProductsUpdateComponent } from './Components/admins-products/CrudAdminProducts/admins-products-update/admins-products-update.component';
import { AdminsProductsDeleteComponent } from './Components/admins-products/CrudAdminProducts/admins-products-delete/admins-products-delete.component';
import { AdminsOrderHistoryComponent } from './Components/admins-order-history/admins-order-history.component';
import { AdminsOrderDetailsComponent } from './Components/admins-order-details/admins-order-details.component';
import { AdminsUsersComponent } from './Components/admins-users/admins-users.component';
import { AdminsUsersCreateComponent } from './Components/admins-users/CrudAdminUsers/admins-users-create/admins-users-create.component';
import { AdminsUsersUpdateComponent } from './Components/admins-users/CrudAdminUsers/admins-users-update/admins-users-update.component';
import { AdminsUsersDeleteComponent } from './Components/admins-users/CrudAdminUsers/admins-users-delete/admins-users-delete.component';
import { AdminsProfileComponent } from './Components/admins-profile/admins-profile.component';
import { AdminsDashboardComponent } from './Components/admins-dashboard/admins-dashboard.component';
import { AdminsCategoriesComponent } from './Components/admins-categories/admins-categories.component';
import { AdminsCategoriesCreateComponent } from './Components/admins-categories/CrudAdminCategories/admins-categories-create/admins-categories-create.component';
import { AdminsCategoriesUpdateComponent } from './Components/admins-categories/CrudAdminCategories/admins-categories-update/admins-categories-update.component';
import { AdminsCategoriesDeleteComponent } from './Components/admins-categories/CrudAdminCategories/admins-categories-delete/admins-categories-delete.component';
import { AdminsShippingComponent } from './Components/admins-shipping/admins-shipping.component';
import { AdminsShippingUpdateComponent } from './Components/admins-shipping/CrudAdminShipping/admins-shipping-update/admins-shipping-update.component';
import { AdminsShippingPendingComponent } from './Components/admins-shipping/admins-shipping-pending/admins-shipping-pending.component';
import { AdminsReviewsComponent } from './Components/admins-reviews/admins-reviews.component';
import { AdminsWishlistsComponent } from './Components/admins-wishlists/admins-wishlists.component';
import { AdminsPaymentsComponent } from './Components/admins-payments/admins-payments.component';
import { AdminsPaymentsPendingComponent } from './Components/admins-payments/admins-payments-pending/admins-payments-pending.component';

const routes: Routes = [
  {
    path: '',
    component: AdminsHomeComponent,
    children: [
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
      { path: 'dashboard', component: AdminsDashboardComponent },
      
      { path: 'categories', component: AdminsCategoriesComponent },
      { path: 'categories/create', component: AdminsCategoriesCreateComponent },
      { path: `categories/update/:id`, component: AdminsCategoriesUpdateComponent },
      { path: `categories/delete/:id`, component: AdminsCategoriesDeleteComponent },

      { path: 'products', component: AdminsProductsComponent },
      { path: 'products/create', component: AdminsProductsCreateComponent },
      { path: `products/update/:id`, component: AdminsProductsUpdateComponent },
      { path: `products/delete/:id`, component: AdminsProductsDeleteComponent },

      { path: 'orders', component: AdminsOrderHistoryComponent },
      { path: `orders/:id`, component: AdminsOrderDetailsComponent },

      { path: 'shipping', component: AdminsShippingComponent },
      { path: 'shipping/pending', component: AdminsShippingPendingComponent },
      { path: `shipping/update/:id`, component: AdminsShippingUpdateComponent },

      { path: 'reviews', component: AdminsReviewsComponent },

      { path: 'wishlists', component: AdminsWishlistsComponent },

      { path: 'payments', component: AdminsPaymentsComponent },
      { path: 'payments/pending', component: AdminsPaymentsPendingComponent },

      { path: 'users', component: AdminsUsersComponent },
      { path: 'users/create', component: AdminsUsersCreateComponent },
      { path: `users/update/:id`, component: AdminsUsersUpdateComponent },
      { path: `users/delete/:id`, component: AdminsUsersDeleteComponent },

      { path: 'profile', component: AdminsProfileComponent },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminsRoutingModule { }
