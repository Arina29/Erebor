using Erebor.Infrastructure.Repositories;

namespace Erebor.Data.Repositories;

public sealed class Repository<T>: GenericRepository<T> where T: class
{
    public Repository(EreborDbContext context): base(context)
    {
    }
}