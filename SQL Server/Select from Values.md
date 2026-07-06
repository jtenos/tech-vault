```sql
SELECT * FROM (
    VALUES (1, 2), (3, 4)
) X(a, b);

-- a    b
-- 1    2
-- 3    4
```