USE RailStream

/* ---------------------------- Cтатусы ---------------------------- */

/* ----- | Статус подключения | ----- */

SET IDENTITY_INSERT ConnectionStatus ON;

INSERT INTO ConnectionStatus(ConnectionStatusId, Status) VALUES
(1, 'В сети'),
(2, 'Не в сети'),
(3, 'Не беспокоить');

SET IDENTITY_INSERT ConnectionStatus OFF;


/* ----- | Статус уведомления | ----- */

SET IDENTITY_INSERT NotificationStatus ON;

INSERT INTO NotificationStatus(NotificationStatusId, Status) VALUES
(1, 'Прочитано'),
(2, 'Не прочитано'),
(3, 'Заплонировано'),
(4, 'Отмененно'),
(5, 'Ошибка');

SET IDENTITY_INSERT NotificationStatus OFF;


/* ----- | Статус маршрута | ----- */

SET IDENTITY_INSERT RouteStatus ON;

INSERT INTO RouteStatus(RouteStatusId, Status) VALUES
(1, 'Активный'),
(2, 'Неактивный'),
(3, 'Отмененый'),
(4, 'Архивированный')

SET IDENTITY_INSERT RouteStatus OFF;


/* ----- | Статус поезда | ----- */

SET IDENTITY_INSERT TrainStatus ON;

INSERT INTO TrainStatus(TrainStatusId, Status) VALUES
(1, 'В пути'),
(2, 'Ожидает отправления'),
(3, 'Опаздывает'),
(4, 'Отменен'),
(5, 'Задержан'),
(6, 'На техническом обслуживании'),
(7, 'В резерве'),
(8, 'Выведен из эксплуатации'),
(9, 'Списан');

SET IDENTITY_INSERT TrainStatus OFF;

/* ----------------------------------------------------------------- */


/* ---------------------------- Типы ---------------------------- */


/* ----- | Тип вагона | ----- */

SET IDENTITY_INSERT WagonType ON;

INSERT INTO WagonType(WagonTypeId, Type) VALUES
(1, 'Плацкарт'),
(2, 'Купе'),
(3, 'Люкс'),
(4, 'Спальный'),
(5, 'Ресторанный'),
(6, 'Почтовый'),
(7, 'Багажный'),
(8, 'Платформа'),
(9, 'Цистерна');

SET IDENTITY_INSERT WagonType OFF;


/* ----- | Тип поезда | ----- */

SET IDENTITY_INSERT TrainType ON;

INSERT INTO TrainType(TrainTypeId, Type) VALUES
(1, 'Пассажирский'),
(2, 'Скоростной'),
(3, 'Пригородный'),
(4, 'Грузовой');

SET IDENTITY_INSERT TrainType OFF;

/* -------------------------------------------------------------- */


/* ----- | Роли | ----- */

SET IDENTITY_INSERT Roles ON;

INSERT INTO Roles(RoleId, RoleTitle, RoleDescription, Sections) VALUES 
(1, 'Пользователь', 'Пользователь системы', '["Просмотр личной информации", "Редактирование личной информации"]'),
(2, 'Менеджер', 'Менеджер с расширенными возможностями', '["Просмотр и редактирование информации о всех пользователях", "Управление ролями пользователей", "Создание и редактирование разделов"]'),
(3, 'Кассир', 'Кассир с доступом к финансовым операциям', '["Просмотр и редактирование финансовой информации", "Обработка платежей", "Выдача билетов"]'),
(4, 'Системный администратор', 'Администратор с полным доступом к системе', '["Полный доступ ко всем функциям и данным системы"]');

SET IDENTITY_INSERT Roles OFF;


/* ----- | * Поезда | ----- */

--SET IDENTITY_INSERT Train ON;

--INSERT INTO Train (TrainId, TrainTypeId, TrainStatusId, TrainBrand, ReleaseDate, Location) VALUES 
--(1),

--SET IDENTITY_INSERT Train OFF;


/* ----- | * Вагоны | ----- */

--SET IDENTITY_INSERT Train ON;

--INSERT INTO Train(RoleId, RoleTitle, RoleDescription, Sections) VALUES 
--();

--SET IDENTITY_INSERT Train OFF;


/* ----- | * Маршруты | ----- */

--SET IDENTITY_INSERT TrainType ON;

--INSERT INTO Roles (RoleId, RoleTitle, RoleDescription, Sections) VALUES ();

--SET IDENTITY_INSERT TrainType OFF;


/* ----- | * Пользователи | ----- */

--SET IDENTITY_INSERT TrainType ON;

--INSERT INTO Roles (RoleId, RoleTitle, RoleDescription, Sections) VALUES ();

--SET IDENTITY_INSERT TrainType OFF;


/* ----- | * Билеты | ----- */

--SET IDENTITY_INSERT TrainType ON;

--INSERT INTO Roles (RoleId, RoleTitle, RoleDescription, Sections) VALUES ();

--SET IDENTITY_INSERT TrainType OFF;