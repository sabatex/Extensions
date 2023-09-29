using Microsoft.OData.ModelBuilder;

namespace Sabatex.AspNetCore.OData.Extensions
{
    public static class ODataEnumExtension
    {
        public static void AddEnumTypeAsInt(this ODataConventionModelBuilder builder, Type enumType)
        {
            var e = builder.AddEnumType(enumType);
            foreach (var item in Enum.GetValues(enumType))
            {
                var m = e.AddMember((Enum)item);
                m.Name = ((int)item).ToString();
            }
        }
    }
}