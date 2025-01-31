import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { DashboardVM } from 'src/app/Shared/Models/llm-ecommerce-efdb-vm';
import { Roles, UserProfileVM } from 'src/app/Shared/Models/user-auth-vm';
import { AuthService } from 'src/app/Shared/Services/auth.service';
import { CrudService } from 'src/app/Shared/Services/crud.service';

@Component({
  selector: 'app-admins-dashboard',
  templateUrl: './admins-dashboard.component.html',
  styleUrls: ['./admins-dashboard.component.css']
})
export class AdminsDashboardComponent implements OnInit {
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
  dashboardVM: DashboardVM = { userProfile: this.userProfileData, customersOrders: [] };

  constructor(
    private authService: AuthService,
    private router: Router,
    private crudService: CrudService
  ) {}

  ngOnInit() {
        this.authService.getUserProfile().subscribe((data) => {
          this.dashboardVM.userProfile = data;
        });
        this.crudService.customerOrders().subscribe((data) => {
          this.dashboardVM.customersOrders = data;
        });
  }

  Logout() {
    this.authService.logout();

    this.router.navigate(['/home']);
  }
}
