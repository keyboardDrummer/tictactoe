namespace tictactoe_cs
{
	public struct Position
	{
		public int R;
		public int C;

		public Position(int r, int c)
		{
			R = r;
			C = c;
			Index = R * 3 + C;
		}

		public int Index { get; }

		public override string ToString()
		{
			return $"{nameof(R)}: {R}, {nameof(C)}: {C}";
		}

		public bool Equals(Position other)
		{
			return Index == other.Index;
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			return obj is Position && Equals((Position) obj);
		}

		public override int GetHashCode()
		{
			return Index;
		}
	}
}