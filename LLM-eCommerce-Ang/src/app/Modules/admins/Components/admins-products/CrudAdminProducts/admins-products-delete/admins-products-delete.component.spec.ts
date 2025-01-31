import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminsProductsDeleteComponent } from './admins-products-delete.component';

describe('AdminsProductsDeleteComponent', () => {
  let component: AdminsProductsDeleteComponent;
  let fixture: ComponentFixture<AdminsProductsDeleteComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminsProductsDeleteComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdminsProductsDeleteComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
