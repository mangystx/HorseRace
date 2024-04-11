using System.Collections.ObjectModel;
using System.Diagnostics;
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
using System.Windows.Threading;

namespace HorseRace
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private ObservableCollection<Horse> horses = new();
		private Stopwatch raceStopwatch = new();
		private DispatcherTimer animationTimer = new();

		private List<string> _horsesNames = ["Lucky", "Ranger", "Willow", "Tucker", "Sabrina"];

		private List<string> _activeHorsesNames = ["Lucky"];
		private int _currentActiveHorseIndex = 0;
		public string CurrentActiveHorse => _activeHorsesNames[_currentActiveHorseIndex];

		private List<int> _bets = [10, 20, 50, 100, 200, 300, 500, 1000];
		private int _currentBetIndex = 0;
		public int CurrentBet => _bets[_currentBetIndex];

		private List<Color> _jockeyColors = [Colors.Red, Colors.Blue, Colors.Green, Colors.White, Colors.Orange];


		private int _finishedCount = 0;

		private double _balance = 1000;
		public double Balance
		{
			get { return _balance; }
			set { _balance = value; }
		}


		public MainWindow()
		{
			InitializeComponent();
			InitializeAnimationTimer();
		}

		private void InitializeHorses()
		{
			var offsetY = 110;

			var racetrackHeight = 120;

			var numberOfHorses = int.Parse((numberOfHorsesComboBox.SelectedItem as ComboBoxItem).Content.ToString());

			var space = racetrackHeight / (numberOfHorses - 1);

			for (var i = 0; i < numberOfHorses; i++)
			{
				var horse = new Horse(_horsesNames[i], _jockeyColors[i], 20, offsetY);

				horses.Add(horse);

				offsetY += space;

				var jockeyRectangle = new Rectangle
				{
					Width = horse.JockeyImage.Width,
					Height = horse.JockeyImage.Height,
					Fill = new SolidColorBrush(horse.Color),
					OpacityMask = new ImageBrush
					{
						ImageSource = horse.JockeyImage.Source
					}
				};

				raceTrack.Children.Add(jockeyRectangle);

				raceTrack.Children.Add(horse.HorseImage);
				Canvas.SetLeft(horse.HorseImage, horse.PositionX);
				Canvas.SetTop(horse.HorseImage, horse.PositionY);
				Canvas.SetLeft(jockeyRectangle, horse.PositionX);
				Canvas.SetTop(jockeyRectangle, horse.PositionY - 30);
			}
		}

		private void InitializeAnimationTimer()
		{
			animationTimer.Interval = TimeSpan.FromMilliseconds(100);
			animationTimer.Tick += async (sender, e) => await UpdateHorsePositionsAsync();
		}

		private async Task UpdateHorsePositionsAsync()
		{
			var distanceTasks = horses.Select(horse => horse.MoveAsync()).ToList();

			 await Task.WhenAll(distanceTasks);

			foreach (var horse in horses)
			{
				Canvas.SetLeft(horse.HorseImage, horse.PositionX);
				Canvas.SetTop(horse.HorseImage, horse.PositionY);
				Canvas.SetLeft(horse.JockeyImage, horse.PositionX);
				Canvas.SetTop(horse.JockeyImage, horse.PositionY - 30);
				

				if (horse.PositionX >= 600 && !horse.Finished)
				{
					horse.Time = raceStopwatch.Elapsed;
					_finishedCount++;
					if (_finishedCount >= horses.Count - 1)
					{
						EndSimulation();
						break;
					}
				}
			}

			horses = new ObservableCollection<Horse>(horses.OrderByDescending(horse => horse.PositionX));
			HorsesDataGrid.ItemsSource = horses;

			for (var i = 0; i < horses.Count; i++) 
			{
				horses[i].CurrentPosition = i + 1;
			}
		}

		private void Play_Button_Click(object sender, RoutedEventArgs e)
		{
			InitializeHorses();
			animationTimer.Start();
			raceStopwatch.Start();
			playPanel.Visibility = Visibility.Collapsed;
			_finishedCount = 0;
		}

		private void EndSimulation()
		{
			animationTimer.Stop();
			raceStopwatch.Stop();

			playPanel.Visibility = Visibility.Visible;

			horses.Clear();
			for (int i = raceTrack.Children.Count - 1; i >= 0; i--)
			{
				var child = raceTrack.Children[i];
				if (child is FrameworkElement element && element.Tag as string == "tmp")
				{
					raceTrack.Children.RemoveAt(i);
				}
			}
		}

		private void Previous_Bet_Btn_Click(object sender, RoutedEventArgs e)
		{
			if (_currentBetIndex == 0) _currentBetIndex = _bets.Count - 1;
			else _currentBetIndex--;
		}

		private void Next_Bet_Btn_Click(object sender, RoutedEventArgs e)
		{
			if (_currentBetIndex == _bets.Count - 1) _currentBetIndex = 0;
			else _currentBetIndex++;
		}

		private void Previous_Horse_Btn_Click(object sender, RoutedEventArgs e)
		{

		}

		private void Next_Horse_Btn_Click(object sender, RoutedEventArgs e)
		{

		}

		private void BetBtn_Click(object sender, RoutedEventArgs e)
		{

		}
	}
}