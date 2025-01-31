import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminsCategoriesComponent } from './admins-categories.component';

describe('AdminsCategoriesComponent', () => {
  let component: AdminsCategoriesComponent;
  let fixture: ComponentFixture<AdminsCategoriesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminsCategoriesComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdminsCategoriesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
