import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminsCategoriesCreateComponent } from './admins-categories-create.component';

describe('AdminsCategoriesCreateComponent', () => {
  let component: AdminsCategoriesCreateComponent;
  let fixture: ComponentFixture<AdminsCategoriesCreateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminsCategoriesCreateComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdminsCategoriesCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
