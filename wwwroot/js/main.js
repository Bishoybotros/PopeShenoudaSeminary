/**
 * ---------------------------------------------
 * Initialize Plyr Video Player
 * ---------------------------------------------
 * - Targets element with ID #player
 * - Forces 16:9 aspect ratio
 * - Enables YouTube privacy-enhanced mode
 * - Disables related videos & info
 */
const player = new Plyr('#player', {
    ratio: '16:9',
    youtube: {
        noCookie: true,
        rel: 0,
        showinfo: 0
    }
});


/**
 * ---------------------------------------------
 * Initialize Owl Carousel
 * ---------------------------------------------
 * - Enables RTL support
 * - Autoplay with pause on hover
 * - Responsive items & navigation
 */
$(function () {
    $('.owl-carousel').owlCarousel({
        loop: true,
        margin: 24,
        nav: true,
        rtl: true,
        responsiveClass: true,
        responsive: {
            0: {
                items: 1,
            },
            600: {
                items: 2,
            },
            1000: {
                items: 4,
            }
        }
    });
});


/**
 * ---------------------------------------------
 * Scroll Progress Bar (jQuery)
 * ---------------------------------------------
 * - Calculates scroll percentage
 * - Updates width of #scroll-progress element
 */
$(window).on("scroll", function () {
    const scrollTop = $(window).scrollTop();
    const docHeight = $(document).height() - $(window).height();
    const scrollPercent = (scrollTop / docHeight) * 100;

    $("#scroll-progress").css("width", scrollPercent + "%");
});


/**
 * ---------------------------------------------
 * Initialize Lenis Smooth Scroll
 * ---------------------------------------------
 * - Smooth scrolling enabled
 * - Scroll disabled until page is fully loaded
 */
const lenis = new Lenis({
    smooth: true,
    lerp: 0.08
});

// Disable scrolling during preloader
lenis.stop();


/**
 * ---------------------------------------------
 * Lenis RAF Loop
 * ---------------------------------------------
 * - Keeps Lenis in sync with requestAnimationFrame
 */
function raf(time) {
    lenis.raf(time);
    requestAnimationFrame(raf);
}
requestAnimationFrame(raf);


/**
 * ---------------------------------------------
 * Preloader Fade Out on Page Load
 * ---------------------------------------------
 * - Fades preloader after full page load
 * - Enables scrolling after fade animation
 */
window.addEventListener('load', () => {
    const preloader = document.querySelector('.preloader');

    setTimeout(() => {
        preloader.classList.add('is-loaded');
        lenis.start(); // Enable scroll after fade
    }, 300);
});


$(window).scroll(function () {
    if ($(this).scrollTop() > 300) {
        $('#backToTop').fadeIn();
    } else {
        $('#backToTop').fadeOut();
    }
});

$('#backToTop').click(function () {
    $('html, body').animate({ scrollTop: 0 }, 600);
});