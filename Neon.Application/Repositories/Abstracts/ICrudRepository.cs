using Neon.Domain.Abstracts;

namespace Neon.Application.Repositories.Abstracts;

public interface ICrudRepository<TEntity, TKey> :
    ICreateOnlyRepository<TEntity, TKey>,
    IReadOnlyRepository<TEntity, TKey>,
    IUpdateOnlyRepository<TEntity, TKey>,
    IDeleteOnlyRepository<TEntity, TKey>
    where TEntity : IEntity<TKey>
    where TKey : IEquatable<TKey>;