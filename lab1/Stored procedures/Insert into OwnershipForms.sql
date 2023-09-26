create proc InsertIntoOwnershipForms
	@Name nvarchar(100)
as
	insert into OwnershipForms values(@Name);