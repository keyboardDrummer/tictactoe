using System.Collections.Generic;

namespace tictactoe_cs
{
	class Evolver
	{
		public Evolver(IAI first, IAI second)
		{
			First = first;
			Second = second;
		}

		public IAI First { get; }

		public IAI Second { get; }

		public IEnumerable<Statistics> PlayGames()
		{
			var statistics = new Statistics(0,0,0);
			while (true)
			{
				var winner = FindWinner(true);
				statistics = statistics.WithWinner(winner == null ? (bool?)null : winner == First);
				yield return statistics;
			}
		}

		public IAI FindWinner(bool firstBegins)
		{
			var board = new Board();
			var currentPlayer = firstBegins ? First : Second;
			var boardPerPlayer = new Dictionary<IAI, IBoard> {
				{ currentPlayer, board },
				{ GetNextPlayer(currentPlayer), new FlippedBoard(board)}};

			while (board.CanPlay())
			{
				var playerBoard = boardPerPlayer[currentPlayer];
				var choice = currentPlayer.Step(playerBoard);
				board.Set(choice, playerBoard == board);
				var winState = playerBoard.HasWon();
				if (winState != null)
				{
					var winner = winState.Value ? currentPlayer : GetNextPlayer(currentPlayer);
					winner.Learn(boardPerPlayer[winner], true);
					var loser = GetNextPlayer(winner);
					loser.Learn(boardPerPlayer[loser], false);
					return winner;
				}
				currentPlayer = GetNextPlayer(currentPlayer);
			}
			return null;
		}

		IAI GetNextPlayer(IAI player)
		{
			return player == First ? Second : First;
		}
	}
}
