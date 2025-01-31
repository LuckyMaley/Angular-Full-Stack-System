import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Category, UsersProductsVM } from 'src/app/Shared/Models/llm-ecommerce-efdb-vm';
import { AuthService } from 'src/app/Shared/Services/auth.service';
import { CrudService } from 'src/app/Shared/Services/crud.service';
import { HomeService } from 'src/app/Shared/Services/home.service';

@Component({
  selector: 'app-sellers-products',
  templateUrl: './sellers-products.component.html',
  styleUrls: ['./sellers-products.component.css']
})
export class SellersProductsComponent implements OnInit {
  usersProducts: UsersProductsVM[] = [];
  categories: Category[] = [];

  constructor(
    private authService: AuthService,
    private router: Router,
    private crudService: CrudService,
    private homeService: HomeService
  ) {}

  ngOnInit() {
    this.homeService.getCategories().subscribe((data: Category[]) => {
      this.categories = data;
    });
    this.crudService.sellersProducts().subscribe((data) => {
      console.log(data);
      this.usersProducts = data;
    });
  }

  getCategoryName(id: number){
    const cat = this.categories.find(p => p.categoryId === id);
    return cat?.name;
  }

  Logout() {
    this.authService.logout();

    this.router.navigate(['/home']);
  }

}
