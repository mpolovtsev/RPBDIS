create proc InsertIntoProducedProducts
	@OrganizationId int,
	@ProductTypeId int,
	@ProductQuantity int,
	@HeatEnergyQuantity real,
	@Date date
as
	insert into ProducedProducts values(@OrganizationId, @ProductTypeId, @ProductQuantity, @HeatEnergyQuantity, @Date);