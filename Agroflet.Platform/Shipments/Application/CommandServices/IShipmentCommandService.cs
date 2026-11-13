using Agroflet.Platform.Shipments.Domain.Model.Commands;
using Agroflet.Platform.Shipments.Domain.Model.Aggregates;
namespace Agroflet.Platform.Shipments.Application.CommandServices;

public interface IShipmentCommandService
{
    Task<Result<Shipment>> Handle(CreateShipmentCommand command, CancellationToken ct = default);
    Task<Result<Shipment>> Handle(UpdateShipmentCommand command, CancellationToken ct = default);
}
