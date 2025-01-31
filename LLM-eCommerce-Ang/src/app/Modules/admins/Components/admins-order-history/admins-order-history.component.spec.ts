import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminsOrderHistoryComponent } from './admins-order-history.component';

describe('AdminsOrderHistoryComponent', () => {
  let component: AdminsOrderHistoryComponent;
  let fixture: ComponentFixture<AdminsOrderHistoryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminsOrderHistoryComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdminsOrderHistoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
