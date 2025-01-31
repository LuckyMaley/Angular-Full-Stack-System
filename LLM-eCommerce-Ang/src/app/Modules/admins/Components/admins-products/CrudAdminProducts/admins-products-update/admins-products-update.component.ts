import { Component, ElementRef, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { GlobalConstants } from 'src/app/global-constants';
import { Category, Product, ProductsVM } from 'src/app/Shared/Models/llm-ecommerce-efdb-vm';
import { AuthService } from 'src/app/Shared/Services/auth.service';
import { CrudService } from 'src/app/Shared/Services/crud.service';
import { HomeService } from 'src/app/Shared/Services/home.service';
import { InputNumService } from 'src/app/Shared/Services/input-num.service';

@Component({
  selector: 'app-admins-products-update',
  templateUrl: './admins-products-update.component.html',
  styleUrls: ['./admins-products-update.component.css']
})
export class AdminsProductsUpdateComponent implements OnInit {
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
  product: Product = {
    $id: '',
    productId: 0,
    name: '',
    brand: '',
    description: '',
    type: '',
    price: 0,
    categoryId: 0,
    stockQuantity: 0,
    modifiedDate: '',
    imageUrl: '',
    category: {
      $id:'',
      categoryId:0,
      name:'',
      products:{
        $id: '',
        $values:[]
      }
    },
    efUserProducts: {
      $id: '',
      $values: [],
    },
    orderDetails: {
      $id: '',
      $values: [],
    },
    reviews: {
      $id: '',
      $values: []
    },
    wishlists: {
      $id: '',
      $values: []
    },
  };
  selectedImage: File | null = null;
  errorMessage: string | null = null;
  categories: Category[] = [];
  idPassedIn: number = 0;

  constructor(private router: Router, private route: ActivatedRoute, private homeService: HomeService, private authService: AuthService, private crudService: CrudService, private elementRef: ElementRef,
    private inputValidationService: InputNumService) {}

  ngOnInit(): void {
    this.route.paramMap.subscribe(
      paramMap => {
        let stringId:any = paramMap.get('id');
        let id = Number.parseInt(stringId);
        this.idPassedIn = id;
        this.homeService.getProductsById(id).subscribe((data: Product) => {
          this.product = data;
          this.formModel = {
            type: data.type,
            name: data.name,
            brand: data.brand,
            description: data.description,
            price: data.price,
            imageUrl: data.imageUrl,
            stockQuantity: data.stockQuantity,
            categoryId : data.categoryId
          };
          if(this.formModel.imageUrl){}
          else{
            this.formModel.imageUrl= 'assets/images/no-image.png';
          }
        });
        const inputs = this.elementRef.nativeElement.querySelectorAll('.num-input');
        inputs.forEach((input: HTMLInputElement) => {
          input.addEventListener('input', (event) => {
            this.inputValidationService.validateNumberInput(event.target as HTMLInputElement);
          });
        });
        this.homeService.getCategories().subscribe((data: Category[]) => {
          this.categories = data;
        });
    });
    
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
      this.crudService.uploadImage(this.selectedImage);
    }
  }

  onUploadClick(): void {
    this.uploadImage();
  }

  onSubmit(id: number, form: NgForm) {
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
    this.crudService.updateProducts(id, this.formModel).subscribe(
      response => { 
        console.log(response); 
        alert('Product update successfully!');
        this.router.navigate(['/admin/products']);
      },
      error => {
        console.error('Product update failed:', error);
        this.errorMessage = 'Product update failed. Please try again.';
      }
    );
  }

  Logout() {
    this.authService.logout();

    this.router.navigate(['/home']);
  }
}