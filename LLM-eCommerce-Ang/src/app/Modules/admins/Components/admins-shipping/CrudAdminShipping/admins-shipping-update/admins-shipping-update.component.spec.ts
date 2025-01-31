import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminsShippingUpdateComponent } from './admins-shipping-update.component';

describe('AdminsShippingUpdateComponent', () => {
  let component: AdminsShippingUpdateComponent;
  let fixture: ComponentFixture<AdminsShippingUpdateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminsShippingUpdateComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdminsShippingUpdateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
