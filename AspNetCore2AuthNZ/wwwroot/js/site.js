$('.add-to-cart').click(function () {
    var data =
    {
        product: $(this).attr('data-product')
    }
    $.post('/Cart/Add/', data, function (data, status) {
        $('#cart-count').text(data);
    });
})