use HeatEnergyConsumption;

declare @Symbols char(52) = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz',
		@Numbers char(10)= '1234567890',
		@RowCount int,
		@MinNumberSymbols int,
		@MaxNumberSymbols int,
		@Limit int,
		@i int,
	    @OwnershipFormName nvarchar(100),
		@ManagerName nvarchar(20),
		@ManagerSurname nvarchar(20),
		@ManagerMiddleName nvarchar(20),
		@ManagerPhoneNumber nvarchar(12),
		@OrganizationName nvarchar(100),
		@OwnershipFormId int,
		@Address nvarchar(100),
		@ManagerId int,
		@ChiefPowerEngineerName nvarchar(20),
		@ChiefPowerEngineerSurname nvarchar(20),
		@ChiefPowerEngineerMiddleName nvarchar(20),
		@ChiefPowerEngineerPhoneNumber nvarchar(12),
		@OrganizationId int,
		@ProductTypeName nvarchar(100),
		@Code nvarchar(6),
		@Unit nvarchar(10),
		@ServiceTypeName nvarchar(100),
		@ProductTypeId int,
		@ProductQuantity int,
		@ActualHeatEnergyQuantity real,
		@NormalHeatEnergyQuantity real,
		@StartDate date,
		@EndDate date,
		@NumberMonths int,
		@Date date,
		@ServiceTypeId int,
		@ServicesQuantity int,
		@NumberOwnershipForms int,
		@NumberManagers int,
		@NumberChiefPowerEngineers int,
		@NumberOrganizations int,
		@NumberProductsTypes int,
		@NumberServicesTypes int,
		@NumberProducedProducts int,
		@NumberProvidedServices int

set @NumberOwnershipForms = 500;
set @NumberManagers = 500;
set @NumberOrganizations = 25000;
set @NumberChiefPowerEngineers = 500;
set @NumberProductsTypes = 500;
set @NumberServicesTypes = 500;
set @NumberProducedProducts = 25000;
set @NumberProvidedServices = 25000;
set @StartDate = '20000101';
set @EndDate = '20231201';
set @NumberMonths = datediff(month, @StartDate, @EndDate);

begin tran

	-- Заполнение таблицы OwnershipForms

	set @RowCount = 1;
	set @MinNumberSymbols = 5;
	set @MaxNumberSymbols = 100;

	while @RowCount <= @NumberOwnershipForms
	begin		

		set @Limit = @MinNumberSymbols + rand() * (@MaxNumberSymbols - @MinNumberSymbols);
		set @i = 1;
        set @OwnershipFormName = '';

		while @i <= @Limit
		begin

			set @OwnershipFormName += substring(@Symbols, cast(1 + rand() * (len(@Symbols) - 1) as int), 1);
			set @i += 1;

		end

		insert into OwnershipForms values(@OwnershipFormName);
		set @RowCount += 1;

	end;

	-- Заполнение таблицы Managers

	set @RowCount = 1;
	set @MinNumberSymbols = 5;
	set @MaxNumberSymbols = 20;

	while @RowCount <= @NumberManagers
	begin		

		set @Limit = @MinNumberSymbols + rand() * (@MaxNumberSymbols - @MinNumberSymbols);
		set @i = 1;
        set @ManagerName = '';
		set @ManagerSurname = '';
		set @ManagerMiddleName = '';

		while @i <= @Limit
		begin

			set @ManagerName += substring(@Symbols, cast(1 + rand() * (len(@Symbols) - 1) as int), 1);
			set @ManagerSurname += substring(@Symbols, cast(1 + rand() * (len(@Symbols) - 1) as int), 1);
			set @ManagerMiddleName += substring(@Symbols, cast(1 + rand() * (len(@Symbols) - 1) as int), 1);
			set @i += 1;

		end

		set @i = 1;
		set @ManagerPhoneNumber = '37544';

		while @i <= 7
		begin

			set @ManagerPhoneNumber += substring(@Numbers, cast(1 + rand() * (len(@Numbers) - 1) as int), 1);
			set @i += 1;

		end

		insert into Managers values(@ManagerName, @ManagerSurname, @ManagerMiddleName, @ManagerPhoneNumber);
		set @RowCount += 1;

	end;

	-- Заполнение таблицы Organizations

	set @RowCount = 1;	
	set @MinNumberSymbols = 5;
	set @MaxNumberSymbols = 100;

	while @RowCount <= @NumberOrganizations
	begin		

		set @Limit = @MinNumberSymbols + rand() * (@MaxNumberSymbols - @MinNumberSymbols);
		set @OwnershipFormId = cast((1 + rand() * (@NumberOwnershipForms - 1)) as int);
		set @ManagerId = cast((1 + rand() * (@NumberManagers - 1)) as int);
		set @i = 1;
		set @OrganizationName = '';

		while @i <= @Limit
		begin

			set @OrganizationName += substring(@Symbols, cast(1 + rand() * (len(@Symbols) - 1) as int), 1);
			set @i += 1;

		end

		set @Limit = @MinNumberSymbols + rand() * (@MaxNumberSymbols - @MinNumberSymbols);
		set @i = 1;
		set @Address = '';

		while @i <= @Limit
		begin

			set @Address += substring(@Symbols, cast(1 + rand() * (len(@Symbols) - 1) as int), 1);
			set @i += 1;

		end

		insert into Organizations values(@OrganizationName, @OwnershipFormId, @Address, @ManagerId);
		set @RowCount += 1;

	end;

	-- Заполнение таблицы ChiefPowerEngineers

	set @RowCount = 1;
	set @MinNumberSymbols = 5;
	set @MaxNumberSymbols = 20;	

	while @RowCount <= @NumberChiefPowerEngineers
	begin		

		set @Limit = @MinNumberSymbols + rand() * (@MaxNumberSymbols - @MinNumberSymbols);
		set @OrganizationId = cast((1 + rand() * (@NumberOrganizations - 1)) as int);
		set @i = 1;
        set @ChiefPowerEngineerName = '';
		set @ChiefPowerEngineerSurname = '';
		set @ChiefPowerEngineerMiddleName = '';

		while @i <= @Limit
		begin

			set @ChiefPowerEngineerName += substring(@Symbols, cast(1 + rand() * (len(@Symbols) - 1) as int), 1);
			set @ChiefPowerEngineerSurname += substring(@Symbols, cast(1 + rand() * (len(@Symbols) - 1) as int), 1);
			set @ChiefPowerEngineerMiddleName += substring(@Symbols, cast(1 + rand() * (len(@Symbols) - 1) as int), 1);
			set @i += 1;

		end

		set @i = 1;
		set @ChiefPowerEngineerPhoneNumber = '37544';

		while @i <= 7
		begin

			set @ChiefPowerEngineerPhoneNumber += substring(@Numbers, cast(1 + rand() * (len(@Numbers) - 1) as int), 1);
			set @i += 1;

		end

		insert into ChiefPowerEngineers values(@ChiefPowerEngineerName, @ChiefPowerEngineerSurname, 
		@ChiefPowerEngineerMiddleName, @ChiefPowerEngineerPhoneNumber, @OrganizationId);
		set @RowCount += 1;

	end;

	-- Заполнение таблицы ProductsTypes

	set @RowCount = 1;	

	while @RowCount <= @NumberProductsTypes
	begin		
		
		set @MinNumberSymbols = 5;
		set @MaxNumberSymbols = 100;
		set @Limit = @MinNumberSymbols + rand() * (@MaxNumberSymbols - @MinNumberSymbols);
		set @i = 1;
        set @ProductTypeName = '';

		while @i <= @Limit
		begin

			set @ProductTypeName += substring(@Symbols, cast(1 + rand() * (len(@Symbols) - 1) as int), 1);
			set @i += 1;

		end

		set @MinNumberSymbols = 2;
		set @MaxNumberSymbols = 10;
		set @Limit = @MinNumberSymbols + rand() * (@MaxNumberSymbols - @MinNumberSymbols);
		set @i = 1;
		set @Unit = '';

		while @i <= @Limit
		begin

			set @Unit += substring(@Symbols, cast(1 + rand() * (len(@Symbols) - 1) as int), 1);
			set @i += 1;

		end

		set @i = 1;
		set @Code = '';

		while @i <= 6
		begin
			
			set @Code += substring(@Numbers, cast(1 + rand() * (len(@Numbers) - 1) as int), 1);
			set @i += 1;

		end

		insert into ProductsTypes values(@Code, @ProductTypeName, @Unit);
		set @RowCount += 1;

	end;

	-- Заполнение таблицы ServicesTypes

	set @RowCount = 1;	

	while @RowCount <= @NumberServicesTypes
	begin		

		set @MinNumberSymbols = 5;
		set @MaxNumberSymbols = 100;
		set @Limit = @MinNumberSymbols + rand() * (@MaxNumberSymbols - @MinNumberSymbols);
		set @i = 1;
        set @ServiceTypeName = '';

		while @i <= @Limit
		begin

			set @ServiceTypeName += substring(@Symbols, cast(1 + rand() * (len(@Symbols) - 1) as int), 1);
			set @i += 1;

		end

		set @MinNumberSymbols = 2;
		set @MaxNumberSymbols = 10;
		set @Limit = @MinNumberSymbols + rand() * (@MaxNumberSymbols - @MinNumberSymbols);
		set @i = 1;
		set @Unit = '';

		while @i <= @Limit
		begin

			set @Unit += substring(@Symbols, cast(1 + rand() * (len(@Symbols) - 1) as int), 1);
			set @i += 1;

		end

		set @i = 1;
		set @Code = '';

		while @i <= 6
		begin
			
			set @Code += substring(@Numbers, cast(1 + rand() * (len(@Numbers) - 1) as int), 1);
			set @i += 1;

		end

		insert into ServicesTypes values(@Code, @ServiceTypeName, @Unit);
		set @RowCount += 1;

	end;

	-- Заполнение таблиц ProducedProducts и HeatEnergyConsumptionRates

	set @RowCount = 1;	

	while @RowCount <= @NumberProducedProducts
	begin
		
		set @OrganizationId = cast((1 + rand() * (@NumberOrganizations - 1)) as int);
		set @ProductTypeId = cast((1 + rand() * (@NumberProductsTypes - 1)) as int);
		set @ProductQuantity =  cast((rand() * 99 + 1) as int);
		set @ActualHeatEnergyQuantity = rand() * 990 + 10;
		set @NormalHeatEnergyQuantity =  rand() * 990 + 10;
		set @Date = dateadd(month, 1 + rand() *(@NumberMonths - 1), @StartDate)
        
		insert into ProducedProducts values(@OrganizationId, @ProductTypeId, @ProductQuantity,  @ActualHeatEnergyQuantity, @Date);
		insert into HeatEnergyConsumptionRates values(@OrganizationId, @ProductTypeId, @NormalHeatEnergyQuantity, @Date);
		set @RowCount += 1;

	end;

	-- Заполнение таблицы ProvidedServices

	set @RowCount = 1;	

	while @RowCount <= @NumberProvidedServices
	begin		
		
		set @OrganizationId = cast((1 + rand() * (@NumberOrganizations - 1)) as int);
		set @ServiceTypeId = cast((1 + rand() * (@NumberServicesTypes - 1)) as int);
		set @ServicesQuantity =  cast((rand() * 99 + 1) as int);
		set @Date = dateadd(month, 1 + rand() *(@NumberMonths - 1), @StartDate)
        
		insert into ProvidedServices values(@OrganizationId, @ServiceTypeId, @ServicesQuantity, @Date);
		set @RowCount += 1;

	end;

commit tran;