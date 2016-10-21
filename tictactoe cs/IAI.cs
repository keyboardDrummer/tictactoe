namespace tictactoe_cs
{
	interface IAI
	{
		Position Step(IBoard board);
		void Learn(IBoard endGame, bool youWon);
	}
}