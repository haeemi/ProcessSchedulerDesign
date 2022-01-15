using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace Schd
{
    class ReadyQueueElementPri
    {
        public int processID;
        public int burstTime;
        public int waitingTime;
        public int arriveTime;
        public int responseTime;
        public int turnaroundTime;
        public int priority;


        public ReadyQueueElementPri(int processID, int burstTime, int waitingTime, int arriveTime, int priority, int responseTime=-1, int turnaroundTime=-1)
        {
            this.processID = processID;
            this.burstTime = burstTime;
            this.waitingTime = waitingTime;
            this.arriveTime = arriveTime;
            this.responseTime = responseTime;
            this.turnaroundTime = turnaroundTime;
            this.priority = priority;
        }
    }

    class Priority
    {
        public static List<Result> Run(List<Process> jobList, List<Result> resultList)
        {
            int currentProcess = 0;
            int cpuTime = 0;
            int cpuDone = 0;
            int runTime = 0;
            List<ReadyQueueElementPri> readyQueue = new List<ReadyQueueElementPri>();
            do
            {
                while (jobList.Count != 0)
                {
                    Process frontJob = jobList.ElementAt(0);
                    if (frontJob.arriveTime == runTime)
                    {
                        readyQueue.Add(new ReadyQueueElementPri(frontJob.processID, frontJob.burstTime, 0, frontJob.arriveTime, frontJob.priority));
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
                        readyQueue = readyQueue.OrderBy(x => x.priority).ThenBy(x => x.arriveTime).ToList();
                        ReadyQueueElementPri rq = readyQueue.ElementAt(0);
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
                        ReadyQueueElementPri rq = readyQueue.ElementAt(Index);

                        if (rq.turnaroundTime == -1)
                            readyQueue.ElementAt(0).turnaroundTime = rq.turnaroundTime = runTime - rq.arriveTime;
                        else readyQueue.ElementAt(0).turnaroundTime = rq.turnaroundTime = -999;

                        resultList.Add(new Result(rq.processID, runTime - cpuTime, cpuTime, rq.waitingTime, rq.responseTime, rq.turnaroundTime));
                        readyQueue.RemoveAt(readyQueue.FindIndex(x => x.processID == currentProcess));
                        currentProcess = 0;
                        continue;
                    }
                    else
                    {
                        readyQueue = readyQueue.OrderBy(x => x.priority).ThenBy(x => x.burstTime).ThenBy(x => x.arriveTime).ToList(); //우선순위로 정렬 후 도착시간순으로 정렬
                        if (currentProcess != readyQueue.ElementAt(0).processID)
                        {
                            int Index = readyQueue.FindIndex(x => x.processID == currentProcess);
                            ReadyQueueElementPri rq = readyQueue.ElementAt(Index);     //현재 실행중인 프로세스에서 쫓겨나기
                            if (rq.priority != readyQueue.ElementAt(0).priority)
                            {   //새로 정렬된 readyQueue의 프로세스 중 우선수위가 같으면 현재 프로세스 계속 실행.
                                resultList.Add(new Result(rq.processID, runTime - cpuTime, cpuTime, rq.waitingTime, rq.responseTime, rq.turnaroundTime));
                                currentProcess = 0;
                                continue;
                            }
                        }
                    }
                }
                if (readyQueue.Count != 0)
                {
                    int currentIndex = readyQueue.FindIndex(x => x.processID == currentProcess);
                    readyQueue.ElementAt(currentIndex).burstTime--;

                    for (int i = 0; i < readyQueue.Count; i++)      //나머지 readyqueue에있는 프로세스에대한 대기시간 증가
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