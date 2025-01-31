import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Roles, UserAccountVM, UserProfileVM } from 'src/app/Shared/Models/user-auth-vm';
import { AuthService } from 'src/app/Shared/Services/auth.service';
import { CrudService } from 'src/app/Shared/Services/crud.service';

@Component({
  selector: 'app-admins-users',
  templateUrl: './admins-users.component.html',
  styleUrls: ['./admins-users.component.css']
})
export class AdminsUsersComponent implements OnInit{
  userRoles: Roles = {$id:'', $values:['']};
  roles: Roles[] = []
  userProfileData: UserProfileVM = {
    firstName: '',
    lastName: '',
    userName: '',
    email: '',
    address:'',
    phoneNumber:'',
    userRoles: this.userRoles
  }
  users: UserAccountVM[] = [];

  constructor( private  authService: AuthService, private crudService: CrudService, private router: Router) {}

  ngOnInit() {
    this.authService.getUserProfile().subscribe(
        data =>{
          this.userProfileData = data;
        }
      );
    this.crudService
    .getAllUserProfile()
    .subscribe((data) => {      
      this.users = data.filter(p =>  p.userName.trim().toLowerCase() !== this.userProfileData.userName.trim().toLowerCase());
    });

  }

  getUserRole(userName: string): string[] {
    const user = this.users.find(p => p.userName === userName);
    return user?.rolesHeld?.$values || [];
  }

  Logout() {
    this.authService.logout();

    this.router.navigate(['/home']);
  }
}
