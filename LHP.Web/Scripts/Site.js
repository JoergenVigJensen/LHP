$(document).ready(function() {

    $(window).resize(function() {
        $('body').css('padding-top', parseInt($('#main-navbar').css("height")) + 10);

    });

    $(window).load(function() {
        $('body').css('padding-top', parseInt($('#main-navbar').css("height")) + 10);
        $(".foreign").hide();
        $(".non-foreign").hide();
        $(".cust_sub").hide();
        $(".jointname").hide();
    });
   
    $('#data').dataTable();
    $('#data').show();

});

$(function() {
    // This will make every element with the class "date-picker" into a DatePicker element
    $('.date-picker').datepicker().formatDate("DD, MM d, yy", new Date(2007, 7 - 1, 14), {
        dayNamesShort: $.datepicker.regional["Da"].dayNamesShort,
        dayNames: $.datepicker.regional["Da"].dayNames,
        monthNamesShort: $.datepicker.regional["Da"].monthNamesShort,
        monthNames: $.datepicker.regional["Da"].monthNames
    });
});
