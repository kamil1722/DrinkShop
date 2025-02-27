## Стек:
* ASP.NET CORE MVC
* [EntityFramework](https://docs.microsoft.com/ru-ru/ef/core/)
* [Ajax](https://api.jquery.com/jQuery.ajax/)
* Микросервисы
* [RabbitMQ](https://www.rabbitmq.com/)
* Unit тестирование(В разработке)
* [Docker](https://www.docker.com/)(В разработке)
* [API](https://metanit.com/sharp/aspnet5/23.1.php)

## Описание проекта:
##### Проект представляет с собой web страницу ASP.NET MVC с пользовательским и административным интерфейсом магазина с напитками. Пользователь получает текущий ассортимент напитков и проводит с ним разные операции. Администратору доступны удаление, добавление, изменение напитков. В обоих интерфейсах реализована система пользовательского интерфейса с помощью JS(DrinkShop/wwwroot/js), которая помимо навигации и предупреждений для пользователя, предоставляет возможность совершать действия без перезагрузки страницы.Так же реализована аутиндетификация с помощью Indentify

## Описание функционала:
1. Ajax: на стороне клиента позволяет делать запросы на сервер без перезагрузки страницы
2. Аутиндетификация(Indentify): реализован отдельным кастомным микросервисом - AuthModule, который можно интегрировать в другие решения и проекты. Реализованы функции регистрации и аутиндетификации пользователя. Данные пользователя хранятся в базе данных, пороли хэшируются
3. RabbitMQ: позволяет создавать очередь из запросов на внешние сервисы, не блокируя основной поток. В данном проекте подключение к RabbitMQ реализовано с помощью проекта EmailWorkService, который в свою очередь развернута как служба .msc. Так же стоит отметить про подключение к email аккаунту с которой отправлются письма. Google аккаунт позволяет создавать пороли для приложений  "SmtpPassword": "qfpb mfgs jijh rvcv"(EmailWorkerService\appsettings.json), и используя основной ник, можно безопасно рассылать письма со своего приложения

## Архитектура:
![Новый рисунок (1)](https://github.com/user-attachments/assets/2d228cfe-d320-4392-9733-d6810afc2d14)

### Интерфейс:
1. Ajax, администрирование, авторизация: https://drive.google.com/file/d/1U2hxZcspzEgWeA4wNyXp4dzwdexvRGsD/view?usp=sharing
2. RabbitMQ: https://drive.google.com/file/d/1GL6bvAakGewXZhgMM1Zvs54AfZK33goy/view?usp=drive_link

