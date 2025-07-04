using CSharpFunctionalExtensions;

namespace RentalControl.Interfaces;

public interface ICrudService<TGet, in TCreate, in TUpdate>
{
    ValueTask<Result<TGet>> Get(Guid id, CancellationToken cancellationToken);
    ValueTask<Result<TGet[]>> GetAll(CancellationToken cancellationToken);
    ValueTask<Result<TGet>> Create(TCreate data, CancellationToken cancellationToken);
    ValueTask<Result<TGet>> Update(TUpdate data, CancellationToken cancellationToken);
    ValueTask<Result> Delete(Guid id, CancellationToken cancellationToken);
}