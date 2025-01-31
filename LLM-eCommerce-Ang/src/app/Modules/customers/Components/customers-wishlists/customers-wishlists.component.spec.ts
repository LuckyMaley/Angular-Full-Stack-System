import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CustomersWishlistsComponent } from './customers-wishlists.component';

describe('CustomersWishlistsComponent', () => {
  let component: CustomersWishlistsComponent;
  let fixture: ComponentFixture<CustomersWishlistsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CustomersWishlistsComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CustomersWishlistsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
