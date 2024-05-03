using System;
using System.Diagnostics;
using System.Drawing;

namespace VoronoiDiagram
{
	public partial class Form1 : Form
	{
		private readonly List<Point> points;
		private readonly List<Color> colors;
		private readonly Random random;
		private readonly Bitmap bitmap;
		private bool isMultiThreadEnabled;

		public Form1()
		{
			InitializeComponent();

			points = [];
			colors = [];
			random = new();
			bitmap = new(ClientSize.Width, ClientSize.Height);
			isMultiThreadEnabled = false;
		}

		private static double Distance(Point point1, Point point2) => Math.Sqrt(Math.Pow(point1.X - point2.X, 2) + Math.Pow(point1.Y - point2.Y, 2));

		private Point FindClosestPoint(Point point)
		{
			var closestPoint = points[0];

			var minDistance = Distance(point, closestPoint);

			for (var i = 1; i < points.Count; i++)
			{
				var distance = Distance(point, points[i]);

				if (distance < minDistance)
				{
					minDistance = distance;
					closestPoint = points[i];
				}
			}

			return closestPoint;
		}

		private void DrawPoints(Graphics graphics)
		{
			foreach (var point in points)
			{
				graphics.FillEllipse(Brushes.Black, point.X, point.Y, 4, 4);
			}
		}

		private void DrawSinglethread()
		{
			using var graphics = Graphics.FromImage(bitmap);

			for (var x = 0; x < bitmap.Width; x++)
			{
				for (var y = 0; y < bitmap.Height; y++)
				{
					Point closestPoint = FindClosestPoint(new Point(x, y));

					var index = points.IndexOf(closestPoint);

					graphics.FillRectangle(new SolidBrush(colors[index]), x, y, 1, 1);
				}
			}
		}

		private void DrawMultithread()
		{
			using var graphics = Graphics.FromImage(bitmap);

			var segmentWidth = (int)Math.Ceiling((double)bitmap.Width / Environment.ProcessorCount);
			var segmentHeight = bitmap.Height;

			List<Task> tasks = [];

			for (var t = 0; t < Environment.ProcessorCount; t++)
			{
				var startX = t * segmentWidth;
				var endX = Math.Min((t + 1) * segmentWidth, bitmap.Width);

				tasks.Add(Task.Run(() =>
				{
					for (var x = startX; x < endX; x++)
					{
						for (var y = 0; y < segmentHeight; y++)
						{
							var closestPoint = FindClosestPoint(new Point(x, y));

							var index = points.IndexOf(closestPoint);

							lock (graphics)
							{
								graphics.FillRectangle(new SolidBrush(colors[index]), x, y, 1, 1);
							}
						}
					}
				}));
			}

			Task.WaitAll([.. tasks]);
		}

		private void Clear(object sender, EventArgs e)
		{
			using var graphics = Graphics.FromImage(bitmap);

			points.Clear();
			colors.Clear();
			graphics.Clear(Color.White);

			DrawPoints(graphics);

			Invalidate();
		}

		private void Form1_MouseClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				Invalidate();

				var newPoint = e.Location;

				for (var i = 0; i < points.Count; i++)
				{
					if (Distance(newPoint, points[i]) <= 4)
					{
						points.RemoveAt(i);
						colors.RemoveAt(i);

						return;
					}
				}

				points.Add(newPoint);

				var newColor = Color.FromArgb(random.Next(256), random.Next(256), random.Next(256));

				while (colors.Any(c => c.R == newColor.R && c.G == newColor.G && c.B == newColor.B))
				{
					newColor = Color.FromArgb(random.Next(256), random.Next(256), random.Next(256));
				}

				colors.Add(newColor);

				if (points.Count >= 2)
				{
					if (isMultiThreadEnabled) DrawMultithread();
					else DrawSinglethread();

					Invalidate();
				}
			}
		}

		private void Form1_Paint(object sender, PaintEventArgs e)
		{
			var graphics = e.Graphics;

			graphics.DrawImage(bitmap, 0, 0);

			DrawPoints(graphics);
		}

		private void Multiåhread_CheckedChanged(object sender, EventArgs e) => isMultiThreadEnabled = Multiåhread.Checked;

		private void RandomBtn_Click(object sender, EventArgs e)
		{
			Clear(sender, e);

			int numberPoints = 50;

			for (var i = 0; i < numberPoints; i++)
			{
				points.Add(new Point(random.Next(ClientSize.Width), random.Next(ClientSize.Height)));
				colors.Add(Color.FromArgb(random.Next(256), random.Next(256), random.Next(256)));
			}

			if (isMultiThreadEnabled) DrawMultithread();
			else DrawSinglethread();

			Invalidate();
		}

		private void ClearBtn_Click(object sender, EventArgs e) => Clear(sender, e);
	}
}
