import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AdminUserProductsVM, Category } from 'src/app/Shared/Models/llm-ecommerce-efdb-vm';
import { AuthService } from 'src/app/Shared/Services/auth.service';
import { CrudService } from 'src/app/Shared/Services/crud.service';
import { HomeService } from 'src/app/Shared/Services/home.service';

@Component({
  selector: 'app-admins-products',
  templateUrl: './admins-products.component.html',
  styleUrls: ['./admins-products.component.css']
})
export class AdminsProductsComponent implements OnInit {
  usersProducts: AdminUserProductsVM[] = [];
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
    this.crudService.adminsProducts().subscribe((data) => {
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
