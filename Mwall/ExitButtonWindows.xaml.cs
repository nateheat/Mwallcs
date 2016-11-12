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
using System.Windows.Shapes;

namespace Mwall
{
    /// <summary>
    /// Interaction logic for ExitButtonWindows.xaml
    /// </summary>
    public partial class ExitButtonWindows : Window
    {
        private bool bMLeftPressed = false;
        private bool bDragging = false;
        private Point pointMDown = new Point();

        public ExitButtonWindows()
        {
            InitializeComponent();

            this.Top = 30;
            double scrWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            this.Left = scrWidth - this.Width;
        }

        private void ExitButtonWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            bMLeftPressed = true;
            pointMDown.X = e.GetPosition(this).X;
            pointMDown.Y = e.GetPosition(this).Y;
        }

        private void ExitButtonWindow_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if(!bDragging) Application.Current.Shutdown();
            bMLeftPressed = false;
            bDragging = false;

        }

        private void ExitButtonWindow_MouseMove(object sender, MouseEventArgs e)
        {
            if (!bMLeftPressed || MouseButtonState.Pressed != e.LeftButton) return;
            if (Math.Abs(e.GetPosition(this).X - pointMDown.X) + Math.Abs(e.GetPosition(this).Y - pointMDown.Y) > 4)
            {
                bDragging = true;
                DragMove();
            }

        }
    }
}
