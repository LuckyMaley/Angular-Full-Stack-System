import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminsUsersCreateComponent } from './admins-users-create.component';

describe('AdminsUsersCreateComponent', () => {
  let component: AdminsUsersCreateComponent;
  let fixture: ComponentFixture<AdminsUsersCreateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminsUsersCreateComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdminsUsersCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
