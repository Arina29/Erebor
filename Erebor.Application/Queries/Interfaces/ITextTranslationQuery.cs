using Erebor.Application.Models.Requests;
using Erebor.Infrastructure.Queries;

namespace Erebor.Application.Queries.Interfaces;

public interface ITextTranslationQuery: IQuery<string, string>
{
}