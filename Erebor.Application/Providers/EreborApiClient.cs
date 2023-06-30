
using System.Text;
using System.Text.Json;
using Erebor.Infrastructure.Monads;
using Microsoft.Extensions.Logging;

namespace Erebor.Application.Providers;

internal class EreborApiClient<TResponse, TRequest> : IEreborApiClient<TResponse, TRequest>
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<EreborApiClient<TResponse, TRequest>> _logger;

    public EreborApiClient(IHttpClientFactory httpClientFactory, ILogger<EreborApiClient<TResponse, TRequest>> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public async Task<Either<TResponse, string>> PerformPostRequest(
        TRequest body, 
        string uri, 
        Dictionary<string, string>? headers = null)
    {
        var httpClient = _httpClientFactory.CreateClient();
        var requestBody = JsonSerializer.Serialize(body);
        using var request = new HttpRequestMessage
        {
            RequestUri = new Uri(uri),
            Content = new StringContent(requestBody, Encoding.UTF8, "application/json"),
            Method = HttpMethod.Post,
        };
        if (headers is not null)
        {
            foreach (var header in headers)
            {
                request.Headers.Add(header.Key, header.Value);
            }
        }

        HttpResponseMessage response = await httpClient.SendAsync(request).ConfigureAwait(false);
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("Unexpected Status Code : {statusCode} after calling {uri}", response.StatusCode, request.RequestUri);
            return Either.Left<TResponse, string>(string.Empty);
        }

        var responseContent = await response.Content.ReadAsStringAsync();

        var jsonResponse = JsonDocument.Parse(responseContent);
        var result = jsonResponse.Deserialize<TResponse>();
        //var a = Either.Try(() => jsonResponse.Deserialize<TResponse>());

        return result is not null ? Either.Right<TResponse, string>(result) : Either.Left<TResponse, string>(string.Empty);
    }

}