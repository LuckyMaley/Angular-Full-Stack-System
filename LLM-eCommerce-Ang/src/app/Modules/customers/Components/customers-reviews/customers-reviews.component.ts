import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CustomersReviewsVM, Product } from 'src/app/Shared/Models/llm-ecommerce-efdb-vm';
import { AuthService } from 'src/app/Shared/Services/auth.service';
import { HomeService } from 'src/app/Shared/Services/home.service';

@Component({
  selector: 'app-customers-reviews',
  templateUrl: './customers-reviews.component.html',
  styleUrls: ['./customers-reviews.component.css']
})
export class CustomersReviewsComponent implements OnInit{
  cusReviews: CustomersReviewsVM[] = [];
  errorMessage: string | null = null;
  idPassed: number = 0;
  products: Product[] = [];

  constructor(private homeService: HomeService, private authService: AuthService, private router: Router,private route: ActivatedRoute) {}

  ngOnInit(): void {
    this.fetchData();    
  }

  fetchData(): void {
    this.homeService.getMyReviews().subscribe((data: CustomersReviewsVM[]) => {
      this.cusReviews = data;
    },
    error => {
      this.errorMessage = 'Error fetching reviews';
      console.error(error);
    });

    this.homeService.getProducts().subscribe(data => {
      this.products = data;
   },
   error => {
     this.errorMessage = 'Error fetching products';
     console.error(error);
   });
  }

  getAverageRating(productId: number): number {
    const productReviews = this.cusReviews.filter(r => r.productId === productId);
    return productReviews.length > 0 ? productReviews.reduce((sum, r) => sum + r.rating, 0) / productReviews.length : 0;
  }

  getProductImg(prodId:number){
    return this.products.find(product => product.productId == prodId)?.imageUrl;
  }

  Logout() {
    this.authService.logout();

    this.router.navigate(['/home']);
  }

}
