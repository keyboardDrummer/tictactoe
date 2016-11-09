using System;
using System.Linq;

namespace tictactoe_cs
{
	class RandomIai : IAI
	{
		public Random Random { get; } = new Random();
		
		public Position Step(IBoard board)
		{
			var emptyCells =
				Enumerable.Range(0, 9)
					.Select(index => new Position (index%3, index/3))
					.Where(b => board.GetPosition(b) == CellValue.Empty)
					.ToList();
			var chosenCell = Random.Next(emptyCells.Count);
			return emptyCells[chosenCell];
		}

		public void Learn(IBoard endGame, Position choice, bool youWon)
		{
		}
	}
}