using System.Collections.Generic;
using System.Linq;

namespace tictactoe_cs
{
	class QLearningTicTacToeAI : QLearner<Position, Board>, IAI
	{
		public Position Step(IBoard board)
		{
			return base.Step(new Board(board));
		}

		public void Learn(IBoard previousState, IBoard iState, Position action, bool? youWin)
		{
			Learn(action, new Board(iState), new Board(previousState), youWin == null ? 0 : youWin.Value ? 1.0 : -1.0);
		}

		protected override IEnumerable<Position> GetActionsValidForState(Board state)
		{
			return BoardExtensions.GetPositions().Where(position => state.GetPosition(position) == CellValue.Empty);
		}
	}
}