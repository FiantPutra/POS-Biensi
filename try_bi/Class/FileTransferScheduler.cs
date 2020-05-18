using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace try_bi.Class
{
    public static class FileTransferScheduler
    {
        public static void IntervalInMinutes(int hour, int min, double interval, Action task)
        {
            interval = interval / 60;
            FileTransferSchedulerService.Instance.ScheduleTask(hour, min, interval, task);
        }
        public static void IntervalInHours(int hour, int min, double interval, Action task)
        {
            FileTransferSchedulerService.Instance.ScheduleTask(hour, min, interval, task);
        }
    }
}
