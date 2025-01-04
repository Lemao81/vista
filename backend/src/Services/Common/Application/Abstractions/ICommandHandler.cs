﻿using Domain;
using MediatR;

namespace Application.Abstractions;

public interface ICommandHandler<in TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
	where TRequest : IRequest<TResponse>, IBaseCommand where TResponse : Result;
