var  homepageController = (function(){
   
    var carouselInit = function(element){
        $(element).owlCarousel({
            responsiveClass:true,    
            responsive:{
                0:{
                    loop: false,
                    margin: 20,
                    items: 1,
                    nav: false,
                    dots: true,
                    dotsEach: 1, 
                    stagePadding: 10
                },
                768:{
                    loop: false,
                    margin: 20,
                    items: 2,
                    nav: false,
                    dots: true,
                    dotsEach: 2, 
                    stagePadding: 20
                },
                1024:{
                    loop: false,
                    margin: 60,
                    items: 2,
                    nav: false,
                    dotsEach: 3, 
                    stagePadding: 10
                },
                1440:{
                    loop: false,
                    margin: 60,
                    items: 3,
                    nav: false,
                    dotsEach: 3, 
                    stagePadding: 10
                },
            }
        })
    }

    var toggleMenu = function(){
        $(this).toggleClass('active')
        $('ul.menu-list').toggleClass('active')
        if($(this).hasClass('active')){
            $(this).text('close');
        } else {
            $(this).text('menu');
        }
    }

    var zoomImage = function(){
        var urlImage = $(this).find('img').attr('src');
        //$(this).find('img').wrap('<span style="display:inline-block; width: 100%; height: 100%;"></span>')
        //.css('display', 'block')
        //.parent()
        //.zoom();
    }

    var fixNav = function(event){
        if(event.currentTarget.scrollY >= 50){
            $('ul.menu-list').addClass('fixed');
            $('section.menu-container').addClass('fixed');
        } else {
            $('ul.menu-list').removeClass('fixed');
            $('section.menu-container').removeClass('fixed');
        }
    }

    return{
        carouselInit: carouselInit,
        toggleMenu: toggleMenu,
        zoomImage: zoomImage,
        fixNav: fixNav
    }
})();

var homepageUI = (function(){
    var DOMElement = {
        carousel: '.owl-carousel',
        btnMenu: 'span.btn-menu',
        zoomImage: 'div.fotorama__active.fotorama__loaded.fotorama__loaded--img'
    }

    return{
        DOMElement: DOMElement
    }
})()

var homepage = (function(homepageCtrl, homepageUI){

    var DOMElement = homepageUI.DOMElement;

    var init = function(){
        homepageCtrl.carouselInit(DOMElement.carousel);
        $(document).on('click', DOMElement.btnMenu, homepageCtrl.toggleMenu );
        $(document).on('mouseover', DOMElement.zoomImage, homepageCtrl.zoomImage );
        $(window).on('scroll', homepageCtrl.fixNav)
    }   

    return{
        init: init
    }
})(homepageController, homepageUI)