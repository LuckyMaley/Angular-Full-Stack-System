import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Category, Product } from 'src/app/Shared/Models/llm-ecommerce-efdb-vm';
import { AuthService } from 'src/app/Shared/Services/auth.service';
import { CrudService } from 'src/app/Shared/Services/crud.service';
import { HomeService } from 'src/app/Shared/Services/home.service';

@Component({
  selector: 'app-sellers-products-delete',
  templateUrl: './sellers-products-delete.component.html',
  styleUrls: ['./sellers-products-delete.component.css']
})
export class SellersProductsDeleteComponent implements OnInit {
  errorMessage: string | null = null;
  categories: Category[] = [];
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
  idPassedIn: number = 0;

  constructor(private router: Router, private route: ActivatedRoute, private homeService: HomeService, private authService: AuthService, private crudService: CrudService) {}

  ngOnInit(): void {
    this.route.paramMap.subscribe(
      paramMap => {
        let stringId:any = paramMap.get('id');
        let id = Number.parseInt(stringId);
        this.idPassedIn = id;
        this.homeService.getProductsById(id).subscribe((data: Product) => {
          this.product = data;
        });
        this.homeService.getCategories().subscribe((data: Category[]) => {
          this.categories = data;
        });
  });
  }

  getCategoryName(id: number){
    const cat = this.categories.find(p => p.categoryId === id);
    return cat?.name;
  }


  onSubmit(id:number) {
    
    this.crudService.deleteProducts(id).subscribe(
      response => { 
        console.log(response);
        alert('Product deleted successfully!');
        this.router.navigate(['/seller/products']);
      },
      error => {
        console.error('Product deletion failed:', error);
        this.errorMessage = 'Product deletion failed. Please try again.';
      }
    );
  }

  Logout() {
    this.authService.logout();

    this.router.navigate(['/home']);
  }

}
