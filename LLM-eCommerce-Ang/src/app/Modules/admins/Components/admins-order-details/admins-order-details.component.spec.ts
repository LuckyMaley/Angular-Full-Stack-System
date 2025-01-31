import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminsOrderDetailsComponent } from './admins-order-details.component';

describe('AdminsOrderDetailsComponent', () => {
  let component: AdminsOrderDetailsComponent;
  let fixture: ComponentFixture<AdminsOrderDetailsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminsOrderDetailsComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdminsOrderDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
