import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CustomersReviewsComponent } from './customers-reviews.component';

describe('CustomersReviewsComponent', () => {
  let component: CustomersReviewsComponent;
  let fixture: ComponentFixture<CustomersReviewsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CustomersReviewsComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CustomersReviewsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
