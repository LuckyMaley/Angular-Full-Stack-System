import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { UserLoginFormVM } from 'src/app/Shared/Models/user-auth-vm';
import { AuthService } from 'src/app/Shared/Services/auth.service';

@Component({
  selector: 'app-user-login',
  templateUrl: './user-login.component.html',
  styleUrls: ['./user-login.component.css']
})
export class UserLoginComponent {
  formModel: UserLoginFormVM = {
    userName: '',
    password: ''
  };
  
  errorMessage: string | null = null;

  constructor(private auth: AuthService, private router: Router) {

  }

  ngOnInit(){

  }


  pageRedirect(): any {
    if (this.auth.isLoggedIn()) {
      this.router.navigate(['/userprofile']);
    }

    this.auth.currentUserIsAdmin = this.auth.isAdmin();

    if (this.auth.currentUserIsAdmin) {
      this.router.navigate(['/admin']);
    }
    else { this.router.navigate(['/home']); }
  }

  onSubmit(form: NgForm) {
    //console.log(form.value);
    this.auth.userLogin(form.value.userName, form.value.password).subscribe((data) => {
      console.log(data);

      setTimeout(() => {
        console.log("Wait for 1 second");
        this.auth.currentUserIsAdmin = this.auth.isAdmin();

        if (this.auth.currentUserIsAdmin) {
          this.router.navigate(['/admin']);
        }
        else if(this.auth.isSeller())
          {this.router.navigate(['/seller']);}
        else if(this.auth.isCustomer())
          { this.router.navigate(['/home']); }
        else{
          this.errorMessage = "ERROR: Username or password not VALID.";
        }
      }, 1000);
    },
    error => {
      
    }
  );

  }
}
