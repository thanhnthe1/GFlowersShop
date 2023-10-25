$(".btn").on("click", function () {


    var $button = $(this),
        $divQuantity = $button.closest('.Cart-Items').find(".count"),
        $textTotal = $button.closest('.CartContainer').find(".total-amount"),
        $unitPrice = $button.closest('.Cart-Items').find(".unitprice"),
        $id = $button.closest('.Cart-Items').find(".id-product"),
        $price = $button.closest('.Cart-Items').find(".amount");
    var oldQuantity = parseInt($divQuantity.text()), newQuantity, newPrice, total = 0,
        valUnitPrice = parseFloat($unitPrice.text().replace('$', '').replace(',', '.')), product = $id.text().trim();
    var cells = document.querySelectorAll('.amount');
    if ($.trim($button.text()) == "+") {

        $.ajax({
            type: "get",
            url: '/Cart?handler=PlusQuantity',
            data: {
                productId: product
            },
            success: function (data) {
                if (data.success == true) {
                    newQuantity = oldQuantity + 1;
                    $divQuantity.text(newQuantity); // quantity them 1
                    newPrice = valUnitPrice * newQuantity;
                    $price.text('$' + newPrice.toFixed(2)); //set lai price moi
                    for (i = 0; i < cells.length; i++) { //tính lại tổng
                        total = total + parseFloat(cells[i].textContent.replace('$', '').replace(',', '.'));
                    }
                    total = total.toFixed(2);
                    $textTotal.text('$' + total);
                }
            }
        });
    } else {
        // Don't allow decrementing below zero
        if (oldQuantity > 1) {
            $.ajax({
                type: 'get',
                url: '/Cart?handler=MinusQuantity',
                data: {
                    productId: product
                },
                success: function (data) {
                    if (data.success == true) {
                        newQuantity = oldQuantity - 1;
                        $divQuantity.text(newQuantity); // quantity them 1
                        newPrice = valUnitPrice * newQuantity;
                        $price.text('$' + newPrice.toFixed(2)); //set lai price moi
                        for (i = 0; i < cells.length; i++) { //tính lại tổng
                            total = total + parseFloat(cells[i].textContent.replace('$', '').replace(',', '.'));
                        }
                        total = total.toFixed(2);
                        $textTotal.text('$' + total);
                    }
                }
            });
        }
    }

});
$(".remove").on("click", function () {
    var $button = $(this),
        $block = $button.closest('.Cart-Items'),
        $price = $button.closest('.Cart-Items').find(".amount"),
        $item = $button.closest('.Cart-Items').find(".items"),
        $textTotal = $button.closest('.CartContainer').find(".total-amount"),
        $id = $block.find(".id-product");
    var product = $id.text().trim();
    var numOld = $("#number-cart").text(), totalText = $textTotal.text(), i, total = 0;
    $.ajax({
        type: 'get',
        url: '/Cart?handler=RemoveCart',
        data: {
            productId: product
        },
        success: function (data) {
            if (data.success == true) {
                $("#number-cart").text(parseInt(numOld) - 1);
                $block.remove();
                total = parseFloat(totalText.replace('$', ''));
                total = total - parseFloat($price.text().replace('$', ''));
                total = total.toFixed(2);
                $textTotal.text('$' + total);
                $item.text(parseInt($item.text()) - 1);
            }
        }
    });
});

$(".btn-checkout").on("click", function () {

    window.location.href = "/payment"
});