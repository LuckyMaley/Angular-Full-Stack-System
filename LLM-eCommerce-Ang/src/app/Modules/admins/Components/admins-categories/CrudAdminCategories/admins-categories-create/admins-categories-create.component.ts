import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { Category, CategoryVM } from 'src/app/Shared/Models/llm-ecommerce-efdb-vm';
import { AuthService } from 'src/app/Shared/Services/auth.service';
import { CrudService } from 'src/app/Shared/Services/crud.service';
import { HomeService } from 'src/app/Shared/Services/home.service';

@Component({
  selector: 'app-admins-categories-create',
  templateUrl: './admins-categories-create.component.html',
  styleUrls: ['./admins-categories-create.component.css']
})
export class AdminsCategoriesCreateComponent implements OnInit {
  formModel: CategoryVM = {
    name: ''
  };
  errorMessage: string | null = null;
  categories: Category[] = [];

  constructor(private router: Router, private homeService: HomeService, private authService: AuthService, private crudService: CrudService) {}

  ngOnInit(): void {
    this.homeService.getCategories().subscribe((data: Category[]) => {
      this.categories = data;
    });
  }

  onSubmit(form: NgForm) {
    this.formModel.name = form.value.name;
    if(this.formModel.name === ''){
      this.errorMessage = "Please enter a category";
      return;
    }
    this.crudService.addCategories(this.formModel).subscribe(
      response => { 
        console.log(response); 
        this.router.navigate(['/admin/categories']);
      },
      error => {
        console.error('Category submission failed:', error);
        this.errorMessage = 'Category submission failed. Please try again.';
      }
    );
  }

  Logout() {
    this.authService.logout();

    this.router.navigate(['/home']);
  }
}
