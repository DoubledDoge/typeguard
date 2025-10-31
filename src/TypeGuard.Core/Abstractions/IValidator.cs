namespace TypeGuard.Core.Abstractions;

using Rules;

public interface IValidator<T>
{
    IValidator<T> AddRule(IValidationRule<T> rule);
    Task<T> GetValidInputAsync(CancellationToken cancellationToken = default);
    T GetValidInput();
}
