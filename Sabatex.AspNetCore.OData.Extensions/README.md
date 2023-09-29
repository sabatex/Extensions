# AspNetCore OData Extensions

## change enum routing from string to int 

---
using Sabatex.AspNetCore.OData.Extensions

IEdmModel GetEdmModel()
{
    var builder = new ODataConventionModelBuilder();
    ...
    builder.AddEnumTypeAsInt(typeof(Enum Type));
    return builder.GetEdmModel();
}
---
