create proc InsertIntoProvidedServices
	@OrganizationId int,
	@ServiceTypeId int,
	@Quantity int,
	@Date date
as
	insert into ProvidedServices values(@OrganizationId, @ServiceTypeId, @Quantity, @Date);