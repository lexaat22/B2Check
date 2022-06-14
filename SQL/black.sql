---------------------------------------------
-- Export file for user SMS@KAPITAL        --
-- Created by lexa on 14.06.2022, 10:20:46 --
---------------------------------------------

set define off
spool black.log

prompt
prompt Creating table BLACK_CONTR
prompt ==========================
prompt
create table BLACK_CONTR
(
  site       NUMBER not null,
  id         NUMBER not null,
  max_id     NUMBER default 0 not null,
  last_name  VARCHAR2(256),
  first_name VARCHAR2(256)
)
;
create unique index XUK_BLACK_CONTR on BLACK_CONTR (SITE, ID);
create index XUK_BLACK_CONTR_MAX on BLACK_CONTR (MAX_ID);

prompt
prompt Creating table BLACK_LIST
prompt =========================
prompt
create table BLACK_LIST
(
  source_id NUMBER not null,
  id        NUMBER not null,
  entity_id NUMBER not null,
  name      VARCHAR2(256) not null,
  name_norm VARCHAR2(256) not null
)
;
comment on table BLACK_LIST
  is 'Копия санкционных списков';
create index IDX_BLACK_LIST on BLACK_LIST (SOURCE_ID, ENTITY_ID, NAME);
alter table BLACK_LIST
  add constraint PK_BLACK_LIST primary key (ID);

prompt
prompt Creating table BLACK_RESULT
prompt ===========================
prompt
create table BLACK_RESULT
(
  site       NUMBER not null,
  id         NUMBER not null,
  list_id    NUMBER not null,
  similar    NUMBER not null,
  stime      DATE default sysdate not null,
  checked    NUMBER(1) default 0 not null,
  checked_by VARCHAR2(128)
)
;
comment on column BLACK_RESULT.stime
  is 'Время инсерта записи';
create index IDX_BLACK_RESULT_LIST on BLACK_RESULT (LIST_ID);
create index IDX_BLACK_RESULT_SIMILAR on BLACK_RESULT (SIMILAR);
create index IDX_BLACK_RESULT_SITE on BLACK_RESULT (SITE, ID);
create unique index XUK_BLACK_RESULT_SITE on BLACK_RESULT (SITE, ID, LIST_ID);
alter table BLACK_RESULT
  add constraint FK_BLACK_RESULT foreign key (LIST_ID)
  references BLACK_LIST (ID)
  disable
  novalidate;

prompt
prompt Creating table BLACK_SOURCE
prompt ===========================
prompt
create table BLACK_SOURCE
(
  id    NUMBER,
  sname VARCHAR2(20)
)
;
create unique index XUK_BLACK_SOURCE on BLACK_SOURCE (ID);

spool off
