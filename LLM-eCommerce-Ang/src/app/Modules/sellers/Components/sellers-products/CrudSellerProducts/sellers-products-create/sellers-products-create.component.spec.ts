import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SellersProductsCreateComponent } from './sellers-products-create.component';

describe('SellersProductsCreateComponent', () => {
  let component: SellersProductsCreateComponent;
  let fixture: ComponentFixture<SellersProductsCreateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SellersProductsCreateComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SellersProductsCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
