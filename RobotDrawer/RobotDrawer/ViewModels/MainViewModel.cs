using MvvmDialogs;
using RobotDrawer.Models;
using RobotDrawer.Models.Exceptions;
using RobotDrawer.Utils;
using RobotDrawer.Views;
using System;
using System.Linq;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;

namespace RobotDrawer.ViewModels
{
    class MainViewModel : ViewModelBase
    {
        #region Parameters       
        private readonly IDialogService DialogService;
        private Timer penSampling = new Timer()
        {
            Interval = 1,
            Enabled = true,
            AutoReset = true
    };
        #region Drawing Properties
        private Func<StylusPoint[], StylusPointCollection> DrawShape;
        private Stroke lastStroke = null;
        private StylusPoint startPoint, endPoint;
        private StylusPointCollection newStrokes;
        public DrawingAttributes DefaultDrawingAttributes
        {
            get
            {
                return _defaultDrawingAttributes;
            }
            set
            {
                _defaultDrawingAttributes = value;
                OnPropertyChanged();
            }
        }
        private DrawingAttributes _defaultDrawingAttributes = new DrawingAttributes();
        private InkCanvasEditingMode _editingMode;
        public InkCanvasEditingMode EditingMode
        {
            get
            {
                return _editingMode;
            }
            set
            {
                _editingMode = value;
            }
        }
        public StrokeCollection Strokes
        {
            get
            {
                return _strokes;
            }
        }
        private StrokeCollection _strokes;

        #endregion

        #region ButtonProperties
        public bool BlackRadiobuttonChecked
        {
            get
            {
                return _blackRadiobuttonChecked;
            }
            set
            {
                if (_blackRadiobuttonChecked != value)
                {
                    _blackRadiobuttonChecked = value;
                    ChangePencilColour();
                    OnPropertyChanged();
                }
            }
        }
        private bool _blackRadiobuttonChecked;
        public bool RedRadiobuttonChecked
        {
            get
            {
                return _redRadiobuttonChecked;
            }
            set
            {
                if (_redRadiobuttonChecked != value)
                {
                    _redRadiobuttonChecked = value;
                    ChangePencilColour();
                    OnPropertyChanged();
                }
            }
        }
        private bool _redRadiobuttonChecked;
        public bool GreenRadiobuttonChecked
        {
            get
            {
                return _greenRadiobuttonChecked;
            }
            set
            {
                if (_greenRadiobuttonChecked != value)
                {
                    _greenRadiobuttonChecked = value;
                    ChangePencilColour();
                    OnPropertyChanged();
                }
            }
        }
        private bool _greenRadiobuttonChecked;

        private bool _lineButtonChecked;
        public bool LineButtonChecked
        {
            get { return _lineButtonChecked; }
            set
            {
                if (_lineButtonChecked != value)
                {
                    _lineButtonChecked = value;
                    OnPropertyChanged();
                }
            }
        }
        private bool _rectangleButtonChecked;
        public bool RectangleButtonChecked
        {
            get { return _rectangleButtonChecked; }
            set
            {
                if (_rectangleButtonChecked != value)
                {
                    _rectangleButtonChecked = value;
                    ChangeEditingMode();
                    OnPropertyChanged();
                }
            }
        }
        private bool _circleButtonChecked;
        public bool CircleButtonChecked
        {
            get { return _circleButtonChecked; }
            set
            {
                if (_circleButtonChecked != value)
                {
                    _circleButtonChecked = value;
                    OnPropertyChanged();
                }
            }
        }
        private bool _eraserButtonChecked;
        public bool EraserButtonChecked
        {
            get { return _eraserButtonChecked; }
            set
            {
                if (_eraserButtonChecked != value)
                {
                    _eraserButtonChecked = value;
                    OnPropertyChanged();
                    ChangeEditingMode();
                }
            }
        }
        private bool _penButtonChecked;
        public bool PenButtonChecked
        {
            get { return _penButtonChecked; }
            set
            {
                if (_penButtonChecked != value)
                {
                    _penButtonChecked = value;
                   // penSampling.Elapsed += new ElapsedEventHandler(OnSampleEvent);
                    ChangeEditingMode();
                    OnPropertyChanged();
                }
            }
        }
        private bool _connectButtonChecked;
        public bool ConnectButtonChecked
        {
            get { return _connectButtonChecked; }
            set
            {
                if (_connectButtonChecked != value)
                {                
                    Connect();
                    if(IsRobotConnected == true)
                    {
                        _connectButtonChecked = value;
                        OnPropertyChanged();
                    }                  
                }
            }
        }
        private bool _disconnectButtonChecked;
        public bool DisconnectButtonChecked
        {
            get { return _disconnectButtonChecked; }
            set
            {
                if (_disconnectButtonChecked != value)
                {
                    
                    Disconnect();
                    if (IsRobotConnected == false)
                    {
                        _disconnectButtonChecked = value;
                        OnPropertyChanged();
                    }                  
                }
            }
        }
        private bool _isRobotConnected;
        public bool IsRobotConnected
        {
            get { return _isRobotConnected; }
            set
            {
                _isRobotConnected = value;
            }
        }

        #endregion

        private bool AlwaysTrue() { return true; }
        private bool CanExecuteShapes() {
            if (EditingMode != InkCanvasEditingMode.None)
                return false;
            else return true;
           }

        /// <summary>
        /// Title of the application, as displayed in the top bar of the window
        /// </summary>
        public string Title
        {
            get { return "RobotDrawer"; }
        }

        #endregion

        #region Constructors
        public MainViewModel()
        {
            // DialogService is used to handle dialogs
            this.DialogService = new MvvmDialogs.DialogService();
            _strokes = new StrokeCollection();
            //_strokes.StrokesChanged += OnStrokesChange;
        }

        #endregion

        #region Methods
        private void MouseDownCommandExecuted(MouseEventArgs e)
        {
            if(EditingMode==InkCanvasEditingMode.None)
            {
                startPoint = new StylusPoint(e.GetPosition((InkCanvas)e.Source).X, 
                    e.GetPosition((InkCanvas)e.Source).Y);
                lastStroke = new Stroke(new StylusPointCollection() { startPoint });
                lastStroke.DrawingAttributes.Color = DefaultDrawingAttributes.Color;
                Strokes.Add(lastStroke);
            }
            else
            {

            }
        }
        private void MouseMoveCommandExecuted(MouseEventArgs e)
        {
            if (EditingMode == InkCanvasEditingMode.None)
            {
                newStrokes = null;
                endPoint = new StylusPoint(e.GetPosition((InkCanvas)e.Source).X, e.GetPosition((InkCanvas)e.Source).Y);
                if (lastStroke != null)
                {
                    var _points = new StylusPoint[2] { startPoint, endPoint };
                    if (LineButtonChecked)
                    {
                        DrawShape = Shapes.DrawLine;
                        newStrokes = DrawShape(_points);
                    }
                    if (RectangleButtonChecked)
                    {
                        DrawShape = Shapes.DrawRectangle;
                        newStrokes = DrawShape(_points);
                    }
                    if (CircleButtonChecked)
                    {
                        DrawShape = Shapes.DrawCircle;
                        newStrokes = DrawShape(_points);
                    }
                    if (PenButtonChecked)
                    {
                        //    newStrokes = new StylusPointCollection();
                        //    if(e.LeftButton == MouseButtonState.Pressed)
                        //    {
                        //       // penSampling.Start();
                        //       // penSampling.
                        //        //newStrokes.Add(new StylusPoint(endPoint.X, endPoint.Y));
                        //    }
                        //    else
                        //    {
                        //        penSampling.Stop();
                        //    }
                    }
                }
                if (newStrokes != null) lastStroke.StylusPoints = newStrokes;
            }
        }
        private void OnSampleEvent(object source, ElapsedEventArgs e)
        {
            if (newStrokes == null) newStrokes = new StylusPointCollection();
            newStrokes.Add(new StylusPoint(endPoint.X, endPoint.Y));
        }
        private void MouseUpCommandExecuted()
        {
            if (lastStroke != null)
            {
                var CanvasSize = App.GetCanvasSize();
                var robotCoordinates = CoordinateScaler.ToRobotCoordinates(Strokes.Last(), CanvasSize[0], CanvasSize[1]);
                var robotLines = Models.PointConverter.ToLine(robotCoordinates);
                //ABBManager.Instance.ShapesPending.Enqueue(new ObjectToDraw(
                //    robotLines, DefaultDrawingAttributes.Color));
                lastStroke = null;
            }
        }
        private void ChangeEditingMode()
        {
            if (EraserButtonChecked)
            {
                EditingMode = InkCanvasEditingMode.EraseByPoint;
            }
            else if (PenButtonChecked)
            {
                EditingMode = InkCanvasEditingMode.Ink;
            }
            else
            {
                EditingMode = InkCanvasEditingMode.None;
            }
            OnPropertyChanged("EditingMode");
        }
        private void ChangePencilColour()
        {
            if (BlackRadiobuttonChecked)
            {
                DefaultDrawingAttributes.Color = Colors.Black;
            }
            if (RedRadiobuttonChecked)
            {
                DefaultDrawingAttributes.Color = Colors.Red;
            }
            if (GreenRadiobuttonChecked)
            {
                DefaultDrawingAttributes.Color = Colors.Green;
            }
            OnPropertyChanged("DefaultDrawingAttributes");
        }
        private void ClearCanvas()
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                Strokes.Clear();
            }));
        }
        private void Connect()
        {
            try
            {
                if (!IsRobotConnected)
                {
                    ABBManager.Instance.Connect();
                    IsRobotConnected = true;
                    //OnPropertyChanged("IsEnabled");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Connection error! Please contact support.");
                IsRobotConnected = false;
            }
        }
        private void Disconnect()
        {
            if (IsRobotConnected)
            {
                ABBManager.Instance.Disconnect();
                IsRobotConnected = false;
                OnPropertyChanged("IsEnabled");
            }
        }
        //private void ReadConfigFile()
        //{
        //    var settings = new OpenFileDialogSettings
        //    {
        //        Title = "Open",
        //        Filter = "(.txt)|*.txt",
        //        CheckFileExists = false
        //    };

        //    bool? success = DialogService.ShowOpenFileDialog(this, settings);
        //    if (success == true)
        //    {
        //        string[] _controllerConfigLines = File.ReadAllLines(settings.FileName);
        //        Log.Info("Opening file: " + settings.FileName);
        //    }
        //}
        #endregion

        #region Commands
        public RelayCommand<object> SampleCmdWithArgument { get { return new RelayCommand<object>(OnSampleCmdWithArgument); } }

        public ICommand MouseUpCommand { get { return new RelayCommand(MouseUpCommandExecuted); } }
        public ICommand MouseDownCommand { get { return new RelayCommand<MouseEventArgs>(MouseDownCommandExecuted); } }
        public ICommand MouseMoveCommand { get { return new RelayCommand<MouseEventArgs>(MouseMoveCommandExecuted); } }
        public ICommand ClearCanvasCommand { get { return new RelayCommand(ClearCanvas, AlwaysTrue); } }
        public ICommand ShowAboutDialogCmd { get { return new RelayCommand(OnShowAboutDialog, AlwaysTrue); } }
        public ICommand ExitCmd { get { return new RelayCommand(OnExitApp, AlwaysTrue); } }

        private void OnSampleCmdWithArgument(object obj)
        {
            // TODO
        }
        private void OnShowAboutDialog()
        {
            Log.Info("Opening About dialog");
            AboutViewModel dialog = new AboutViewModel();
            var result = DialogService.ShowDialog<About>(this, dialog);
        }
        private void OnExitApp()
        {
            Application.Current.MainWindow.Close();
        }
        #endregion

        #region Events

        #endregion
    }
}
