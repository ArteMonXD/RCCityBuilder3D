using System;
using UnityEngine;

namespace Domain.Models
{
    public readonly struct GridPosition : IEquatable<GridPosition>
    {
        public readonly int X;
        public readonly int Z;

        public GridPosition(int x, int z) => (X, Z) = (x, z);

        public Vector3 ToWorldPosition() => new Vector3(X, 0, Z);
        public static GridPosition FromWorldPosition(Vector3 worldPos) =>
            new(Mathf.RoundToInt(worldPos.x), Mathf.RoundToInt(worldPos.z));

        public bool Equals(GridPosition other) => X == other.X && Z == other.Z;
        public override bool Equals(object obj) => obj is GridPosition other && Equals(other);
        public override int GetHashCode() => HashCode.Combine(X, Z);

        public static bool operator ==(GridPosition a, GridPosition b) => a.Equals(b);
        public static bool operator !=(GridPosition a, GridPosition b) => !a.Equals(b);

        public override string ToString() => $"({X}, {Z})";
    }
}
