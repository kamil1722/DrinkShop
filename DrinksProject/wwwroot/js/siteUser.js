function getCode(id) {
    document.getElementById("codeDrink").value = id;
}

function SetCoin(btnCoin) {
    var selectValueCoin = document.getElementById(btnCoin).value;
    var currentValueCoin = document.getElementById("valueCoin").value;

    document.getElementById("valueCoin").value = +selectValueCoin + +currentValueCoin;
}

$(document).ready(function () {
    if (document.getElementById("balance")) {
        var balanceElement = document.getElementById("balance");

        var balance = Math.floor(Math.random() * 51) + 100;
        balanceElement.innerHTML = balance + " руб";

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
                    } else {
                        callback(true);
                    }
                },
                error: function (error) {
                    console.log(error);
                    callback(false);
                }
            });
        }

        // Обработчик события для кнопки "Set Pay"
        $('#btnSetPay').click(function (e) {
            e.preventDefault();

            var codeDrink = $('#codeDrink').val();
            var currentBalance = $('#balance').text();

            var inputCodeDrink = document.getElementById("codeDrink");
            var inputValueCoin = document.getElementById("valueCoin");

            if (!inputCodeDrink || !inputCodeDrink.value) {
                alert('Ошибка при отправке данных: заполните поле "Код напитка"');
                return;
            } else if (!inputValueCoin || !inputValueCoin.value) {
                alert('Ошибка при отправке данных: заполните поле "Сумма(руб)"');
                return;
            } else if (+inputValueCoin.value > parseInt(currentBalance)) {
                alert('Ошибка: Недостаточно средств"');
                return;
            }

            checkPrice(codeDrink, inputValueCoin, function (result) {
                if (!result) {
                    alert('Ошибка: Введенной сумму недостаточно для совершения платежа');
                    return;
                } else {
                    $.ajax({
                        type: 'POST',
                        url: '/User/UpdateCount',
                        data: {
                            codeDrink: codeDrink
                        },
                    })
                        .then(function (result) {
                            console.log(result);

                            return $.ajax({
                                url: '/User/GetPrice',
                                type: 'GET',
                                data: {
                                    codeDrink: codeDrink
                                },
                            });
                        })
                        .then(function (data) {
                            var newBalance = parseInt(currentBalance) - +data;
                            balanceElement.innerHTML = newBalance + " руб"; // Используем balanceElement

                            return $.ajax({
                                url: '/User/GetCards',
                                type: 'GET',
                            });
                        })
                        .then(function (data) {
                            $('#cardsContainer').html(data);
                        })
                        .catch(function (error) {
                            console.log(error);
                        });
                }
            });
        });

    } else {
        console.log("Элемент balance на этой странице.");
    }
});
