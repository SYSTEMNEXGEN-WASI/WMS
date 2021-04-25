$('.collapse').on('shown.bs.collapse', function () {
    $(this).parent().find(".glyphicon-chevron-right").removeClass("glyphicon-chevron-right").addClass("glyphicon-remove");
}).on('hidden.bs.collapse', function () {
    $(this).parent().find(".glyphicon-remove").removeClass("glyphicon-remove").addClass("glyphicon-chevron-right");
});