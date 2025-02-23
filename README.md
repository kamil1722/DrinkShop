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
##### Проект представляет с собой web страницу ASP.NET MVC с пользовательским и административным интерфейсом магазина с напитками. Пользователь получает текущий ассортимент напитков и проводит с ним разные операции. Администратору доступны удаление, добавление, изменение напитков. В обоих интерфейсах реализована система пользовательского интерфейса с помощью JS(VVM/wwwroot/js), которая помимо навигации и предупреждений для пользователя, предоставляет возможность совершать действия без перезагрузки страницы.Так же реализована аутиндетификация с помощью Indentify

## Описание функционала:
1. Ajax на стороне клиента позволяет делать запросы на сервер без перезагрузки страницы
2. Реализована аутиндетификация(Indentify) отдельным кастомным микросервисом - AuthModule, который можно интегрировать в другие решения и проекты

## Архитектура:
![Новый рисунок](https://github.com/user-attachments/assets/d637917e-f5bc-48bf-b422-dac2e6a973e6)

### Интерфейс:
https://drive.google.com/file/d/1rr3x86gMN1uo8mLJLALRwnaD_eVpXTgG/view?usp=drive_link

