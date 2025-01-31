import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CustomersHomeComponent } from './Components/customers-home/customers-home.component';
import { CustomersOrderHistoryComponent } from './Components/customers-order-history/customers-order-history.component';
import { CustomersOrderDetailsComponent } from './Components/customers-order-details/customers-order-details.component';
import { CustomersProfileComponent } from './Components/customers-profile/customers-profile.component';
import { CustomersDashboardComponent } from './Components/customers-dashboard/customers-dashboard.component';
import { CustomersReviewsComponent } from './Components/customers-reviews/customers-reviews.component';
import { CustomersReviewsCreateComponent } from './Components/customers-reviews/customers-reviews-create/customers-reviews-create.component';
import { CustomersReviewsUpdateComponent } from './Components/customers-reviews/customers-reviews-update/customers-reviews-update.component';
import { CustomersReviewsDeleteComponent } from './Components/customers-reviews/customers-reviews-delete/customers-reviews-delete.component';
import { CustomersWishlistsComponent } from './Components/customers-wishlists/customers-wishlists.component';

const routes: Routes = [
  {
    path: '',
    component: CustomersHomeComponent,
    children: [
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
      { path: 'dashboard', component: CustomersDashboardComponent },
      { path: 'orders', component: CustomersOrderHistoryComponent },
      { path: `orders/:id`, component: CustomersOrderDetailsComponent },

      { path: 'reviews', component: CustomersReviewsComponent },
      { path: `reviews/create`, component: CustomersReviewsCreateComponent },
      { path: `reviews/update/:id`, component: CustomersReviewsUpdateComponent },
      { path: `reviews/delete/:id`, component: CustomersReviewsDeleteComponent },

      { path: 'wishlists', component: CustomersWishlistsComponent },

      { path: 'profile', component: CustomersProfileComponent },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CustomersRoutingModule { }
