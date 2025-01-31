import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PrivacyInfoComponent } from './privacy-info.component';

describe('PrivacyInfoComponent', () => {
  let component: PrivacyInfoComponent;
  let fixture: ComponentFixture<PrivacyInfoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PrivacyInfoComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PrivacyInfoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
