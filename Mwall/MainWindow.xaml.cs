using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Threading;
//using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Windows.Interop;

namespace Mwall
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer refreshTimer = new DispatcherTimer();
        //private List<MColumn> columnList = new List<MColumn>();
        private MScreen mscreen;

        const int SUPPOSED_COLUMN_WIDTH = 25;

        public MainWindow()
        {
            InitializeComponent();

            mscreen = new MScreen(BackCanv);
            //mscreen.GenerateDistributedColumn();
            mscreen.GenerateVerticalColumn();
             
            refreshTimer = new System.Windows.Threading.DispatcherTimer();
            refreshTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            refreshTimer.Interval = new TimeSpan(0, 0, 0, 0, 800);
            refreshTimer.Start();

            new ExitButtonWindows().Show();
        }

        /// <summary>
        /// Set the windows's controls not clickable etc (more than transparent, right?)
        /// </summary>
        /// <param name="e"></param>
        protected override void OnSourceInitialized(EventArgs e)
        {
          base.OnSourceInitialized(e);
          var hwnd = new WindowInteropHelper(this).Handle;
          WindowsServices.SetWindowExTransparent(hwnd);
        }


        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            mscreen.Draw();
        }

    }

    public static class WindowsServices
    {
      const int WS_EX_TRANSPARENT = 0x00000020;
      const int GWL_EXSTYLE = (-20);

      [DllImport("user32.dll")]
      static extern int GetWindowLong(IntPtr hwnd, int index);

      [DllImport("user32.dll")]
      static extern int SetWindowLong(IntPtr hwnd, int index, int newStyle);

      public static void SetWindowExTransparent(IntPtr hwnd)
      {
        var extendedStyle = GetWindowLong(hwnd, GWL_EXSTYLE);
        SetWindowLong(hwnd, GWL_EXSTYLE, extendedStyle | WS_EX_TRANSPARENT);
      }
    }

}
