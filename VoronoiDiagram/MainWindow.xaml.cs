using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace VoronoiDiagram
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private readonly List<Point> _points = [];
		private readonly Dictionary<Point, Color> _color = [];
		private readonly Random _random = new();
		private bool _singleThreadMode = true;

		public MainWindow()
		{
			InitializeComponent();
			InitializeUI();
		}

		private void InitializeUI()
		{
			DiagramCanvas.Background = Brushes.White;
			DiagramCanvas.MouseDown += Canvas_MouseDown;
		}

		private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.LeftButton == MouseButtonState.Pressed)
			{
				Point clickedPoint = e.GetPosition((UIElement)sender);
				bool pointRemoved = false;

				for (int i = 0; i < _points.Count; i++)
				{
					if (Distance(clickedPoint, _points[i]) <= 5)
					{	
						_color.Remove(_points[i]);
						_points.RemoveAt(i);
						pointRemoved = true;
						break;
					}
				}

				if (!pointRemoved)
				{
					_points.Add(clickedPoint);
					_color[clickedPoint] = GenerateRandomColor();
				}

				DrawVoronoiDiagram();
			}
		}

		private void ClearCanvas()
		{
			_points.Clear();
			_color.Clear();
			DrawVoronoiDiagram();
		}

		private void DrawVoronoiDiagram()
		{
			DiagramCanvas.Children.Clear();

			if (_points.Count >= 2)
			{
				if (_singleThreadMode) DrawSingleThread();
				else DrawMultiThread();
			}

			DrawVertices();
		}

		private void DrawVertices()
		{
			DiagramCanvas.Children.Clear();

			foreach (Point point in _points)
			{
				Ellipse ellipse = new()
				{
					Width = 8,
					Height = 8,
					Fill = Brushes.Black
				};
				Canvas.SetLeft(ellipse, point.X - 4);
				Canvas.SetTop(ellipse, point.Y - 4);
				DiagramCanvas.Children.Add(ellipse);
			}
		}

		private void DrawSingleThread()
		{
			for (int x = 0; x < (int)DiagramCanvas.ActualWidth; x++)
			{
				for (int y = 0; y < (int)DiagramCanvas.ActualHeight; y++)
				{
					Point closestPoint = FindClosestPoint(new Point(x, y));

					Rectangle rect = new()
					{
						Width = 1,
						Height = 1,
						Fill = new SolidColorBrush(_color[closestPoint])
					};

					Canvas.SetLeft(rect, x);
					Canvas.SetTop(rect, y);
					DiagramCanvas.Children.Add(rect);
				}
			}
		}

		private void DrawMultiThread()
		{
			const int numberThreads = 4;
			List<Task> tasks = [];

			for (int t = 0; t < numberThreads; t++)
			{
				int threadId = t;

				Task task = Task.Run(() =>
				{
					for (int x = threadId; x < (int)DiagramCanvas.ActualWidth; x += numberThreads)
					{
						for (int y = 0; y < (int)DiagramCanvas.ActualHeight; y++)
						{
							Point closestPoint = FindClosestPoint(new Point(x, y));

							Application.Current.Dispatcher.Invoke(() =>
							{
								Rectangle rect = new();
								rect.Width = 1;
								rect.Height = 1;
								rect.Fill = new SolidColorBrush(_color[closestPoint]);
								Canvas.SetLeft(rect, x);
								Canvas.SetTop(rect, y);
								DiagramCanvas.Children.Add(rect);
							});
						}
					}
				});
				tasks.Add(task);
			}

			Task.WaitAll(tasks.ToArray());
		}

		private Point FindClosestPoint(Point point)
		{
			Point closestPoint = _points[0];
			double minDistance = Distance(point, closestPoint);

			for (var i = 1; i < _points.Count; i++)
			{
				double distance = Distance(point, _points[i]);

				if (distance < minDistance)
				{
					minDistance = distance;
					closestPoint = _points[i];
				}
			}

			return closestPoint;
		}

		private double Distance(Point p1, Point p2)
		{
			double dx = p1.X - p2.X;
			double dy = p1.Y - p2.Y;

			return Math.Sqrt(dx * dx + dy * dy);
		}

		private Color GenerateRandomColor()
		{
			while (true)
			{
				Color color = Color.FromArgb((byte)_random.Next(256), (byte)_random.Next(256), (byte)_random.Next(256), (byte)_random.Next(256));

				if (!_color.Values.Contains(color))
				{
					return color;
				}
			}
		}

		private void MultiThread_Checked(object sender, RoutedEventArgs e)
		{
			_singleThreadMode = false;
			DrawVoronoiDiagram();
		}

		private void ClearBtnClick(object sender, RoutedEventArgs e)
		{
			ClearCanvas();
		}

		private void RandomBtnClick(object sender, RoutedEventArgs e)
		{

		}
	}
}