using SharedKernel.Commons.Bases;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharedKernel.Abstractions.Messaging;

public interface IDispatcher
{
    // Método que despacha una solicitud que puede ser un comando o una consulta
    Task<BaseResponse<TResponse>> Dispatch<TRequest, TResponse>(
            TRequest request,
            CancellationToken cancellationToken
        )
          where TRequest : IRequest<TResponse>;
}
