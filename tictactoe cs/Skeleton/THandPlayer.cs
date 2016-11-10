namespace tictactoe_cs
{
	internal class THandPlayer : RandomPlayer
	{
		public override int Play(Board board, int moveFor)
		{
			var ownWinningMove = FindWinningMove(board, moveFor);
			if (ownWinningMove != -1)
				return ownWinningMove;
			var opponentWinningMove = FindWinningMove(board, moveFor == 1 ? 2 : 1);
			if (opponentWinningMove != -1)
				return opponentWinningMove;
			return base.Play(board, moveFor);
		}

		protected int FindWinningMove(Board board, int player)
		{
			var moves = board.GetValidMoves();

			foreach (var move in moves)
			{
				var newBoard = board.MakeMove(move, player);
				if (newBoard.GetReward() == (player == 1 ? 1.0 : -1.0))
					return move;
			}
			return -1;
		}
	}
}