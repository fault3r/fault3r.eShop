
using System;

namespace AccountService.Domain.ValueObjects
{
    public sealed class Role : IEquatable<Role>
    {
        public string Name { get; }

        private Role(string name)
        {
            Name = name;
        }

        public override string ToString() => Name;
        
        public bool Equals(Role? other) => 
            other is not null && Name == other.Name;

        public static Role Admin() => new("Admin");
        public static Role User() => new("User");

        public override bool Equals(object? obj) =>
            Equals(obj as Role);
        public override int GetHashCode() =>
            Name.GetHashCode(StringComparison.OrdinalIgnoreCase);

        public static bool operator ==(Role left, Role right) =>
            Equals(left, right);
        public static bool operator !=(Role left, Role right) =>
            !Equals(left, right);
    }
}
