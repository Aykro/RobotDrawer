using log4net;
using RobotDrawer.Models;
using System;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace RobotDrawer.Views
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public MainWindow()
        {
            InitializeComponent();
            this.Closing += MainView_Closing;
            MyCanvas.EditingMode = InkCanvasEditingMode.Ink;
        }

        private void MainView_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                ABBManager.Instance.RobotManagerThread.Abort();   
            }
            catch(ThreadStateException ex)
            {
                ABBManager.Instance.RobotManagerThread.Resume();
            }
            finally
            {
                Log.Info("Closing App");
            }
        }

    }
}
