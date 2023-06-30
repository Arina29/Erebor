
using Erebor.Infrastructure.Monads;

namespace Erebor.Application.Providers;

public interface IEreborApiClient<TResponse, in TRequest>
{
    Task<Either<TResponse, string>> PerformPostRequest(TRequest body, string uri, Dictionary<string, string>? headers = null);
}