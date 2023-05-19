create database studentinfo;

use studentinfo;

----CREATE DEPT AND STAFF TABLES---------
Create table staffs( staffid bigint identity(11,10) primary key, 
staffname varchar (50), 
staffnumber bigint,
staffemail varchar (50));

create table Students( studentid bigint identity (111,10) primary key,
studentname varchar (50),
studentLname varchar(50),
studentnumber bigint,
studentemail varchar(50),
Datemeeting date,
mettingtime time, 
meetingwith varchar (50),
staffid bigint, foreign key (staffid) references staff(staffid));



use studentinfo;
INSERT INTO staff (staffname, staffnumber, staffemail)
Values
('Arjinder', 0271501124, 'arjinder@gmail.com'),
('Suede', 0221897640, 'Suede@hotmail.com'),
('Rashmi', 0262842271, 'Rashmi@outlook.com'),
('Connor', 0211178994, 'Connor@gmail.co.nz'),
('Richard',0192812932,'Richard@gmail.co.nz');

----- add in new colums-----
/*
use studentinfo;
alter table Student 
add meetingaim varchar (50);
*/

----- delete in new colums-----
/*
use studentinfo;
alter table Student 
drop meetingaim varchar (50);
*/