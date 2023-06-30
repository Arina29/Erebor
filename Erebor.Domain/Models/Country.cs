using Erebor.Domain.Enums;
using Erebor.Infrastructure.Attributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace Erebor.Domain.Models;

[EntityObject]
public record Country(string Name, Dangerousness Dangerousness)
{
    public Guid Id { get; private set; }

    public string? Description { get; set; }

    public DateTimeOffset? UpdatedAt { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public IEnumerable<King>? Kings { get; set; }
}