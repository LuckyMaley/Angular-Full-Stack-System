import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Roles, UserAccountVM, UserProfileVM } from 'src/app/Shared/Models/user-auth-vm';
import { AuthService } from 'src/app/Shared/Services/auth.service';
import { CrudService } from 'src/app/Shared/Services/crud.service';

@Component({
  selector: 'app-admins-users-delete',
  templateUrl: './admins-users-delete.component.html',
  styleUrls: ['./admins-users-delete.component.css']
})
export class AdminsUsersDeleteComponent implements OnInit{
  userRoles: Roles = {$id:'', $values:['']};
  
  userProfileData: UserAccountVM = {
    firstName: '',
    lastName: '',
    userName: '',
    email: '',
    address:'',
    phoneNumber:'',
    rolesHeld: this.userRoles
  }
  errorMessage: string | null = null;
  idPassedIn:string= '';

  constructor( private  authService: AuthService, private crudService: CrudService, private route: ActivatedRoute, private router: Router) {}

  ngOnInit() {
    
    this.route.paramMap.subscribe((paramMap) => {
      let stringId: any = paramMap.get('id');
      this.idPassedIn = stringId;
      this.crudService
      .getSpecificUserProfile(stringId)
      .subscribe(( data) => {
        console.log('data: ', data);
        this.userProfileData =  data;
      });
     
    });
  }

  onSubmit(id:string) {
    
    this.crudService
      .deleteUserProfileById(id)
      .subscribe(( data) => {
        console.log('data: ', data);
        this.router.navigate(['/admin/users']);
      },
      error => {
        console.error('User deletion failed:', error);
        this.errorMessage = 'User deletion failed. Please try again.';
      }
    );
  }

  getUserRole(): string[] {
    return this.userProfileData?.rolesHeld?.$values || [];
  }

  Logout() {
    this.authService.logout();

    this.router.navigate(['/home']);
  }

}
