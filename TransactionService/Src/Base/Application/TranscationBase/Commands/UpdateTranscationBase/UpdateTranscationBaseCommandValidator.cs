using FluentValidation;
using Common.Validation;
using Common;

namespace Application.TranscationBases.Commands.UpdateTranscationBase;

public class UpdateTranscationBaseCommandValidator : AbstractValidator<UpdateTranscationBaseCommand>
{
    public UpdateTranscationBaseCommandValidator()
    {
        RuleFor(v => v.DeviceType)
              .MinimumLength(10).WithMessage(string.Format(ErrorMessage.MinimumLength, "DeviceType"))
              .MaximumLength(100).WithMessage(string.Format(ErrorMessage.MaximumLength, "DeviceType"))
              .NotEmpty().WithMessage(string.Format(ErrorMessage.NotEmpty, "DeviceType"));


    }
}