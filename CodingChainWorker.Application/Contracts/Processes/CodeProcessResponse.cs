using System;
using System.Collections.Generic;

namespace Application.Contracts.Processes
{
    public record CodeProcessResponse(Guid ParticipationId, string? Errors, string? Output, IList<Guid> TestsPassedIds);
}