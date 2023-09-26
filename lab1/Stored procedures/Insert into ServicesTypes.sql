create proc InsertIntoServicesTypes
	@Code nchar(6),
	@Name nvarchar(100),
	@Unit nvarchar(10)
as
	insert into ServicesTypes values(@Code, @Name, @Unit);