using System;

namespace Todddo.Core.Domain
{
    public interface IEntity<TID>
    {
        TID Id { get; }
    }
}
