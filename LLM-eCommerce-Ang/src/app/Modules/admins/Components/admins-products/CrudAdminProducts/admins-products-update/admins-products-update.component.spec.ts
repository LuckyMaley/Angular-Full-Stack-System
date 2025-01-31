import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminsProductsUpdateComponent } from './admins-products-update.component';

describe('AdminsProductsUpdateComponent', () => {
  let component: AdminsProductsUpdateComponent;
  let fixture: ComponentFixture<AdminsProductsUpdateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminsProductsUpdateComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdminsProductsUpdateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
