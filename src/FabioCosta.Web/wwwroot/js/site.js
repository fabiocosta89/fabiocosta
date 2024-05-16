$(function () {

    // MENU
    $('.nav-link').on('click', function () {
        $(".navbar-collapse").collapse('hide');
    });


    // AOS ANIMATION
    AOS.init({
        duration: 800,
        anchorPlacement: 'center-bottom'
    });


    // SMOOTH SCROLL
    $(function () {
        $('.smoothScroll').on('click', function (event) {
            const $anchor = $(this);
            $('html, body').stop().animate({
                scrollTop: $($anchor.attr('href')).offset().top - 0
            }, 1000);
            event.preventDefault();
        });
    });

    // When the user scrolls down 80px from the top of the document, resize the navbar's padding
    window.onscroll = function () { scrollFunction() };

    function scrollFunction() {
        if (window.scrollY > 80) {
            document.getElementById("navbar").classList.add("navbar-effects");
        } else {
            document.getElementById("navbar").classList.remove("navbar-effects");
        }
    }

    // Back to top button
    const btn = $('#backTopButton');

    $(window).scroll(function () {
        if ($(window).scrollTop() > 300) {
            btn.addClass('show');
        } else {
            btn.removeClass('show');
        }
    });

    btn.on('click', function (e) {
        e.preventDefault();
        $('html, body').animate({ scrollTop: 0 }, '300');
    });
});