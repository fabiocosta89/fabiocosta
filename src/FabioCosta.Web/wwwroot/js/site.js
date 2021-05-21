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
            var $anchor = $(this);
            $('html, body').stop().animate({
                scrollTop: $($anchor.attr('href')).offset().top - 0
            }, 1000);
            event.preventDefault();
        });
    });


    // PROJECT SLIDE
    $('#project-slide').owlCarousel({
        loop: true,
        center: true,
        autoplayHoverPause: false,
        autoplay: true,
        margin: 40,
        responsiveClass: true,
        responsive: {
            0: {
                items: 1,
            },
            768: {
                items: 2,
            }
        }
    });

    // When the user scrolls down 80px from the top of the document, resize the navbar's padding
    window.onscroll = function () { scrollFunction() };

    function scrollFunction() {
        if (document.body.scrollTop > 80 || document.documentElement.scrollTop > 80) {
            document.getElementById("navbar").classList.add("navbar-small");
        } else {
            document.getElementById("navbar").classList.remove("navbar-small");
        }
    }

    // Back to top button
    var btn = $('#backTopButton');

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