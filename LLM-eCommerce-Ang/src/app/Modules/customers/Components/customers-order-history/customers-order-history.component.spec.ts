import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CustomersOrderHistoryComponent } from './customers-order-history.component';

describe('CustomersOrderHistoryComponent', () => {
  let component: CustomersOrderHistoryComponent;
  let fixture: ComponentFixture<CustomersOrderHistoryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CustomersOrderHistoryComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CustomersOrderHistoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
