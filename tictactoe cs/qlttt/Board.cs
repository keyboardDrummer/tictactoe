using System;
using System.Linq;
using tictactoe_cs;

namespace qlttt
{
	public class Board : IEquatable<Board>
	{
		public Board()
		{
		}

		public Board(Grid grid)
		{
			for (int r = 0; r < 3; r++)
			{
				for (int c = 0; c < 3; c++)
				{
					var cell = grid.Matrix[r][c].display;
					pos[r*3 + c] = cell == ' ' ? 0 : cell == 'X' ? 1 : 2;
				}
			}
		}

		protected int[] pos { get; set; } = new int[9];

		public string Show
		{
			get
			{
				var result = "";
				for (var i = 0; i < 9; i++)
				{
					result = result + pos[i];
					if ((i + 1)%3 == 0)
						result = result + Environment.NewLine;
				}
				return result;
			}
		}

		public bool Equals(Board other)
		{
			for (var i = 0; i < 9; i++)
				if (pos[i] != other.pos[i])
					return false;

			return true;
		}

		public Board MakeMove(int i, int player)
		{
			var result = new Board();
			result.pos = pos.Select(v => v).ToArray();

			if (result.pos[i] != 0)
				throw new Exception("Invalid Move!");

			result.pos[i] = player;
			return result;
		}

		public bool HasDiag(int p)
		{
			return ((pos[0] == p) && (pos[4] == p) && (pos[8] == p)) ||
			       ((pos[2] == p) && (pos[4] == p) && (pos[6] == p));
		}

		public bool HasLine(int p)
		{
			for (var i = 0; i < 3; i++)
			{
				if ((pos[i*3] == p) && (pos[i*3 + 1] == p) && (pos[i*3 + 2] == p))
					return true;

				if ((pos[i] == p) && (pos[i + 3] == p) && (pos[i + 6] == p))
					return true;
			}
			return false;
		}

		public double GetReward()
		{
			if (HasLine(1) || HasDiag(1))
				return 1.0;
			if (HasLine(2) || HasDiag(2))
				return -1.0;

			if (GetValidMoves().Length == 0)
				return 0.5;

			return 0.0;
		}

		public int[] GetValidMoves()
		{
			return pos.Select((v, i) => new {Value = v, Index = i}).Where(t => t.Value == 0).Select(t => t.Index).ToArray();
		}

		public override int GetHashCode()
		{
			var hash = Show;
			return hash.GetHashCode();
		}
	}
}