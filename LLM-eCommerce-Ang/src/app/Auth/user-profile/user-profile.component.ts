import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { UserProfileVM, Roles } from 'src/app/Shared/Models/user-auth-vm';
import { AuthService } from 'src/app/Shared/Services/auth.service';

@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.css']
})

export class UserProfileComponent {
  userRoles: Roles = {$id:'', $values:['']};
  
  userProfileData: UserProfileVM = {
    firstName: '',
    lastName: '',
    userName: '',
    email: '',
    address:'',
    phoneNumber:'',
    userRoles: this.userRoles
  }


  constructor( private  authService: AuthService, private router: Router) {}

  ngOnInit() {


     this.authService
      .getUserProfile()
      .subscribe(( data) => {
        console.log('data: ', data);
        this.userProfileData =  data;
      });

  }

}
