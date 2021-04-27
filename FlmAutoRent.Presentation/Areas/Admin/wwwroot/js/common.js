"use strict"

var CommonController = (function(){

    var addClassActive = function(e){
        $(e.data.menuesClass).removeClass('active');
        $(this).addClass('active');
    }

    var displayMenuMobile = function(){
        if ($('div#sidebar').hasClass('active')){
            $('div#sidebar').removeClass('active');
            $('body').css('overflow-y', 'scroll')
        } else {
            $('div#sidebar').addClass('active')
            $('body').css('overflow-y', 'hidden')
        }
    }

    var accordionMenu = function(){
        //1. change arrow
        var accordion = $(this).find('ul');
        
        if(accordion.hasClass('open')){
            //1. change arrow
            $(this).find('i').text('chevron_right');
            accordion.removeClass('open');
        } else {
            //1. change arrow
            $(this).find('i').text('expand_more');
            accordion.addClass('open');
        }
    }

    //SYSTEM GROUPS
    var AddChildChecked = function(){
        var checkboxContainer = $(this).closest('.menu-father').find('input[type="checkbox"]');
        if($(this).is(':checked')){
            checkboxContainer.attr('checked', 'checked')
        } else {
            checkboxContainer.removeAttr('checked')
        }
    }

    var AddFatherChecked = function(){
        var checkboxFather = $(this).closest('.menu-father').find('input[type="checkbox"].checkbox-menu');
        if($(this).is(':checked')){
            checkboxFather.attr('checked', 'checked')
        } 
    }

    //SYSTEM EMAIL GET DEFAULT PROVIDER 
    var getDataEmailProvider = function(){
        var IDEmail = $(this).find(":selected").attr('value');
        fetch('/Admin/System/GetDataEmailProvider/'+IDEmail).then(function(res){
            res.json().then(function(data){

                $('input#EmailPop').val(data.emailPop);
                $('input#EmailPortPop').val(data.emailPortPop);
                $('input#EmailSmtp').val(data.emailSmtp);
                $('input#EmailPortSmtp').val(data.emailPortSmtp);

                $('input[type="radio"].radio-email-ssl').each(function(){
                    if($(this).val() == data.emailSSL){
                        $(this).attr('checked', 'checked');
                    }
                })
            });
        })
    }

    //DISPLAY UPLOAD PREVIEW
    var diplayUploadPreview = function(){
        var input = $(this)[0];
        
        if (input.files && input.files[0]) {
            var reader = new FileReader();
    
            reader.onload = function(e) {
                $('#preview').find('img').attr('src', e.target.result);
            }
            
            reader.readAsDataURL(input.files[0]); // convert to base64 string
        }
    }

    var diplayUploadPreviewVideo = function(){
        var input = $(this)[0];
        
        if (input.files && input.files[0]) {
            var $source = $('#preview').find('source');
            $source[0].src = URL.createObjectURL(input.files[0]);
            $source.parent()[0].load();
        }
    }

    var displayUploadPreviewFile = function(){
        var input = $(this)[0];
        
        if (input.files && input.files[0]) {
            var fileName = $(this).val().split('\\');
            $('p.fileName').text(fileName[fileName.length - 1])
        }
    }

    //MODAL WITH CUSTOM MESSAGE AND CUSTOM LINK
    var createModal = function(){
        var element = '<div class="modal active overlay display-flex flex-direction-column align-items-center justify-content-center">'
        element = element + '<section id="modal-content" class="box-shadow background-color-white display-flex flex-direction-column justify-content-space-around">'
        element = element + '</section>'
        element = element + '</div>'

        $('body').append(element);
    }

    var removeModal = function(){
        $('div.modal').remove();
    }
    
    var appendToModal = function(event){
        var id = $(this).attr('id');
        var element = '<h2 class="padding-top-small text-center">'+event.data.textToDisplay+'</h2>'
        element = element + '<section class="display-flex flex-direction-row align-items-center justify-content-space-around">'
        element = element + '<button class="close-modal btn background-color-black-light color-white text-center">Chiudi</button>'
        element = element + '<a href="'+event.data.link + id + '" class="btn background-color-red color-white text-center">Elimina</button>'
        element = element + '</section>'
        
        $('#modal-content').append(element)
    }

    var findFormSubmit = function(){
        var self = $(this);
        setTimeout(function() {
            if(self.val().length > 3 ){
                self.closest('form').submit();
            }     
        }, 500)
    }

    var changePageSize = function(){
        $(this).closest('form').submit();
    }

    return {
        addClassActive: addClassActive,
        displayMenuMobile: displayMenuMobile,
        accordionMenu: accordionMenu,

        //SYSTEM GROUPS
        AddChildChecked: AddChildChecked,
        AddFatherChecked: AddFatherChecked,
        
        //SYSTEM EMAIL GET DEFAULT PROVIDER 
        getDataEmailProvider: getDataEmailProvider,
        
        //DISPLAY UPLOAD PREVIEW
        diplayUploadPreview: diplayUploadPreview,
        diplayUploadPreviewVideo: diplayUploadPreviewVideo,
        displayUploadPreviewFile: displayUploadPreviewFile,

        //MODAL WITH CUSTOM MESSAGE AND CUSTOM LINK
        createModal: createModal,
        appendToModal: appendToModal,
        removeModal: removeModal,
        
        findFormSubmit: findFormSubmit,
        changePageSize: changePageSize
    }
})();

var CommonUI = (function(){
    var DOMElement = {
        headerMenu : '.header-menu',
        btnDisplayMenuMobile: 'span.menu-tablet-mobile',
        btnAccordionMenu: 'div#sidebar ul li.accordion',
        checkboxMenu: 'input[type="checkbox"].checkbox-menu',
        checkboxMenuChild: 'input[type="checkbox"].checkbox-menu-child',
        selectEmailProvider: 'select.select-provider',
        editorSummerNote: 'textarea.addSummerNote',
        
        //DISPLAY PREVIEW UPLOAD
        inputUpload: 'input[type="file"]',
        inputUploadVideo: 'input[type="file"].video',
        inputUploadFile: 'input[type="file"].file',

        //OPEN MODAL FOR DELETE
        btnDisplayModal: 'input[type="button"].delete',
        btnRemoveModal: 'button.close-modal',

        //1. SYSTEM
        btnDeleteGroup: 'input[type="button"].delete.group-delete',
        btnDeleteEmail: 'input[type="button"].delete.email-delete',
        btnDeleteOperator: 'input[type="button"].delete.operator-delete',
        
        //2. CONTENT
        btnDeleteImage: 'input[type="button"].delete.image-delete',
        btnDeleteVideo: 'input[type="button"].delete.video-delete',
        btnDeleteAttachment: 'input[type="button"].delete.attachment-delete',
        btnDeleteNews: 'input[type="button"].delete.news-delete',

        //3. VEHICLES
        btnDeleteBrand: 'input[type="button"].delete.brand-delete',
        btnDeleteVehicleImage: 'input[type="button"].delete.vehicle-image-delete',

        //FIND
        findFormSubmit: 'input[type="text"]#find',

        //ChangeSizePage
        howManyFieldDisplay: 'select#how-many-fields'
    }

    return {
        DOMElement: DOMElement
    }
})();

var Common = (function(CommonCtrl, CommonUI){

    var init = function(){
        var DOMElement  = CommonUI.DOMElement;

        $(document).on('click', DOMElement.btnDisplayMenuMobile, CommonCtrl.displayMenuMobile);
        $(document).on('click', DOMElement.btnAccordionMenu, CommonCtrl.accordionMenu);

        $(document).on('click', DOMElement.headerMenu, { menuesClass: DOMElement.headerMenu }, CommonCtrl.addClassActive);
        $(document).on('click', DOMElement.checkboxMenu, CommonCtrl.AddChildChecked);
        $(document).on('click', DOMElement.checkboxMenuChild, CommonCtrl.AddFatherChecked);

        $(document).on('click', DOMElement.btnDisplayModal, CommonCtrl.createModal);
        $(document).on('click', DOMElement.btnRemoveModal, CommonCtrl.removeModal);

        //APPEND SUMMER NOTE
        $(DOMElement.editorSummerNote).summernote({
            height: null,               // set editor height
            minHeight: 300,             // set minimum height of editor
            maxHeight: null,            // set maximum height of editor
            focus: true   
        });

        //GET DEFAULT EMAIL PROVIDER
        $(document).on('change',  DOMElement.selectEmailProvider, CommonCtrl.getDataEmailProvider);

        //DISPLAY PREVIEW UPLOAD
        $(document).on('change', DOMElement.inputUpload, CommonCtrl.diplayUploadPreview);
        $(document).on('change', DOMElement.inputUploadVideo, CommonCtrl.diplayUploadPreviewVideo);
        $(document).on('change', DOMElement.inputUploadFile, CommonCtrl.displayUploadPreviewFile);

        //OPEN MODAL FOR DELETE
        $(document).on('click', DOMElement.btnDeleteGroup, { textToDisplay: 'Sei sicuro di voler eliminare il Gruppo?', link: '/Admin/System/GroupDelete/' } ,CommonCtrl.appendToModal);
        $(document).on('click', DOMElement.btnDeleteEmail, { textToDisplay: 'Sei sicuro di voler eliminare l\'Account Email?', link: '/Admin/System/EmailDelete/' } ,CommonCtrl.appendToModal);
        $(document).on('click', DOMElement.btnDeleteOperator, { textToDisplay: 'Sei sicuro di voler eliminare l\'Operatore?', link: '/Admin/System/OperatorDelete/' } ,CommonCtrl.appendToModal);
        $(document).on('click', DOMElement.btnDeleteImage, { textToDisplay: 'Sei sicuro di voler eliminare l\'Immagine?', link: '/Admin/Content/ImageDelete/' } ,CommonCtrl.appendToModal);
        $(document).on('click', DOMElement.btnDeleteVideo, { textToDisplay: 'Sei sicuro di voler eliminare il Video?', link: '/Admin/Content/VideoDelete/' } ,CommonCtrl.appendToModal);
        $(document).on('click', DOMElement.btnDeleteAttachment, { textToDisplay: 'Sei sicuro di voler eliminare l\'Allegato?', link: '/Admin/Content/AttachmentDelete/' } ,CommonCtrl.appendToModal);
        $(document).on('click', DOMElement.btnDeleteNews, { textToDisplay: 'Sei sicuro di voler eliminare la Notizia?', link: '/Admin/Content/NewsDelete/' } ,CommonCtrl.appendToModal);
        $(document).on('click', DOMElement.btnDeleteBrand, { textToDisplay: 'Sei sicuro di voler eliminare il Brand?', link: '/Admin/Vehicles/BrandDelete/' } ,CommonCtrl.appendToModal);
        $(document).on('click', DOMElement.btnDeleteVehicleImage, { textToDisplay: 'Sei sicuro di voler eliminare l\'Immagine del veicolo ?', link: '/Admin/Vehicles/VehicleImageDelete/' } ,CommonCtrl.appendToModal);

        $(document).on('keyup', DOMElement.findFormSubmit, CommonCtrl.findFormSubmit);
        $(document).on('keypress', DOMElement.findFormSubmit, CommonCtrl.findFormSubmit);
        $(document).on('keydown', DOMElement.findFormSubmit, CommonCtrl.findFormSubmit);

        $(document).on('change', DOMElement.howManyFieldDisplay, CommonCtrl.changePageSize);
    }

    return {
        init: init
    }
})(CommonController, CommonUI);

