set identity_insert cars off

alter table cars drop constraint ck_key_req;

-- Table alterations
alter table cars add constraint ck_key_req check((requires_key=1 and bp_1s_count is null) or 
								(requires_key=0 and bp_1s_count is not null))

alter table cars drop constraint ck_max_stars_6
	
alter table cars alter column bp_4s_count int null
alter table cars alter column bp_5s_count int null
alter table cars alter column bp_6s_count int null

alter table cars add class char(1) references class(class);

alter table cars alter column class char(1) not null;

alter table cars add constraint ck_max_stars_bps check( 
														(max_stars=3 and bp_2s_count>0 and bp_3s_count>0 and bp_4s_count is null and bp_5s_count is null and bp_6s_count is null)
														or (max_stars=4 and bp_2s_count>0 and bp_3s_count>0 and bp_4s_count>0 and bp_5s_count is null and bp_6s_count is null)
														or (max_stars=5 and bp_2s_count>0 and bp_3s_count>0 and bp_4s_count>0 and bp_5s_count>0 and bp_6s_count is null)
														or (max_stars=6 and bp_2s_count>0 and bp_3s_count>0 and bp_4s_count>0 and bp_5s_count>0 and bp_6s_count>0)
)
--This is insertion script for class D and C
insert into cars (name ,fuel, class ,top_speed ,acceleration, handling, nitro, max_stars,
				  max_rank, has_eips, no_eips, requires_key, bp_1s_count, bp_2s_count, bp_3s_count, bp_4s_count, bp_5s_count)
values
('MITSUBUSHI LANCER EVOLUTION', 6, 'D', 270.1, 55.03, 53.79, 68.19, 3, 1381, 0,null,0,5,8,30,null,null),
('BMW Z4 LCI E89',6, 'D', 266.8, 68.86, 47.43, 57.49, 3, 1476, 0,null,0,5,8,45,null,null),
('CHEVROLET CAMARO LT',6, 'D', 284.1, 64.81, 48.39, 63.29, 3, 1546, 0,null,0,5,12,30,NULL,NULL),
('NISSAN LEAF NISMO RC',6, 'D', 244.5, 78.87,	59.91, 65.03, 3, 1569, 0,NULL,0,15,25,55,NULL,NULL),
('NISSAN 370Z NISMO',6, 'D', 268.5, 66.61,	81.83, 67.07, 3, 1662, 0,null,0,10,12,30,null,null),
('KTM X-BOW GTX', 6, 'D', 247.5, 83.84, 64.99, 66.99, 3, 1738, 0, NULL, 0, 30,23,54,NULL, NULL),
('VOLKSWAGEN XL SPORT CONCEPT', 6, 'D', 291.2, 60.31, 62.02, 61.94, 3, 1814, 0,NULL,0,20, 12, 30, NULL, NULL),
('DS AUTOMOBILE DS E-TENSE', 6, 'D', 270.1, 76.07, 81.27, 72.30, 3, 1976, 0, NULL, 0, 20, 12, 30,NULL, NULL),
('VOLKSWAGEN ELECTRIC R', 5, 'D', 290.5, 88.5, 57.91, 67.93, 5, 3054, 1, 8, 1, NULL, 22, 30, 35, 38),
('DS AUTOMBILE DS E-TENSE PERFORMANCE' , 5, 'D', 303.8, 86.03, 46.54, 53.59, 5, 3086, 1, 8, 1, NULL, 22, 30, 35, 38),
('DODGE CHALLENGER 392 HEMI SCAT PACK', 6, 'D', 299.3, 72.46, 43.24, 62.34, 3, 2144, 0, NULL, 0, 20, 12, 30, NULL, NULL),
('RENAULT DEZIR', 5, 'D', 293.2, 65.56, 69.43, 68.44, 4, 2213, 1, 4, 0, 30, 23, 33, 42, NULL),
('RENAULT TREZOR', 5, 'D', 294.5, 78.62, 61.93, 61.07, 4, 2667, 1, 4, 0, 30, 23, 33, 42, NULL),
('ITALDESIGN DAVINCI' , 5, 'D', 278.3, 83.53, 73.57, 55.35, 4, 2217, 1, 4, 0, 30, 23, 33, 42, NULL),
('BMW i8 ROADSTER', 5, 'D', 278.3, 79.72, 78.61, 66.88, 4, 2247,1, 4, 0, 30, 23, 33, 42, NULL),
('PEUGEOT SR1', 5, 'D', 310.5, 71.51, 52.39, 31.19, 4, 2307,1, 4, 0, 30, 23, 33, 42, NULL),
('PORSCHE 911 CARRERA 3.8 RS', 5, 'D', 296.9, 70.52, 65.24, 35.55, 4, 2339 ,1, 4, 0, 30, 23, 33, 42, NULL)

insert into cars (name ,fuel, class ,top_speed ,acceleration, handling, nitro, max_stars,
				  max_rank, has_eips, no_eips, requires_key, bp_1s_count, bp_2s_count, bp_3s_count, bp_4s_count, bp_5s_count)
values
('PORSCHE 718 CAYMAN', 5, 'D', 295.7, 70.52, 61.47, 59.34, 4, 2360 ,1, 4, 0, 30, 23, 33, 42, NULL),
('PORSCHE 911 TARGA 4S', 5, 'D', 315.1, 75.37, 41.57, 38.35, 4, 2589, 1, 4, 0, 30, 23, 33, 42, NULL),
('INFINITI PROJECT BLACK S', 5, 'D', 281.2, 76.19, 66.97, 64.68, 4, 2368, 1, 4, 0, 30, 23, 33, 42, NULL),
('LOTUS ELISE SPRINT 220', 5, 'D', 270.0, 82.25, 83.47, 72.71, 4, 2390, 1, 4, 0, 10, 12, 18, 28, NULL),
('LOTUS EMEYA', 5, 'D', 276.5, 81.79, 75.33, 67.23, 4, 2402, 1, 4, 0, 15, 30, 35, 50, NULL)



select * from cars where requires_key = 1;


alter table cars drop column id
alter table cars drop constraint PK__cars__3213E83F772787FF
alter table cars add id int 

alter table cars add constraint pk_id_cars primary key(id)
set identity_insert cars on
update cars set id=1 where name like 'MITSU*';