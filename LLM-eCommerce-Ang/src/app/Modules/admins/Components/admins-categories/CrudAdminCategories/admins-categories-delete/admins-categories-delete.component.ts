import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Category, CategoryVM } from 'src/app/Shared/Models/llm-ecommerce-efdb-vm';
import { AuthService } from 'src/app/Shared/Services/auth.service';
import { CrudService } from 'src/app/Shared/Services/crud.service';
import { HomeService } from 'src/app/Shared/Services/home.service';

@Component({
  selector: 'app-admins-categories-delete',
  templateUrl: './admins-categories-delete.component.html',
  styleUrls: ['./admins-categories-delete.component.css']
})
export class AdminsCategoriesDeleteComponent implements OnInit {
  formModel: CategoryVM = {
    name: ''
  };
  errorMessage: string | null = null;
  idPassedIn: number = 0;

  constructor(private router: Router, private route: ActivatedRoute, private homeService: HomeService, private authService: AuthService, private crudService: CrudService) {}

  ngOnInit(): void {
    this.route.paramMap.subscribe(
      paramMap => {
        let stringId:any = paramMap.get('id');
        let id = Number.parseInt(stringId);
        this.idPassedIn = id;
    this.homeService.getCategoriesById(id).subscribe((data: Category) => {;
      this.formModel = data;
    });
  });
  }

  onSubmit(id:number) {
    if(this.formModel.name === ''){
      this.errorMessage = "Please enter a category";
      return;
    }
    this.homeService.getProducts().subscribe((data) => {
      const prod = data.find(p => p.categoryId === id);
      if(prod){
        this.errorMessage = "Cannot delete a category with products, remove the link first before you can delete this category";
      return;
      }
    });
    this.crudService.deleteCategories(this.idPassedIn).subscribe(
      response => { 
        console.log(response); 
        this.router.navigate(['/admin/categories']);
      },
      error => {
        console.error('Category deletion failed:', error);
        this.errorMessage = 'Category deletion failed. Please try again.';
      }
    );
  }

  Logout() {
    this.authService.logout();

    this.router.navigate(['/home']);
  }
}
