using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Controls;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Reflection.Metadata;

namespace HorseRace
{
    class Horse
    {
		private Random _random = new();

		public Image HorseImage { get; set; }

		public Image JockeyImage { get; set; }

		public string Name { get; private set; }

		public Color Color { get; private set; }

		private TimeSpan _time;

		public TimeSpan Time 
		{
			get => _time;
			set
			{
				_time = value;
				Finished = true;
			}
		}

		public int Speed { get; private set; }

		public int CurrentPosition { get; set; }

		public int PositionX { get; private set; }

		public int PositionY { get; private set; }

		private int _animationFrame;

		public int AnimationFrame
		{
			get => _animationFrame;
			set => _animationFrame = (value % 8);
		}

		private bool _isBidClosed;

		public bool IsBidClosed
		{
			get => _isBidClosed;
			set
			{
				_isBidClosed = value;

				if (value) CloseTheBet();
			}
		}

        public double Money { get; set; }

		private double _coefficient;

        public double Coefficient 
		{
			get => _coefficient; 

			set 
			{
				_coefficient = value;
				if (_coefficient <= 1) IsBidClosed = true;
			}
		}

		public bool Finished { get; private set; }

		public Horse(string name, Color color, int x, int y)
		{
			Name = name;
			Color = color;
			IsBidClosed = false;

			Speed = _random.Next(3, 7);
			Coefficient = 1.7 - Speed / 10.0;
			AnimationFrame = 0;

			HorseImage = new Image
			{
				Source = new BitmapImage(new Uri("Images/Horses/WithOutBorder_0000.png", UriKind.Relative)),
				Width = 100,
				Tag = "tmp"
			};

			JockeyImage = new Image
			{
				Source = new BitmapImage(new Uri($"Images/HorsesMask/mask_0000.png", UriKind.Relative)),
				Width = 50,
				Tag = "tmp"
			};

			PositionX = x;
			PositionY = y;
		}

		public async Task MoveAsync()
		{
			int distance = await Task.Run(() => (int)(Speed * (_random.Next(4, 10) / 10.0)));
			PositionX += distance;

			AnimationFrame++;

			var fileNumber = _animationFrame.ToString().Length > 1 ? _animationFrame.ToString() : "0" + _animationFrame.ToString();

			HorseImage.Source = new BitmapImage(new Uri($"Images/Horses/WithOutBorder_00{fileNumber}.png", UriKind.Relative));
			JockeyImage.Source = new BitmapImage(new Uri($"Images/HorsesMask/mask_00{fileNumber}.png", UriKind.Relative));

			CalculateCoefficient();
		}

		private void CalculateCoefficient()
		{
			Coefficient = Math.Round(1.7 - Speed / 10.0 + CurrentPosition / 10.0 - (CurrentPosition == 1 ? PositionX / 2500.0 : 0), 2);
		}

		private void CloseTheBet() 
		{

		}
	}
}
