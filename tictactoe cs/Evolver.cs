using System;
using System.Collections.Generic;
using System.Linq;

namespace tictactoe_cs
{
	enum WinState { Draw, Won, Loss }

	class Evolver
	{
		IAI first;
		IAI second;

		int gamesPlayed;
		int firstWins;
		int secondWins;

		public WinState PlayGame(bool firstBegins)
		{
			var board = new Board();
			Func<IAI, IAI> getNext = player => player == first ? second : first;
			var currentPlayer = firstBegins ? first : second;
			while (CanPlay(board))
			{
				currentPlayer.Step(board);
				var winState = HasWon(board);
				if (winState != null)
				{
					return winState.Value ? WinState.Won : WinState.Loss;
				}
				currentPlayer = getNext(currentPlayer);
			}
			return WinState.Draw;
		}

		bool CanPlay(Board board)
		{
			return board.GetStates().Any(p => p == null);
		}

		bool? HasWon(Board board)
		{
			for (int r = 0; r < 3; r++)
			{
				if (board[r,0] != null && board[r,0] == board[r,1] &&
					board[r,0] == board[r,2])
				{
					return board[r, 0];
				}
			}
			for (int c = 0; c < 3; c++)
			{
				if (board[0,c]!= null && board[0,c] == board[1,c] &&
					board[0,c] == board[2,c])
				{
					return board[0, c];
				}
			}
			if (board[0,0]!= null && board[0,0] == board[1,1] &&
				board[0,0] == board[2,2])
			{
				return board[0, 0];
			}
			if (board[0,2]!= null && board[0,2] == board[1,1] &&
				board[0,2] == board[2,0])
			{
				return board[0, 2];
			}
			return null;
		}

		class Board : IBoard
		{
			readonly bool?[,] board = new bool?[3, 3];

			public bool? GetPosition(Position position)
			{
				return board[position.R, position.C];
			}

			public bool? this[int row, int column] => board[row, column];

			public IEnumerable<bool?> GetStates()
			{
				return GetPositions().Select(GetPosition);
			}

			static IEnumerable<Position> GetPositions()
			{
				return Enumerable.Range(0, 9).
					Select(index => new Position(index%3, index/3));
			}
		}
	}
}
