using FluentValidation;
using Microsoft.Extensions.Localization;
using Travel.Web.DTOs.CommentDtos;

namespace Travel.Web.Validations.CommentValidations
{
    public class CreateCommentValidator : AbstractValidator<CreateCommentDto>
    {
        public CreateCommentValidator(IStringLocalizer<SharedResource> localizer)
        {
            RuleFor(x => x.Content)
                .NotEmpty().WithMessage(localizer["YorumIcerigiBosBirakilamaz"])
                .MinimumLength(5).WithMessage(localizer["YorumEnAz5Karakter"])
                .MaximumLength(500).WithMessage(localizer["YorumEnFazla500Karakter"]);

            RuleFor(x => x.Rating)
                .InclusiveBetween(1, 5).WithMessage(localizer["GecerliPuanSecin"]);
        }
    }
}
