import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SellersHomeComponent } from './sellers-home.component';

describe('SellersHomeComponent', () => {
  let component: SellersHomeComponent;
  let fixture: ComponentFixture<SellersHomeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SellersHomeComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SellersHomeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
