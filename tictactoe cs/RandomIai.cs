using System;
using System.Linq;

namespace tictactoe_cs
{
	class RandomIai : IAI
	{
		public Position Step(IBoard board)
		{
			var emptyCells =
				Enumerable.Range(0, 9)
					.Select(index => new Position {R = index%3, C = index/3})
					.Where(b => board.GetPositions(b.R, b.C) == null)
					.ToList();
			var chosenCell = new Random().Next(emptyCells.Count);
			return emptyCells[chosenCell];
		}
	}
}