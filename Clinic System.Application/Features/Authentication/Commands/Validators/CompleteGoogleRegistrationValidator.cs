
using Clinic_System.Application.Features.Authentication.Commands.Models;
using Clinic_System.Application.Service.Interface;
using Clinic_System.Core.Interfaces.UnitOfWork;

namespace Clinic_System.Application.Features.Authentication.Commands.Validators
{
    public class CompleteGoogleRegistrationValidator : AbstractValidator<CompleteGoogleRegistrationCommand>
    {
        private readonly IIdentityService _identityService;
        private readonly IUnitOfWork _unitOfWork;

        public CompleteGoogleRegistrationValidator(IIdentityService identityService, IUnitOfWork unitOfWork)
        {
            _identityService = identityService;
            _unitOfWork = unitOfWork;

            ApplyValidationsRules();
            ApplyCustomValidationsRules();
        }

        public void ApplyValidationsRules()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Patient Name is required")
                .MaximumLength(100).WithMessage("Name must not exceed 100 characters");

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Address is required");

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Phone number is required")
                .Matches(@"^\+?[0-9]{10,15}$")
                .WithMessage("Phone number must contain 10–15 digits (numbers only, optional +)");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format");

            RuleFor(x => x.DateOfBirth)
                .NotEmpty().WithMessage("Date of Birth is required")
                .LessThan(DateTime.Now).WithMessage("Date of Birth must be in the past");
        }

        public void ApplyCustomValidationsRules()
        {
            RuleFor(x => x.Email)
                .MustAsync(async (email, cancellationToken) =>
                {
                    bool isUnique = await _identityService.IsEmailUniqueAsync(email);
                    return isUnique;
                })
                .WithMessage("Email already exists in the system.");


        }
    }
}
