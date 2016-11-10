using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace qlttt
{
	internal class QPlayer : IPlayer
	{
		public Dictionary<Board, Dictionary<int, double>> Q { get; set; } = new Dictionary<Board, Dictionary<int, double>>();

		public Random Rnd { get; set; } = new Random();


		public int Play(Board board, int moveFor)
		{
			Dictionary<int, double> moveqs;
			if (!Q.TryGetValue(board, out moveqs) || !moveqs.Any())
				return MakeRandomMove(board);

			var best = moveFor == 1 ? moveqs.Max(p => p.Value) : moveqs.Min(p => p.Value);
			var candidates = moveqs.Where(k => k.Value == best).ToArray();

			if (best == 0.0)
				return MakeRandomMove(board);

			return candidates[Rnd.Next(candidates.Length)].Key;
		}

		public void Learn()
		{
			Console.WriteLine("start learning");
			var sw = new Stopwatch();
			sw.Start();

			for (var g = 0; g < 100000; g++)
			{
				var player = 1;
				var board = new Board();
				var gameover = false;
				do
				{
					double epsilon = g < 30000 ? 0.9 : g < 60000 ? 0.3 : 0.1;
					int move;
					if (Rnd.NextDouble() < epsilon)
						move = MakeRandomMove(board);
					else
						move = Play(board, player);
					var newBoard = board.MakeMove(move, player);

					var reward = newBoard.GetReward();
					if (reward != 0)
						gameover = true;

					Dictionary<int, double> currentBoardQs;
					if (!Q.TryGetValue(board, out currentBoardQs))
					{
						currentBoardQs = new Dictionary<int, double>();
						Q[board] = currentBoardQs;
					}


					Dictionary<int, double> newBoardQs;
					if (!Q.TryGetValue(newBoard, out newBoardQs))
					{
						newBoardQs = new Dictionary<int, double>();
						Q[newBoard] = newBoardQs;
					}

					var nsv = newBoardQs.Values;
					if (player == 1)
						currentBoardQs[move] = reward + (!gameover && nsv.Any() ? 0.8*nsv.Min() : 0.0);
					else
						currentBoardQs[move] = reward + (!gameover && nsv.Any() ? 0.8*nsv.Max() : 0.0);
					board = newBoard;
					player = player == 1 ? 2 : 1;
				} while (!gameover);
			}
			sw.Stop();
			Console.WriteLine("learning took " + sw.Elapsed);
		}

		protected int MakeRandomMove(Board board)
		{
			var validMoves = board.GetValidMoves();
			return validMoves[Rnd.Next(validMoves.Length)];
		}
	}
}