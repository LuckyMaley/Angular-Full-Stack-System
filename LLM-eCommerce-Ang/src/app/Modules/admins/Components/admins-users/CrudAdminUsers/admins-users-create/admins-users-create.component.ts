import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { UserAccountVM, UserRegistrationFormVM, UserRegToApiVM } from 'src/app/Shared/Models/user-auth-vm';
import { AuthService } from 'src/app/Shared/Services/auth.service';
import { CrudService } from 'src/app/Shared/Services/crud.service';

@Component({
  selector: 'app-admins-users-create',
  templateUrl: './admins-users-create.component.html',
  styleUrls: ['./admins-users-create.component.css']
})
export class AdminsUsersCreateComponent implements OnInit{
  userCreated: boolean = false;

  formModel: UserRegistrationFormVM = {
    userName: '',
    email: '',
    firstName: '',
    lastName: '',
    address: '',
    phoneNumber: '',
    password: '',
    confirmPassword: '',
    userRole: '',
  };

  regform: UserRegToApiVM = {
    username: '',
    email: '',
    firstname: '',
    lastname: '',
    address: '',
    phoneNumber: '',
    password: '',
    role: '',
  };
  errorMessage: string | null = null;
  users: UserAccountVM[] = [];

  constructor(private authService: AuthService, private router: Router, private crudService: CrudService) {}
  ngOnInit(): void {
  this.crudService
  .getAllUserProfile()
  .subscribe((data) => {      
    this.users = data;
  });
  }

  getSelectedRoles(): string {
    return this.formModel.userRole;
  }
  
  updateRoles(selectedRole: string): void {
    this.formModel.userRole = selectedRole;
  }

  onSubmit(form: NgForm) {
    //console.log(form.value);
    this.regform.username = form.value.userName;
    this.regform.email = form.value.email;
    this.regform.firstname = form.value.firstName;
    this.regform.lastname = form.value.lastName;
    this.regform.address = form.value.address;
    this.regform.phoneNumber = form.value.phoneNumber;
    this.regform.password = form.value.password;
    this.regform.role = this.getSelectedRoles();
    const isTrue = this.users.find(p => p.userName === this.regform.username);
    if(isTrue){
      this.errorMessage = 'User create failed, Cannot create a user with a username that already exists. Please try again.';
      return;
    }
    if (form.value.password == form.value.confirmPassword) {
      this.authService.userRegistration(this.regform).subscribe((data) => {
        this.userCreated = true;
        console.log("reg data ", data);
         alert(
        'New User Created. Username:' +
          this.regform.username
      );
      this.router.navigate(['/admin/users']);
      },
      error => {
        console.error('User Create failed:', error);
        this.errorMessage = 'User create failed. Please try again.';
      });

     
    }
    else{
      this.errorMessage = 'Error password and confirm password must match';
    }
  }

  Logout() {
    this.authService.logout();

    this.router.navigate(['/home']);
  }
}
