namespace tictactoe_cs
{
	interface IAI
	{
		Position Step(IBoard board);
		void Learn(IBoard previousState, IBoard currentState, Position choice, bool? youWin);
	}
}