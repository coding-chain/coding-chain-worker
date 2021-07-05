using System;
using System.Collections.Generic;

namespace CodingChainApi.Infrastructure.Logs
{
    public record CodeProcessResponseLog(Guid ParticipationId, string? Errors, string? Output, IList<Guid> TestsPassedIds);
}