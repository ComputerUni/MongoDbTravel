using FluentValidation;
using Travel.Web.DTOs.QuestionDtos;

namespace Travel.Web.Validations.QuestionValidations
{
    public class CreateQuestionValidator : AbstractValidator<CreateQuestionDto>
    {
        public CreateQuestionValidator()
        {
            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Soru içeriği boş bırakılamaz");
        }
    }
}
