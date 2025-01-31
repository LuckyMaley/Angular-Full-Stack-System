import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SellersProductsDeleteComponent } from './sellers-products-delete.component';

describe('SellersProductsDeleteComponent', () => {
  let component: SellersProductsDeleteComponent;
  let fixture: ComponentFixture<SellersProductsDeleteComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SellersProductsDeleteComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SellersProductsDeleteComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
