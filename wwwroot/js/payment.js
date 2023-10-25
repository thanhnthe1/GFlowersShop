function payment() {
    // Lấy thông tin từ các input
    var name = document.getElementById("name").value;
    var phone = document.getElementById("phone").value;
    var address = document.getElementById("address").value;
    var cardNumber = document.getElementsByName("card")[0].value;
    var expires = document.getElementsByName("expires")[0].value;
    var cvc = document.getElementsByName("cvc")[0].value;

    // Kiểm tra nếu có bất kỳ trường nào bị bỏ trống
    if (!name || !phone || !address || !cardNumber || !expires || !cvc) {
        document.getElementById("message").innerHTML = "Please fill all fields!";
        return;
    }

    // Kiểm tra thông tin thẻ
    var visaReg = /^4[0-9]{12}(?:[0-9]{3})?$/;
    var mastercardReg = /^5[1-5][0-9]{14}$/;
    var amexReg = /^3[47][0-9]{13}$/;
    var discoverReg = /^6(?:011|5[0-9]{2})[0-9]{12}$/;
    if (!visaReg.test(cardNumber) && !mastercardReg.test(cardNumber) && !amexReg.test(cardNumber) && !discoverReg.test(cardNumber)) {
        document.getElementById("message").innerHTML = "Invalid card number!";
        return;
    }

    // Kiểm tra thông tin hết hạn thẻ
    var expiresReg = /^(0[1-9]|1[0-2])\/?([0-9]{4}|[0-9]{2})$/;
    if (!expiresReg.test(expires)) {
        document.getElementById("message").innerHTML = "Invalid expiration date!";
        return;
    }

    // Kiểm tra thông tin CVC
    var cvcReg = /^[0-9]{3,4}$/;
    if (!cvcReg.test(cvc)) {
        document.getElementById("message").innerHTML = "Invalid CVC!";
        return;
    }

    var total = parseFloat($(".total-amount").text().replace('$', '').replace(',', '.'));
    $.ajax({
        type: 'get',
        url: '/Payment?handler=payment',
        data: {
            name: name,
            phone: phone,
            address: address,
            total : total
        },
        success: function (data) {
            if (data.success == true) {
                document.getElementById("inputPayment").style.display = "none";
                document.getElementById("paymentSuccess").style.display = "";
                document.getElementById("number-cart").innerHTML="";
            }
        }
    });
}