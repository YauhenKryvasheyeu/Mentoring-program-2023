using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GameOfLife
{
    class Cell
    {
        private int _nextGenAge;

        private bool _nextGenIsAlive;

        public int PositionX { get; set; }
        public int PositionY { get; set; }
        public int Age { get; set; }

        public bool IsAlive { get; set; }

        public Ellipse Ellipse { get; }

        public Cell(int row, int column, int age, bool alive)
        {
            PositionX = row * 5;
            PositionY = column * 5;
            Age = _nextGenAge = age;
            IsAlive = _nextGenIsAlive = alive;
            Ellipse = new Ellipse();
            SetupVisuals();
            UpdateCellGraphic();
        }

        private void SetupVisuals()
        {
            Ellipse.Width = Ellipse.Height = 5;
            double left = PositionX;
            double top = PositionY;
            Ellipse.Margin = new Thickness(left, top, 0, 0);
            Ellipse.Fill = Brushes.Gray;
            Ellipse.MouseMove += MouseMove;
            Ellipse.MouseLeftButtonDown += MouseMove;
        }
        private void UpdateNexGeneration(bool alive = false, int age = 0)
        {
            _nextGenAge = age;
            _nextGenIsAlive = alive;
        }

        void MouseMove(object sender, MouseEventArgs e)
        {
            var cellVisual = sender as Ellipse;

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (!IsAlive)
                {
                    IsAlive = true;
                    Age = 0;
                    cellVisual.Fill = Brushes.White;
                }
            }
        }

        private void UpdateCellGraphic()
        {
            Ellipse.Fill = IsAlive? (Age < 2 ? Brushes.White : Brushes.DarkGray) : Brushes.Gray;
        }

        public void UpdateNextGeneration(int count)
        {
            if (IsAlive && count < 2)
            {
                UpdateNexGeneration();
            }

            if (IsAlive && (count == 2 || count == 3))
            {
                Age++;
                UpdateNexGeneration(true, Age);
            }

            if (IsAlive && count > 3)
            {
                UpdateNexGeneration();
            }

            if (!IsAlive && count == 3)
            {
                UpdateNexGeneration(true);
            }
        }

        public void UpdateToNextGeneration()
        {
            IsAlive = _nextGenIsAlive;
            Age = _nextGenAge;
            UpdateCellGraphic();
        }

        public void Clear()
        {
            Age = _nextGenAge = 0;
            IsAlive = _nextGenIsAlive = false;
            Ellipse.Fill = Brushes.Gray;
        }
    }
}