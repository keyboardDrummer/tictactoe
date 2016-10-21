namespace tictactoe_cs
{
	interface IAI
	{
		Position Step(IBoard board);
		void Learn(IBoard board, bool youWon);
	}
}