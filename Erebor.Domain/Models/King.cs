using Erebor.Infrastructure.Attributes;

namespace Erebor.Domain.Models;

[EntityObject]
public record King(string Name, DateTime CreatedAt)
{
    public Guid Id { get; private set; }

    public string? Description { get; set; }

    public Country? Country { get; set; }

    public string? FullTitle { get; set; }

    public DateTime? BirthDate { get; set; }

    public DateTime? DeathDate { get; set; }

    public DateTime? ReignFrom { get; set; }

    public DateTime? ReignTo { get; set; }

    public DateTime? UpdatedAt { get; set; }

}
