use [master]
go

if (exists (select name from master.dbo.sysdatabases where ('[' + name + ']' = N'ALU' or name=N'ALU')))
drop database ALU
go

--Create database ALU
create database ALU;
go

use ALU
go

if OBJECT_ID('class') is not null
drop table class
go

if OBJECT_ID('cars') is not null
drop table cars
go


create table class(
	[class] char(1) primary key,
	[min_stars] tinyint not null,
    [max_stars] tinyint not null,
	[min_fuel] tinyint not null,
	[max_fuel] tinyint not null,
	[valid_rarity] int not null,
	
	--These constratints set the limit on the fields / columns min_stars and max_stars
	constraint [ck_min_stars] check ([min_stars] between 3 and 5),
    constraint [ck_max_stars] check ([max_stars] between 5 and 6),
	constraint [ck_valid_rarity] check ([valid_rarity] between 1 and 7),
	--These constraints set the limit on the fields / columns min_fuel and max_fuel
	constraint [ck_class_min_fuel] check (([class] in ('D','C') and [min_fuel]=4) or 
										  ([class] in ('B','A','S') and [min_fuel]=3)),

	constraint [ck_class_max_fuel] check (([class] in ('D','C','B') and [max_fuel]=6) or 
										  ([class]='A' and [max_fuel]=5) or 
										  ([class]='S' and [max_fuel]=4)),

	constraint [ck_class_valid_rarity] check (([class] in ('D','C','B') and valid_rarity=7) or 
											  ([class]='A' and valid_rarity=3) or 
											  ([class]='S' and valid_rarity=1)),

	
)

insert into class(class,min_stars,max_stars,min_fuel,max_fuel,valid_rarity) 
values 
('S',5,6,3,4,1),
('A',4,6,3,5,3),
('B',3,6,3,6,7),
('C',3,5,4,6,7),
('D',3,5,4,6,7);

create table cars
(
	[id] int primary key,
	[name] varchar(75) not null,
	[class] char(1) not null constraint fk_class references class(class),
	[fuel] tinyint constraint ck_fuel check (fuel<=14 and fuel>=3) not null,
	[rarity] char(4) not null constraint ck_rarity check (rarity in ('COMM','RARE','EPIC')),
	[top_speed] numeric(4,1) not null,
	[acceleration] numeric(4,2) not null,
	[handling] numeric(4,2) not null,
	[nitro] numeric(4,2) not null,
	[max_stars] tinyint constraint ck_stars check (max_stars>=3 and max_stars<=6) not null,
	[max_rank] numeric(4,0) not null,
	[has_eips] bit not null,
	[no_eips] tinyint default null, 
	[requires_key] bit not null,
	[bp_1s_count] int null,
	[bp_2s_count] int not null,
	[bp_3s_count] int not null,
	[bp_4s_count] int null,
	[bp_5s_count] int null ,
	[bp_6s_count] int null ,
	constraint ck_rarity_stars check ((rarity='COMM' and max_stars=3) or (rarity='RARE' and max_stars=4) or (rarity='EPIC' and max_stars in (5,6))),

	constraint ck_rarity_eips check ((rarity='COMM' and ((class in ('D','A','S') and no_eips is NULL) or (class in ('C','B') and has_eips=1 and no_eips=4)))
									or
									(rarity='RARE' and ((class in ('D','C') and has_eips=1 and no_eips=4) or (class in ('B','A') and has_eips=1 and no_eips=8) or (class='S' and no_eips is null)))
									or
									(rarity = 'EPIC' and ((class in ('D','C') and has_eips=1 and no_eips=8) or (class='B' and has_eips=1 and no_eips in (8,12)) or (class in ('A','S') and has_eips=1 and no_eips in (12,16))))
	),

	constraint ck_no_eips check ((has_eips=1 and no_eips between 4 and 16) 
									or
									(has_eips=0 and no_eips is null)),
	constraint ck_key_req check((requires_key=1 and bp_1s_count is null) or 
								(requires_key=0 and bp_1s_count is not null)),

	constraint ck_max_stars_bps check( 
		(max_stars=3 and bp_2s_count>0 and bp_3s_count>0 and bp_4s_count is null and bp_5s_count is null and bp_6s_count is null)
		or (max_stars=4 and bp_2s_count>0 and bp_3s_count>0 and bp_4s_count>0 and bp_5s_count is null and bp_6s_count is null)
		or (max_stars=5 and bp_2s_count>0 and bp_3s_count>0 and bp_4s_count>0 and bp_5s_count>0 and bp_6s_count is null)
		or (max_stars=6 and bp_2s_count>0 and bp_3s_count>0 and bp_4s_count>0 and bp_5s_count>0 and bp_6s_count>0)
	)


)



insert into cars (id ,name ,fuel ,class ,rarity ,top_speed ,acceleration, handling, nitro, max_stars,
				  max_rank, has_eips, no_eips, requires_key, bp_1s_count, bp_2s_count, bp_3s_count, bp_4s_count, bp_5s_count)
values
(1,'MITSUBUSHI LANCER EVOLUTION', 6, 'D','COMM', 270.1, 55.03, 53.79, 68.19, 3, 1381, 0,null,0,5,8,30,null,null),
(2,'BMW Z4 LCI E89',6, 'D','COMM', 266.8, 68.86, 47.43, 57.49, 3, 1476, 0,null,0,5,8,45,null,null),
(3,'CHEVROLET CAMARO LT',6, 'D','COMM', 284.1, 64.81, 48.39, 63.29, 3, 1546, 0,null,0,5,12,30,NULL,NULL),
(4,'NISSAN LEAF NISMO RC',6, 'D','COMM', 244.5, 78.87,	59.91, 65.03, 3, 1569, 0,NULL,0,15,25,55,NULL,NULL),
(5,'NISSAN 370Z NISMO',6, 'D','COMM', 268.5, 66.61,	81.83, 67.07, 3, 1662, 0,null,0,10,12,30,null,null),
(6,'KTM X-BOW GTX', 6, 'D','COMM', 247.5, 83.84, 64.99, 66.99, 3, 1738, 0, NULL, 0, 30,23,54,NULL, NULL),
(7,'VOLKSWAGEN XL SPORT CONCEPT', 6, 'D','COMM', 291.2, 60.31, 62.02, 61.94, 3, 1814, 0,NULL,0,20, 12, 30, NULL, NULL),
(8,'DS AUTOMOBILE DS E-TENSE', 6, 'D','COMM', 270.1, 76.07, 81.27, 72.30, 3, 1976, 0, NULL, 0, 20, 12, 30,NULL, NULL),
(9,'VOLKSWAGEN ELECTRIC R', 4, 'D','EPIC', 290.5, 88.5, 57.91, 67.93, 5, 3054, 1, 8, 1, NULL, 22, 30, 35, 38),
(10,'DS AUTOMBILE DS E-TENSE PERFORMANCE' , 4, 'D','EPIC', 303.8, 86.03, 46.54, 53.59, 5, 3086, 1, 8, 1, NULL, 22, 30, 35, 38),
(11,'DODGE CHALLENGER 392 HEMI SCAT PACK', 6, 'D','COMM', 299.3, 72.46, 43.24, 62.34, 3, 2144, 0, NULL, 0, 20, 12, 30, NULL, NULL),
(12,'RENAULT DEZIR', 5, 'D','RARE', 293.2, 65.56, 69.43, 68.44, 4, 2213, 1, 4, 0, 30, 23, 33, 42, NULL),
(13,'RENAULT TREZOR', 5, 'D','RARE', 294.5, 78.62, 61.93, 61.07, 4, 2667, 1, 4, 0, 30, 23, 33, 42, NULL),
(14,'ITALDESIGN DAVINCI' , 5, 'D','RARE', 278.3, 83.53, 73.57, 55.35, 4, 2217, 1, 4, 0, 30, 23, 33, 42, NULL),
(15,'BMW i8 ROADSTER', 5, 'D','RARE', 278.3, 79.72, 78.61, 66.88, 4, 2247,1, 4, 0, 30, 23, 33, 42, NULL),
(16,'PEUGEOT SR1', 5, 'D','RARE', 310.5, 71.51, 52.39, 31.19, 4, 2307,1, 4, 0, 30, 23, 33, 42, NULL),
(17,'PORSCHE 911 CARRERA 3.8 RS', 5, 'D','RARE', 296.9, 70.52, 65.24, 35.55, 4, 2339 ,1, 4, 0, 30, 23, 33, 42, NULL),
(18,'PORSCHE 718 CAYMAN', 5, 'D','RARE', 295.7, 70.52, 61.47, 59.34, 4, 2360 ,1, 4, 0, 30, 23, 33, 42, NULL),
(19,'PORSCHE 911 TARGA 4S', 5, 'D','RARE', 315.1, 75.37, 41.57, 38.35, 4, 2589, 1, 4, 0, 30, 23, 33, 42, NULL),
(20,'INFINITI PROJECT BLACK S', 5, 'D','RARE', 281.2, 76.19, 66.97, 64.68, 4, 2368, 1, 4, 0, 30, 23, 33, 42, NULL),
(21,'LOTUS ELISE SPRINT 220', 5, 'D','RARE', 270.0, 82.25, 83.47, 72.71, 4, 2390, 1, 4, 0, 10, 12, 18, 28, NULL),
(22,'LOTUS EMEYA', 5, 'D','RARE', 276.5, 81.79, 75.33, 67.23, 4, 2402, 1, 4, 0, 15, 30, 35, 50, NULL)



create table rarity
(
	[rarity] char(4) primary key,
	[value] int not null
)

insert into rarity values ('COMM',4),('RARE',2),('EPIC',1)