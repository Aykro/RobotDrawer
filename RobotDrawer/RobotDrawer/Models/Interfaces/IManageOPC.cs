using OPCAutomation;
using System;
using System.Threading.Tasks;
using System.Windows.Media;

namespace RobotDrawer.Models
{
    public interface IManageOPC
    {
        OPCItem RegisterItem(string itemId);       
        void Connect();
        void Disconnect();
        bool IsRobotIdle();
        void ChangeRobotTool(Color currentTool);
        void RunDrawLinesProcedure();
    }
}
