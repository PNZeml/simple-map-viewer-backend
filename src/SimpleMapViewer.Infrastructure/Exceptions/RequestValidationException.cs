using System;
using System.Collections.Generic;
using FluentValidation.Results;

namespace SimpleMapViewer.Infrastructure.Exceptions {
    public class RequestValidationException : Exception {
        public IList<ValidationFailure> Failures { get; }

        public RequestValidationException(IList<ValidationFailure> failures) {
            Failures = failures;
        }
    }
}