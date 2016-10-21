using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tictactoe_cs
{
	class QLearningAI : IAI
	{
		Random random = new Random((int) DateTime.Now.Ticks);
		private double _initialReward = 0.1;
		private double _learningRate = 1.0;
		private double _discountFactor = 0.0;

		private Dictionary<string, Dictionary<int, double>> StateActionRewards { get; } =
			new Dictionary<string, Dictionary<int, double>>();

		public QLearningAI()
		{
			InitializeRewards();
		}

		private void InitializeRewards()
		{
			var states = new[] {" ", "0", "1"}.ToList();
			// ReSharper disable once ReturnValueOfPureMethodIsNotUsed
			states.SelectMany(c1 =>
				states.SelectMany(c2 =>
					states.SelectMany(c3 =>
						states.SelectMany(c4 =>
							states.SelectMany(c5 =>
								states.SelectMany(c6 =>
									states.SelectMany(c7 =>
										states.SelectMany(c8 =>
											states.SelectMany(c9 =>
												{
													string state = c1 + c2 + c3 + c4 + c5 + c6 + c7 + c8 + c9;
													var actions = new Dictionary<int, double>();
													for (int action = 0; action < 9; action++)
													{
														actions.Add(action, _initialReward);
													}
													StateActionRewards.Add(state, actions);
													return state;
												}
											)))))))));
		}

		public Position Step(IBoard board)
		{
			string state = ToState(board);
			int action = ChooseAction(state);
			double maxRewardNextStep = DetermineMaxRewardNextStep(state, action);
			// Update rewardThisAction with reward when winning.
			double rewardThisAction = _initialReward;
			UpdateReward(state, action, rewardThisAction, maxRewardNextStep);
			return new Position((int) Math.Floor(action/3.0), action%3);
		}

		private void UpdateReward(string state, int action, double reward, double maxRewardNextStep)
		{
			double currentReward = StateActionRewards[state][action];
			StateActionRewards[state][action] = currentReward +
			                                    _learningRate*
			                                    (reward + _discountFactor*maxRewardNextStep - currentReward);
		}

		// This is a more advanced parameter. Currently it is available, just not yet implemented.
		private double DetermineMaxRewardNextStep(string state, int action)
		{
			return 0.0;
		}

		private int ChooseAction(string state)
		{
			var actions = StateActionRewards[state];
			double total = actions.Values.Sum();
			var normalizedRewards = actions.Select(kv => new {kv.Key, Value = kv.Value/total})
				.ToDictionary(kv => kv.Key, kv => kv.Value);
			for (int i = 1; i < 9; i++)
			{
				normalizedRewards[i] += normalizedRewards[i - 1];
			}
			double randomAction = random.NextDouble();
			int action =
				normalizedRewards.Select((kv, i) => new {KeyValue = kv, Action = i})
					.First(kva => kva.KeyValue.Value > randomAction)
					.Action;
			return action;
		}

		public void Learn(IBoard endGame, bool youWon)
		{
			// I need to know the action that resulted in winning!
			string state = ToState(endGame);
			int action = 0; //TODO
			UpdateReward(state, action, 1.0, 0.0);
		}

		string ToState(IBoard board)
		{
			var sb = new StringBuilder();
			for (int r = 0; r < 3; r++)
				for (int c = 0; c < 3; c++)
					sb.Append(AsChar(board.GetPosition(r, c)));
			return sb.ToString();
		}

		char AsChar(bool? cellState)
		{
			if (!cellState.HasValue)
			{
				return ' ';
			}
			return cellState.Value ? '1' : '0';
		}
	}
}