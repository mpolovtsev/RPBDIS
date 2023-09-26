create proc InsertIntoOrganizations
	@Name nvarchar(100),
	@OwnershipFormId int,
	@Address nvarchar(100),
	@ManagerId int
as
	insert into Organizations values(@Name, @OwnershipFormId, @Address, @ManagerId);