create view GetInfoAboutViolatorsOfHeatEnergyConsumptionRates
as
	select OrganizationName, ProductName, 
		abs(NormalizedHeatEnergyConsumptionPerUnit - ActualHeatEnergyConsumptionPerUnit) as 'ExcessConsumption',
		[Year], [Quarter]
	from GetInfoAboutImplementationOfHeatEnergyConsumptionRates
	where (NormalizedHeatEnergyConsumptionPerUnit - ActualHeatEnergyConsumptionPerUnit) < 0;