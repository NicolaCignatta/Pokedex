namespace Pokedex.Domain.Interfaces.Aggregates;

/// <summary>
/// Aggregate root interface representing the root entity of an aggregate.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IAggregateRoot<T>
{
    T Id { get; }
}