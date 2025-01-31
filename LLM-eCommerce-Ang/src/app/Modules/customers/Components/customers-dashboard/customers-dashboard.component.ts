import { Component, OnInit } from '@angular/core';
import { CustomerDashboardVM } from 'src/app/Shared/Models/llm-ecommerce-efdb-vm';
import { Roles, UserProfileVM } from 'src/app/Shared/Models/user-auth-vm';
import { MyOrdersVM } from '../../../../Shared/Models/llm-ecommerce-efdb-vm';
import { AuthService } from 'src/app/Shared/Services/auth.service';
import { Router } from '@angular/router';
import { CrudService } from 'src/app/Shared/Services/crud.service';

@Component({
  selector: 'app-customers-dashboard',
  templateUrl: './customers-dashboard.component.html',
  styleUrls: ['./customers-dashboard.component.css']
})
export class CustomersDashboardComponent implements OnInit {
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
  dashboardVM: CustomerDashboardVM = { userProfile: this.userProfileData, myOrdersVM: [] };

  constructor(
    private authService: AuthService,
    private router: Router,
    private crudService: CrudService
  ) {}

  ngOnInit() {
        this.authService.getUserProfile().subscribe((data) => {
          this.dashboardVM.userProfile = data;
        });
        this.crudService.getMyOrders().subscribe((data) => {
          this.dashboardVM.myOrdersVM = data;
        });
  }

  Logout() {
    this.authService.logout();

    this.router.navigate(['/home']);
  }

}
