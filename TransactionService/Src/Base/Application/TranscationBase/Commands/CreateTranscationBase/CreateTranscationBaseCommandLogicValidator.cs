
using Application.Common.Interfaces;
using Common;
using Common.Exceptions;
using Domain.Entities;

namespace Application.Customers.Commands.CreateCustomer;

public class UpdateCustomerCommandLogicValidator
{

    public void IsValidCustomers(CreateTranscationBaseCommand request)
    {
        CreateTranscationBaseCommandValidator validations = new CreateTranscationBaseCommandValidator();
        var _result = validations.Validate(request);
        if (_result is not null && !_result.IsValid && _result.Errors.Count() > 0) throw new ValidationException(_result.Errors);
    }


}