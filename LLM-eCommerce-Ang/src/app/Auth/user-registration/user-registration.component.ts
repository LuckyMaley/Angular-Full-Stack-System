import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import {
  UserAccountVM,
  UserRegToApiVM,
  UserRegistrationFormVM,
} from 'src/app/Shared/Models/user-auth-vm';
import { AuthService } from 'src/app/Shared/Services/auth.service';
import { CrudService } from 'src/app/Shared/Services/crud.service';

@Component({
  selector: 'app-user-registration',
  templateUrl: './user-registration.component.html',
  styleUrls: ['./user-registration.component.css'],
})
export class UserRegistrationComponent implements OnInit {
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
    userRole: 'Customer',
  };

  regform: UserRegToApiVM = {
    username: '',
    email: '',
    firstname: '',
    lastname: '',
    address: '',
    phoneNumber: '',
    password: '',
    role: 'Customer',
  };
  errorMessage: string | null = null;
  users: UserAccountVM | null = null;

  constructor(
    private auth: AuthService,
    private router: Router,
    private crudService: CrudService
  ) {}
  ngOnInit(): void {}
  onSubmit(form: NgForm) {
    //console.log(form.value);

    this.regform.username = form.value.userName;
    this.regform.email = form.value.email;
    this.regform.firstname = form.value.firstName;
    this.regform.lastname = form.value.lastName;
    this.regform.address = form.value.address;
    this.regform.phoneNumber = form.value.phoneNumber;
    this.regform.password = form.value.password;
    this.crudService
      .getUserRegProfile(this.regform.username)
      .subscribe((data) => {
        this.users = data;
        console.log(data);
        console.log(this.users);
        if (this.users.userName != '') {
          this.errorMessage =
            'User create failed, Cannot create a user with a username that already exists. Please try again.';
          return;
        } else {
          localStorage.setItem('onSubmitUserRegComponent', form.value);
          if (form.value.password == form.value.confirmPassword) {
            this.auth.userRegistration(this.regform).subscribe((data) => {
              this.userCreated = true;
              console.log('reg data ', data);
            });

            alert(
              'New User Created. Username:' +
                this.regform.username +
                ' \nRegistration successful.\nYou will be redirected to your home page!'
            );
            //this.router.navigate(['/login']);
            this.auth
              .userLogin(form.value.userName, form.value.password)
              .subscribe(
                (data) => {
                  console.log('data', data);

                  setTimeout(() => {
                    console.log('Wait for 1 second');
                    this.auth.currentUserIsAdmin = this.auth.isAdmin();

                    if (this.auth.currentUserIsAdmin) {
                      this.router.navigate(['/admin']);
                    } else if (this.auth.isSeller()) {
                      this.router.navigate(['/seller']);
                    } else if (this.auth.isCustomer()) {
                      this.router.navigate(['/customer']);
                    } else {
                      this.errorMessage =
                        'ERROR: Username or password not VALID.';
                    }
                  }, 1000);
                },
                (error) => {}
              );
          } else {
            this.errorMessage =
              'Error password and confirm password must match';
          }
        }
      });
  }
}
