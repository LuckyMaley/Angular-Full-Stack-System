import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminsPaymentsComponent } from './admins-payments.component';

describe('AdminsPaymentsComponent', () => {
  let component: AdminsPaymentsComponent;
  let fixture: ComponentFixture<AdminsPaymentsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminsPaymentsComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdminsPaymentsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
