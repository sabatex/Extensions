# AspNetCore OData Extensions

## Change enum routing from symbolic to numeric

before

```
$filter=State eq 'New'
```
after 
```
$filter=State eq 1
```
### Code sample
```C#
using Sabatex.AspNetCore.OData.Extensions;

IEdmModel GetEdmModel()
{
    var builder = new ODataConventionModelBuilder();
    ...
    builder.AddEnumTypeAsInt(typeof(Enum Type));
    return builder.GetEdmModel();
}
```
