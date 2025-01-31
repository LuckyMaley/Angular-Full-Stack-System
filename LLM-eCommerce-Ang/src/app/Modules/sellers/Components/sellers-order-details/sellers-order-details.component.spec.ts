import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SellersOrderDetailsComponent } from './sellers-order-details.component';

describe('SellersOrderDetailsComponent', () => {
  let component: SellersOrderDetailsComponent;
  let fixture: ComponentFixture<SellersOrderDetailsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SellersOrderDetailsComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SellersOrderDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
