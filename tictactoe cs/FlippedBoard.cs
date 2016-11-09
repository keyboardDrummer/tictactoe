using System;

namespace tictactoe_cs
{
	class FlippedBoard : IBoard
	{
		readonly IBoard _original;

		public FlippedBoard(IBoard original)
		{
			_original = original;
		}

		public CellValue GetPosition(Position position)
		{
			var originalResult = _original.GetPosition(position);
			switch (originalResult)
			{
				case CellValue.Empty:
					return CellValue.Empty;
				case CellValue.Cross:
					return CellValue.Ring;
				case CellValue.Ring:
					return CellValue.Cross;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
	}
}