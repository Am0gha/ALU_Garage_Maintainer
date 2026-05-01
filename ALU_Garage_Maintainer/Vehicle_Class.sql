update class set min_stars=3 where class in ('D','C','B');
update class set min_stars=4 where class = 'A';

update class set min_stars=5 where class = 'S';
insert into class values ('S'),('A'),('B'),('C'),('D');
select * from class
