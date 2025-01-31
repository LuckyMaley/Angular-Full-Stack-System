import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { CustomersRoutingModule } from './customers-routing.module';
import { CustomersHomeComponent } from './Components/customers-home/customers-home.component';
import { CustomersOrderDetailsComponent } from './Components/customers-order-details/customers-order-details.component';
import { CustomersOrderHistoryComponent } from './Components/customers-order-history/customers-order-history.component';
import { CustomersProfileComponent } from './Components/customers-profile/customers-profile.component';
import { CustomersDashboardComponent } from './Components/customers-dashboard/customers-dashboard.component';
import { CustomersWishlistsComponent } from './Components/customers-wishlists/customers-wishlists.component';
import { CustomersReviewsComponent } from './Components/customers-reviews/customers-reviews.component';
import { CustomersReviewsCreateComponent } from './Components/customers-reviews/customers-reviews-create/customers-reviews-create.component';
import { CustomersReviewsDeleteComponent } from './Components/customers-reviews/customers-reviews-delete/customers-reviews-delete.component';
import { CustomersReviewsUpdateComponent } from './Components/customers-reviews/customers-reviews-update/customers-reviews-update.component';


@NgModule({
  declarations: [
    CustomersHomeComponent,
    CustomersOrderDetailsComponent,
    CustomersOrderHistoryComponent,
    CustomersProfileComponent,
    CustomersDashboardComponent,
    CustomersWishlistsComponent,
    CustomersReviewsComponent,
    CustomersReviewsCreateComponent,
    CustomersReviewsDeleteComponent,
    CustomersReviewsUpdateComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    CustomersRoutingModule
  ]
})
export class CustomersModule { }
