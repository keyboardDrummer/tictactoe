using System;

namespace qlttt
{
	internal class RandomPlayer : IPlayer
	{
		public Random Rnd { get; set; } = new Random();

		public virtual int Play(Board board, int moveFor)
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