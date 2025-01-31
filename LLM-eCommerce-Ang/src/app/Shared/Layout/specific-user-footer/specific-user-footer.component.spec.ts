import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SpecificUserFooterComponent } from './specific-user-footer.component';

describe('SpecificUserFooterComponent', () => {
  let component: SpecificUserFooterComponent;
  let fixture: ComponentFixture<SpecificUserFooterComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SpecificUserFooterComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SpecificUserFooterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
