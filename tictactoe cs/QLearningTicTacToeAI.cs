using System;
using System.Collections.Generic;
using System.Linq;

namespace tictactoe_cs
{
	class QLearningTicTacToeAI : QLearner<Position, Board>, IAI
	{
		protected override Dictionary<Board, Dictionary<Position, double>> GetInitialStateActionRewards()
		{
			var result = new Dictionary<Board, Dictionary<Position, double>>();
			var boards = GetAllBoards();
			foreach (var board in boards)
			{
				var stateActionValues = new Dictionary<Position, double>();
				result.Add(board, stateActionValues);
				var unusedPositions = BoardExtensions.GetPositions().Where(position => board.GetPosition(position) == CellValue.Empty);
				foreach (var unusedPosition in unusedPositions)
				{
					stateActionValues[unusedPosition] = 0;
				}
			}
			return result;
		}
		IEnumerable<Board> GetAllBoards()
		{
			return GetAllBoards(0);
		}

		IEnumerable<Board> GetAllBoards(int fromCell)
		{
			if (fromCell == 9)
			{
				yield return new Board();
			}
			else
			{
				var position = new Position(fromCell / 3, fromCell % 3);
				var recursive = GetAllBoards(fromCell + 1);
				foreach (var recursiveResult in recursive)
				{
					foreach (var cellState in Enum.GetValues(typeof(CellValue)).Cast<CellValue>())
					{
						var result = new Board(recursiveResult);
						result.Set(position, cellState);
						yield return result;
					}
				}
			}
		}

		public Position Step(IBoard board)
		{
			return base.Step(new Board(board));
		}

		public void Learn(IBoard previousState, IBoard iState, Position action, bool? youWin)
		{
			Learn(action, new Board(iState), new Board(previousState), youWin == null ? 0 : youWin.Value ? 1.0 : -1.0);
		}
	}
}