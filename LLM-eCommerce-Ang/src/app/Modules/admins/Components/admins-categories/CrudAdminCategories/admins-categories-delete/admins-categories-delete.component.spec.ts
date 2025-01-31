import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminsCategoriesDeleteComponent } from './admins-categories-delete.component';

describe('AdminsCategoriesDeleteComponent', () => {
  let component: AdminsCategoriesDeleteComponent;
  let fixture: ComponentFixture<AdminsCategoriesDeleteComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminsCategoriesDeleteComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdminsCategoriesDeleteComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
