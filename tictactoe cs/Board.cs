using System;

namespace tictactoe_cs
{
	class Board : IBoard
	{
		readonly bool?[,] board = new bool?[3, 3];

		public bool? GetPosition(Position position)
		{
			return board[position.R, position.C];
		}

		public void Set(Position position, bool cross)
		{
			if (board[position.R, position.C] != null)
				throw new NotSupportedException("Stepping on another player's move!");

			board[position.R, position.C] = cross;
		}
	}
}