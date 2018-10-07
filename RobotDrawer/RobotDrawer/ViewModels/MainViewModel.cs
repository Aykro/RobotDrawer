using MvvmDialogs;
using RobotDrawer.Models;
using RobotDrawer.Utils;
using RobotDrawer.Views;
using System;
using System.Linq;
using System.Threading;
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
        private InkCanvasEditingMode _editingMode = InkCanvasEditingMode.Ink;
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
        private bool _isInkCanvasEnabled;
        public bool IsInkCanvasEnabled
        {
            get
            {
                return _isInkCanvasEnabled;
            }
            set
            {
                _isInkCanvasEnabled = value;
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
                    ChangePencilColour();
                    OnPropertyChanged();
                    ChangeEditingMode();
                }
            }
        }
        private bool _eraseAllButtonChecked;
        public bool EraseAllButtonChecked
        {
            get { return _eraseAllButtonChecked; }
            set
            {
                if(_eraseAllButtonChecked != value)
                {
                    _eraseAllButtonChecked = value;
                    ClearCanvas();
                    OnPropertyChanged();
                }
            }
        }
        private bool _penButtonChecked = true;
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
                    if (IsRobotConnected == false && value == true) Connect();

                    _connectButtonChecked = value;
                    OnPropertyChanged();
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
                    if(IsRobotConnected == true && value == true) Disconnect();

                    _disconnectButtonChecked = value;
                    OnPropertyChanged();
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
            else if(EditingMode == InkCanvasEditingMode.Ink & DefaultDrawingAttributes.Color == Colors.White)
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
                    }
                    if (RectangleButtonChecked)
                    {
                        DrawShape = Shapes.DrawRectangle;
                    }
                    if (CircleButtonChecked)
                    {
                        DrawShape = Shapes.DrawCircle;                   
                    }

                    newStrokes = DrawShape(_points);
                }
                if (newStrokes != null) lastStroke.StylusPoints = newStrokes;
            }
        }
        private void MouseUpCommandExecuted()
        {
            lastStroke = null;
            var objectToDraw = PrepareDataToSend(Strokes.Last());
            ABBManager.Instance.ShapesPending.Enqueue(objectToDraw);
        }
        private ObjectToDraw PrepareDataToSend(Stroke Last)
        {
            var CanvasSize = App.GetCanvasSize();
            var filteredPoints = Shapes.FilterPoints(Last);
            var robotCoordinates = CoordinateScaler.ToRobotCoordinates(filteredPoints, CanvasSize[0], CanvasSize[1]);
            var robotLines = Models.PointConverter.ToLine(robotCoordinates);
            return new ObjectToDraw(robotLines, DefaultDrawingAttributes.Color);
        }
        private void ChangeEditingMode()
        {
            if (EraserButtonChecked || PenButtonChecked)
            {
                EditingMode = InkCanvasEditingMode.Ink;
            }
            //else if (PenButtonChecked)
            //{
            //    EditingMode = InkCanvasEditingMode.Ink;
            //}
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
            if(EraserButtonChecked)
            {
                DefaultDrawingAttributes.Color = Colors.White;
            }
            OnPropertyChanged("DefaultDrawingAttributes");
        }
        private void ClearCanvas()
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                ABBManager.Instance.ShapesPending.Enqueue(new ObjectToDraw(EraseAll: true, Colour: Colors.White));
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
                    if(ABBManager.Instance.RobotManagerThread.ThreadState == ThreadState.Suspended)
                        ABBManager.Instance.RobotManagerThread.Resume();
                    IsRobotConnected = true;
                    IsInkCanvasEnabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Connection error! Please contact support.");
                IsRobotConnected = false;
                IsInkCanvasEnabled = false;
            }
            finally
            {
                OnPropertyChanged("IsInkCanvasEnabled");
                OnPropertyChanged("IsRobotConnected");
            }
        }
        private void Disconnect()
        {
            if (IsRobotConnected)
            {
                ABBManager.Instance.Disconnect();
                IsRobotConnected = false;
                OnPropertyChanged("IsRobotConnected");
                IsInkCanvasEnabled = false;
                OnPropertyChanged("IsInkCanvasEnabled");
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
