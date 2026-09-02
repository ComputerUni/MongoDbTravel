using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Travel.Web.Areas.Admin.Extensions;
using Travel.Web.Entities.Enums;

namespace Travel.Web.DTOs.LookupDtos
{
    public class ResultLookupDto
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string TypeDisplayName => int.TryParse(Type, out int t)
            ? ((LookupType)t).DisplayName()
            : Type;
        public string Name { get; set; }
        public string? NameEn { get; set; }
        public bool IsActive { get; set; }
    }
}
