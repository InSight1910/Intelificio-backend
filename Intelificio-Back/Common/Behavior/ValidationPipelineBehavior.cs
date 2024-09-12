using Backend.Common.Response;
using FluentValidation;
using MediatR;

namespace Backend.Common.Behavior
{
    public class ValidationPipelineBehavior<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>



    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationPipelineBehavior(IEnumerable<IValidator<TRequest>> validators) =>
            _validators = validators;


        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken
            )
        {
            if (!_validators.Any())
            {
                return await next();
            }

            var context = new ValidationContext<TRequest>(request);
            ICollection<Error> errors = _validators
                .Select(validator => validator.Validate(context))
                .SelectMany(validationResult => validationResult.Errors)
                .Where(validationFailure => validationFailure is not null)
                .Select(failure => new Error { Code = failure.PropertyName, Message = failure.ErrorMessage })
                .Distinct()
                .ToList();

            if (errors.Any())
            {
                if (typeof(TResponse) == typeof(Result))
                {
                    return (TResponse)(object)Result.WithErrors(errors)!;
                }
                object validationResult = typeof(Result)
                    .GetGenericTypeDefinition()
                    .MakeGenericType(typeof(TResponse)).GenericTypeArguments[0]
                    .GetMethod(nameof(Result.WithErrors))!
                    .Invoke(null, new object[] { errors })!;
                return (TResponse)validationResult;
            }
            return await next();
        }
    }

}
