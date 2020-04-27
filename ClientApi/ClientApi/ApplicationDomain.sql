create table [Type]
(
	TypeId int not null primary key clustered identity(1, 1),
	[Name] nvarchar(4000) not null
);

create table Environment
(
	EnvironmentId int not null primary key clustered identity(1, 1),
	[Name] nvarchar(4000) not null
);

create table [Owner]
(
	OwnerId int not null primary key clustered identity(1, 1),
	[Name] nvarchar(4000) not null
);

create table [Version]
(
	VersionId int not null primary key clustered identity(1, 1),
	[Name] nvarchar(4000) not null
);

create table [Stage]
(
	StageId int not null primary key clustered identity(1, 1),
	[Name] nvarchar(4000) not null
);

create table [Application]
(
	ApplicationId int not null primary key clustered identity(1, 1),
	[Name] nvarchar(4000) not null,
	TypeId int not null foreign key references [Type](TypeId),
	EnvironmentId int not null foreign key references [Environment](EnvironmentId),
	OwnerId int not null foreign key references [Owner](OwnerId),
	VersionId int not null foreign key references [Version](VersionId),
	StageId int not null foreign key references [Stage](StageId),
	CreatedDate datetime2 not null default getdate()
);

insert into [Type]([Name]) values ('Boring');
insert into [Type]([Name]) values ('Useful');
insert into [Type]([Name]) values ('Critical');

insert into Environment([Name]) values ('Production');
insert into Environment([Name]) values ('Test');

insert into [Owner]([Name]) values ('Platform');
insert into [Owner]([Name]) values ('CIS');
insert into [Owner]([Name]) values ('Solutions');
insert into [Owner]([Name]) values ('DEO');
insert into [Owner]([Name]) values ('Accelerator');

insert into [Version]([Name]) values ('1.0');
insert into [Version]([Name]) values ('2.0');

insert into [Stage]([Name]) values ('No idea');
insert into [Stage]([Name]) values ('Prototype');
insert into [Stage]([Name]) values ('Kinda Serious');
insert into [Stage]([Name]) values ('Production');

insert into [Application]([Name], TypeId, EnvironmentId, OwnerId, VersionId, StageId) values ('Platform',		1, 1, 1, 1, 1);
insert into [Application]([Name], TypeId, EnvironmentId, OwnerId, VersionId, StageId) values ('Programs',		2, 1, 5, 1, 2);
insert into [Application]([Name], TypeId, EnvironmentId, OwnerId, VersionId, StageId) values ('Manufacturing',  3, 1, 2, 1, 3);