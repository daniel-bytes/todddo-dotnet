using System;
namespace Todddo.Core.Domain.Todo
{
    public struct TodoId
    {
        public string Value { get; }

        public TodoId(string value)
        {
            Value = value;
        }

        public override bool Equals(object obj)
        {
            if (obj is TodoId)
                return Equals((TodoId)obj);

            return false;
        }

        public bool Equals(TodoId other)
        {
            return Value == other.Value;
        }

        public override int GetHashCode()
        {
            return (Value ?? "").GetHashCode();
        }

        public static TodoId NewId()
        {
            return new TodoId(Guid.NewGuid().ToString().Replace("-", ""));
        }
    }
}
