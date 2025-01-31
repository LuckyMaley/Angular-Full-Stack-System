import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminsPaymentsPendingComponent } from './admins-payments-pending.component';

describe('AdminsPaymentsPendingComponent', () => {
  let component: AdminsPaymentsPendingComponent;
  let fixture: ComponentFixture<AdminsPaymentsPendingComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminsPaymentsPendingComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdminsPaymentsPendingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
