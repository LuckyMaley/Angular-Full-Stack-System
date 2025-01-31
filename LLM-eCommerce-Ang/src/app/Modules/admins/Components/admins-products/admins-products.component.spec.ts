import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminsProductsComponent } from './admins-products.component';

describe('AdminsProductsComponent', () => {
  let component: AdminsProductsComponent;
  let fixture: ComponentFixture<AdminsProductsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminsProductsComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdminsProductsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
