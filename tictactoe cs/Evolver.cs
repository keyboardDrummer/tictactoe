using System.Collections.Generic;
using System.Windows.Forms;

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

		public IList<Statistics> PlayGames(int games)
		{
			var result = new List<Statistics>();
			var statistics = new Statistics(0, 0, 0);
			for (var counter = 0; counter < games; counter++)
			{
				var winner = FindWinner(true);
				statistics = statistics.WithWinner(winner == null ? (bool?)null : winner == First);
				result.Add(statistics);
			}
			return result;
		}

		public IAI FindWinner(bool firstBegins)
		{
			var board = new Board();
			var currentPlayer = firstBegins ? First : Second;
			var boardPerPlayer = new Dictionary<IAI, IBoard>
			{
				{currentPlayer, board},
				{GetNextPlayer(currentPlayer), new FlippedBoard(board)}
			};

			while (board.CanPlay())
			{
				var playerBoard = boardPerPlayer[currentPlayer];
				var choice = currentPlayer.Step(playerBoard);
				if (board.GetPosition(choice) == CellValue.Empty)
				{
					var crossIsPlaying = playerBoard == board;
					board.Set(choice, crossIsPlaying ? CellValue.Cross : CellValue.Ring);
				}
				else
				{
					return GetNextPlayer(currentPlayer);
				}

				var winState = playerBoard.HasWon();
				if (winState != CellValue.Empty)
				{
					currentPlayer.Learn(board, choice, true);
					return currentPlayer;
				}
				currentPlayer.Learn(board, choice, false);
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