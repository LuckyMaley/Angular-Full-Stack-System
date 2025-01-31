import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SellersOrderHistoryComponent } from './sellers-order-history.component';

describe('SellersOrderHistoryComponent', () => {
  let component: SellersOrderHistoryComponent;
  let fixture: ComponentFixture<SellersOrderHistoryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SellersOrderHistoryComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SellersOrderHistoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
