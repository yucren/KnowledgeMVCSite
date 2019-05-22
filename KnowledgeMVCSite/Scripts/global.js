$(function () {
    //添加单击链接匹配
    $('#headLink a').each(function (index, item) {

        if (document.location.pathname == "/") {

            $('#headLink a').first().parent().addClass("active");
        }
        else if ($(this).attr("href") != "/" && document.location.pathname.includes($(this).attr("href"))) {

            $(this).parent().addClass("active")

        }
    });
});
new Vue({
    el: '#head',
    data: {

    },
    methods: {

    },
    computed: {

    }


});
