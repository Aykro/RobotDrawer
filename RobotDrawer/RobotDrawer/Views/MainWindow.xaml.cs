using log4net;
using System.Reflection;
using System.Windows;
using System.Windows.Ink;
using System.Windows.Input;

namespace RobotDrawer.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private Point startPoint, endPoint;
        private Stroke temp;
        public MainWindow()
        {
            InitializeComponent();
            this.Closing += MainView_Closing;
            MyCanvas.EditingMode = System.Windows.Controls.InkCanvasEditingMode.None;
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
        
        //private void MyCanvas_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        //{
        //    if (e.LeftButton == MouseButtonState.Pressed)
        //    {
        //        endPoint = e.GetPosition(this);
        //        DrawLine();
        //    }
        //}

        //private void MyCanvas_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    startPoint = e.GetPosition(this);
        //    temp = null;
        //}

        //private void DrawLine()
        //{
        //    StylusPointCollection newLineStrokes = new StylusPointCollection()
        //    {
        //        new StylusPoint(startPoint.X, startPoint.Y),
        //        new StylusPoint(endPoint.X, endPoint.Y)
        //    };    
        //    if (temp == null)
        //    {
        //        temp = new Stroke(newLineStrokes);
        //        MyCanvas.Strokes.Add(temp);
        //    }
        //    else
        //    {
        //        temp.StylusPoints = newLineStrokes;
        //    }
        //}
    }
}
