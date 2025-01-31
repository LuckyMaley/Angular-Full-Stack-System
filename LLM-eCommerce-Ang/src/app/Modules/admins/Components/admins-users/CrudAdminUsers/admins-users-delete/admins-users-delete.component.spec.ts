import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminsUsersDeleteComponent } from './admins-users-delete.component';

describe('AdminsUsersDeleteComponent', () => {
  let component: AdminsUsersDeleteComponent;
  let fixture: ComponentFixture<AdminsUsersDeleteComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminsUsersDeleteComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdminsUsersDeleteComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
