import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { SellersRoutingModule } from './sellers-routing.module';
import { SellersHomeComponent } from './Components/sellers-home/sellers-home.component';
import { SellersOrderDetailsComponent } from './Components/sellers-order-details/sellers-order-details.component';
import { SellersOrderHistoryComponent } from './Components/sellers-order-history/sellers-order-history.component';
import { SellersProductsComponent } from './Components/sellers-products/sellers-products.component';
import { SellersProfileComponent } from './Components/sellers-profile/sellers-profile.component';
import { SellersProductsDeleteComponent } from './Components/sellers-products/CrudSellerProducts/sellers-products-delete/sellers-products-delete.component';
import { SellersProductsCreateComponent } from './Components/sellers-products/CrudSellerProducts/sellers-products-create/sellers-products-create.component';
import { SellersProductsUpdateComponent } from './Components/sellers-products/CrudSellerProducts/sellers-products-update/sellers-products-update.component';
import { SellersDashboardComponent } from './Components/sellers-dashboard/sellers-dashboard.component';


@NgModule({
  declarations: [
    SellersHomeComponent,
    SellersOrderDetailsComponent,
    SellersOrderHistoryComponent,
    SellersProductsComponent,
    SellersProfileComponent,
    SellersProductsDeleteComponent,
    SellersProductsCreateComponent,
    SellersProductsUpdateComponent,
    SellersDashboardComponent,
  ],
  imports: [
    CommonModule,
    FormsModule,
    SellersRoutingModule
  ]
})
export class SellersModule { }
