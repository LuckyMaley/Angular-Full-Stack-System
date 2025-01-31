import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class NavigationService {
  initializeNavigation() {
    document.addEventListener('DOMContentLoaded', () => {
      const hamburger:any = document.querySelector('.hamburger');
      const Nav:any = document.querySelector('.mobile_nav');

      if (hamburger) {
        hamburger.addEventListener('click', () => {
         
          if (Nav.classList.contains('mobile_nav_hide')) {
            console.log("firing");
            Nav.classList.remove('mobile_nav_hide');
          } else {
            console.log("removing");
            Nav.classList.add('mobile_nav_hide');
          }
        });
      }

      const checkWindowSize = () => {
        if (window.innerWidth > 1070) {
          if (hamburger) {
            hamburger.style.display = 'none';
          }
        } else {
          if (hamburger) {
            hamburger.style.display = 'block';
          }
        }
      };

      checkWindowSize();

      window.addEventListener('resize', () => {
        checkWindowSize();
      });
    });
  }
}