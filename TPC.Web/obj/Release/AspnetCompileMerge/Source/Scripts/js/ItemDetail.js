$('#testbtn').click(function () {
    $.ajax({
        url: "ItemContainerPartial/ItemDetail",
        type: "POST",
        data: { quoteID: this.title },
        datatype: 'html',
        success: function (data) {
            $('#loadingView').html(data);
        }
    });
});