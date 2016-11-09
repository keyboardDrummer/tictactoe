using System;
using System.Collections.Generic;
using System.Linq;

namespace tictactoe_cs
{
	abstract class QLearner<TAction, TState>
	{
		double _learningRate = 0.1;
		double _discountFactor = 1.0;
		readonly Random _random = new Random();

		Dictionary<TState, Dictionary<TAction, double>> StateActionRewards { get; }

		protected abstract Dictionary<TState, Dictionary<TAction, double>> GetInitialStateActionRewards();

		protected QLearner()
		{
			StateActionRewards = GetInitialStateActionRewards();
		}

		protected void Learn(TAction action, TState state, TState previousState, double reward)
		{
			if (StateActionRewards[state].Values.Any(v => v > 0.0))
			{
				Console.Write(" jo");
			}
			double maxRewardNextStep = StateActionRewards[state].Values.Concat(new List<double> { 0.0 }).Max(v => v);
			double currentReward = StateActionRewards[previousState][action];
			var learnedValue = reward + _discountFactor * maxRewardNextStep;
			StateActionRewards[previousState][action] = currentReward + _learningRate * (learnedValue - currentReward);
		}

		public TAction Step(TState board)
		{
			var actions = StateActionRewards[board];
			double total = actions.Values.Sum();
			if (total < 1e-5)
			{
				return actions.Keys.ElementAt(_random.Next(actions.Keys.Count - 1));
			}
			double randomAction = total * _random.NextDouble();
			var normalizedRewards = actions.ToList();
			for (int index = 1; index < normalizedRewards.Count; index++)
			{
				var value = normalizedRewards[index].Value;
				if (value > randomAction)
				{
					return normalizedRewards[index].Key;
				}
				randomAction -= value;
			}
			throw new NotImplementedException();
		}
	}
}