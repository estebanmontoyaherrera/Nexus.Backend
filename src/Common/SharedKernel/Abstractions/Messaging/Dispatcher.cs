using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Commons.Bases;

namespace SharedKernel.Abstractions.Messaging;

// Dispatcher implementa IDispatcher y resuelve los handlers en tiempo de ejecución
public class Dispatcher(IServiceProvider serviceProvider) : IDispatcher
{
    // Servicio de inyección de dependencias
    private readonly IServiceProvider _serviceProvider = serviceProvider;
    public async Task<BaseResponse<TResponse>> Dispatch<TRequest, TResponse>(
        TRequest request,
        CancellationToken cancellationToken
    ) where TRequest : IRequest<TResponse> // El request debe implementar IRequest
    {
        try
        {
            if (request is ICommand<TResponse>)
            {
                // Se construye dinámicamente el tipo ICommandHandler<TRequest, TResponse>
                var handlerType = typeof(ICommandHandler<,>)
                    .MakeGenericType(request.GetType(), typeof(TResponse));

                // Se obtiene el handler registrado desde el contenedor de servicios
                dynamic handler = _serviceProvider.GetRequiredService(handlerType);

                // Ejecuta el handler del comando
                return await handler.Handle((dynamic)request, cancellationToken);
            }

            if (request is IQuery<TResponse>)
            {
                // Construye dinámicamente el tipo IQueryHandler<TRequest, TResponse>
                var handlerType = typeof(IQueryHandler<,>)
                    .MakeGenericType(request.GetType(), typeof(TResponse));

                // Obtiene el handler de consulta
                dynamic handler = _serviceProvider.GetRequiredService(handlerType);

                // Ejecuta el handler de la consulta
                return await handler.Handle((dynamic)request, cancellationToken);
            }

            // Si no es ni comando ni consulta, lanza error
            throw new InvalidOperationException("Tipo de solicitud no compatible.");
        }
        catch (Exception ex)
        {
            // Retorna error genérico si algo falla al despachar
            return new BaseResponse<TResponse>
            {
                IsSuccess = false,
                Message = "Ocurrió un error al despachar la solicitud",
                Errors =
                [
                    new() { PropertyName = "Dispatcher", ErrorMessage = ex.Message }
                ]
            };
        }
    }
}