import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Roles, UserAccountVM, UserAccountVMAdminSubmit, UserProfileVM, UserRegistrationFormVM, UserRegToApiVM } from 'src/app/Shared/Models/user-auth-vm';
import { AuthService } from 'src/app/Shared/Services/auth.service';
import { CrudService } from 'src/app/Shared/Services/crud.service';

@Component({
  selector: 'app-admins-users-update',
  templateUrl: './admins-users-update.component.html',
  styleUrls: ['./admins-users-update.component.css']
})
export class AdminsUsersUpdateComponent implements OnInit{
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
  idPassedIn: string = '';

  constructor( private  authService: AuthService, private crudService: CrudService, private route: ActivatedRoute, private router: Router) {}

  ngOnInit(): void {
    this.route.paramMap.subscribe((paramMap) => {
      let stringId: any = paramMap.get('id');
      this.idPassedIn = stringId;
      this.crudService
      .getSpecificUserProfile(stringId)
      .subscribe(( data) => {
        console.log('data: ', data);
        this.formModel= {
        userName: data.userName,
        email: data.email,
        firstName: data.firstName,
        lastName: data.lastName,
        address: data.address,
        phoneNumber: data.phoneNumber,
        role: data.rolesHeld.$values[0],
      };
      });
     
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
    if (form.value.password == form.value.confirmPassword) {
      this.crudService.updateUserProfile(this.formModel).subscribe((data) => {
        alert(
        'New User Updated. Username:' +
          this.formModel.userName
        );
        this.router.navigate(['/admin/users']);
      },
      error => {
        console.error('User update failed:', error);
        this.errorMessage = 'User update failed. Please try again.';
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
