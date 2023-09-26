create proc InsertIntoManagers
	@Name nvarchar(20),
	@Surname nvarchar(20),
	@MiddleName nvarchar(20),
	@PhoneNumber nchar(12)
as
	insert into Managers values(@Name, @Surname, @MiddleName, @PhoneNumber);