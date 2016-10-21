namespace tictactoe_cs
{
	class Statistics
	{
		public int GamesPlayed { get; }

		public int FirstWins { get; }

		public int SecondWins { get; }

		public Statistics(int gamesPlayed, int firstWins, int secondWins)
		{
			GamesPlayed = gamesPlayed;
			FirstWins = firstWins;
			SecondWins = secondWins;
		}

		public Statistics WithWinner(bool? winner)
		{
			return winner.HasValue
				? (winner.Value
					? new Statistics(GamesPlayed + 1, FirstWins + 1, SecondWins)
					: new Statistics(GamesPlayed + 1, FirstWins, SecondWins + 1))
				: new Statistics(GamesPlayed + 1, FirstWins, SecondWins);
		}

		public double FirstWinPercentage => (double) FirstWins/GamesPlayed;
	}
}