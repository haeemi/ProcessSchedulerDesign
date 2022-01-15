using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace Schd
{
    class ReadyQueueElementSJF
    {
        public int processID;
        public int burstTime;
        public int waitingTime;
        public int arriveTime;
        public int responseTime;
        public int turnaroundTime;

        public ReadyQueueElementSJF(int processID, int burstTime, int waitingTime, int arriveTime, int responseTime = -1, int turnaroundTime = -1)
        {
            this.processID = processID;
            this.burstTime = burstTime;
            this.waitingTime = waitingTime;
            this.arriveTime = arriveTime;
            this.responseTime = responseTime;
            this.turnaroundTime = turnaroundTime;
        }
    }

    class SJF
    {
        public static List<Result> Run(List<Process> jobList, List<Result> resultList)
        {
            int currentProcess = 0;
            int cpuTime = 0;
            int cpuDone = 0;
            int runTime = 0;
            List<ReadyQueueElementSJF> readyQueue = new List<ReadyQueueElementSJF>();
            do
            {
                while (jobList.Count != 0)
                {
                    Process frontJob = jobList.ElementAt(0);
                    if (frontJob.arriveTime == runTime)
                    {
                        readyQueue.Add(new ReadyQueueElementSJF(frontJob.processID, frontJob.burstTime, 0, frontJob.arriveTime));
                        jobList.RemoveAt(0);
                    }
                    else
                    {
                        break;
                    }
                }

                if (currentProcess == 0)
                {
                    if (readyQueue.Count != 0)
                    {
                        ReadyQueueElementSJF rq = readyQueue.ElementAt(0);
                        if (rq.responseTime == -1)
                            readyQueue.ElementAt(0).responseTime = rq.responseTime = runTime - rq.arriveTime;
                        else readyQueue.ElementAt(0).responseTime = rq.responseTime = -999;
                        cpuDone = rq.burstTime;
                        cpuTime = 0;
                        currentProcess = rq.processID;

                    }
                }
                else
                {
                    if (cpuTime == cpuDone)
                    {
                        int Index = readyQueue.FindIndex(x => x.processID == currentProcess);
                        ReadyQueueElementSJF rq = readyQueue.ElementAt(Index);
                        if (rq.turnaroundTime == -1)
                            readyQueue.ElementAt(0).turnaroundTime = rq.turnaroundTime = runTime - rq.arriveTime;
                        else readyQueue.ElementAt(0).turnaroundTime = rq.turnaroundTime = -999;
                        resultList.Add(new Result(rq.processID, runTime - cpuTime, cpuTime, rq.waitingTime, rq.responseTime, rq.turnaroundTime));
                        readyQueue.RemoveAt(0);
                        currentProcess = 0;
                        continue;
                    }
                    else
                    {
                        readyQueue = readyQueue.OrderBy(x => x.burstTime).ThenBy(x => x.arriveTime).ToList();
                        if (currentProcess != readyQueue.ElementAt(0).processID)
                        {
                            int Index = readyQueue.FindIndex(x => x.processID == currentProcess);
                            ReadyQueueElementSJF rq = readyQueue.ElementAt(Index);
                            resultList.Add(new Result(rq.processID, runTime - cpuTime, cpuTime, rq.waitingTime, rq.responseTime, rq.turnaroundTime));
                            currentProcess = 0;
                            continue;
                        }
                    }
                }
                if (readyQueue.Count != 0)
                {
                    int currentIndex = readyQueue.FindIndex(x => x.processID == currentProcess);
                    readyQueue.ElementAt(currentIndex).burstTime--;
                    for (int i = 0; i < readyQueue.Count; i++)
                    {
                        if (!readyQueue.ElementAt(i).processID.Equals(currentProcess))
                            readyQueue.ElementAt(i).waitingTime++;
                    }
                }
                cpuTime++;
                runTime++;

            } while (jobList.Count != 0 || readyQueue.Count != 0 || currentProcess != 0);

            return resultList;
        }
    }
}