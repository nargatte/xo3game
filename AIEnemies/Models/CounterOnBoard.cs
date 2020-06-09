namespace AIEnemies.Models
{
    public class CounterOnBoard
    {
        public bool Color { get; }
        public FieldCoordinates Coordinates { get; }

        public CounterOnBoard(bool color, FieldCoordinates coordinates)
        {
            Color = color;
            Coordinates = coordinates;
        }

        protected bool Equals(CounterOnBoard other)
        {
            return Color == other.Color && Equals(Coordinates, other.Coordinates);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((CounterOnBoard) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Color.GetHashCode() * 397) ^ (Coordinates != null ? Coordinates.GetHashCode() : 0);
            }
        }
    }
}