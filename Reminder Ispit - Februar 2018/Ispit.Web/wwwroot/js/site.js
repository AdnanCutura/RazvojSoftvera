function ajaxPoziv() {

    $('a[ajaxpoziv="da" ]').click(function () {

        var url = $(this).attr('ajaxurl');
        var rezultatId = $(this).attr('ajaxrezultat');;

        $.get(
            url,
            function (rezultat) {
                $('#' + rezultatId).html(rezultat);
            });
    });
}

function ajaxForma() {

    $('form[ajaxpoziv="da" ]').submit(function (e) {

        var forma = $(this);
        e.preventDefault();
        var url = $(this).attr('ajaxurl');
        var rezultatId = $(this).attr('ajaxrezultat');;

        $.ajax({
            type: 'POST',
            url: url,
            data: forma.serialize(),
            success: function (rezultat) {
                $('#' + rezultatId).html(rezultat);
            }

        });
    });
}

$(document).ready(function () {
    var object = $('[ajaxOnLoad="da"]')
    var url = object.attr('ajaxUrl');
    var rezultatId = object.attr('ajaxRezultat');

    $.get(
        url,
        function (rezultat) {
            $('#' + rezultatId).html(rezultat);
        });
});

$(document).ajaxComplete(function () {
    ajaxPoziv();
    ajaxForma();
});
