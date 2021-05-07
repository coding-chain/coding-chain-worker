using System;
using Domain.Contracts;

namespace Domain.TestExecution
{
    public record ParticipationId(Guid Value) : IEntityId
    {
        public override string ToString() => Value.ToString();
    }
}