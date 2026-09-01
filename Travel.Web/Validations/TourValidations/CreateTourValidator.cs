using FluentValidation;
using Travel.Web.DTOs.TourDtos;

namespace Travel.Web.Validations.TourValidations
{
    public class CreateTourValidator : AbstractValidator<CreateTourDto>
    {
        public CreateTourValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Tur adı boş bırakılamaz.")
                .MaximumLength(150).WithMessage("Tur adı en fazla 150 karakter olmalıdır.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Tur açıklaması boş bırakılamaz.");

            RuleFor(x => x.ShortDescription)
                .NotEmpty().WithMessage("Kısa açıklama boş bırakılamaz.")
                .MaximumLength(300).WithMessage("Kısa açıklama en fazla 300 karakter olmalıdır.");

            RuleFor(x => x.CategoryId)
                .NotEmpty().WithMessage("Lütfen bir kategori seçiniz.");

            RuleFor(x => x.DestinationId)
                .NotEmpty().WithMessage("Lütfen bir destinasyon seçiniz.");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Tur fiyatı 0'dan büyük olmalıdır.");

            RuleFor(x => x.ChildPrice)
                .GreaterThanOrEqualTo(0).WithMessage("Çocuk fiyatı 0'dan küçük olamaz.");

            RuleFor(x => x.Night)
                .GreaterThanOrEqualTo(0).WithMessage("Gece sayısı 0'dan küçük olamaz.");

            RuleFor(x => x.Duration)
                .GreaterThan(0).WithMessage("Tur süresi (gün) 0'dan büyük olmalıdır.");

            RuleFor(x => x.GroupSize)
                .GreaterThan(0).WithMessage("Grup boyutu 0'dan büyük olmalıdır.");

            RuleFor(x => x.MinParticipant)
                .GreaterThan(0).WithMessage("Minimum katılımcı sayısı 0'dan büyük olmalıdır.");

            RuleFor(x => x.DepartureCity)
                .NotEmpty().WithMessage("Kalkış şehri boş bırakılamaz.");

            RuleFor(x => x.Transport)
                .NotEmpty().WithMessage("Ulaşım bilgisi boş bırakılamaz.");

            RuleFor(x => x.Accommodation)
                .NotEmpty().WithMessage("Konaklama bilgisi boş bırakılamaz.");

            RuleFor(x => x.GuideLanguage)
                .NotEmpty().WithMessage("Rehber dil bilgisi boş bırakılamaz.");

            RuleFor(x => x.MeetingPoint)
                .NotEmpty().WithMessage("Buluşma noktası boş bırakılamaz.");

            RuleFor(x => x.CoverImage)
                .NotNull().WithMessage("Lütfen bir kapak görseli yükleyiniz.");

            RuleFor(x => x.Included)
                .Must(x => x != null && x.Count > 0)
                .WithMessage("En az bir adet dahil olan hizmet eklemelisiniz.");

            RuleFor(x => x.Dates)
                .Must(x => x != null && x.Count > 0)
                .WithMessage("En az bir tur tarihi eklemelisiniz.");

            RuleFor(x => x.DayPrograms)
                .Must(x => x != null && x.Count > 0)
                .WithMessage("En az bir günlük program eklemelisiniz.");
        }
    }
}
