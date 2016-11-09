using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.CSharp.RuntimeBinder;

namespace tictactoe_cs
{
	class Grid : System.Windows.Forms.Panel, IBoard
	{
		readonly cell[][] _matrix = new cell[3][];
		char first = 'X';
		char current = 'X';
		bool gameOver;
		int moveCounter;
		IAI _iai;

		public CellValue GetPosition(Position position)
		{
			switch (_matrix[position.R][position.C].display)
			{
				case ' ':
					return CellValue.Empty;
				case 'X':
					return CellValue.Cross;
				case '0':
					return CellValue.Ring;
			}
			throw new NotSupportedException();
		}


		public Grid()
		{
			this.Size = new Size(210, 210);
			for (int r = 0; r < 3; r++)
			{
				_matrix[r] = new cell[3];
			}
			for (int r = 0; r < 3; r++)
			{
				for (int c = 0; c < 3; c++)
				{
					_matrix[r][c] = new cell();
					_matrix[r][c].display = ' ';
					_matrix[r][c].color = Color.Black;
				}
			}
		}

		protected override void OnMouseClick(System.Windows.Forms.MouseEventArgs e)
		{
			if (!gameOver)
			{
				int r = (int) Math.Floor((double) e.Y/70);
				int c = (int) Math.Floor((double) e.X/70);
				if (_matrix[r][c].display == ' ')
				{
					_matrix[r][c].display = current;
					current = '0';
					Invalidate();
					moveCounter++;
					gameOver = CheckLines();
					if (!gameOver && moveCounter < 9)
					{
						PlayMove();
						moveCounter++;
					}
				}
			}
			base.OnMouseClick(e);
		}

		protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
		{
			base.OnPaint(e);
			e.Graphics.DrawLine(Pens.Black, 70, 0, 70, 210);
			e.Graphics.DrawLine(Pens.Black, 140, 0, 140, 210);
			e.Graphics.DrawLine(Pens.Black, 0, 70, 210, 70);
			e.Graphics.DrawLine(Pens.Black, 0, 140, 210, 140);

			for (int r = 0; r < 3; r++)
			{
				for (int c = 0; c < 3; c++)
				{
					if (_matrix[r][c].display != ' ')
					{
						e.Graphics.DrawString(_matrix[r][c].display.ToString(), new Font("Times New Roman", 20, FontStyle.Bold),
							new SolidBrush(_matrix[r][c].color), (float) ((c*70) + 25), (float) ((r*70) + 25));
					}
				}
			}
		}


		public bool CheckLines()
		{
			for (int r = 0; r < 3; r++)
			{
				if (_matrix[r][0].display != ' ' && _matrix[r][0].display == _matrix[r][1].display &&
				    _matrix[r][0].display == _matrix[r][2].display)
				{
					_matrix[r][0].color = _matrix[r][1].color = _matrix[r][2].color = Color.Red;
					return true;
				}
			}
			for (int c = 0; c < 3; c++)
			{
				if (_matrix[0][c].display != ' ' && _matrix[0][c].display == _matrix[1][c].display &&
				    _matrix[0][c].display == _matrix[2][c].display)
				{
					_matrix[0][c].color = _matrix[1][c].color = _matrix[2][c].color = Color.Red;
					return true;
				}
			}
			if (_matrix[0][0].display != ' ' && _matrix[0][0].display == _matrix[1][1].display &&
			    _matrix[0][0].display == _matrix[2][2].display)
			{
				_matrix[0][0].color = _matrix[1][1].color = _matrix[2][2].color = Color.Red;
				return true;
			}
			if (_matrix[0][2].display != ' ' && _matrix[0][2].display == _matrix[1][1].display &&
			    _matrix[0][2].display == _matrix[2][0].display)
			{
				_matrix[0][2].color = _matrix[1][1].color = _matrix[2][0].color = Color.Red;
				return true;
			}
			return false;
		}

		public void NewGame(IAI ai)
		{
			_iai = ai;
			gameOver = false;
			for (int r = 0; r < 3; r++)
			{
				for (int c = 0; c < 3; c++)
				{
					_matrix[r][c].display = ' ';
					_matrix[r][c].color = Color.Black;
				}
			}
			first = first == 'X' ? '0' : 'X';
			current = first;
			Invalidate();
			moveCounter = 0;
			if (current == '0')
			{
				PlayMove();
			}
		}

		public void PlayMove()
		{
			var aiChoice = _iai.Step(this);
			var cell = _matrix[aiChoice.R][aiChoice.C];
			if (cell.display == ' ')
			{

				cell.display = '0';
				current = 'X';
				gameOver = CheckLines();
			}
			else
			{
				MessageBox.Show("AI made a move on an existing position!");
				gameOver = true;
			}
			Invalidate();
		}
	}
}