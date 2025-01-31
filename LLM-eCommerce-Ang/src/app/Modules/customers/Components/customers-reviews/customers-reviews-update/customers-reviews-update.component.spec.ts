import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CustomersReviewsUpdateComponent } from './customers-reviews-update.component';

describe('CustomersReviewsUpdateComponent', () => {
  let component: CustomersReviewsUpdateComponent;
  let fixture: ComponentFixture<CustomersReviewsUpdateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CustomersReviewsUpdateComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CustomersReviewsUpdateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
