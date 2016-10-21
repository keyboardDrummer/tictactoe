using System;
using System.Linq;

namespace tictactoe_cs
{
	class RandomIai : IAI
	{
		readonly Random _random = new Random();

		public Position Step(IBoard board)
		{
			var emptyCells =
				Enumerable.Range(0, 9)
					.Select(index => new Position (index%3, index/3))
					.Where(b => board.GetPosition(b) == null)
					.ToList();
			var chosenCell = _random.Next(emptyCells.Count);
			return emptyCells[chosenCell];
		}
	}
}