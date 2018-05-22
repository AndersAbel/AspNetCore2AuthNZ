$('.add-to-cart').click(function () {
    var data =
    {
        product: $(this).attr('data-product')
    };
    $.post('/Cart/Add/', data, function (data, status) {
        $('#cart-count').text(data);
    });
});

$('.linked-item').click(function () {
    window.location = $(this).attr('data-link');
});

var config = {
    authority: "https://localhost:44380",
    client_id: "js",
    redirect_uri: "https://localhost:44334/callback.html",
    response_type: "id_token token",
    scope: "openid cart"
};

var mgr = new Oidc.UserManager(config);

mgr.getUser().then(function (user) {
    if (!user) {
        mgr.signinRedirect();
    }
})