//site.js

    //var ele = document.getElementById("username");
    //ele.innerHTML = "Fabio Duque Com Javascript";


    //var main = document.getElementById("main");
    //main.onmouseenter = function () {
    //    main.style.backgroundColor = "#888";
    //};
    //main.onmouseleave = function () {
    //    main.style.backgroundColor = "";
    //};

(function () {
    //var ele = $("#username");
    //ele.text("Fabio Duque Com Javascript");


    //var main = $("#main");
    //main.on("mouseenter", function () {
    //    main.css("background-color","#888");
        
    //});
    //main.on("mouseleave", function () {
    //    main.css("background-color", "");
    //});


    ////new code
    //var menuItem = $("ul.menu li a");
    //menuItem.on("click", function () {
    //    var me = $(this); //this captura o elemento que foi clicado...
    //    alert(me.text());

    //    //alert("Hello");

    //});


     var ele = document.getElementById("username");
    ele.innerHTML = "Fabio Duque Com Javascript";




    var $sidebarAndWrapper = $("#sidebar,#wrapper");
    var $icon = $("#sidebartoggle i.fa");

    $("#sidebartoggle").on("click", function () {
        $sidebarAndWrapper.toggleClass("hide-sidebar"); //toggleClass() é equivalente a dar um New.
        if ($sidebarAndWrapper.hasClass("hide-sidebar")) {
            $icon.removeClass("fa-angle-left");
            $icon.addClass("fa-angle-right");
        } else {
            $icon.addClass("fa-angle-left");
            $icon.removeClass("fa-angle-right");
        }


    });


})();