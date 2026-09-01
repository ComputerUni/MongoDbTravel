using FluentValidation;
using Travel.Web.Areas.Admin.Models;

namespace Travel.Web.Validations.TourValidations
{
    public class CreateTourViewModelValidator : AbstractValidator<CreateTourViewModel>
    {
        public CreateTourViewModelValidator()
        {
            RuleFor(x => x.Tour).SetValidator(new CreateTourValidator());
        }
    }
}
