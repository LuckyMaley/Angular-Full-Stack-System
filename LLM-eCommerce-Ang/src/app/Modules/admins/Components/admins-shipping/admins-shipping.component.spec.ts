import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminsShippingComponent } from './admins-shipping.component';

describe('AdminsShippingComponent', () => {
  let component: AdminsShippingComponent;
  let fixture: ComponentFixture<AdminsShippingComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminsShippingComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdminsShippingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
