import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminsWishlistsComponent } from './admins-wishlists.component';

describe('AdminsWishlistsComponent', () => {
  let component: AdminsWishlistsComponent;
  let fixture: ComponentFixture<AdminsWishlistsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminsWishlistsComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdminsWishlistsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
