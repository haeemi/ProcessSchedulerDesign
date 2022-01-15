using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Schd
{
    class Result
    {
        public int processID;
        public int startP;
        public int burstTime;
        public int waitingTime;
        public int responseTime;
        public int turnaroundTime;

        public Result(int processID, int startP, int burstTime, int waitingTime, int responseTime=-1, int turnaroundTime=-1)
        {
            this.processID = processID;
            this.startP = startP;
            this.burstTime = burstTime;
            this.waitingTime = waitingTime;
            this.responseTime = responseTime;
            this.turnaroundTime = turnaroundTime;
        }
    }
}
