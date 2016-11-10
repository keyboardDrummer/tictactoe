using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tictactoe_cs
{
	class QLearningAI : IAI
	{
		Random random = new Random((int) DateTime.Now.Ticks);
		private double _initialReward = 0.001;
		private double _learningRate = 1.0;
		private double _discountFactor = 0.5;

		private Dictionary<string, Dictionary<int, double>> StateActionRewards { get; set; }

		public QLearningAI()
		{
			InitializeRewards();
		}

		private void InitializeRewards()
		{
			var states = new[] {" ", "0", "1"}.ToList();
			StateActionRewards = states.SelectMany(c1 =>
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
													return new[] {new KeyValuePair<string, Dictionary<int, double>>(state, actions)};
												}
											))))))))).ToDictionary(kv => kv.Key, kv => kv.Value);
		}

		public Position Step(IBoard board)
		{
			string state = ToState(board);
			int action = ChooseAction(state);
			return new Position((int) Math.Floor(action/3.0), action%3);
		}

		private void UpdateReward(string state, int action, double reward, double maxRewardNextStep)
		{
			double currentReward = StateActionRewards[state][action];
			StateActionRewards[state][action] = currentReward +
			                                    _learningRate*
			                                    (reward + _discountFactor*maxRewardNextStep - currentReward);
		}

		private int ChooseAction(string state)
		{
			var validActions = state.Select((c, i) => new {c, i}).Where(ci => ci.c == ' ').Select(ci => ci.i);
			var actions = StateActionRewards[state].Where(kv => validActions.Contains(kv.Key))
				.ToDictionary(kv => kv.Key, kv => kv.Value);
			double total = actions.Values.Sum();
			var normalizedRewards = actions.Select(kv => new {kv.Key, Value = kv.Value/total})
				.ToDictionary(kv => kv.Key, kv => kv.Value);
			double currentSumReward = 0.0;
			foreach (int i in validActions)
			{
				currentSumReward += normalizedRewards[i];
				normalizedRewards[i] = currentSumReward;
			}
			double randomAction = random.NextDouble();
			int action =
				normalizedRewards.First(kv => kv.Value > randomAction).Key;
			return action;
		}

		public void Learn(IBoard endGame, Position choice, bool youWon)
		{
			string state = ToState(endGame);
			int action = choice.R*3 + choice.C;
			string previousState = state.Remove(action, 1).Insert(action, " ");
			double maxRewardNextStep = 0.0; // youWon ? 1.0 : StateActionRewards[state].Values.Max(v => v);
			UpdateReward(previousState, action, youWon ? 1.0 : _initialReward, maxRewardNextStep);
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