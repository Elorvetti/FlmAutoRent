var  homepageController = (function(){
   
    var carouselInit = function(){
        if($('div#preview').attr('device') === 'mobile'){
            $(".owl-carousel").owlCarousel('destroy'); 
            $(".owl-carousel").owlCarousel({
                loop: false,
                margin: 20,
                items: 1,
                nav: false,
                dots: true,
                dotsEach: 1, 
                stagePadding: 10
            });
        } else if($('div#preview').attr('device') === 'tablet'){
            $(".owl-carousel").owlCarousel('destroy'); 
            $(".owl-carousel").owlCarousel({
                loop: false,
                margin: 20,
                items: 2,
                nav: false,
                dots: true,
                dotsEach: 2, 
                stagePadding: 20
            });
        } else {
            $(".owl-carousel").owlCarousel('destroy'); 
            $(".owl-carousel").owlCarousel({
                loop: false,
                margin: 60,
                items: 3,
                nav: false,
                dotsEach: 3, 
                stagePadding: 10
            });
        }
    }

    var setDevice = function(){
        $('span.step').removeClass('active');
        $(this).addClass('active');

        var device = $(this).attr('device');
        $('div#preview').attr('device', device);
        carouselInit();
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

    var displayModalHeader = function(){
        $('div.modal').addClass('active');
        $('section#header-image').addClass('active');
    }

    var displayModalPresentation = function(){
        $('div.modal').addClass('active');
        $('section#presentation-image').addClass('active');
        $(window).scrollTop(0);
    }

    var closeModal = function(){
        $('div.modal').removeClass('active');
        $('section#header-image').removeClass('active');
        $('section#presentation-image').removeClass('active');
    }

    var changeHeaderImage = function(){
        var imagePath = $(this).closest('tr').find('img').attr('src');
        $('section#header').css('background-image', 'url("'+ imagePath + '")')
    }
    
    var changePresentationImage = function(){
        var imagePath = $(this).closest('tr').find('img').attr('src');
        $('section#presentation').css('background-image', 'url("'+ imagePath + '")')
    }


    return{
        carouselInit: carouselInit,
        setDevice: setDevice,
        toggleMenu: toggleMenu,
        displayModalHeader: displayModalHeader,
        displayModalPresentation: displayModalPresentation,
        closeModal: closeModal,
        changeHeaderImage: changeHeaderImage,
        changePresentationImage: changePresentationImage
    }
})();

var homepageUI = (function(){
    var DOMElement = {
        carousel: '.owl-carousel',
        btnDevice: 'span.step[device]',
        btnMenu: 'span.btn-menu',
        btnEditHeaderImage: 'span.btn.btn-header-image',
        btnEditPresentationImage: 'span.btn.btn-presentation-image',
        btnCloseModal: 'span.btn.btn-close-modal',
        btnRadioHeaderImage: 'section#header-image input[type="radio"]',
        btnRadioPresentationImage: 'section#presentation-image input[type="radio"]',
    }

    return{
        DOMElement: DOMElement
    }
})()

var homepage = (function(homepageCtrl, homepageUI){

    var DOMElement = homepageUI.DOMElement;

    var init = function(){
        homepageCtrl.carouselInit(DOMElement.carousel);
        $(document).on('click', DOMElement.btnDevice, homepageCtrl.setDevice );
        $(document).on('click', DOMElement.btnMenu, homepageCtrl.toggleMenu );

        $(document).on('click', DOMElement.btnEditHeaderImage, homepageCtrl.displayModalHeader);
        $(document).on('change', DOMElement.btnRadioHeaderImage, homepageCtrl.changeHeaderImage);
        
        $(document).on('click', DOMElement.btnEditPresentationImage, homepageCtrl.displayModalPresentation);
        $(document).on('change', DOMElement.btnRadioPresentationImage, homepageCtrl.changePresentationImage);
        
        $(document).on('click', DOMElement.btnCloseModal, homepageCtrl.closeModal);
    }

    return{
        init: init
    }
})(homepageController, homepageUI)