import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Roles, UserAccountVMAdminSubmit } from 'src/app/Shared/Models/user-auth-vm';
import { AuthService } from 'src/app/Shared/Services/auth.service';
import { CrudService } from 'src/app/Shared/Services/crud.service';

@Component({
  selector: 'app-admins-profile',
  templateUrl: './admins-profile.component.html',
  styleUrls: ['./admins-profile.component.css']
})
export class AdminsProfileComponent implements OnInit{
  userRoles: Roles = {$id:'', $values:['']};
  roles: Roles[] = []
  formModel: UserAccountVMAdminSubmit = {
    firstName: '',
    lastName: '',
    userName: '',
    email: '',
    address:'',
    phoneNumber:'',
    role: ''
  }
  errorMessage: string | null = null;

  constructor( private  authService: AuthService, private crudService: CrudService, private route: ActivatedRoute, private router: Router) {}

  ngOnInit(): void {
      this.authService
      .getUserProfile()
      .subscribe(( data) => {
        this.formModel= {
        userName: data.userName,
        email: data.email,
        firstName: data.firstName,
        lastName: data.lastName,
        address: data.address,
        phoneNumber: data.phoneNumber,
        role: data.userRoles.$values[0],
      };
      });
     
  }

  getSelectedRoles(): string {
    return this.formModel.role;
  }
  
  updateRoles(selectedRole: string): void {
    this.formModel.role = selectedRole;
  }

  onSubmit(form: NgForm) {
    //console.log(form.value);
    this.formModel.userName = form.value.userName;
    this.formModel.email = form.value.email;
    this.formModel.firstName = form.value.firstName;
    this.formModel.lastName = form.value.lastName;
    this.formModel.address = form.value.address;
    this.formModel.phoneNumber = form.value.phoneNumber;
    this.formModel.role = form.value.role;
      this.crudService.updateMyProfile(this.formModel).subscribe((data) => {
        alert(
        'profile Updated. Username:' +
        data.userName
        );
        this.router.navigate(['/admin']);
      },
      error => {
        console.error('User update failed:', error);
        this.errorMessage = 'User update failed. Please try again.';
      });
    
  }

  Logout() {
    this.authService.logout();

    this.router.navigate(['/home']);
  }


}
