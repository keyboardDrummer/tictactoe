namespace tictactoe_cs
{
	class Statistics
	{
		public int GamesPlayed { get; }

		public int CrossWins { get; }

		public int RingWins { get; }

		public Statistics(int gamesPlayed, int crossWins, int ringWins)
		{
			GamesPlayed = gamesPlayed;
			CrossWins = crossWins;
			RingWins = ringWins;
		}

		public Statistics WithWinner(Player? winner)
		{
			return winner.HasValue
				? (winner.Value == Player.Cross
					? new Statistics(GamesPlayed + 1, CrossWins + 1, RingWins)
					: new Statistics(GamesPlayed + 1, CrossWins, RingWins + 1))
				: new Statistics(GamesPlayed + 1, CrossWins, RingWins);
		}

		public double FirstWinPercentage => (double) CrossWins / GamesPlayed;
		public double FirstWinOrTiePercentage => (double) (GamesPlayed - RingWins) / GamesPlayed;
	}
}