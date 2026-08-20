using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Travel.Web.Areas.Admin.Extensions
{
    public static class EnumExtensions
    {
        public static string DisplayName(this Enum e) =>
            e.GetType().GetField(e.ToString())!
             .GetCustomAttribute<DisplayAttribute>()?.Name ?? e.ToString();

    }
}
