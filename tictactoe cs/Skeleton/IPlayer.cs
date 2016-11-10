namespace tictactoe_cs
{
	public interface IPlayer
	{
		int Play(Board board, int player);
		void Learn();
	}
}