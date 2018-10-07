using OPCAutomation;
using RobotDrawer.Properties;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace RobotDrawer.Models
{
    public class ABBManager : IManageOPC
    {
        #region Constructors
        static ABBManager() { }
        private ABBManager(string OPCServerName, string linesArray, string linesToDrawCount,
            string doneLinesCount, string ReadyToDraw, string Draw, string ChangeTool,
            string ClearAll, int linesArraySize)
        {
            RobotManagerThread = new Thread(() => RunDrawLinesProcedure());
            OpcServer = new OPCServer();
            //OpcServer.Connect(OPCServerName, "");

            //OpcGroup = OpcServer.OPCGroups.Add();
            //OpcGroup.UpdateRate = 50;
            //OpcGroup.IsActive = true;
            //OpcGroup.IsSubscribed = true;

            //this.OPCServerName = OPCServerName;
            //this.LinesArray = RegisterItem(linesArray);
            //this.LinesArraySize = linesArraySize;
            //this.LinesToDrawCount = RegisterItem(linesToDrawCount);
            //this.DoneLinesCount = RegisterItem(doneLinesCount);
            //this.ReadyToDraw = RegisterItem(ReadyToDraw);
            //this.Draw = RegisterItem(Draw);
            //this.ChangeTool = RegisterItem(ChangeTool);
            //this.ClearAll = RegisterItem(ClearAll);
            //this.OPCServerName = OPCServerName;
            //dropOffset = 0.0;
            IsEraserHeld = false;
        }
        #endregion

        #region Properties
        private static readonly ABBManager instance = new ABBManager(
                         ControllerConfig.Default.OPCServerName,
                         ControllerConfig.Default.robotItemLinesArray,
                         ControllerConfig.Default.robotItemLinesToDrawCount,
                         ControllerConfig.Default.robotItemCurrentlyDrawnLine,
                         ControllerConfig.Default.robotItemReadyToDraw,
                         ControllerConfig.Default.robotItemDraw,
                         ControllerConfig.Default.robotItemChangeTool,
                         ControllerConfig.Default.robotItemClearAll,
                         Int32.Parse(ControllerConfig.Default.robotItemArraySize));
        public static ABBManager Instance { get { return instance; } }
        public QueueExtensions ShapesPending = new QueueExtensions();
        public Thread RobotManagerThread;
        private readonly string OPCServerName;
        private readonly OPCServer OpcServer;
        private readonly OPCGroup OpcGroup;
        private readonly int LinesArraySize;
        private readonly OPCItem LinesArray;                // 3-dimensional array embedded one withing another
        private readonly OPCItem ToolOffsetArray;           // 1-dimensional array of marker offsets
        private readonly OPCItem LinesToDrawCount;
        private readonly OPCItem DoneLinesCount;
        private readonly OPCItem ReadyToDraw;
        private readonly OPCItem Draw;
        private readonly OPCItem ChangeTool;
        private readonly OPCItem ClearAll;
        private readonly OPCItem GrabMarker;
        private readonly OPCItem GrabEraser;
        private readonly OPCItem EraserHeld;
        private Color LastTool;
        private double grabOffset;
        private double dropOffset;
        private bool IsEraserHeld;
        #endregion

        #region Methods
        public OPCItem RegisterItem(string itemID)
        {
            try
            {
                return OpcGroup.OPCItems.AddItem(itemID, 1);
            }
            catch (Exception e)
            {
                throw new Exceptions.InvalidOPCItemIDException(itemID, e);
            }
        }
        public void Connect()
        {
            // OpcServer.Connect(OPCServerName, "");
            //ShapesPending.QueueNotEmpty += async (s, e) => await RunDrawLinesProcedure(s, e);  //new EventHandler<RobotWorkEventArgs>(RunDrawLinesProcedure);
            //await RunMalowanie();
        }
        public void Disconnect()
        {
            RobotManagerThread.Suspend();
            //OpcServer.Disconnect();
        }
        public bool IsRobotIdle()
        {
            return (bool)ReadyToDraw.Value;
        }
        private void RunChangeToolProcedure()
        {
            ChangeTool.Write(true);
        }
        private void SetGrabMarker()
        {
            GrabMarker.Write(true);
        }
        private void SetGrabEraser()
        {
            GrabEraser.Write(true);
        }
        private void RunClearAllProcedure()
        {
            ClearAll.Write(true);
        }
        public void StartDraw()
        {
            Draw.Write(true);
        }
        public void RunDrawLinesProcedure()
        {
                try
                {
                    while (ShapesPending.Count > 0)
                    {
                        var objectToDraw = ShapesPending.Dequeue();
                        Thread.Sleep(2000);
                        if (objectToDraw.Colour != LastTool)
                        {
                            //ChangeRobotTool(objectToDraw.Colour);
                        }
                        if (objectToDraw.EraseAll == true)
                        {
                            //RunClearAllProcedure();
                        }
                        else
                        {
                            var lines = new object[LinesArraySize];
                            for (int i = 0; i < LinesArraySize; i++)
                            {
                                if (i >= objectToDraw.Lines.GetLength(0))                       // Get length of first dimension, it represents number of lines
                                {
                                    lines[i] = CreateLine(0, 0, 0, 0);
                                }
                                else
                                {
                                    lines[i] = CreateLine(objectToDraw.Lines[i, 0], objectToDraw.Lines[i, 1],
                                        objectToDraw.Lines[i, 2], objectToDraw.Lines[i, 3]);
                                }
                            }
                            //    LinesArray.Write(lines);
                            //    LinesToDrawCount.Write(objectToDraw.Lines.GetLength(0));
                            //    DoneLinesCount.Write(1);
                            //    StartDraw();
                        }
                        //while (!IsRobotIdle())
                        //{

                        //}
                        LastTool = objectToDraw.Colour;
                    }
                    RobotManagerThread.Suspend();
                }
                catch(ThreadAbortException ex)
                {
                    return;
                }
        }
        public void ChangeRobotTool(Color currentTool)
        {
            if (currentTool == Colors.White)
            {
                SetGrabEraser();
            }
            else
            {
                if (currentTool == Colors.Black)
                {
                    grabOffset = 0.0;
                }
                else if (currentTool == Colors.Red)
                {
                    grabOffset = -45.0;
                }
                else if (currentTool == Colors.Green)
                {
                    grabOffset = -90.0;
                }
                SetToolOffsetArray(dropOffset, grabOffset);
                SetGrabMarker();
                dropOffset = grabOffset;
            }
            RunChangeToolProcedure();
        }
        private void SetToolOffsetArray(double dropOffset, double grabOffset)
        {
            ToolOffsetArray.Write(new object[]
            {
                dropOffset,
                grabOffset
            });
        }
        private object[] CreateLine(double x1, double y1, double x2, double y2)
        {
            return new object[2]
            {
                    new float[2] {(float) x1, (float) y1},
                    new float[2] {(float) x2, (float) y2}
            };
        }
        #endregion

        #region Events
        #endregion
    }
}
