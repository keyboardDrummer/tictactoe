using System;
using System.Collections.Generic;
using System.Linq;

namespace tictactoe_cs
{
	class QLearningAI : IAI
	{
		readonly Random _random = new Random();
		double _learningRate = 1.0;
		double _discountFactor = 1.0;

		Dictionary<Board, Dictionary<Position, double>> StateActionRewards { get; } 
			= new Dictionary<Board, Dictionary<Position, double>>();

		public QLearningAI()
		{
			InitializeRewards();
		}

		IEnumerable<Board> GetAllBoards()
		{
			return GetAllBoards(0);
		}

		IEnumerable<Board> GetAllBoards(int fromCell)
		{
			if (fromCell == 9)
			{
				yield return new Board();
			}
			else
			{
				var position = new Position(fromCell / 3, fromCell % 3);
				var recursive = GetAllBoards(fromCell + 1);
				foreach (var recursiveResult in recursive)
				{
					foreach (var cellState in Enum.GetValues(typeof(CellValue)).Cast<CellValue>())
					{
						var result = new Board(recursiveResult);
						result.Set(position, cellState);
						yield return result;
					}
				}
			}
		}

		void InitializeRewards()
		{
			var boards = GetAllBoards();
			foreach (var board in boards)
			{
				var stateActionValues = new Dictionary<Position, double>();
				StateActionRewards.Add(board, stateActionValues);
				var unusedPositions = BoardExtensions.GetPositions().Where(position => board.GetPosition(position) == CellValue.Empty);
				foreach (var unusedPosition in unusedPositions)
				{
					stateActionValues[unusedPosition] = 0;
				}
			}
		}

		public Position Step(IBoard iBoard)
		{
			var board = new Board(iBoard);
			return ChooseAction(board);
		}

		Position ChooseAction(Board board)
		{
			var actions = StateActionRewards[board];
			double total = actions.Values.Sum();
			if (total < 1e-5)
			{
				return actions.Keys.ElementAt(_random.Next(actions.Keys.Count - 1));
			}
			var normalizedRewards = actions.Select(kv => new {kv.Key, Value = kv.Value / total})
				.ToDictionary(kv => kv.Key, kv => kv.Value);
			var previousPosition = normalizedRewards.Keys.First();
			foreach (var position in normalizedRewards.Keys.Skip(1).ToList())
			{
				normalizedRewards[position] += normalizedRewards[previousPosition];
				previousPosition = position;
			}
			double randomAction = _random.NextDouble();
			return normalizedRewards.First(kva => kva.Value > randomAction).Key;
		}

		public void Learn(IBoard iState, Position action, bool youWin)
		{
			var state = new Board(iState);
			var previousState = new Board(state);
			previousState.Set(action, CellValue.Empty);
			Learn(action, state, previousState, youWin ? 1.0 : 0);
		}

		void Learn(Position action, Board state, Board previousState, double reward)
		{
			if (StateActionRewards[state].Values.Any(v => v > 0.0))
			{
				Console.Write(" jo");
			}
			double maxRewardNextStep = StateActionRewards[state].Values.Concat(new List<double> {0.0}).Max(v => v);
			double currentReward = StateActionRewards[previousState][action];
			var learnedValue = reward + _discountFactor * maxRewardNextStep;
			StateActionRewards[previousState][action] = currentReward + _learningRate * (learnedValue - currentReward);
		}
	}
}