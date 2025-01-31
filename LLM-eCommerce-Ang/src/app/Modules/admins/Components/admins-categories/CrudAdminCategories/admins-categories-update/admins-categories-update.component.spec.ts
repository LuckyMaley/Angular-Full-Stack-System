import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminsCategoriesUpdateComponent } from './admins-categories-update.component';

describe('AdminsCategoriesUpdateComponent', () => {
  let component: AdminsCategoriesUpdateComponent;
  let fixture: ComponentFixture<AdminsCategoriesUpdateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminsCategoriesUpdateComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdminsCategoriesUpdateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
