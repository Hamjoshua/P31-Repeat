using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace prog
{
    /// <summary>
    /// Логика взаимодействия для Task1.xaml
    /// </summary>
    public partial class Task1 : Page
    {
        public double Size
        {
            get
            {
                return Convert.ToDouble(SizeBox.Value);
            }
        }
        public double Velocity
        {
            get
            {
                return Convert.ToDouble(SpeedBox.Value);
            }
        }

        public double RepairTime
        {
            get
            {
                return Convert.ToDouble(TimeBox.Value);
            }
}

private string _wholeTimeFormatText = "Общее время движения: {0} с";
        private string _middleSpeedFormatText = "Средняя скорость автомобиля: {0} м/с";

        public Task1()
        {
            InitializeComponent();
            UpdateTextBlocks(0, 0);
        }

        public double CalculateAllTime()
        {
            double actionTime = Size / Velocity;
            double wholeTime = actionTime + RepairTime;

            return wholeTime;
        }

        public double CalculateMiddleVelocity(double wholeTime)
        {
            double middleSpeed = Size / wholeTime;

            return middleSpeed;
        }

        private void CalculateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                double wholeTime = CalculateAllTime();
                double middleSpeed = CalculateMiddleVelocity(wholeTime);

                UpdateTextBlocks(wholeTime, middleSpeed);
            }
            catch (DivideByZeroException)
            {
                MessageBox.Show("Не все поля заполнены!");
            }
        }

        private void UpdateTextBlocks(double wholeTime, double middleSpeed)
        {
            WholeTimeTextBlock.Text = String.Format(_wholeTimeFormatText, wholeTime);
            MiddleSpeedTextBlock.Text = String.Format(_middleSpeedFormatText, middleSpeed);
        }
    }
}
