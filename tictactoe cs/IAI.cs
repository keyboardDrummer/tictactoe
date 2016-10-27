namespace tictactoe_cs
{
	interface IAI
	{
		Position Step(IBoard board);
		void Learn(IBoard endGame, Position choice, bool youWon);
	}
}