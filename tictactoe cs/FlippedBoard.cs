namespace tictactoe_cs
{
	class FlippedBoard : IBoard
	{
		readonly IBoard original;

		public FlippedBoard(IBoard original)
		{
			this.original = original;
		}

		public bool? GetPosition(Position position)
		{
			var originalResult = original.GetPosition(position);
			return !originalResult;
		}
	}
}