using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace tictactoe_cs
{
	class Evolver
	{
		public Evolver(IAI cross, IAI ring)
		{
			Cross = cross;
			Ring = new AIPlayingAsRing(ring);
		}

		public IAI Cross { get; }

		public IAI Ring { get; }

		public IAI GetAI(Player cross)
		{
			return cross == Player.Cross ? Cross : Ring;
		}

		public void PlayGames(int count)
		{
			for (int counter = 0; counter < count; counter++)
			{
				PlayGame(true);
			}
		}

		public IEnumerable<Statistics> PlayGames()
		{
			var statistics = new Statistics(0, 0, 0);
			while (true)
			{
				var winner = PlayGame(true);
				statistics = statistics.WithWinner(winner);
				yield return statistics;
			}
		}

		public Player? PlayGame(bool crossBegins)
		{
			var board = new Board();
			var currentPlayer = crossBegins ? Player.Cross : Player.Ring;

			Action<IBoard, bool?> learn = null;
			while (board.CanPlay())
			{
				var boardBeforePlay = board;
				var currentAI = GetAI(currentPlayer);
				var choice = currentAI.Step(boardBeforePlay);
				var boardAfterPlay = new Board(board);
				if (board.GetPosition(choice) == CellValue.Empty)
				{
					var crossIsPlaying = currentPlayer == Player.Cross;
					boardAfterPlay.Set(choice, crossIsPlaying ? CellValue.Cross : CellValue.Ring);
				}
				else
				{
					MessageBox.Show("Your AI made an invalid move");
					return GetNextPlayer(currentPlayer);
				}

				var winState = boardAfterPlay.HasWon();
				if (winState != CellValue.Empty)
				{
					currentAI.Learn(board, boardAfterPlay, choice, true);
					learn?.Invoke(boardAfterPlay, false);
					return currentPlayer;
				}
				board = boardAfterPlay;
				currentPlayer = GetNextPlayer(currentPlayer);
				learn?.Invoke(boardAfterPlay, null);

				var learnBoard = boardBeforePlay;
				learn = (newState, result) => currentAI.Learn(learnBoard, newState, choice, result);
			}
			return null;
		}

		Player GetNextPlayer(Player player)
		{
			return player == Player.Cross ? Player.Ring : Player.Cross;
		}
	}
}