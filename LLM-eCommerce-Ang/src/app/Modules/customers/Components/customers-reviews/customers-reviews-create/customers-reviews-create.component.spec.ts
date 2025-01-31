import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CustomersReviewsCreateComponent } from './customers-reviews-create.component';

describe('CustomersReviewsCreateComponent', () => {
  let component: CustomersReviewsCreateComponent;
  let fixture: ComponentFixture<CustomersReviewsCreateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CustomersReviewsCreateComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CustomersReviewsCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
