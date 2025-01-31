import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Shipping, ShippingsVM } from 'src/app/Shared/Models/llm-ecommerce-efdb-vm';
import { AuthService } from 'src/app/Shared/Services/auth.service';
import { CrudService } from 'src/app/Shared/Services/crud.service';
import { HomeService } from 'src/app/Shared/Services/home.service';

@Component({
  selector: 'app-admins-shipping-update',
  templateUrl: './admins-shipping-update.component.html',
  styleUrls: ['./admins-shipping-update.component.css']
})
export class AdminsShippingUpdateComponent implements OnInit {
  formModel: ShippingsVM ={
    shippingDate: new Date().toISOString(),
    shippingAddress: '',
    shippingMethod: '',
    trackingNumber: '',
    deliveryStatus: ''
  };
  
  errorMessage: string | null = null;
  idPassedIn: number = 0;

  constructor(private router: Router, private route: ActivatedRoute, private homeService: HomeService, private authService: AuthService, private crudService: CrudService) {}

  ngOnInit(): void {
    this.route.paramMap.subscribe(
      paramMap => {
        let stringId:any = paramMap.get('id');
        let id = Number.parseInt(stringId);
        this.idPassedIn = id;
        this.homeService.getShippingsById(id).subscribe((data: ShippingsVM) => {
          this.formModel = {
            shippingDate: data.shippingDate.toString().split('T')[0],
            shippingAddress: data.shippingAddress,
            shippingMethod: data.shippingMethod,
            trackingNumber: data.trackingNumber,
            deliveryStatus: data.deliveryStatus
          };
        });
    });
    
  }

  getDate(): string| null {
    return this.formModel.shippingDate;
  }

  setDate(date: string): void {
    this.formModel.shippingDate = date;
  }

  onSubmit(id: number, form: NgForm) {
    this.formModel.shippingAddress= form.value.shippingAddress;
    this.formModel.shippingMethod= form.value.shippingMethod;
    this.formModel.trackingNumber= form.value.trackingNumber;
    this.formModel.deliveryStatus= form.value.deliveryStatus;
    this.crudService.updateShippings(id, this.formModel).subscribe(
      response => { 
        console.log(response); 
        this.router.navigate(['/admin/shipping']);
      },
      error => {
        console.error('Shipping update failed:', error);
        this.errorMessage = 'Shipping update failed. Please try again.';
      }
    );
  }

  Logout() {
    this.authService.logout();

    this.router.navigate(['/home']);
  }

}
