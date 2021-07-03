create table tab_registrationlog
(
    id integer not null primary key autoincrement ,
    type_event text not null check(type_event != ' '),
    date_time_event text not null,
    user text not null,
    message text not null
);

create table tab_successlog
(
    id integer not null primary key autoincrement ,
    id_registration integer not null ,
    success_message text not null
);

create table tab_errorslog
(
    id integer not null primary key autoincrement ,
    id_registration integer not null ,
    errors_message text not null
);
