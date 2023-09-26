create proc InsertIntoHeatEnergyConsumptionRates
	@OrganizationId int,
	@ProductTypeId int,
	@Quantity real,
	@Date date
as
	insert into HeatEnergyConsumptionRates values(@OrganizationId, @ProductTypeId, @Quantity, @Date);