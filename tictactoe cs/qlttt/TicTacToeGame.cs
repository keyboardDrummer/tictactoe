using System.Collections.Generic;

namespace qlttt
{
	public class TicTacToeGame
	{
		public double PlayGame(IPlayer player1, IPlayer player2)
		{
			var board = new Board();
			var boards = new List<Board>();
			while (true)
			{
				var move1 = player1.Play(board, 1);
				board = board.MakeMove(move1, 1);
				boards.Add(board);
				var reward1 = board.GetReward();
				if (reward1 != 0)
					return reward1;
				var move2 = player2.Play(board, 2);
				board = board.MakeMove(move2, 2);
				boards.Add(board);
				var reward2 = board.GetReward();
				if (reward2 != 0)
					return reward2;
			}
		}
	}
}