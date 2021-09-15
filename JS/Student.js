$(document).ready(function () {
    $(".files").attr('data-before', "Prevucite fajl ili kliknite na dugme iznad");
    $('input[type="file"]').change(function (e) {
        var fileName = e.target.files[0].name;
        $(".files").attr('data-before', fileName);

    });
});