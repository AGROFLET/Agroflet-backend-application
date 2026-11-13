namespace Agroflet.Platform.Shared.Domain.Model.Queries;

public record CollectionQuery(IReadOnlyDictionary<string, string[]> Filters);

