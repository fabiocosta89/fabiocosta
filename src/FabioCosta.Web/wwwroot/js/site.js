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


    //// PROJECT SLIDE
    //$('#project-slide').owlCarousel({
    //    loop: true,
    //    center: true,
    //    autoplayHoverPause: false,
    //    autoplay: true,
    //    margin: 40,
    //    responsiveClass: true,
    //    responsive: {
    //        0: {
    //            items: 1,
    //        },
    //        768: {
    //            items: 2,
    //        }
    //    }
    //});

    const languagesTitle = "Languages";
    const languagesTitleIcon = "fas fa-laptop-code";
    const languages = [
        "HTML5 / CSS3",
        "Javascript / Typescript",
        "C#",
        "T-SQL"
    ];

    const frameworksTitle = "Frameworks";
    const frameworksTitleIcon = "fab fa-windows";
    const frameworks = [
        "ASP.NET WebForms / MVC / Core",
        "Entity Framework / Core",
        "REST API / SOAP",
        "React"
    ];

    const toolsTitle = "Tools/Others";
    const toolsTitleIcon = "fas fa-cog";
    const tools = [
        "Visual Studio",
        "Visual Studio Code",
        "SQL Server Management Studio",
        "Azure DevOps",
        "Git",
        "Wordpress / Woocommerce"
    ];

    // Donut chart
    const getOrCreateTooltip = (chart) => {
        let tooltipEl = chart.canvas.parentNode.querySelector('div');

        if (!tooltipEl) {
            tooltipEl = document.createElement('div');
            tooltipEl.classList.add("chart-tooltip");
            tooltipEl.onclick = () => {
                tooltipEl.style.opacity = 0;
            };
            tooltipEl.style.opacity = 1;
            tooltipEl.style.pointerEvents = 'none';
            chart.canvas.parentNode.appendChild(tooltipEl);
        }

        return tooltipEl;
    };

    const externalTooltipHandler = (context) => {
        // Tooltip Element
        const { chart, tooltip } = context;
        const tooltipEl = getOrCreateTooltip(chart);

        // Hide if no tooltip
        if (tooltip.opacity === 0) {
            tooltipEl.style.opacity = 0;
            return;
        }

        // Set Text
        if (tooltip.body) {
            // Remove old children
            while (tooltipEl.firstChild) {
                tooltipEl.firstChild.remove();
            }

            const bodyLines = tooltip.body[0].lines[0];

            let title = languagesTitle;
            let titleIcon = languagesTitleIcon;
            let itemsArray = languages;
            if (bodyLines.includes(frameworksTitle)) {
                title = frameworksTitle;
                titleIcon = frameworksTitleIcon;
                itemsArray = frameworks;
            } else if (bodyLines.includes(toolsTitle)) {
                title = toolsTitle;
                titleIcon = toolsTitleIcon;
                itemsArray = tools;
            }
            const h3 = document.createElement('h3');
            const icon = document.createElement('i');
            icon.className = titleIcon;
            h3.appendChild(icon);
            const titleNode = document.createTextNode(` ${title}`);
            h3.appendChild(titleNode);
            tooltipEl.appendChild(h3);

            const ul = document.createElement('ul');
            ul.classList.add("list-detail");
            itemsArray.forEach((value) => {
                const li = document.createElement('li');
                const liItem = document.createTextNode(value);
                li.appendChild(liItem);
                ul.appendChild(li);
            });
            tooltipEl.appendChild(ul);
        }

        const { offsetHeight, offsetWidth } = chart.canvas;

        // Display, position
        tooltipEl.style.opacity = 1;
        tooltipEl.style.top = (offsetHeight / 4) + 'px';
        tooltipEl.style.left = (offsetWidth / 2) + 'px';
    };

    new Chart(document.getElementById("doughnut-chart"), {
        type: 'doughnut',
        data: {
            labels: ["Languages", "Frameworks", "Tools/Others"],
            datasets: [
                {
                    label: "Skills",
                    backgroundColor: ["#4191EB", "#f1c111", "#666262"],
                    data: [1, 1, 1],
                    hoverOffset: 5
                }
            ]
        },
        options: {
            plugins: {
                tooltip: {
                    enabled: false,
                    position: 'nearest',
                    external: externalTooltipHandler
                }
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