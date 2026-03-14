using SharedKernel.Commons.Bases;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharedKernel.Abstractions.Messaging;

public interface ICommandHandler<in TCommand, TResponse>
    where TCommand : ICommand<TResponse> // Restringe TCommand a que sea del tipo ICommand
{
    // Método para manejar el comando, ejecuta la acción solicitada y retorna un resultado estructurado.
    Task<BaseResponse<TResponse>> Handle(TCommand command, CancellationToken cancellationToken);
}
