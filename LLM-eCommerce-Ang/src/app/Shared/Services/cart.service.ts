import { Injectable, Renderer2, RendererFactory2 } from '@angular/core';
import { CartItem } from '../Models/llm-ecommerce-efdb-vm';

@Injectable({
  providedIn: 'root'
})
export class CartService {

  private cartItems: CartItem[] = [];
  private renderer: Renderer2;

  constructor(rendererFactory: RendererFactory2) {
    this.renderer = rendererFactory.createRenderer(null, null);
    this.loadCartItems();
  }

  initializeAddToCartButtons(): void {
    const buttons = document.getElementsByClassName('cart-addBtn');
  
    if (buttons.length > 0) {
      for (let i = 0; i < buttons.length; i++) {
        const button = buttons[i] as HTMLButtonElement;
        if (button && !button.classList.contains('initialized')) {
          this.renderer.listen(button, 'click', () => {
            this.addItemToCart(button);
          });
          button.classList.add('initialized');
        }
      }
    }
  }

  private addItemToCart(button: HTMLButtonElement): void {
    button.textContent = 'Item added to Cart!';
    button.disabled = true;
    this.renderer.addClass(button, 'disabledBtn');
  }

  initializeAddToWishButtons(): void {
    const anchors = document.getElementsByClassName('wish-addBtn');
  
    if (anchors.length > 0) {
      for (let i = 0; i < anchors.length; i++) {
        const anchor = anchors[i] as HTMLAnchorElement;
        if (anchor && !anchor.classList.contains('initialized')) {
          this.renderer.listen(anchor, 'click', () => {
            this.toggleWishlistIcon(anchor);
          });
          anchor.classList.add('initialized');
        }
      }
    }
  }
  
  private toggleWishlistIcon(anchor: HTMLAnchorElement): void {
    const icon = anchor.querySelector('i.fa-heart') as HTMLElement;
    if (icon) {
      icon.classList.remove('fa-heart');
      icon.classList.add('fa-check');
    }
  }

  private loadCartItems(): void {
    const items = JSON.parse(localStorage.getItem('cart') || '[]');
    this.cartItems = items || [];
  }

  private saveCartItems(): void {
    localStorage.setItem('cart', JSON.stringify(this.cartItems));
  }

  getCartItems(): CartItem[] {
    return this.cartItems;
  }

  addToCart(newItem: CartItem): void {
    const existingItem = this.cartItems.find(item => item.id === newItem.id);
    if (existingItem) {
      existingItem.qty += newItem.qty;
    } else {
      this.cartItems.push(newItem);
    }
    this.saveCartItems();
  }

  deleteCartItem(itemId: string): void {
    this.cartItems = this.cartItems.filter(item => item.id !== itemId);
    this.saveCartItems();
  }

  clearCart(): void {
    this.cartItems = [];
    this.saveCartItems();
  }
}
