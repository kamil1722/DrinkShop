// timer.js

// Получаем оставшееся время из localStorage или устанавливаем 60 секунд
let timeLeft = localStorage.getItem('timeLeft') ? parseInt(localStorage.getItem('timeLeft')) : 60;

const timerElement = document.getElementById("timer");

const countdown = setInterval(function () {
    if (timeLeft <= 0) {
        clearInterval(countdown);
        timerElement.textContent = "Время вышло!";
        document.querySelector('button[type="submit"]').disabled = true;
        localStorage.removeItem('timeLeft'); // Удаляем значение после завершения
    } else {
        // Форматируем время в mm:ss
        const minutes = Math.floor(timeLeft / 60);
        const seconds = timeLeft % 60;
        timerElement.textContent = (minutes < 10 ? '0' : '') + minutes + ':' + (seconds < 10 ? '0' : '') + seconds;
        localStorage.setItem('timeLeft', timeLeft); // Сохраняем оставшееся время
    }
    timeLeft -= 1;
}, 1000);
