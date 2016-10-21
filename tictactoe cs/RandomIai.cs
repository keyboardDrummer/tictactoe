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
					.Select(index => new Position (index%3, index/3))
					.Where(b => board.GetPosition(b) == null)
					.ToList();
			var chosenCell = new Random().Next(emptyCells.Count);
			return emptyCells[chosenCell];
		}
	}
}