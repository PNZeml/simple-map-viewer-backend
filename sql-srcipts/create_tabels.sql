create table users
(
    id              bigint       constraint users_pk primary key,
    name            varchar(250) not null,
    email           varchar(340) not null,
    password_hashed varchar(250) not null,
    access_token    varchar(250)
);

create table geo_files
(
    id          bigint       constraint geo_files_pk primary key,
    name        varchar(250) not null,
    upload_date date         not null,
    size        bigint       not null
);

create table user_geo_file_rels
(
    id          bigint constraint user_geo_file_rels_pk primary key,
	user_id     bigint not null,
	file_id     bigint not null,
	access_type int    not null check (access_type in (0, 1, 2))
);

ALTER TABLE user_geo_file_rels
    ADD CONSTRAINT user_geo_file_rels_fk0 FOREIGN KEY (user_id) REFERENCES users(id),
    ADD CONSTRAINT user_geo_file_rels_fk1 FOREIGN KEY (file_id) REFERENCES geo_files(id)
;

