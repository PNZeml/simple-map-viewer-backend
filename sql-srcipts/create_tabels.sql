create table users (
    id                bigserial                           not null
        constraint users_pk
            primary key,
    name              varchar(250)                        not null,
    email             varchar(340)                        not null,
    password_hashed   varchar(250)                        not null,
    access_token      varchar(250),
    avatar            bytea,
    creating_datetime timestamp default CURRENT_TIMESTAMP not null,
    is_deleted        boolean   default false             not null
);

create unique index users_password_hashed_uindex on users (password_hashed);

create sequence user_id_seq;

create table geo_files
(
    id                bigserial                           not null
        constraint geo_files_pk
            primary key,
    name              varchar(250)                        not null,
    created           timestamp                           not null,
    size              bigint                              not null,
    modified          timestamp,
    opened            timestamp,
    is_deleted        boolean   default false             not null,
    creating_datetime timestamp default CURRENT_TIMESTAMP not null
);

create sequence geo_file_id_seq;

create table user_geo_file_rels
(
    id                bigserial                           not null
        constraint user_geo_file_rels_pk
            primary key,
    user_id           bigint                              not null,
    geo_file_id       bigint                              not null,
    access_type       integer                             not null
        constraint user_geo_file_rels_access_type_check
            check (access_type = ANY (ARRAY [0, 1, 2])),
    creating_datetime timestamp default CURRENT_TIMESTAMP not null,
    is_deleted        boolean   default false             not null
);

create sequence user_geo_file_rel_id_seq;

create table geo_file_activity_records
(
    id                bigserial                           not null
        constraint geo_file_activity_records_pk
            primary key,
    geo_file_id       bigint                              not null,
    user_id           bigint                              not null,
    activity_type     integer                             not null,
    occured           timestamp                           not null,
    creating_datetime timestamp default CURRENT_TIMESTAMP not null,
    is_deleted        boolean   default false             not null
);

create sequence geo_file_activity_records_id_seq;

create table geo_file_comments
(
    id                bigserial                           not null
        constraint geo_file_comments_pk
            primary key,
    comment           varchar(1000)                       not null,
    x                 numeric                             not null,
    y                 numeric                             not null,
    geo_file_id       bigint                              not null,
    user_id           bigint                              not null,
    creating_datetime timestamp default CURRENT_TIMESTAMP not null,
    is_deleted        boolean   default false             not null
);

create sequence geo_file_comments_id_seq;

alter table user_geo_file_rels
    add constraint user_geo_file_rels_fk0 foreign key (user_id) references users(id),
    add constraint user_geo_file_rels_fk1 foreign key (file_id) references geo_files(id)
;

alter table geo_file_activity_records
    add constraint geo_file_activity_records_fk0 foreign key (user_id) references users(id),
    add constraint geo_file_activity_records_fk1 foreign key (file_id) references geo_files(id)
;

alter table geo_file_comments
    add constraint geo_file_comments_fk0 foreign key (user_id) references users(id),
    add constraint geo_file_comments_fk1 foreign key (file_id) references geo_files(id)
;
