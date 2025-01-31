document.addEventListener('DOMContentLoaded', function () {
  const hamburger = document.querySelector('.hamburger')
  const Nav = document.querySelector('.mobile_nav')

  if (hamburger) {
    hamburger.addEventListener('click', () => {
      Nav.classList.toggle('mobile_nav_hide')
    })
  }

  const checkWindowSize = () => {
    if (window.innerWidth > 1070) {
      hamburger.style.display = 'none'
      if (!Nav.classList.contains('mobile_nav_hide')) {
        Nav.classList.toggle('mobile_nav_hide')
      }
    } else {
      document.addEventListener('scroll', handleScroll);
      hamburger.style.display = 'block'
    }
  }

  checkWindowSize()

  window.addEventListener('resize', function () {
    checkWindowSize()
  })
})




function debounce(func, wait) {
  let timeout;
  return function(...args) {
      const context = this;
      clearTimeout(timeout);
      timeout = setTimeout(() => func.apply(context, args), wait);
  };
}

const handleScroll = debounce(function() {
  const mobileNav = document.querySelector('.mobile_nav');
  if (window.scrollY > 0) {
      mobileNav.style.top = '0';
  } else {
      mobileNav.style.top = '120px';
  }
}, 10);


