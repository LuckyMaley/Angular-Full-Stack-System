import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { SellerDashboardVM } from 'src/app/Shared/Models/llm-ecommerce-efdb-vm';
import { Roles, UserProfileVM } from 'src/app/Shared/Models/user-auth-vm';
import { AuthService } from 'src/app/Shared/Services/auth.service';
import { CrudService } from 'src/app/Shared/Services/crud.service';

@Component({
  selector: 'app-sellers-dashboard',
  templateUrl: './sellers-dashboard.component.html',
  styleUrls: ['./sellers-dashboard.component.css']
})
export class SellersDashboardComponent implements OnInit {
  userRoles: Roles = {$id:'', $values:[]};
  
  userProfileData: UserProfileVM = {
    firstName: '',
    lastName: '',
    userName: '',
    email: '',
    address:'',
    phoneNumber:'',
    userRoles: this.userRoles
  }
  dashboardVM: SellerDashboardVM = { userProfile: this.userProfileData, usersProductsOrders: [] };

  constructor(
    private authService: AuthService,
    private router: Router,
    private crudService: CrudService
  ) {}

  ngOnInit() {
        this.authService.getUserProfile().subscribe((data) => {
          this.dashboardVM.userProfile = data;
        });
        this.crudService.sellerUserProductsOrders().subscribe((data) => {
          this.dashboardVM.usersProductsOrders = data;
        });
  }

  Logout() {
    this.authService.logout();

    this.router.navigate(['/home']);
  }
}
