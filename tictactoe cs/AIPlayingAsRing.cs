namespace tictactoe_cs
{
	class AIPlayingAsRing : IAI
	{
		readonly IAI _original;

		public AIPlayingAsRing(IAI original)
		{
			this._original = original;
		}

		public Position Step(IBoard board)
		{
			return _original.Step(new FlippedBoard(board));
		}

		public void Learn(IBoard previousState, IBoard currentState, Position choice, bool? youWin)
		{
			_original.Learn(new FlippedBoard(previousState), new FlippedBoard(currentState), choice, youWin);
		}
	}
}