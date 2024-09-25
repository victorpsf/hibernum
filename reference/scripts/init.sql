create table auth (
	id bigint not null unique,
	name text not null,
	email varchar(500) not null,
	passphrase text not null,
	enabled bool default true,
	force_passphrase_change bool default false,
	created_at timestamp default current_timestamp,
	deleted_at timestamp,
	primary key (id)
);

create table company (
	id bigint not null unique,
	name varchar(500) not null unique,
	created_at timestamp default current_timestamp,
	deleted_at timestamp,
	primary key (id)
);

create table person (
	id bigint not null unique,
	type int not null,
	name varchar(500) not null,
	birth_date timestamp not null,
	companyid bigint,
	created_at timestamp default current_timestamp,
	deleted_at timestamp,
	primary key (id),
	foreign key (companyid) references company(id)
);

create table personcontact (
	id bigint not null unique,
	personid bigint not null,
	type int not null,
	value varchar(500) not null,
	created_at timestamp default current_timestamp,
	deleted_at timestamp,
	primary key (id),
	foreign key (personid) references person(id)
);

create table personaddress (
	id bigint not null unique,
	personid bigint not null,
	type int not null,
	value varchar(500) not null,
	created_at timestamp default current_timestamp,
	deleted_at timestamp,
	primary key (id),
	foreign key (personid) references person(id)
);

create table persondocument (
	id bigint not null unique,
	personid bigint not null,
	type int not null,
	value varchar(500) not null,
	created_at timestamp default current_timestamp,
	deleted_at timestamp,
	primary key (id),
	foreign key (personid) references person(id)
);

create table companyxperson (
	id bigint not null unique,
	personid bigint not null,
	companyid bigint not null,
	foreign key (personid) references person(id),
	foreign key (companyid) references company(id),
	primary key (id)
);

create table authxcompany (
	id bigint not null unique,
	authid bigint not null,
	companyid bigint not null,
	foreign key (authid ) references auth(id),
	foreign key (companyid) references company(id),
	primary key (id)
);

create sequence auth_sequence_generator 						as bigint increment 1 start 1 owned by public.auth.id;
create sequence company_sequence_generator 						as bigint increment 1 start 1 owned by public.company.id;
create sequence person_sequence_generator 						as bigint increment 1 start 1 owned by public.person.id;
create sequence person_contact_sequence_generator 				as bigint increment 1 start 1 owned by public.personcontact.id;
create sequence person_address_sequence_generator 				as bigint increment 1 start 1 owned by public.personaddress.id;
create sequence person_document_sequence_generator 				as bigint increment 1 start 1 owned by public.persondocument.id;
create sequence company_person_sequence_generator 				as bigint increment 1 start 1 owned by public.companyxperson.id;
create sequence auth_company_sequence_generator 				as bigint increment 1 start 1 owned by public.authxcompany.id;

insert into auth 
	(id, name, email, passphrase, enabled, force_passphrase_change)
values
	(
		(select nextval('public.auth_sequence_generator')), 
		'mMkAbsZsGS8tw0O0E4hw8hKBkhV5O26tFKKl88Fx32ukfgmvfHRJ17tladiN/IJ2ydOTWaY67DIRGyVmixHQfA==',
		'victorpsf2@hotmail.com',
		'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',
		true,
		false
	);

insert into auth 
	(id, name, email, passphrase, enabled, force_passphrase_change)
values
	(
		(select nextval('public.auth_sequence_generator')), 
		-- developer
		'EJUgK96NX9sJkMJdxmbXkRL4kVySb7bNx30nhvX5TbJlQGjxNl/N+rUG8EhZrm9elyXIG+ocdxdVNFzDb0pmaA==',
		'victorpsf2@gmail.com',
		-- dev@password#123
		'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',
		true,
		false
	);

insert into company 
	(id, "name")
values
	(
		(select nextval('public.company_sequence_generator')), 
		'Development'
	);
	
insert into authxcompany 
	(id, authid, companyid)
values 
	(
		(select nextval('public.auth_company_sequence_generator')),
		2,
		1
	);

insert into authxcompany 
	(id, authid, companyid)
values 
	(
		(select nextval('public.auth_company_sequence_generator')),
		1,
		1
	);