import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Product, ReviewsVM } from 'src/app/Shared/Models/llm-ecommerce-efdb-vm';
import { AuthService } from 'src/app/Shared/Services/auth.service';
import { CrudService } from 'src/app/Shared/Services/crud.service';
import { HomeService } from 'src/app/Shared/Services/home.service';

@Component({
  selector: 'app-customers-reviews-update',
  templateUrl: './customers-reviews-update.component.html',
  styleUrls: ['./customers-reviews-update.component.css']
})
export class CustomersReviewsUpdateComponent implements OnInit {
  products: Product[] = [];
  selectedProduct: any; 
  review: ReviewsVM = { rating: 5, title: '', comment: '', productId: 0 };
  errorMessage: string | null = null;
  idPassedIn:number = 0;

  constructor(
    private homeService: HomeService,
    private auth: AuthService,
    private crudService: CrudService,
    private route: ActivatedRoute,
    private router: Router
  ){}

  ngOnInit() {
    this.loadProducts();
    this.route.paramMap.subscribe(
      paramMap => {
        let stringId:any = paramMap.get('id');
        let id = Number.parseInt(stringId);
        this.idPassedIn = id;
  });

  
  }

  loadProducts() {
    this.homeService.getProducts().subscribe(data => {
      this.products = data;
      this.homeService.getReviewById(this.idPassedIn).subscribe((data) => {
        this.review.productId = data.productId;
        this.review.title = data.title;
        this.review.comment = data.comment;
        this.review.rating = data.rating;
        this.selectedProduct = this.products.find(p => p.productId == this.review.productId);
      },
      error => {
        this.errorMessage = 'Error fetching reviews';
        console.error(error);
      });
    });
  }

  updateSelectedProduct(event: any) {
    const productId = event.target.value;
    this.selectedProduct = this.products.find(product => product.productId == productId);
    this.review.productId = productId;
  }

  updateRating(event: any) {
    this.review.rating = parseInt(event.target.value, 10);
  }

  submitReview() {
    if (this.review.productId && this.review.rating && this.review.comment) {
      this.crudService.updateReview(this.idPassedIn,this.review).subscribe(response => {
        alert('Review updated successfully!');
        this.router.navigate(['/customer/reviews']);
      }, error => {
        this.errorMessage ='Error updating review';
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
