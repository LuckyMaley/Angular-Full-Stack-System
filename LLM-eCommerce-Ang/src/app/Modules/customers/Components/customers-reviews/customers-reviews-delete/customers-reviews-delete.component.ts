import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CustomersReviewsVM, Product, Review } from 'src/app/Shared/Models/llm-ecommerce-efdb-vm';
import { AuthService } from 'src/app/Shared/Services/auth.service';
import { CrudService } from 'src/app/Shared/Services/crud.service';
import { HomeService } from 'src/app/Shared/Services/home.service';

@Component({
  selector: 'app-customers-reviews-delete',
  templateUrl: './customers-reviews-delete.component.html',
  styleUrls: ['./customers-reviews-delete.component.css']
})
export class CustomersReviewsDeleteComponent implements OnInit {
  errorMessage: string | null = null;
  review: Review | null = null;
  products: Product[] = [];
  cusReviews: CustomersReviewsVM[] = [];
  idPassedIn: number = 0;

  constructor(private router: Router, private route: ActivatedRoute, private homeService: HomeService, private authService: AuthService, private crudService: CrudService) {}

  ngOnInit(): void {
    this.route.paramMap.subscribe(
      paramMap => {
        let stringId:any = paramMap.get('id');
        let id = Number.parseInt(stringId);
        this.idPassedIn = id;
        this.homeService.getReviewById(id).subscribe((data: Review) => {
          this.review = data;
        });
  });
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

    this.homeService.getProducts().subscribe((data: Product[]) => {
      this.products = data;
    },
    error => {
      this.errorMessage = 'Error fetching reviews';
      console.error(error);
    });
  }


  onSubmit(id:number) {
    
    this.crudService.deleteReviews(id).subscribe(
      response => { 
        console.log(response); 
        alert('Review deleted successfully!');
        this.router.navigate(['/customer/reviews']);
      },
      error => {
        console.error('Review deletion failed:', error);
        this.errorMessage = 'Review deletion failed. Please try again.';
      }
    );
  }

  getAverageRating(productId: any): number {
    const productReviews = this.cusReviews.filter(r => r.productId === productId);
    return productReviews.length > 0 ? productReviews.reduce((sum, r) => sum + r.rating, 0) / productReviews.length : 0;
  }

  getProductName(productId: any){
    return this.products.find(p => p.productId === productId)?.name;
  }

  Logout() {
    this.authService.logout();

    this.router.navigate(['/home']);
  }

}
