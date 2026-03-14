using SharedKernel.Commons.Bases;
using SharedKernel.Commons.Behaviours;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Logging;
using ValidationException = SharedKernel.Commons.Exceptions.ValidationException;
namespace SharedKernel.Abstractions.Messaging;

// Clase auxiliar que ejecuta lógica con validación y manejo de errores
public class HandlerExecutor(IValidationService validationService, ILogger<HandlerExecutor> logger)
{
    // Servicio para validar la solicitud
    private readonly IValidationService _validationService = validationService;

    // Logger para registrar errores o advertencias
    private readonly ILogger<HandlerExecutor> _logger = logger;

    public async Task<BaseResponse<T>> ExecuteAsync<TRequest, T>(
    TRequest request,
    Func<Task<BaseResponse<T>>> action,
    CancellationToken cancellationToken)
    {
        try
        {
            // Valida el request antes de ejecutarlo
            await _validationService.ValidateAsync(request, cancellationToken);
            // Ejecuta la acción si la validación fue exitosa
            return await action();
        }
        catch (ValidationException ex)
        {
            // Logea advertencia con los errores de validación
            _logger.LogWarning("Validation failed for request {@Request}. Errors: {@Errors}", request, ex.Errors);

            // Retorna respuesta fallida con errores de validación
            return new BaseResponse<T>
            {
                IsSuccess = false,
                Message = "Errores de validación",
                Errors = ex.Errors
            };
        }
        catch (Exception ex)
        {
            // Logea el error inesperado
            _logger.LogError(ex, "Unhandled exception occurred while executing handler for request {@Request}", request);

            // Retorna respuesta con error genérico
            return new BaseResponse<T>
            {
                IsSuccess = false,
                Message = "Ocurrió un error inesperado",
                Errors =
                [
                    new() { PropertyName = "Exception", ErrorMessage = ex.Message }
                ]
            };
        }
    }
}
