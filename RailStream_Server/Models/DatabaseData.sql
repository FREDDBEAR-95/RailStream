USE RailStream

/* ---------------------------- C������ ---------------------------- */

/* ----- | ������ ����������� | ----- */

SET IDENTITY_INSERT ConnectionStatus ON;

INSERT INTO ConnectionStatus(ConnectionStatusId, Status) VALUES
(1, '� ����'),
(2, '�� � ����'),
(3, '�� ����������');

SET IDENTITY_INSERT ConnectionStatus OFF;


/* ----- | ������ ����������� | ----- */

SET IDENTITY_INSERT NotificationStatus ON;

INSERT INTO NotificationStatus(NotificationStatusId, Status) VALUES
(1, '���������'),
(2, '�� ���������'),
(3, '�������������'),
(4, '���������'),
(5, '������');

SET IDENTITY_INSERT NotificationStatus OFF;


/* ----- | ������ �������� | ----- */

SET IDENTITY_INSERT RouteStatus ON;

INSERT INTO RouteStatus(RouteStatusId, Status) VALUES
(1, '��������'),
(2, '����������'),
(3, '���������'),
(4, '��������������')

SET IDENTITY_INSERT RouteStatus OFF;


/* ----- | ������ ������ | ----- */

SET IDENTITY_INSERT TrainStatus ON;

INSERT INTO TrainStatus(TrainStatusId, Status) VALUES
(1, '� ����'),
(2, '������� �����������'),
(3, '����������'),
(4, '�������'),
(5, '��������'),
(6, '�� ����������� ������������'),
(7, '� �������'),
(8, '������� �� ������������'),
(9, '������');

SET IDENTITY_INSERT TrainStatus OFF;

/* ----------------------------------------------------------------- */


/* ---------------------------- ���� ---------------------------- */


/* ----- | ��� ������ | ----- */

SET IDENTITY_INSERT WagonType ON;

INSERT INTO WagonType(WagonTypeId, Type) VALUES
(1, '��������'),
(2, '����'),
(3, '����'),
(4, '��������'),
(5, '�����������'),
(6, '��������'),
(7, '��������'),
(8, '���������'),
(9, '��������');

SET IDENTITY_INSERT WagonType OFF;


/* ----- | ��� ������ | ----- */

SET IDENTITY_INSERT TrainType ON;

INSERT INTO TrainType(TrainTypeId, Type) VALUES
(1, '������������'),
(2, '����������'),
(3, '�����������'),
(4, '��������');

SET IDENTITY_INSERT TrainType OFF;

/* -------------------------------------------------------------- */


/* ----- | ���� | ----- */

SET IDENTITY_INSERT Roles ON;

INSERT INTO Roles(RoleId, RoleTitle, RoleDescription, Sections) VALUES 
(1, '������������', '������������ �������', '["�������� ������ ����������", "�������������� ������ ����������"]'),
(2, '��������', '�������� � ������������ �������������', '["�������� � �������������� ���������� � ���� �������������", "���������� ������ �������������", "�������� � �������������� ��������"]'),
(3, '������', '������ � �������� � ���������� ���������', '["�������� � �������������� ���������� ����������", "��������� ��������", "������ �������"]'),
(4, '��������� �������������', '������������� � ������ �������� � �������', '["������ ������ �� ���� �������� � ������ �������"]');

SET IDENTITY_INSERT Roles OFF;


/* ----- | * ������ | ----- */

--SET IDENTITY_INSERT Train ON;

--INSERT INTO Train (TrainId, TrainTypeId, TrainStatusId, TrainBrand, ReleaseDate, Location) VALUES 
--(1),

--SET IDENTITY_INSERT Train OFF;


/* ----- | * ������ | ----- */

--SET IDENTITY_INSERT Train ON;

--INSERT INTO Train(RoleId, RoleTitle, RoleDescription, Sections) VALUES 
--();

--SET IDENTITY_INSERT Train OFF;


/* ----- | * �������� | ----- */

--SET IDENTITY_INSERT TrainType ON;

--INSERT INTO Roles (RoleId, RoleTitle, RoleDescription, Sections) VALUES ();

--SET IDENTITY_INSERT TrainType OFF;


/* ----- | * ������������ | ----- */

--SET IDENTITY_INSERT TrainType ON;

--INSERT INTO Roles (RoleId, RoleTitle, RoleDescription, Sections) VALUES ();

--SET IDENTITY_INSERT TrainType OFF;


/* ----- | * ������ | ----- */

--SET IDENTITY_INSERT TrainType ON;

--INSERT INTO Roles (RoleId, RoleTitle, RoleDescription, Sections) VALUES ();

--SET IDENTITY_INSERT TrainType OFF;