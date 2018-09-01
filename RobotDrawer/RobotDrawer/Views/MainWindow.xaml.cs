using log4net;
using System.Reflection;
using System.Windows;


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
        }

        private void MainView_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            /*
                if (((MainViewModel)(this.DataContext)).Data.IsModified)
                if (!((MainViewModel)(this.DataContext)).PromptSaveBeforeExit())
                {
                    e.Cancel = true;
                    return;
                }
            */
            Log.Info("Closing App");
        }
    }
}
