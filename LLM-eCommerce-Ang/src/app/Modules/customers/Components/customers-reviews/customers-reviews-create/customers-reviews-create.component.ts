import { Component, OnInit } from '@angular/core';
import { Product, ReviewsVM } from '../../../../../Shared/Models/llm-ecommerce-efdb-vm';
import { HomeService } from 'src/app/Shared/Services/home.service';
import { AuthService } from 'src/app/Shared/Services/auth.service';
import { CrudService } from 'src/app/Shared/Services/crud.service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-customers-reviews-create',
  templateUrl: './customers-reviews-create.component.html',
  styleUrls: ['./customers-reviews-create.component.css']
})
export class CustomersReviewsCreateComponent implements OnInit {
  products: Product[] = [];
  selectedProduct: any; 
  review: ReviewsVM = { rating: 5, title: '', comment: '', productId: 0 };
  errorMessage: string | null = null;

  constructor(
    private homeService: HomeService,
    private auth: AuthService,
    private crudService: CrudService,
    private route: ActivatedRoute,
    private router: Router
  ){}

  ngOnInit() {
    this.loadProducts();
    
  }

  loadProducts() {
    this.homeService.getProducts().subscribe(data => {
      this.products = data;
      this.selectedProduct = this.products[0];
      console.log(this.selectedProduct)
    });
  }

  updateSelectedProduct(event: any) {
    const productId = event.target.value;
    console.log(this.products)
    this.selectedProduct = this.products.find(product => product.productId == productId);
    console.log(this.selectedProduct)
    this.review.productId = productId;
  }

  updateRating(event: any) {
    this.review.rating = parseInt(event.target.value, 10);
  }

  submitReview() {
    if (this.review.productId && this.review.rating && this.review.comment) {
      this.crudService.postReview(this.review).subscribe(response => {
        alert('Review created successfully!');
        this.router.navigate(['/customer/reviews']);
      }, error => {
        this.errorMessage ='Error submitting review';
      });
    } else {
      this.errorMessage ='Please fill all fields';
    }
  }

  Logout() {
    this.auth.logout();

    this.router.navigate(['/home']);
  }
}
