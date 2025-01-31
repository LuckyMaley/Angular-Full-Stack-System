import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Category, Product } from 'src/app/Shared/Models/llm-ecommerce-efdb-vm';
import { AuthService } from 'src/app/Shared/Services/auth.service';
import { HomeService } from 'src/app/Shared/Services/home.service';

@Component({
  selector: 'app-admins-categories',
  templateUrl: './admins-categories.component.html',
  styleUrls: ['./admins-categories.component.css']
})
export class AdminsCategoriesComponent implements OnInit {
  categories: Category[] = [];
  products: Product[] = [];

  constructor(
    private authService: AuthService,
    private router: Router,
    private homeService: HomeService
  ) {}

  ngOnInit() {
    this.homeService.getCategories().subscribe((data: Category[]) => {
      this.categories = data;
    });

    this.homeService.getProducts().subscribe((data: Product[]) => {
      this.products = data;
    });
  }

  getProductCount(id:number){
    return this.products.filter(p => p.categoryId === id).length;
  }

  Logout() {
    this.authService.logout();

    this.router.navigate(['/home']);
  }

}
