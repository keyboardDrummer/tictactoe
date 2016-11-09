namespace tictactoe_cs
{
	class DualBoard : IBoard
	{
		readonly Board _crossPerspective = new Board();
		readonly Board _ringPerspective = new Board();

		public CellValue GetPosition(Position position)
		{
			return _crossPerspective.GetPosition(position);
		}
	}
}