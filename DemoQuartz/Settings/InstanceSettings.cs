using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoQuartz.Settings
{
    public class InstanceSettings
    {
        public static bool IsBeginingDay { get; set; } = false;
    }

    public class XnCodeSettings
    {
        public static bool IsChangeXnCode { get; set; } = false;

        public static List<string> ListXnCode { get; set; } = new List<string>();
    }
}
