namespace Pokedex.Domain.Interfaces.Aggregates;

public interface IAggregateRoot<T>
{
    T Id { get; }
}