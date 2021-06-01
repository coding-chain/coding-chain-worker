using System;

namespace Application.Read.Execution
{
    public record CodeProcessResponse(Guid ParticipationId, string? Errors, string? Output);
}