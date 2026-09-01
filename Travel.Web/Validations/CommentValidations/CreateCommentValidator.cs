using FluentValidation;
using Travel.Web.DTOs.CommentDtos;

namespace Travel.Web.Validations.CommentValidations
{
    public class CreateCommentValidator : AbstractValidator<CreateCommentDto>
    {
        public CreateCommentValidator()
        {
            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Yorum içeriği boş bırakılamaz")
                .MinimumLength(5).WithMessage("Yorum en az 5 karakter olmalıdır.")
                .MaximumLength(500).WithMessage("Yorum en fazla 500 karakter olabilir.");

            RuleFor(x => x.Rating)
                .InclusiveBetween(1, 5).WithMessage("Lütfen 1 ile 5 arasında geçerli bir puan seçin.");
        }
    }
}
