import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CustomersOrderDetailsComponent } from './customers-order-details.component';

describe('CustomersOrderDetailsComponent', () => {
  let component: CustomersOrderDetailsComponent;
  let fixture: ComponentFixture<CustomersOrderDetailsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CustomersOrderDetailsComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CustomersOrderDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
