function ajaxLink() {

    $('a[ajax-poziv="da"]').click(function () {

        var objekt = $(this);
        console.log(objekt);
        var url = objekt.attr('ajax-url');
        var rezultatId = objekt.attr('ajax-rezultat');

        $.get(
            url,
            function success(rezultat) {
                $('#' + rezultatId).html(rezultat);
            });

    });

    $('button[ajax-poziv="da"]').click(function () {

        var objekt = $(this);
        console.log(objekt);
        var url = objekt.attr('ajax-url');
        var rezultatId = objekt.attr('ajax-rezultat');

        $.get(
            url,
            function success(rezultat) {
                $('#' + rezultatId).html(rezultat);
            });

    });

}

function ajaxForm() {

    $('form[ajax-poziv="da"]').submit(function (e) {

        $(this).attr("ajax-poziv", "dodan");
        e.preventDefault();
        var form = $(this);
        var url = form.attr('ajax-url');
        var rezultatId = form.attr('ajax-rezultat');

        $.ajax({
            type: "POST",
            url: url,
            data: form.serialize(),
            success: function success(rezultat) {
                $('#' + rezultatId).html(rezultat);
            }
        });

    });
}

function ajaxInit() {
    var objekat = $('h2[ajax-poziv="da"]');
    var url = objekat.attr('ajax-url');
    var rezultatId = objekat.attr('ajax-rezultat');

    $.get(
        url,
        function success(rezultat) {
            $('#' + rezultatId).html(rezultat);
        }
    );
}

function notifikacija() {
    $('#messageBox').delay(3000).fadeOut(500).slideUp(500, function () {
        $(this).remove();
    });
}

$(document).ready(function () {

    //Inicijalni ajax
    ajaxInit();

    /*Ukidanje notifikacije nakon 3000ms*/
    notifikacija();

    //poziv ajax forme
    ajaxForm();

});

$(document).ajaxComplete(function () {
    //poziv ajax linka
    ajaxLink();

    //poziv ajax forme
    ajaxForm();
});