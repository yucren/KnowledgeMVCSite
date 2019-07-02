$(function () {

    $('#headLink').height(window.screen.availHeight);
    $('#headrBtn').click(function () {
        $('body').toggleClass('overflow-y', 'hidden');

    });

    $('#headLink a').not("[data-toggle='dropdown']").click(function () {

        $('#navbar-collapse-1').collapse('hide');
       
    });




});