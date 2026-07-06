```csharp
var itemQuery = _context.Stores
    .GroupBy(s => new { s.Country.CountryName, s.RegionName })
    .Where(g => g.Count() > 1)
    .OrderByDescending(g => g.Count())
    .Select(g => new { g.Key.CountryName, g.Key.RegionName, NumStores = g.Count() });

// OR
var itemQuery =
    from s in _context.Stores
    group s by new { s.Country.CountryName, s.RegionName } into g
    where g.Count() > 1
    orderby g.Count() descending
    select new { g.Key.CountryName, g.Key.RegionName, NumStores = g.Count() };
```