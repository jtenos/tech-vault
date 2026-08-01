```sql
declare @people table (id int, name varchar(50));
insert @people values (1, 'John');
insert @people values (2, 'Mary');
insert @people values (3, 'Pat');

declare @cars table (id int, model varchar(50), person_id int);
insert @cars values (1, 'Corvette', 1);
insert @cars values (2, 'Mustang', 1);
insert @cars values (3, 'Viper', 2);

select
    p.id [@id]
    ,p.name [@name]
    ,(
        select model [@model]
        from @cars
        where person_id = p.id
        for xml path('Car'), type
    )
from @people p
for xml path('Person'), type;

/*

<Person id="1" name="John">
  <Car model="Corvette" />
  <Car model="Mustang" />
</Person>
<Person id="2" name="Mary">
  <Car model="Viper" />
</Person>
<Person id="3" name="Pat" />

*/

-- Or to include a root element:

declare @people table (id int, name varchar(50));
insert @people values (1, 'John');
insert @people values (2, 'Mary');
insert @people values (3, 'Pat');

declare @cars table (id int, model varchar(50), person_id int);
insert @cars values (1, 'Corvette', 1);
insert @cars values (2, 'Mustang', 1);
insert @cars values (3, 'Viper', 2);

declare @xml xml;

set @xml = (select
    p.id [@id]
    ,p.name [@name]
    ,(
        select model [@model]
        from @cars
        where person_id = p.id
        for xml path('Car'), type
    )
from @people p
for xml path('Person'), type);

select @xml for xml path('People'), type;

/*

<People>
  <Person id="1" name="John">
    <Car model="Corvette" />
    <Car model="Mustang" />
  </Person>
  <Person id="2" name="Mary">
    <Car model="Viper" />
  </Person>
  <Person id="3" name="Pat" />
</People>

*/
```