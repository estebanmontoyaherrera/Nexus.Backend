using SharedKernel.Commons.Bases;

namespace SharedKernel.Abstractions.Messaging;

public interface IQueryHandler<in TQuery, TResponse>
    where TQuery : IQuery<TResponse>  // Restringe que TQuery debe ser una implementación de IQuery
{
    // Método que maneja la lógica de la consulta y devuelve una respuesta tipada.
    Task<BaseResponse<TResponse>> Handle(TQuery query, CancellationToken cancellationToken);
}
