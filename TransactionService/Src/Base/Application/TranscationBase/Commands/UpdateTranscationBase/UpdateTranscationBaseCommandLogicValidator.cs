
using Application.Common.Interfaces;
using Common;
using Common.Exceptions;
using Domain.Entities;

namespace Application.TranscationBases.Commands.UpdateTranscationBase;

public class UpdateTranscationBaseCommandLogicValidator
{

    public void IsValidCustomers(UpdateTranscationBaseCommand request)
    {
        UpdateTranscationBaseCommandValidator validations = new UpdateTranscationBaseCommandValidator();
        var _result = validations.Validate(request);
        if (_result is not null && !_result.IsValid && _result.Errors.Count() > 0) throw new ValidationException(_result.Errors);
    }


}