create view GetInfoAboutProductsForViolators
as		
select ProductsTypes.[Name] as 'ProductName', ProductsTypes.[Code] as 'Code', Organizations.[Name] as 'OrganizationName',
		datepart(year, ProducedProducts.[Date]) as 'Year', datepart(quarter, ProducedProducts.[Date]) as 'Quarter'
	from ProducedProducts
		join Organizations on ProducedProducts.OrganizationId = Organizations.Id
		join ProductsTypes on ProducedProducts.ProductTypeId = ProductsTypes.Id
		join HeatEnergyConsumptionRates on ProducedProducts.OrganizationId = HeatEnergyConsumptionRates.OrganizationId and
		ProducedProducts.ProductTypeId = HeatEnergyConsumptionRates.ProductTypeId and
		ProducedProducts.[Date] = HeatEnergyConsumptionRates.[Date]
	group by Organizations.[Name], ProductsTypes.[Name], ProductsTypes.Code, datepart(year, ProducedProducts.[Date]), datepart(quarter, ProducedProducts.[Date])
	having sum(HeatEnergyConsumptionRates.Quantity) / sum(ProducedProducts.ProductQuantity) - 
		sum(ProducedProducts.HeatEnergyQuantity) / sum(ProducedProducts.ProductQuantity) < 0;