-- Создание базы данных
use master;
go
if db_id('HeatEnergyConsumption') is not null
drop database HeatEnergyConsumption;
go
create database HeatEnergyConsumption;
go

use HeatEnergyConsumption;

-- Создание таблицы для хранения форм собственности
create table OwnershipForms
(Id int identity not null,
[Name] nvarchar(100) not null,
constraint PK_OwnershipForm_Id primary key(Id),
constraint UQ_OwnershipForm_Name unique([Name]),
constraint CK_Len_OwnershipForm_Name check(len([Name]) != 0));

-- Создание таблицы для хранения руководителей
create table Managers
(Id int identity not null,
[Name] nvarchar(20) not null,
Surname nvarchar(20) not null,
MiddleName nvarchar(20),
PhoneNumber nchar(12) not null,
constraint PK_Manager_Id primary key(Id),
constraint CK_Len_Manager_Name check(len([Name]) != 0),
constraint CK_Len_Manager_Surname check(len(Surname) != 0),
constraint CK_Len_Manager_MiddleName check(len(MiddleName) != 0),
constraint UQ_Manager_PhoneNumber unique(PhoneNumber),
constraint CK_Pattern_Manager_PhoneNumber check(
	PhoneNumber like '37525[0-9][0-9][0-9][0-9][0-9][0-9][0-9]' or 
	PhoneNumber like '37529[0-9][0-9][0-9][0-9][0-9][0-9][0-9]' or 
	PhoneNumber like '37533[0-9][0-9][0-9][0-9][0-9][0-9][0-9]' or 
	PhoneNumber like '37544[0-9][0-9][0-9][0-9][0-9][0-9][0-9]')); 

-- Создание таблицы для хранения организаций
create table Organizations
(Id int identity not null,
[Name] nvarchar(100) not null,
OwnershipFormId int not null,
[Address] nvarchar(100) not null,
ManagerId int,
constraint PK_Organization_Id primary key(Id),
constraint CK_Len_Organization_Name check(len([Name]) != 0),
constraint FK_Organization_OwnershipFormId foreign key(OwnershipFormId) 
	references OwnershipForms(Id) on delete cascade on update cascade,
constraint CK_Len_Organization_Address check(len([Address]) != 0),
constraint FK_Organization_ManagerId foreign key(ManagerId) 
	references Managers(Id) on delete set null on update cascade);

-- Создание таблицы для хранения главных энергетиков
create table ChiefPowerEngineers
(Id int identity not null,
[Name] nvarchar(20) not null,
Surname nvarchar(20) not null,
MiddleName nvarchar(20),
PhoneNumber nchar(12) not null,
OrganizationId int not null,
constraint PK_ChiefPowerEngineer_Id primary key(Id),
constraint CK_Len_ChiefPowerEngineer_Name check(len([Name]) != 0),
constraint CK_Len_ChiefPowerEngineer_Surname check(len(Surname) != 0),
constraint CK_Len_ChiefPowerEngineer_MiddleName check(len(MiddleName) != 0),
constraint UQ_ChiefPowerEngineer_PhoneNumber unique(PhoneNumber),
constraint CK_Pattern_ChiefPowerEngineer_PhoneNumber check(
	PhoneNumber like '37525[0-9][0-9][0-9][0-9][0-9][0-9][0-9]' or 
	PhoneNumber like '37529[0-9][0-9][0-9][0-9][0-9][0-9][0-9]' or 
	PhoneNumber like '37533[0-9][0-9][0-9][0-9][0-9][0-9][0-9]' or 
	PhoneNumber like '37544[0-9][0-9][0-9][0-9][0-9][0-9][0-9]'),
constraint FK_ChiefPowerEngineer_OrganizationId foreign key(OrganizationId) 
	references Organizations(Id) on delete cascade on update cascade);

-- Создание таблицы для хранения видов продукции
create table ProductsTypes
(Id int identity not null,
Code nchar(6) not null,
[Name] nvarchar(100) not null,
Unit nvarchar(10) not null,
constraint PK_ProductType_Id primary key(Id),
constraint UQ_ProductType_Code unique(Code),
constraint CK_Len_ProductType_Code check(len(Code) != 0),
constraint CK_Len_ProductType_Name check(len([Name]) != 0),
constraint CK_Len_ProductType_Unit check(len(Unit) != 0));

-- Создание таблицы для хранения видов услуг
create table ServicesTypes
(Id int identity not null,
Code nchar(6) not null,
[Name] nvarchar(100) not null,
Unit nvarchar(10) not null,
constraint PK_ServiceType_Id primary key(Id),
constraint UQ_ServiceType_Code unique(Code),
constraint CK_Len_ServiceType_Code check(len(Code) != 0),
constraint CK_Len_ServiceType_Name check(len([Name]) != 0),
constraint CK_Len_ServiceType_Unit check(len(Unit) != 0));

-- Создание таблицы для хранения информации о произведенной продукции
create table ProducedProducts
(Id int identity not null,
OrganizationId int not null,
ProductTypeId int not null,
ProductQuantity int not null,
HeatEnergyQuantity real not null,
[Date] date not null,
constraint PK_ProducedProduct_Id primary key(Id),
constraint FK_ProducedProduct_OrganizationId foreign key(OrganizationId) 
	references Organizations(Id) on delete cascade on update cascade,
constraint FK_ProducedProduct_ProductTypeId foreign key(ProductTypeId) 
	references ProductsTypes(Id) on delete cascade on update cascade,
constraint CK_Value_ProducedProduct_ProductQuantity check(ProductQuantity > 0),
constraint CK_Day_ProducedProduct_Date check(datepart(day, [Date]) = 1),
constraint CK_Year_ProducedProduct_Date check(datepart(year, [Date]) >= 2000 and datepart(year, [Date]) <= 2023),
constraint UQ_ProducedProduct_Record unique(OrganizationId, ProductTypeId, [Date]));

-- Создание таблицы для хранения норм расхода теплоэнергии
create table HeatEnergyConsumptionRates
(Id int identity not null,
OrganizationId int not null,
ProductTypeId int not null,
Quantity real not null,
[Date] date not null,
constraint PK_HeatEnergyConsumptionRate_Id primary key(Id),
constraint FK_HeatEnergyConsumptionRate_OrganizationId foreign key(OrganizationId) 
	references Organizations(Id) on delete cascade on update cascade,
constraint FK_HeatEnergyConsumptionRate_ProductTypeId foreign key(ProductTypeId) 
	references ProductsTypes(Id) on delete cascade on update cascade,
constraint CK_Value_HeatEnergyConsumptionRate_Quantity check(Quantity > 0),
constraint CK_Day_HeatEnergyConsumptionRate_Date check(datepart(day, [Date]) = 1),
constraint CK_Year_HeatEnergyConsumptionRate_Date check(datepart(year, [Date]) >= 2000 and datepart(year, [Date]) <= 2023),
constraint UQ_HeatEnergyConsumptionRate_Record unique(OrganizationId, ProductTypeId, [Date]));

-- Создание таблицы для хранения информации о предоставленных услугах
create table ProvidedServices
(Id int identity not null,
OrganizationId int not null,
ServiceTypeId int not null,
Quantity int not null,
[Date] date not null,
constraint PK_ProvidedService_Id primary key(Id),
constraint FK_ProvidedService_OrganizationId foreign key(OrganizationId) 
	references Organizations(Id) on delete cascade on update cascade,
constraint FK_ProvidedService_ServiceTypeId foreign key(ServiceTypeId) 
	references ServicesTypes(Id) on delete cascade on update cascade,
constraint CK_Value_ProvidedService_Quantity check(Quantity > 0),
constraint CK_Day_ProvidedService_Date check(datepart(day, [Date]) = 1),
constraint CK_Year_ProvidedService_Date check(datepart(year, [Date]) >= 2000 and datepart(year, [Date]) <= 2023),
constraint UQ_ProvidedServices_Record unique(OrganizationId, ServiceTypeId, [Date]));