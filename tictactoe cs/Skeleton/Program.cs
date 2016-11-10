using System;

namespace tictactoe_cs
{
	internal class Stats
	{
		public static void Print(IPlayer player)
		{
			var qplayer = player;
			var rplayer = new THandPlayer(); // new RandomPlayer();

			var game = new TicTacToeGame();
			var qplayerWins1 = 0;
			var otherPlayerWins1 = 0;
			var draw1 = 0;
			var totalgames1 = 0;
			for (var g = 0; g < 5000; g++)
			{
				var result = game.PlayGame(qplayer, rplayer);
				if (result == 1.0)
					qplayerWins1++;
				if (result == -1.0)
					otherPlayerWins1++;
				if (result == 0.5)
					draw1++;
				totalgames1++;
			}

			Console.WriteLine("-------qplayer as player 1-----------------");
			Console.WriteLine("      qplayer wins: " + qplayerWins1);
			Console.WriteLine(" other player wins: " + otherPlayerWins1);
			Console.WriteLine("             draws: " + draw1);
			Console.WriteLine("            #games: " + totalgames1);
			Console.WriteLine("              win%: " + 100.0*(qplayerWins1/(double) totalgames1));

			game = new TicTacToeGame();
			var qplayerWins2 = 0;
			var otherPlayerWins2 = 0;
			var draw2 = 0;
			var totalgames2 = 0;
			for (var g = 0; g < 5000; g++)
			{
				var result = game.PlayGame(rplayer, qplayer);
				if (result == -1.0)
					qplayerWins2++;
				if (result == 1.0)
					otherPlayerWins2++;
				if (result == 0.5)
					draw2++;
				totalgames2++;
			}
			Console.WriteLine("-------qplayer as player 2-----------------");
			Console.WriteLine("      qplayer wins: " + qplayerWins2);
			Console.WriteLine(" other player wins: " + otherPlayerWins2);
			Console.WriteLine("             draws: " + draw2);
			Console.WriteLine("            #games: " + totalgames2);
			Console.WriteLine("              win%: " + 100.0*qplayerWins2/totalgames2);

			Console.WriteLine("------overall------------");
			Console.WriteLine("      qplayer wins: " + (qplayerWins1 + qplayerWins2));
			Console.WriteLine(" other player wins: " + (otherPlayerWins1 + otherPlayerWins2));
			Console.WriteLine("             draws: " + (draw1 + draw2));
			Console.WriteLine("            #games: " + (totalgames1 + totalgames2));
			Console.WriteLine("              win%: " + 100.0*(qplayerWins1 + qplayerWins2)/(totalgames1 + totalgames2));
		}
	}
}