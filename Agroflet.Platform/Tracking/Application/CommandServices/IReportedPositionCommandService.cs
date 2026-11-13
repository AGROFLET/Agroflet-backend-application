using Agroflet.Platform.Tracking.Domain.Model.Commands;
using Agroflet.Platform.Tracking.Domain.Model.Aggregates;
namespace Agroflet.Platform.Tracking.Application.CommandServices;

public interface IReportedPositionCommandService
{
    Task<Result<ReportedPosition>> Handle(CreateReportedPositionCommand command, CancellationToken ct = default);

}
