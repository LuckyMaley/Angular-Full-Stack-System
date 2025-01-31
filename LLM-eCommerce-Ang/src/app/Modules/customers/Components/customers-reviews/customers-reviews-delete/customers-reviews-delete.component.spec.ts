import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CustomersReviewsDeleteComponent } from './customers-reviews-delete.component';

describe('CustomersReviewsDeleteComponent', () => {
  let component: CustomersReviewsDeleteComponent;
  let fixture: ComponentFixture<CustomersReviewsDeleteComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CustomersReviewsDeleteComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CustomersReviewsDeleteComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
