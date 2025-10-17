﻿using System.ComponentModel.DataAnnotations;
using DirectoryService.Domain.Shared;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace DirectoryService.Application.Validation;

public static class ValidationExtensions
{
    public static Errors ToList(this ValidationResult validationResult)
    {
        var validationErrors = validationResult.Errors;

        var errors = from validationError in validationErrors
            let errorMessage = validationError.ErrorMessage
            let error = Error.Deserialize(errorMessage)
            select Error.Validation(error.Code, error.Message, validationError.PropertyName);

        return errors.ToList();
    }
}