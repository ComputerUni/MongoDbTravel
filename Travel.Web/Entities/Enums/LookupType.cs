using System.ComponentModel.DataAnnotations;

namespace Travel.Web.Entities.Enums
{
    public enum LookupType
    {
        [Display(Name = "Tur Tipi")]
        TourType = 1,

        [Display(Name = "Başlangıç Şehri")]
        City = 2,

        [Display(Name = "Ulaşım Aracı")]
        Transport = 3,

        [Display(Name = "Rehber Dili")]
        GuideLanguage = 4,

        [Display(Name = "Vize Bilgisi")]
        VisaInfo = 5
    }
}
