import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SellersProductsUpdateComponent } from './sellers-products-update.component';

describe('SellersProductsUpdateComponent', () => {
  let component: SellersProductsUpdateComponent;
  let fixture: ComponentFixture<SellersProductsUpdateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SellersProductsUpdateComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SellersProductsUpdateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
