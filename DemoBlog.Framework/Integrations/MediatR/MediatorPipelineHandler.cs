using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace DemoBlog.Framework.Integrations.MediatR
{
    public class MediatorPipelineHandler<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            //implement validaton...

            //log information before ...
            var response = await next();
            //log information after ...
            return response;
        }
    }
}