import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminsUsersUpdateComponent } from './admins-users-update.component';

describe('AdminsUsersUpdateComponent', () => {
  let component: AdminsUsersUpdateComponent;
  let fixture: ComponentFixture<AdminsUsersUpdateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminsUsersUpdateComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdminsUsersUpdateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
