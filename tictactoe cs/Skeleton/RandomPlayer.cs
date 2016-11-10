using System;

namespace tictactoe_cs
{
	internal class RandomPlayer : IPlayer
	{
		public Random Rnd { get; set; } = new Random();

		public virtual int Play(Board board, int player)
		{
			var validMoves = board.GetValidMoves();
			var move = validMoves[Rnd.Next(validMoves.Length)];
			return move;
		}

		public void Learn()
		{
			// done!
		}
	}
}