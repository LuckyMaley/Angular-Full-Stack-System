import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CustomersProfileComponent } from './customers-profile.component';

describe('CustomersProfileComponent', () => {
  let component: CustomersProfileComponent;
  let fixture: ComponentFixture<CustomersProfileComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CustomersProfileComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CustomersProfileComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
