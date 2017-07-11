var Script = function () {



//    sidebar dropdown menu

    jQuery('#sidebar .sub-menu > a').click(function () {
        var last = jQuery('.sub-menu.open', $('#sidebar'));
        last.removeClass("open");
        jQuery('.arrow', last).removeClass("open");
        jQuery('.sub', last).slideUp(200);
        var sub = jQuery(this).next();
        if (sub.is(":visible")) {
            jQuery('.arrow', jQuery(this)).removeClass("open");
            jQuery(this).parent().removeClass("open");
            sub.slideUp(200);
        } else {
            jQuery('.arrow', jQuery(this)).addClass("open");
            jQuery(this).parent().addClass("open");
            sub.slideDown(200);
        }
        var o = ($(this).offset());
        diff = 200 - o.top;
        if(diff>0)
            $("#sidebar").scrollTo("-="+Math.abs(diff),500);
        else
            $("#sidebar").scrollTo("+="+Math.abs(diff),500);
    });

//    sidebar toggle


    $(function() {
        function responsiveView() {
            var wSize = $(window).width();
            if (wSize <= 768) {
                $('#container').addClass('sidebar-close');
                $('#sidebar > ul').hide();
            }

            if (wSize > 768) {
                $('#container').removeClass('sidebar-close');
                $('#sidebar > ul').show();
            }
        }
        $(window).on('load', responsiveView);
        $(window).on('resize', responsiveView);
    });

    $('.icon-reorder').click(function () {
        if ($('#sidebar > ul').is(":visible") === true) {
            $('#main-content').css({
                'margin-left': '0px'
            });
            $('#sidebar').css({
                'margin-left': '-180px'
            });
            $('#sidebar > ul').hide();
            $("#container").addClass("sidebar-closed");
        } else {
            $('#main-content').css({
                'margin-left': '180px'
            });
            $('#sidebar > ul').show();
            $('#sidebar').css({
                'margin-left': '0'
            });
            $("#container").removeClass("sidebar-closed");
        }
    });

// custom scrollbar
    $("#sidebar").niceScroll({styler:"fb",cursorcolor:"#e8403f", cursorwidth: '3', cursorborderradius: '10px', background: '#404040', cursorborder: ''});

    $("html").niceScroll({styler:"fb",cursorcolor:"#e8403f", cursorwidth: '6', cursorborderradius: '10px', background: '#404040', cursorborder: '', zindex: '1000'});

// widget tools

    jQuery('.widget .tools .icon-chevron-down').click(function () {
        var el = jQuery(this).parents(".widget").children(".widget-body");
        if (jQuery(this).hasClass("icon-chevron-down")) {
            jQuery(this).removeClass("icon-chevron-down").addClass("icon-chevron-up");
            el.slideUp(200);
        } else {
            jQuery(this).removeClass("icon-chevron-up").addClass("icon-chevron-down");
            el.slideDown(200);
        }
    });

    jQuery('.widget .tools .icon-remove').click(function () {
        jQuery(this).parents(".widget").parent().remove();
    });

//    tool tips

    $('.tooltips').tooltip();

//    popovers

    $('.popovers').popover();



// custom bar chart

    if ($(".custom-bar-chart")) {
        $(".bar").each(function () {
            var i = $(this).find(".value").html();
            $(this).find(".value").html("");
            $(this).find(".value").animate({
                height: i
            }, 2000)
        })
    }

// myCode
    // required field
    $(".required").append('<i class="required-field">*</i>');

    // changing url
    //var pathname = window.location.pathname;

    //var isTrade = pathname.includes("/Trade/");
    //var isVehicle = pathname.includes("/Vehicle/");
    //var isUser = pathname.includes("/User/");

    //localStorage.setItem('isTrade', 'true');
    //localStorage.setItem('isVehicle', 'true');
    //localStorage.setItem('isUser', 'true');

    //if(!isTrade)
    //{
    //    localStorage.setItem('isTrade', null);
    //    //Session["Trade_Name"] == null;
    //    //Session["Trade_GroupId"] == null;
    //    //Session["Trade_Tel"] == null;
    //    //Session["Trade_Address"] == null;
    //    //Session["Trade_Status"] == null;
    //    //localStorage.setItem('Trade_Name', null);
    //    //localStorage.setItem('Trade_GroupId', null);
    //    //localStorage.setItem('Trade_Tel', null);
    //    //localStorage.setItem('Trade_Address', null);
    //    //localStorage.setItem('Trade_Status', null);

        

    //    //$.session.set('Trade_Name', '');
    //    //$.session.set('Trade_GroupId', 0);
    //    //$.session.set('Trade_Tel', '');
    //    //$.session.set('Trade_Address', '');
    //    //$.session.set('Trade_Status', -1);


    //}

    //if(!isVehicle)
    //{
    //    localStorage.setItem('isVehicle', null);
    //    //Session["Vehicle_Model"] == null;
    //    //Session["Vehicle_GroupId"] == null;
    //    //Session["Vehicle_NumberPlates"] == null;
    //    //Session["Vehicle_VINName"] == null;
    //    //Session["Vehicle_Color"] == null;
    //    //Session["Vehicle_IsHybrid"] == null;
    //    //Session["Vehicle_Status"] == null;
    //}

    //if(!isUser)
    //{
    //    localStorage.setItem('isUser', null);
    //    //Session["User_Name"] == null;
    //    //Session["User_Family"] == null;
    //    //Session["User_RoleId"] == null;
    //    //Session["User_FatherName"] == null;
    //    //Session["User_Gender"] == null;
    //    //Session["User_NationalCode"] == null;
    //    //Session["User_IdNo"] == null;
    //    //Session["User_Mobile"] == null;
    //    //Session["User_Tel"] == null;
    //    //Session["User_Username"] == null;
    //    //Session["User_IsInsuranced"] == null;
    //    //Session["User_Status"] == null;
    //}

}();