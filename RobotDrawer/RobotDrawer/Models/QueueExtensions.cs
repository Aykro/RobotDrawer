using System.Collections.Generic;
using System.Threading;

namespace RobotDrawer.Models
{
    public class QueueExtensions : Queue<ObjectToDraw>
    {
        #region Constructors
        public QueueExtensions()
            : base()
        {
        }
        #endregion

        #region Properties
        #endregion

        #region Methods
        public virtual new void Enqueue(ObjectToDraw item)
        {
            base.Enqueue(item);
            if (Count == 1 && ABBManager.Instance.RobotManagerThread.ThreadState != ThreadState.Running)
            {
                if (ABBManager.Instance.RobotManagerThread.ThreadState == ThreadState.Unstarted)
                {
                    ABBManager.Instance.RobotManagerThread.Start();
                }
                else if(ABBManager.Instance.RobotManagerThread.ThreadState == ThreadState.Suspended)
                {
                    ABBManager.Instance.RobotManagerThread.Resume();
                }
            }
        }
       #endregion

        #region Events
        #endregion
    }
}
//public class QueueExtensions
//    {
//        #region Constructors
//        static QueueExtensions() { }
//        private static readonly QueueExtensions instance = new QueueExtensions();
//        public static QueueExtensions Instance { get { return instance; } }
//        #endregion

//        #region Properties
//        private Queue<ObjectToDraw> Queue = new Queue<ObjectToDraw>();
//        #endregion

//        #region Methods
//        public void Enqueue(ObjectToDraw item)
//        {
//            Queue.Enqueue(item);
//            if (Queue.Count == 1)
//            {
//                OnQueueNotEmpty(item);
//            }
//        }
//        public bool IsEmpty()
//        {
//            if (Queue.Any())
//            {
//                var last = Queue.Dequeue();
//                OnQueueNotEmpty(last);
//                return true;
//            }
//            else return false;
//        }
//        protected virtual void OnQueueNotEmpty(ObjectToDraw objectToDraw)
//        {
//            var del = QueueNotEmpty as EventHandler<RobotWorkEventArgs>;
//            if(del != null)
//            {
//                del(this, new RobotWorkEventArgs(objectToDraw));
//            }
//        }
//        #endregion

//        #region Events
//        public event EventHandler<RobotWorkEventArgs> QueueNotEmpty;
//        #endregion
//    }
//}