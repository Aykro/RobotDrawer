using MvvmDialogs;
using log4net;
using MvvmDialogs.FrameworkDialogs.OpenFile;
using MvvmDialogs.FrameworkDialogs.SaveFile;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Windows.Input;
using System.Xml.Linq;
using RobotDrawer.Views;
using RobotDrawer.Utils;
using System.Windows.Ink;
using System.Windows.Media;
using System.Collections.Specialized;
using System.Windows;

namespace RobotDrawer.ViewModels
{
    class MainViewModel : ViewModelBase
    {
        #region Parameters
        private readonly IDialogService DialogService;

        /// <summary>
        /// Title of the application, as displayed in the top bar of the window
        /// </summary>
        public string Title
        {
            get { return "RobotDrawer"; }
        }

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
                    PreprarePencilColour();
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
                    PreprarePencilColour();
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
                    PreprarePencilColour();
                    OnPropertyChanged();
                }
            }
        }
        private bool _greenRadiobuttonChecked;

        private void PreprarePencilColour()
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

        public StrokeCollection Strokes
        {
            get
            {
                return _strokes;
            }
        }
        private StrokeCollection _strokes;
        #endregion

        #region Constructors
        public MainViewModel()
        {
            // DialogService is used to handle dialogs
            this.DialogService = new MvvmDialogs.DialogService();
            _strokes = new StrokeCollection();
            _strokes.StrokesChanged += OnStrokesChange;
        }

        #endregion

        #region Methods
        private void OnStrokesChange(object sender, StrokeCollectionChangedEventArgs e)
        {

        }
        #endregion

        #region Commands
        public RelayCommand<object> SampleCmdWithArgument { get { return new RelayCommand<object>(OnSampleCmdWithArgument); } }

        public ICommand ClearCanvaCommand { get { return new RelayCommand(ClearCanva, AlwaysTrue); } }

        private void ClearCanva()
        {
            Application.Current.Dispatcher.Invoke(new Action(() => {
                Strokes.Clear();
            }));
        }

        public ICommand SaveAsCmd { get { return new RelayCommand(OnSaveAsTest, AlwaysFalse); } }
        public ICommand SaveCmd { get { return new RelayCommand(OnSaveTest, AlwaysFalse); } }
        public ICommand NewCmd { get { return new RelayCommand(OnNewTest, AlwaysFalse); } }
        public ICommand OpenCmd { get { return new RelayCommand(OnOpenTest, AlwaysFalse); } }
        public ICommand ShowAboutDialogCmd { get { return new RelayCommand(OnShowAboutDialog, AlwaysTrue); } }
        public ICommand ExitCmd { get { return new RelayCommand(OnExitApp, AlwaysTrue); } }

        private bool AlwaysTrue() { return true; }
        private bool AlwaysFalse() { return false; }

        private void OnSampleCmdWithArgument(object obj)
        {
            // TODO
        }

        private void OnSaveAsTest()
        {
            var settings = new SaveFileDialogSettings
            {
                Title = "Save As",
                Filter = "Sample (.xml)|*.xml",
                CheckFileExists = false,
                OverwritePrompt = true
            };

            bool? success = DialogService.ShowSaveFileDialog(this, settings);
            if (success == true)
            {
                // Do something
                Log.Info("Saving file: " + settings.FileName);
            }
        }
        private void OnSaveTest()
        {
            // TODO
        }
        private void OnNewTest()
        {
            // TODO
        }
        private void OnOpenTest()
        {
            var settings = new OpenFileDialogSettings
            {
                Title = "Open",
                Filter = "Sample (.xml)|*.xml",
                CheckFileExists = false
            };

            bool? success = DialogService.ShowOpenFileDialog(this, settings);
            if (success == true)
            {
                // Do something
                Log.Info("Opening file: " + settings.FileName);
            }
        }
        private void OnShowAboutDialog()
        {
            Log.Info("Opening About dialog");
            AboutViewModel dialog = new AboutViewModel();
            var result = DialogService.ShowDialog<About>(this, dialog);
        }
        private void OnExitApp()
        {
            System.Windows.Application.Current.MainWindow.Close();
        }
        #endregion

        #region Events

        #endregion
    }
}
