import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminsProfileComponent } from './admins-profile.component';

describe('AdminsProfileComponent', () => {
  let component: AdminsProfileComponent;
  let fixture: ComponentFixture<AdminsProfileComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminsProfileComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdminsProfileComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
