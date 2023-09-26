create proc InsertIntoProductsTypes
	@Code nchar(6),
	@Name nvarchar(100),
	@Unit nvarchar(10)
as
	insert into ProductsTypes values(@Code, @Name, @Unit);