namespace tictactoe_cs
{
	public interface IPlayer
	{
		int Play(Board board, int moveFor);
		void Learn();
	}
}