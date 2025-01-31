import { Component, ElementRef, OnInit} from '@angular/core';
import { Category, ProductsVM} from 'src/app/Shared/Models/llm-ecommerce-efdb-vm';
import { HomeService } from '../../../../../../Shared/Services/home.service';
import { NgForm } from '@angular/forms';
import { AuthService } from 'src/app/Shared/Services/auth.service';
import { CrudService } from 'src/app/Shared/Services/crud.service';
import { Router } from '@angular/router';
import { InputNumService } from 'src/app/Shared/Services/input-num.service';
import { GlobalConstants } from 'src/app/global-constants';

@Component({
  selector: 'app-admins-products-create',
  templateUrl: './admins-products-create.component.html',
  styleUrls: ['./admins-products-create.component.css']
})

export class AdminsProductsCreateComponent implements OnInit {
  formModel: ProductsVM = {
    name: '',
    brand: '',
    description: '',
    type: '',
    price: 0,
    categoryId: 0,
    stockQuantity: 0,
    imageUrl: 'assets/images/no-image.png'
  };
  selectedImage: File | null = null;
  errorMessage: string | null = null;
  categories: Category[] = [];

  constructor(private router: Router, private homeService: HomeService, private authService: AuthService, private crudService: CrudService, private elementRef: ElementRef,
    private inputValidationService: InputNumService) {}

  ngOnInit(): void {
    const inputs = this.elementRef.nativeElement.querySelectorAll('.num-input');
    inputs.forEach((input: HTMLInputElement) => {
      input.addEventListener('input', (event) => {
        this.inputValidationService.validateNumberInput(event.target as HTMLInputElement);
      });
    });
    this.homeService.getCategories().subscribe((data: Category[]) => {
      this.categories = data;
    });
    this.formModel.categoryId = this.categories.length ? this.categories[0].categoryId : 0;
  }

  onFileSelected(event: any): void {
    const file = event.target.files[0];
    if (file) {
      this.selectedImage = file;
      const reader = new FileReader();
      reader.onload = (e: any) => {
        this.formModel.imageUrl = e.target.result;
      };
      reader.readAsDataURL(file);
    }
  }

  uploadImage(): void {
    if (this.selectedImage) {
      this.crudService.uploadImage(this.selectedImage)
    }
    else {
      this.errorMessage = 'No image selected for upload.';
      return;
    }
  }

  onUploadClick(): void {
    this.uploadImage();
  }

  onSubmit(form: NgForm) {
    this.formModel.name = form.value.name;
    this.formModel.brand = form.value.brand;
    this.formModel.description = form.value.description;
    this.formModel.type = form.value.type;
    this.formModel.price = form.value.price;
    this.formModel.categoryId = form.value.categoryId
    this.formModel.stockQuantity = form.value.stockQuantity
    if(this.formModel.categoryId === 0){
      this.errorMessage = "Please select a category";
      return;
    }
    this.uploadImage();
    this.crudService.addProducts(this.formModel).subscribe(
      response => { 
        console.log(response); 
        alert('Product created successfully!');
        this.router.navigate(['/admin/products']);
      },
      error => {
        console.error('Product submission failed:', error);
        this.errorMessage = 'Product submission failed. Please try again.';
      }
    );
  }

  Logout() {
    this.authService.logout();

    this.router.navigate(['/home']);
  }
}
