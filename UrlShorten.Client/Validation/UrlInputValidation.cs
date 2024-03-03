using FluentValidation;
using UrlShorten.Data.DTO;

namespace UrlShorten.Client.Validation
{
    public class UrlInputValidation : AbstractValidator<UserUrlInputDto>
    {
        public UrlInputValidation()
        {
            RuleFor(e => e.InputUrl).NotEmpty().WithMessage("Please enter URL before clicking the button");
        }
    }
}
