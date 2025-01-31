import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Category, CategoryVM } from 'src/app/Shared/Models/llm-ecommerce-efdb-vm';
import { AuthService } from 'src/app/Shared/Services/auth.service';
import { CrudService } from 'src/app/Shared/Services/crud.service';
import { HomeService } from 'src/app/Shared/Services/home.service';

@Component({
  selector: 'app-admins-categories-update',
  templateUrl: './admins-categories-update.component.html',
  styleUrls: ['./admins-categories-update.component.css']
})
export class AdminsCategoriesUpdateComponent implements OnInit {
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

  onSubmit(form: NgForm) {
    this.formModel.name = form.value.name;
    if(this.formModel.name === ''){
      this.errorMessage = "Please enter a category";
      return;
    }
    this.crudService.updateCategories(this.idPassedIn,this.formModel).subscribe(
      response => { 
        console.log(response); 
        this.router.navigate(['/admin/categories']);
      },
      error => {
        console.error('Category update failed:', error);
        this.errorMessage = 'Category update failed. Please try again.';
      }
    );
  }

  Logout() {
    this.authService.logout();

    this.router.navigate(['/home']);
  }

}
