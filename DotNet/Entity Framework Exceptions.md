```csharp
// This worked in old versions, haven’t tested yet in Entity Framework Core – not sure if the structure changed.

public override int SaveChanges() {
    var allEntries = ChangeTracker.Entries().ToList();
    var allAddedEntries = allEntries.Where(e => e.State == EntityState.Added).ToList();
    var allModifiedEntries = allEntries.Where(e => e.State == EntityState.Modified).ToList();
    var allDeletedEntries = allEntries.Where(e => e.State == EntityState.Deleted).ToList();
    try {
        int result = base.SaveChanges();
        return result;
    } catch (Exception ex) {
 
        try {
            var sb = new StringBuilder(Environment.NewLine);
            sb.AppendLine(string.Format("ADDED:{0}", FormatEntities(allAddedEntries)));
            sb.AppendLine(string.Format("MODIFIED:{0}", FormatEntities(allModifiedEntries)));
            sb.AppendLine(string.Format("DELETED:{0}", FormatEntities(allDeletedEntries)));
 
            // TODO: Do something with the details
        } catch {
            // If the exception logging fails, just give up
        }
        throw;
    }
}
 
/// <summary>
/// Takes a collection of entities, retrieves the simple properties from them (numbers, strings, etc.),
/// and JSON-formats the results.
/// </summary>
/// <param name="entries">A collection of Entity-Framework entries.</param>
/// <returns>The JSON-formatted entities.</returns>
private static string FormatEntities(IEnumerable<DbEntityEntry> entries) {
    try {
        var sb = new StringBuilder(Environment.NewLine);
        var simplePropertyTypes = new[]
            {
                typeof(byte), typeof(char), typeof(short), typeof(int), typeof(long),
                typeof(bool), typeof(decimal), typeof(float), typeof(double),
                typeof(string), typeof(DateTime), typeof(ushort), typeof(uint), typeof(ulong),
                typeof(byte?), typeof(char?), typeof(short?), typeof(int?), typeof(long?),
                typeof(bool?), typeof(decimal?), typeof(float?), typeof(double?),
                typeof(DateTime?), typeof(ushort?), typeof(uint?), typeof(ulong?)
            };
        var collection = entries.Select(e => e.Entity).ToList();
        foreach (var entity in collection) {
            var entityType = entity.GetType();
 
            // Removing the dynamically-generated characters from the end of the entity class name.
            string entityTypeName = Regex.Replace(entityType.Name, @"_[0-9A-F]{64}", string.Empty);
 
            sb.AppendFormat("{0}: ", entityTypeName);
            var dict = new Dictionary<string, object>();
            var simpleProperties = entityType.GetProperties().Where(p => simplePropertyTypes.Contains(p.PropertyType));
            foreach (var prop in simpleProperties) {
                try {
                    if (prop.GetCustomAttributes(typeof(NotMappedAttribute), true).Length == 0) {
                        dict[prop.Name] = prop.GetValue(entity, null);
                    }
                } catch (Exception) {
                    dict[prop.Name] = "[UNKNOWN]";
                }
            }
            sb.AppendLine(JsonConvert.SerializeObject(dict, Formatting.Indented));
        }
        return sb.ToString();
    } catch (Exception ex) {
        return string.Format("Failed to format entities: {0}", ex.Message);
    }
}
```