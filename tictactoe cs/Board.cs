using System;
using System.Collections;

namespace tictactoe_cs
{
	public enum CellValue { Empty, Cross, Ring }
	class Board : IBoard
	{
		readonly CellValue[] _board = new CellValue[9];

		public Board(Board board)
		{
			Array.Copy(board._board, _board, 9);
		}

		public Board(IBoard board)
		{
			foreach (var position in BoardExtensions.GetPositions())
			{
				Set(position, board.GetPosition(position));
			}
		}

		public Board()
		{
		}

		public CellValue GetPosition(Position position)
		{
			return _board[position.Index];
		}

		public void Set(Position position, CellValue cross)
		{
			_board[position.Index] = cross;
		}

		protected bool Equals(Board other)
		{
			return ((IStructuralEquatable)_board).Equals(other._board, StructuralComparisons.StructuralEqualityComparer);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType()) return false;
			return Equals((Board) obj);
		}

		public override int GetHashCode()
		{
			return ((IStructuralEquatable) _board).GetHashCode(StructuralComparisons.StructuralEqualityComparer);
		}
	}
}