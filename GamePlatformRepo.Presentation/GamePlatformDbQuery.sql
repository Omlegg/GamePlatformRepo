create table Games(
	[Id] int not null primary key identity,
	[Name] nvarchar(max) not null,
	[Description] nvarchar(max),
	[Views] int,
	[Price] money,
)

CREATE TABLE Logs (
    RequestId  int primary key identity not null,
    Url NVARCHAR(MAX),
    RequestBody NVARCHAR(MAX),
    RequestHeaders NVARCHAR(MAX),
    MethodType NVARCHAR(10),
    ResponseBody NVARCHAR(MAX),
    ResponseHeaders NVARCHAR(MAX),
    StatusCode INT,
    CreationDateTime DATETIME DEFAULT GETUTCDATE(),
    EndDateTime DATETIME NULL,
    ClientIp NVARCHAR(45)
);

drop table Logs

drop table Games

create table Games(
	[Id] int not null primary key identity,
	[Name] nvarchar(50) not null,
	[Description] nvarchar(250),
	[Views] int,
	[Price] money,
	[Creator] nvarchar(50) not null,
	[dateOfPublication] datetime
)

create table Comments(
	[Id] int not null primary key identity,
	[Title] nvarchar(50) not null,
	[Description] nvarchar(250),
	[Author] nvarchar(50) not null,
	[GameId] int not null,
	FOREIGN KEY (GameId) REFERENCES Games(Id)
)