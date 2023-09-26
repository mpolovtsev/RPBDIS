create view GetInfoAboutImplementationOfHeatEnergyConsumptionRates
as		
select Organizations.[Name] as 'OrganizationName', ProductsTypes.[Name] as 'ProductName',
		sum(ProducedProducts.HeatEnergyQuantity) / sum(ProducedProducts.ProductQuantity) as 'ActualHeatEnergyConsumptionPerUnit',
		sum(HeatEnergyConsumptionRates.Quantity) / sum(ProducedProducts.ProductQuantity) as 'NormalizedHeatEnergyConsumptionPerUnit',
		datepart(year, ProducedProducts.[Date]) as 'Year', datepart(quarter, ProducedProducts.[Date]) as 'Quarter'
	from ProducedProducts
		join Organizations on ProducedProducts.OrganizationId = Organizations.Id
		join ProductsTypes on ProducedProducts.ProductTypeId = ProductsTypes.Id
		join HeatEnergyConsumptionRates on ProducedProducts.OrganizationId = HeatEnergyConsumptionRates.OrganizationId and
		ProducedProducts.ProductTypeId = HeatEnergyConsumptionRates.ProductTypeId and
		ProducedProducts.[Date] = HeatEnergyConsumptionRates.[Date]
	group by Organizations.[Name], ProductsTypes.[Name], datepart(year, ProducedProducts.[Date]), datepart(quarter, ProducedProducts.[Date]);