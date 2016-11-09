using System.Collections.Generic;
using System.Linq;

namespace tictactoe_cs
{
	public static class BoardExtensions
	{
		public static CellValue GetPosition(this IBoard board, int row, int column)
		{
			return board.GetPosition(new Position(row, column));
		}

		public static CellValue HasWon(this IBoard board)
		{
			for (int r = 0; r < 3; r++)
			{
				if (board.GetPosition(r, 0) != CellValue.Empty && board.GetPosition(r, 0) == board.GetPosition(r, 1) &&
				    board.GetPosition(r, 0) == board.GetPosition(r, 2))
				{
					return board.GetPosition(r, 0);
				}
			}
			for (int c = 0; c < 3; c++)
			{
				if (board.GetPosition(0, c) != CellValue.Empty && board.GetPosition(0, c) == board.GetPosition(1, c) &&
				    board.GetPosition(0, c) == board.GetPosition(2, c))
				{
					return board.GetPosition(0, c);
				}
			}
			if (board.GetPosition(0, 0) != CellValue.Empty && board.GetPosition(0, 0) == board.GetPosition(1, 1) &&
			    board.GetPosition(0, 0) == board.GetPosition(2, 2))
			{
				return board.GetPosition(0, 0);
			}
			if (board.GetPosition(0, 2) != CellValue.Empty && board.GetPosition(0, 2) == board.GetPosition(1, 1) &&
			    board.GetPosition(0, 2) == board.GetPosition(2, 0))
			{
				return board.GetPosition(0, 2);
			}
			return CellValue.Empty;
		}

		public static bool CanPlay(this IBoard board)
		{
			return board.GetStates().Any(p => p == CellValue.Empty);
		}

		public static IEnumerable<CellValue> GetStates(this IBoard me)
		{
			return GetPositions().Select(me.GetPosition);
		}

		public static IEnumerable<Position> GetPositions()
		{
			return Enumerable.Range(0, 9).Select(index => new Position(index % 3, index / 3));
		}
	}
}