if (!Modernizr.inputtypes.date) {
    $(function () {
        $(".datefield").datepicker({ maxDate: '0' });
    });
}