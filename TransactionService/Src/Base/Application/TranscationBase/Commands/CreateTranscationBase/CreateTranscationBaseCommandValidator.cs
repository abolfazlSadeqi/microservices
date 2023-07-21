using Common;
using Common.Validation;
using FluentValidation;
using MediatR;


namespace Application.Customers.Commands.CreateCustomer;

public class CreateTranscationBaseCommandValidator : AbstractValidator<CreateTranscationBaseCommand>
{
    public CreateTranscationBaseCommandValidator()
    {
       

        RuleFor(v => v.DeviceType)
            .MinimumLength(10).WithMessage(string.Format(ErrorMessage.MinimumLength, "DeviceType"))
            .MaximumLength(100).WithMessage(string.Format(ErrorMessage.MaximumLength, "DeviceType"))
            .NotEmpty().WithMessage(string.Format(ErrorMessage.NotEmpty, "DeviceType"));


    }
}
