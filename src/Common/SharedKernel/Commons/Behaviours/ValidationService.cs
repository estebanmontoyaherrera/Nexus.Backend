using FluentValidation;
using SharedKernel.Commons.Bases;
using Microsoft.Extensions.DependencyInjection;
using ValidationException = SharedKernel.Commons.Exceptions.ValidationException;

namespace SharedKernel.Commons.Behaviours;

public class ValidationService : IValidationService
{
    // Inyecta el proveedor de servicios para resolver validadores
    private readonly IServiceProvider _serviceProvider;

    public ValidationService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    // Ejecuta validación asincrónica sobre un objeto request
    public async Task ValidateAsync<T>(T request, CancellationToken cancellationToken = default)
    {
        // Obtiene todos los validadores registrados para el tipo T
        var validators = _serviceProvider.GetServices<IValidator<T>>();

        // Si no hay validadores, no hace nada
        if (!validators.Any()) return;

        // Crea el contexto de validación
        var context = new ValidationContext<T>(request);

        // Ejecuta todas las validaciones en paralelo
        var validationResults = await Task.WhenAll(validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        // Filtra los errores encontrados
        var failures = validationResults
            .Where(r => r.Errors.Any())
            .SelectMany(r => r.Errors)
            .Select(err => new BaseError
            {
                PropertyName = err.PropertyName,
                ErrorMessage = err.ErrorMessage
            })
            .ToList();

        // Si hay errores, lanza excepción personalizada
        if (failures.Any())
            throw new ValidationException(failures);
    }
}
