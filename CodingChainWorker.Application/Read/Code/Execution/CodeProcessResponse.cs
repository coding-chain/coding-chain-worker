using System;

namespace Application.Read.Code.Execution
{
    public class CodeProcessResponse
    {
        private Guid ParticipationId { get; }
        public string? Output;
        public string? Errors;

        public CodeProcessResponse(Guid participationId, string? errors, string? output)
        {
            ParticipationId = participationId;
            Errors = errors;
            Output = output;
        }
    }
}