namespace qlttt
{
	public interface IPlayer
	{
		int Play(Board board, int moveFor);
		void Learn();
	}
}