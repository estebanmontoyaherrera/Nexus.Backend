using System;
using System.Collections.Generic;
using System.Text;

namespace SharedKernel.Abstractions.Messaging;

// Interfaz base genérica que representa una solicitud que devuelve una respuesta de tipo TResponse. 
// Se usa para unificar comandos y consultas.
public interface IRequest<out TResponse> { }

// Extiende IRequest, representa un "comando" que generalmente modifica el estado del sistema.
public interface ICommand<out TResponse> : IRequest<TResponse> { }

// Extiende IRequest, representa una "consulta" que solo recupera datos y no modifica el estado.
public interface IQuery<out TResponse> : IRequest<TResponse> { }