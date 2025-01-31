import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CustomersReviewsVM } from 'src/app/Shared/Models/llm-ecommerce-efdb-vm';
import { AuthService } from 'src/app/Shared/Services/auth.service';
import { CartService } from 'src/app/Shared/Services/cart.service';
import { HomeService } from 'src/app/Shared/Services/home.service';

@Component({
  selector: 'app-admins-reviews',
  templateUrl: './admins-reviews.component.html',
  styleUrls: ['./admins-reviews.component.css']
})
export class AdminsReviewsComponent implements OnInit{
  cusReviews: CustomersReviewsVM[] = [];
  errorMessage: string | null = null;
  idPassed: number = 0;

  constructor(private homeService: HomeService, private authService: AuthService, private router: Router,private route: ActivatedRoute, private cartService: CartService) {}

  ngOnInit(): void {
    this.fetchData();    
  }

  fetchData(): void {
    this.homeService.getAllCustomerReviews().subscribe((data: CustomersReviewsVM[]) => {
      this.cusReviews = data;
    },
    error => {
      this.errorMessage = 'Error fetching reviews';
      console.error(error);
    });
  }

  getAverageRating(productId: number): number {
    const productReviews = this.cusReviews.filter(r => r.productId === productId);
    return productReviews.length > 0 ? productReviews.reduce((sum, r) => sum + r.rating, 0) / productReviews.length : 0;
  }

  Logout() {
    this.authService.logout();

    this.router.navigate(['/home']);
  }
}
