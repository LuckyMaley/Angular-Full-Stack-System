import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminsShippingPendingComponent } from './admins-shipping-pending.component';

describe('AdminsShippingPendingComponent', () => {
  let component: AdminsShippingPendingComponent;
  let fixture: ComponentFixture<AdminsShippingPendingComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminsShippingPendingComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdminsShippingPendingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
