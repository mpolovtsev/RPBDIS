create proc InsertIntoChiefPowerEngineers
	@Name nvarchar(20),
	@Surname nvarchar(20),
	@MiddleName nvarchar(20),
	@PhoneNumber nchar(12),
	@OrganizationId int
as
	insert into ChiefPowerEngineers values(@Name, @Surname, @MiddleName, @PhoneNumber, @OrganizationId);