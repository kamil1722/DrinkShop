var balance = Math.floor(Math.random() * 51) + 100;
document.getElementById("balance").innerHTML = balance + " руб"
document.getElementById("restMoney").innerHTML = 0 + " руб"


function SetCoin(btnCoin) {
    var selectValueCoin = document.getElementById(btnCoin).value;
    var currentValueCoin = document.getElementById("valueCoin").value;

    document.getElementById("valueCoin").value = +selectValueCoin + +currentValueCoin;
}

function getCode(id) {
    document.getElementById("codeDrink").value = id;
}

function checkPrice(codeDrink, inputValueCoin, callback) {
    $.ajax({
        url: '/User/GetPrice',
        type: 'GET',
        data: { codeDrink: codeDrink },
        success: function (data) {
            var price = +data;
            var inputPrice = +inputValueCoin.value;

            if (price > inputPrice) {
                callback(false);
            }
            else {
                callback(true);
            }
        },
        error: function (error) {
            console.log(error);
            callback(false);
        }
    });
}

$(function () {
    $('#btnSetPay').click(function (e) {
        e.preventDefault();

        var codeDrink = $('#codeDrink').val();
        var currentBalance = $('#balance').text();
        var currentRestMoney = $('#restMoney').text();

        var inputCodeDrink = document.getElementById("codeDrink");
        var inputValueCoin = document.getElementById("valueCoin");

        if ((inputCodeDrink && inputCodeDrink.value) == false) {
            alert('Ошибка при отправке данных: заполните поле "Код напитка"');
            return;
        }
        else if ((inputValueCoin && inputValueCoin.value) == false) {
            alert('Ошибка при отправке данных: заполните поле "Сумма(руб)"');
            return;
        }
        else if (+inputValueCoin.value > parseInt(currentBalance)) {
            alert('Ошибка: Недостаточно средств"');
            return;
        }

        checkPrice(codeDrink, inputValueCoin, function (result) {
            if (result == false) {
                alert('Ошибка: Введенной сумму недостаточно для совершения платежа');
                return;
            }
            else {
                $.ajax({
                    type: 'POST',
                    url: '/User/UpdateCount',
                    data: { codeDrink: codeDrink },
                })
                    .then(function (result) {
                        console.log(result);

                        return $.ajax({
                            url: '/User/GetPrice',
                            type: 'GET',
                            data: { codeDrink: codeDrink },
                        });
                    })
                    .then(function (data) {
                        var newBalance = parseInt(currentBalance) - +data;
                        var restMoney = (+inputValueCoin.value - +data) + parseInt(currentRestMoney);
                        document.getElementById("balance").innerHTML = newBalance + " руб";
                        document.getElementById("restMoney").innerHTML = restMoney + " руб";

                        return $.ajax({
                            url: '/User/GetCards',
                            type: 'GET',
                        });
                    })
                    .then(function (data) {
                        // Обновляем содержимое контейнера cardsContainer
                        $('#cardsContainer').html(data);
                    })
                    .catch(function (error) {
                        console.log(error);
                    });
            }
        });
    });
});

$(function () {
    $('#btnGetPay').click(function (e) { 
        var currentRestMoney = parseInt($('#restMoney').text());
        var currentBalance = parseInt($('#balance').text());

        document.getElementById("balance").innerHTML = currentBalance + currentRestMoney + " руб";
        document.getElementById("restMoney").innerHTML = 0 + " руб";
    });
});

