import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { SellersHomeComponent } from './Components/sellers-home/sellers-home.component';
import { SellersProductsComponent } from './Components/sellers-products/sellers-products.component';
import { SellersProductsCreateComponent } from './Components/sellers-products/CrudSellerProducts/sellers-products-create/sellers-products-create.component';
import { SellersProductsUpdateComponent } from './Components/sellers-products/CrudSellerProducts/sellers-products-update/sellers-products-update.component';
import { SellersProductsDeleteComponent } from './Components/sellers-products/CrudSellerProducts/sellers-products-delete/sellers-products-delete.component';
import { SellersOrderDetailsComponent } from './Components/sellers-order-details/sellers-order-details.component';
import { SellersOrderHistoryComponent } from './Components/sellers-order-history/sellers-order-history.component';
import { SellersProfileComponent } from './Components/sellers-profile/sellers-profile.component';
import { SellersDashboardComponent } from './Components/sellers-dashboard/sellers-dashboard.component';


const routes: Routes = [
  {
    path: '',
    component: SellersHomeComponent,
    children: [
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
      { path: 'dashboard', component: SellersDashboardComponent },
      { path: 'products', component: SellersProductsComponent },
      { path: 'products/create', component: SellersProductsCreateComponent },
      { path: `products/update/:id`, component: SellersProductsUpdateComponent },
      { path: `products/delete/:id`, component: SellersProductsDeleteComponent },

      { path: 'orders', component: SellersOrderHistoryComponent },
      { path: `orders/:id`, component: SellersOrderDetailsComponent },

      { path: 'profile', component: SellersProfileComponent },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SellersRoutingModule { }
