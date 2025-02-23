create table Games(
	[Id] int not null primary key identity,
	[Name] nvarchar(max) not null,
	[Description] nvarchar(max),
	[Views] int,
	[Price] money,
)