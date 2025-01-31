import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SellersDashboardComponent } from './sellers-dashboard.component';

describe('SellersDashboardComponent', () => {
  let component: SellersDashboardComponent;
  let fixture: ComponentFixture<SellersDashboardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SellersDashboardComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SellersDashboardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
