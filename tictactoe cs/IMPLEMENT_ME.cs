using System;
using System.Diagnostics;

namespace tictactoe_cs
{
	internal class MyOwnPlayer : IPlayer
	{
		public int Play(Board board, int player)
		{
			throw new NotImplementedException("IMPLEMENT ME!");
		}

		public void Learn()
		{
			Console.WriteLine("start learning");
			var sw = new Stopwatch();
			sw.Start();

			throw new NotImplementedException("IMPLEMENT ME!");

			sw.Stop();
			Console.WriteLine("learning took " + sw.Elapsed);
		}
	}
}