using System;
using System.Collections.Generic;
using System.Reactive.Linq;

namespace tictactoe_cs
{
	class Evolver
	{
		readonly IAI _first;
		readonly IAI _second;

		public Evolver(IAI first, IAI second)
		{
			_first = first;
			_second = second;
		}

		public IEnumerable<Statistics> PlayGames()
		{
			var statistics = new Statistics(0,0,0);
			var winner = FindWinner(true);
			statistics.WithWinner(winner == null ? (bool?)null : winner == _first);
			yield return statistics;
		}

		public IAI FindWinner(bool firstBegins)
		{
			var board = new Board();
			Func<IAI, IAI> getNext = player => player == _first ? _second : _first;
			var currentPlayer = firstBegins ? _first : _second;
			var boardPerPlayer = new Dictionary<IAI, IBoard> {
				{ currentPlayer, board},
				{ getNext(currentPlayer), new FlippedBoard(board)}};
			while (board.CanPlay())
			{
				var playerBoard = boardPerPlayer[currentPlayer];
				currentPlayer.Step(playerBoard);
				var winState = playerBoard.HasWon();
				if (winState != null)
				{
					return winState.Value ? currentPlayer : getNext(currentPlayer);
				}
				currentPlayer = getNext(currentPlayer);
			}
			return null;
		}
	}
}
