using Agroflet.Platform.Fleet.Domain.Model.Commands;
using Agroflet.Platform.Fleet.Domain.Model.Aggregates;
namespace Agroflet.Platform.Fleet.Application.CommandServices;

public interface IDriverCommandService
{
    Task<Result<Driver>> Handle(CreateDriverCommand command, CancellationToken ct = default);
    Task<Result<Driver>> Handle(UpdateDriverCommand command, CancellationToken ct = default);
}
