create or replace package black as

  -- Author  : LEXA
  -- Created : 02.06.2022 17:06:54
  -- Purpose : Поиск контрагентов в Б2, которые есть в списках ОФАК, EU и т.д.

  -- Минимальный процент совпадения
  min_percent_ constant number := 90;
  client_info_ constant varchar2(128) := 'BLACK.StartSearch running';
  max_threads_ constant number := 20;

  TYPE ref_cursor IS REF CURSOR;

  TYPE source_record IS RECORD(
    id   NUMBER(5),
    name VARCHAR2(20));
  TYPE source_table IS TABLE OF source_record;

  TYPE result_record IS RECORD(
    checked    NUMBER(2),
    siteid     NUMBER(10),
    id         NUMBER(10),
    name_b2    VARCHAR2(254),
    name_list  VARCHAR2(256),
    list_id    NUMBER,
    entity_id  NUMBER,
    sname      VARCHAR2(20),
    similar    NUMBER,
    stime      VARCHAR2(22),
    checked_by VARCHAR2(128));
  TYPE result_table IS TABLE OF result_record;
  -- Обновление черных списков
  procedure updateLists;

  -- Обновление списка контрагентов
  procedure updateContrs;

  -- Поиск контрагентов в черных списках
  procedure startSearch;

  -- Поиск контрагентов в черных списках в max_threads_ потоков
  procedure startSearchMax;

  -- Получение прогресса выполнения процедуры StartSearch. 1=завершён
  function getProgress return number;

  -- Получение количества запущенных процессов StartSearch. 0=процессов нет
  function getProcCount return number;

  -- Получение списка справочников
  function getSources RETURN source_table
    PIPELINED;

  -- Получение результата поиска
  function getSearchResult(proc_ in number,
                           list_ in varchar2,
                           fio_  in varchar2) RETURN result_table
    PIPELINED;

  -- Выделение строки
  procedure setChecked(site_    in number,
                       id_      in number,
                       list_id_ in number,
                       checked_ in number);

  -- Починялка нумерации черного списка
  procedure fixList(max_id number);
end black;
/
create or replace package body black as

  --
  procedure updateLists is
    max_id number;
  begin
    --Обновление списка EU
    select max(id) into max_id from BLACK_LIST;
    BEGIN
       execute immediate 'drop sequence seq_black_list';
    EXCEPTION
      WHEN OTHERS THEN
        null;
    END;
   
    execute immediate 'create sequence seq_black_list start with ' ||
                      max_id || ' increment by 1';
    execute immediate 'MERGE INTO BLACK_LIST l
    USING (select 10 source_id, id, euentity_id, wholename name, name_norm
             from (select distinct first_value(id) OVER(PARTITION BY wholename ORDER BY id DESC) AS id,
                                   euentity_id,
                                   wholename,
                                   trim(REGEXP_REPLACE(upper(wholename),
                                                       ''[^A-Z А-Я]+'',
                                                       '''')) name_norm
                     from CREATOR.EUNAME@B2PROD)
            where length(name_norm) > 0
            order by id) s
    ON (l.source_id = s.source_id and l.entity_id = s.euentity_id and l.name = s.name)
    WHEN MATCHED THEN
      UPDATE SET l.name_norm = s.name_norm
    WHEN NOT MATCHED THEN
      INSERT
        (l.source_id, l.id, l.entity_id, l.name, l.name_norm)
      VALUES
        (s.source_id,
         seq_black_list.nextval,
         s.euentity_id,
         s.name,
         s.name_norm)';
  
    --Обновление списка OFAC и др.
    execute immediate 'MERGE INTO BLACK_LIST l
    USING (select *
             from (select sourcerecid source_id,
                          0 id,
                          id entity_id,
                          name_lat name,
                          trim(REGEXP_REPLACE(upper(name_lat),
                                              ''[^A-Z А-Я]+'',
                                              '''')) name_norm
                     from CREATOR.FMBLACKLIST@B2PROD)
            where length(name_norm) > 0
            order by id) s
    ON (l.source_id = s.source_id and l.entity_id = s.entity_id and l.name = s.name)
    WHEN MATCHED THEN
      UPDATE SET l.name_norm = s.name_norm
    WHEN NOT MATCHED THEN
      INSERT
        (l.source_id, l.id, l.entity_id, l.name, l.name_norm)
      VALUES
        (s.source_id,
         seq_black_list.nextval,
         s.entity_id,
         s.name,
         s.name_norm)';
    commit;
    fixlist(max_id);
  end;

  --
  procedure updateContrs is
  begin
    MERGE INTO BLACK_CONTR b
    USING (select siteid site,
                  id,
                  trim(REGEXP_REPLACE(upper(clientlastname),
                                      '[^A-Z А-Я]+',
                                      '')) last_name,
                  trim(REGEXP_REPLACE(upper(clientname), '[^A-Z А-Я]+', '')) first_name
             from CREATOR.CONTRAGENT@B2PROD
            where contragentstateid <> 4
              and contragenttypeid = '08') c
    ON (b.site = c.site and b.id = c.id)
    WHEN NOT MATCHED THEN
      INSERT
        (b.site, b.id, b.max_id, b.last_name, b.first_name)
      VALUES
        (c.site, c.id, 0, c.last_name, c.first_name);
    commit;
  end;

  --
  procedure startSearch is
    prog_   number;
    max_id_ number;
    fio_    varchar2(256);
  begin
    if (getProcCount >= max_threads_) then
      return;
    end if;
    DBMS_APPLICATION_INFO.SET_CLIENT_INFO(client_info_);
    UpdateLists;
    UpdateContrs;
    prog_ := getProgress;
  
    select max(id) into max_id_ from BLACK_LIST;
  
    WHILE prog_ < 1 LOOP
      -- Выбираем рандомно одного контрагента и ищем его по спискам
      FOR r IN (select *
                  from (select *
                          from BLACK_CONTR
                         where MAX_ID < max_id_
                         ORDER BY DBMS_RANDOM.RANDOM)
                 where rownum <= 1) LOOP
      
        -- Поиск по фамилии и имени   
        fio_ := r.last_name || ' ' || r.first_name;
        BEGIN
          MERGE INTO BLACK_RESULT b
          USING (select r.site site, r.id id, id list_id, jws
                   from (SELECT id,
                                greatest(UTL_MATCH.jaro_winkler_similarity(fio_, name_norm), UTL_MATCH.jaro_winkler_similarity(fio_, nvl(REGEXP_SUBSTR(name_norm, '[^ ]+ [^ ]+'), name_norm))) AS jws
                           FROM BLACK_LIST)
                  WHERE jws >= min_percent_
                    and id > r.max_id) c
          ON (b.site = c.site and b.id = c.id and b.list_id = c.list_id)
          WHEN MATCHED THEN
            UPDATE set b.similar = c.jws
          WHEN NOT MATCHED THEN
            INSERT
              (b.site, b.id, b.list_id, b.similar)
            VALUES
              (c.site, c.id, c.list_id, c.jws);
        EXCEPTION
          WHEN OTHERS THEN
            DBMS_OUTPUT.PUT_LINE('Error code : ' || SQLCODE ||
                                 ' Message : ' || SUBSTR(SQLERRM, 1, 200));
        END;
        commit;
      
        -- Поиск по имени и фамилии   
        fio_ := r.first_name || ' ' || r.last_name;
        BEGIN
          MERGE INTO BLACK_RESULT b
          USING (select r.site site, r.id id, id list_id, jws
                   from (SELECT id,
                                greatest(UTL_MATCH.jaro_winkler_similarity(fio_, name_norm), UTL_MATCH.jaro_winkler_similarity(fio_, nvl(REGEXP_SUBSTR(name_norm, '[^ ]+ [^ ]+'), name_norm))) AS jws
                           FROM BLACK_LIST)
                  WHERE jws >= min_percent_
                    and id > r.max_id) c
          ON (b.site = c.site and b.id = c.id and b.list_id = c.list_id)
          WHEN MATCHED THEN
            UPDATE set b.similar = c.jws
          WHEN NOT MATCHED THEN
            INSERT
              (b.site, b.id, b.list_id, b.similar)
            VALUES
              (c.site, c.id, c.list_id, c.jws);
        EXCEPTION
          WHEN OTHERS THEN
            DBMS_OUTPUT.PUT_LINE('Error code : ' || SQLCODE ||
                                 ' Message : ' || SUBSTR(SQLERRM, 1, 200));
        END;
        commit;
      
        -- Сохраняем максимальный ИД списка для данного контрагента
        update BLACK_CONTR
           set max_id = max_id_
         where site = r.site
           and id = r.id;
        commit;
      END LOOP; --for
      prog_ := getProgress;
    END LOOP; --while
    DBMS_APPLICATION_INFO.SET_CLIENT_INFO(NULL);
  EXCEPTION
    WHEN OTHERS THEN
      DBMS_APPLICATION_INFO.SET_CLIENT_INFO(NULL);
      RAISE;
  end;

  --
  procedure startSearchMax is
    i number;
  begin
    for i in 1 .. max_threads_ loop
      dbms_scheduler.create_job(job_name   => 'B2CheckJob#' || i,
                                job_type   => 'PLSQL_BLOCK',
                                job_action => 'begin black.startSearch; end;',
                                start_date => sysdate,
                                enabled    => TRUE,
                                auto_drop  => TRUE,
                                comments   => 'B2CheckJob#' || i ||
                                              '. Поиск контрагентов в черных списках');
    end loop;
  EXCEPTION
    WHEN OTHERS THEN
      DBMS_OUTPUT.PUT_LINE('Error code : ' || SQLCODE || ' Message : ' ||
                           SUBSTR(SQLERRM, 1, 200));
  end;

  --
  function getProgress return number is
    res number;
  begin
    with m as
     (select max(l.id) max_id from BLACK_LIST l)
    select sum(b.max_id) / (count(*) * max(m.max_id))
      into res
      from BLACK_CONTR b, m;
    return res;
  end;

  --
  function getProcCount return number is
    res number;
  begin
    SELECT count(*)
      into res
      FROM v$session
     WHERE client_info = client_info_
       and status = 'ACTIVE';
    return res;
  end;

  --
  function getSources RETURN source_table
    PIPELINED is
    rec source_record;
  BEGIN
    FOR rec in (select id, sname
                  from (select id, sname
                          from creator.FMBLACKLISTSOURCE@b2prod
                        union
                        select 10, 'EU'
                          from dual)) LOOP
      PIPE ROW(rec);
    END LOOP;
    RETURN;
  END getSources;

  --
  function getSearchResult(proc_ in number,
                           list_ in varchar2,
                           fio_  in varchar2) RETURN result_table
    PIPELINED is
    rec      result_record;
    l_cursor SYS_REFCURSOR;
    str      varchar2(1000);
    proc     number;
    list     varchar2(100);
    fio      varchar2(100);
  BEGIN
    if proc_ is null then
      proc := 97;
    else
      proc := proc_;
    end if;
    if list_ is null then
      list := '1,10';
    else
      list := list_;
    end if;
    if fio_ is null then
      fio := '';
    else
      fio := fio_;
    end if;
  
    str := trim(fio);
    if (length(str) > 0) then
      str := 'and (to_char(c.id) = ''' || fio || ''' or c.name like ''%' || fio ||
             '%'' or b.name_norm like ''%' || fio || '%'')';
    end if;
    str := 'select r.checked, c.siteid, c.id, c.name name_b2, b.name name_list, b.id list_id, b.entity_id, l.sname, r.similar, to_char(r.stime, ''dd.mm.yyyy hh24:mi:ss'') stime, r.checked_by from BLACK_RESULT r inner join creator.contragent@b2prod c on r.site = c.siteid and r.id = c.id inner join BLACK_LIST b on b.id = r.list_id inner join BLACK_SOURCE l on l.id = b.source_id where r.similar >= ' || proc ||
           ' and l.id in (' || list || ') ' || str;
    DBMS_OUTPUT.put_line(str);
    OPEN l_cursor FOR str;
    LOOP
      FETCH l_cursor
        INTO rec;
      EXIT WHEN l_cursor%NOTFOUND;
      PIPE ROW(rec);
      DBMS_OUTPUT.put_line(rec.name_list);
    END LOOP;
    CLOSE l_cursor;
  END getSearchResult;

  --
  procedure setChecked(site_    in number,
                       id_      in number,
                       list_id_ in number,
                       checked_ in number) is
  begin
    update black_result
       set checked = checked_, checked_by = user
     where site = site_
       and id = id_
       and list_id = list_id_;
    commit;
  end;

  -- Починялка нумерации черного списка
  procedure fixList(max_id number) is
    l_cursor SYS_REFCURSOR;
    str      varchar2(1000);
    i        number := max_id;
    id_      number;
  begin
    str := 'select id from BLACK_LIST where id > '||max_id||' order by id';
    OPEN l_cursor FOR str;
    LOOP
      FETCH l_cursor
        INTO id_;
      EXIT WHEN l_cursor%NOTFOUND;
      i := i + 1;
      update BLACK_LIST set id = i where id = id_;
      update BLACK_RESULT set list_id = i where list_id = id_;
    END LOOP;
    CLOSE l_cursor;
    update BLACK_CONTR set max_id = i where max_id > max_id;
    commit;
  end;

end black;
/
