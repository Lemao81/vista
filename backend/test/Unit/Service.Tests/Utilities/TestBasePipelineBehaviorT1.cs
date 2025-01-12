using Application.Abstractions;
using MediatR;
using SharedKernel;

namespace Service.Tests.Utilities;

public class TestBasePipelineBehaviorT0 : BasePipelineBehavior<TestRequest, Result>
{
	public override Task<Result> Handle(TestRequest                    request,
	                                    RequestHandlerDelegate<Result> next,
	                                    CancellationToken              cancellationToken) => next();

	public Result CreateErrorResultPublic(Error error) => CreateErrorResult(error);
}

public class TestBasePipelineBehaviorT1 : BasePipelineBehavior<TestRequest, Result<TestResponse>>
{
	public override Task<Result<TestResponse>> Handle(TestRequest                                  request,
	                                                  RequestHandlerDelegate<Result<TestResponse>> next,
	                                                  CancellationToken                            cancellationToken) => next();

	public Result<TestResponse> CreateErrorResultPublic(Error error) => CreateErrorResult(error);
}
