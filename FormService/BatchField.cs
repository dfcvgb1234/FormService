using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormService
{
    public class BatchField
    {
        public string InName;
        public string FormatDate;
        public string FormatTime;
        public string DateOutName;
        public string TimeOutName;

        public BatchField(string inname, string formatdate, string formattime, string dateoutname, string timeoutname)
        {
            this.InName = inname;
            this.FormatDate = formatdate;
            this.FormatTime = formattime;
            this.DateOutName = dateoutname;
            this.TimeOutName = timeoutname;
        }
    }
}
