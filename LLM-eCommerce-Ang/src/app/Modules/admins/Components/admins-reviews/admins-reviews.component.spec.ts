import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminsReviewsComponent } from './admins-reviews.component';

describe('AdminsReviewsComponent', () => {
  let component: AdminsReviewsComponent;
  let fixture: ComponentFixture<AdminsReviewsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminsReviewsComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdminsReviewsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
