using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace tictactoe_cs
{
	abstract class QLearner<TAction, TState>
	{
		double _learningRate = 0.1;
		double _discountFactor = 1.0;
		readonly Random _random = new Random();

		Dictionary<TState, Dictionary<TAction, double>> StateActionRewards { get; } = new Dictionary<TState, Dictionary<TAction, double>>();

		protected void Learn(TAction action, TState state, TState previousState, double reward)
		{
			if (GetActionRewards(state).Values.Any(v => v > 0.0))
			{
				Console.Write(" jo");
			}
			double maxRewardNextStep = GetActionRewards(state).Values.Concat(new List<double> { 0.0 }).Max(v => v);
			double currentReward = StateActionRewards[previousState][action];
			var learnedValue = reward + _discountFactor * maxRewardNextStep;
			GetActionRewards(previousState)[action] = currentReward + _learningRate * (learnedValue - currentReward);
		}

		Dictionary<TAction, double> GetActionRewards(TState state)
		{
			Dictionary<TAction, double> result;
			if (StateActionRewards.TryGetValue(state, out result))
			{
				return result;
			}

			result = new Dictionary<TAction, double>();
			foreach (var action in GetActionsValidForState(state))
			{
				result[action] = 0;
			}
			StateActionRewards[state] = result;
			return result;
		}

		protected abstract IEnumerable<TAction> GetActionsValidForState(TState state);

		public TAction Step(TState board)
		{
			var actions = GetActionRewards(board);
			double total = actions.Values.Sum();
			if (total < 1e-5)
			{
				return actions.Keys.ElementAt(_random.Next(actions.Keys.Count - 1));
			}
			double randomAction = total * _random.NextDouble();
			foreach (var actionReward in actions)
			{
				var value = actionReward.Value;
				if (value >= randomAction)
				{
					return actionReward.Key;
				}
				randomAction -= value;
			}
			throw new NotImplementedException();
		}
	}
}