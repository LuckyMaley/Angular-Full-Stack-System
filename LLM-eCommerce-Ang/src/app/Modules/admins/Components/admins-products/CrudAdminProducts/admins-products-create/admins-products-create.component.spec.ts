import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminsProductsCreateComponent } from './admins-products-create.component';

describe('AdminsProductsCreateComponent', () => {
  let component: AdminsProductsCreateComponent;
  let fixture: ComponentFixture<AdminsProductsCreateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminsProductsCreateComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdminsProductsCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
