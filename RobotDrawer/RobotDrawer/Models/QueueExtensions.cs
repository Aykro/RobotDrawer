using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
            if (Count == 1)
            {
                var objectToDraw = Dequeue();
                OnQueueNotEmpty(this, objectToDraw);
            }
        }
        public virtual bool IsEmpty()
        { 
            if(Count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public virtual void RepeatUntilEmpty()
        {
            if (!IsEmpty())
            {
                var objectToDraw = Dequeue();
                OnQueueNotEmpty(this, objectToDraw);
            }
            else
            {
                return;
            }
        }
        public virtual void OnQueueNotEmpty(object sender, ObjectToDraw objectToDraw)
        {
            QueueNotEmpty?.Invoke(sender, new RobotWorkEventArgs(objectToDraw));
            //var queueNotEmptyDelegate = QueueNotEmpty as EventHandler<RobotWorkEventArgs>;
            //if (queueNotEmptyDelegate != null)
            //{
            //    queueNotEmptyDelegate(sender, new RobotWorkEventArgs(objectToDraw));
            //}
        }
        #endregion

        #region Events
        public event EventHandler<RobotWorkEventArgs> QueueNotEmpty;
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