##### Стек :
###### 1.ASP.NET CORE MVC
###### 2.EntityFramework
###### 3.Ajax
###### 4.Микросервисы
###### 5.RabbitMQ
###### 6.Unit тестирование
###### 7.Docker

##### Инструкция по работе для программистов:
###### 1. В строке подключения к бд(файл: appsettings.json) меняем Server и Database на собственные значения
###### 2. Используем скрипты создания таблицы и добавление данных для нее - /App_Data/SQL в своей базе данных

##### Немного про проект
###### Проект представляет с собой web страницу с пользовательским и административным интерфейсом магазина с напитками. Пользователь может : вносить деньги, получать товар, получать сдачу. Администратору доступны удаление, добавление, изменение напитков. В обоих интерфейсах реализована система пользовательского интерфейса с помощью JS(VVM/wwwroot/js), которая помимо навигации и предупреждений для пользователя, предоставляет возможность совершать изменения без перезагрузки страницы. 

##### Основные релизованные фичи:
###### 1. Ajax на стороне клиента(позволяет обновлять данные без перезагрузки страницы)
###### 2. Микросервисная архитектура (на данный момент реализована аутиндетификация AuthModule отдельным кастомным микросервисом, в будущем планируется добавить еще модули)

![Новый рисунок](https://github.com/user-attachments/assets/d637917e-f5bc-48bf-b422-dac2e6a973e6)

##### Для тех кто хочет глянуть интерфейс 
###### https://drive.google.com/file/d/1rr3x86gMN1uo8mLJLALRwnaD_eVpXTgG/view?usp=drive_link

