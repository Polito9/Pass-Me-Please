drop database if exists Jueguito;
create database Jueguito;
use Jueguito;

create table JUGADOR(
Id_jgdr varchar(30) primary key not null,
contra_jgdr varchar(20),
nombre varchar(20)
);
create table CONFIGURACIONES(
Id_config int primary key not null auto_increment,
indicador varchar(20),
config varchar(7),
Id_jgdr varchar(30),
foreign key (Id_jgdr) references JUGADOR(Id_jgdr) on update cascade on delete cascade
);
create table PARTIDA(
Num_partida int primary key not null auto_increment,
d1 int,
batQuim int,
batMusic int,
batBio int,
batMate int,
batHist int,
batFisic int,
Id_jgdr varchar(30),
foreign key (Id_jgdr) references JUGADOR(Id_jgdr) on update cascade on delete cascade
);

create table Posicion(
id_mapa int primary key not null auto_increment,
num_mapa int,
x float(4),
y float(4),
Id_jgdr varchar(30),
foreign key (Id_jgdr) references JUGADOR(Id_jgdr) on update cascade on delete cascade
);

create table MEJORA(
mejora_pista bit,
mejora_saltarP bit,
mejora_velocidad bit,
Id_jgdr varchar(30), 
foreign key (Id_jgdr) references JUGADOR(Id_jgdr) on update cascade on delete cascade
);