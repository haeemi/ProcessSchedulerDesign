using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Schd
{
    public partial class Scheduling : Form
    {
        string[] readText;
        private bool readFile = false;
        private bool actionableStatus = true;
        List<Process> pList, pView;
        List<Result> resultList;


        public Scheduling()
        {
            InitializeComponent();
        }

        private void OpenFile_Click(object sender, EventArgs e)
        {
            pView.Clear();
            pList.Clear();

            //파일 오픈
            string path = SelectFilePath();
            if (path == null) return;

            readText = File.ReadAllLines(path);

            //토큰 분리
            for (int i = 0; i < readText.Length; i++)
            {
                string[] token = readText[i].Split(' ');
                Process p = new Process(int.Parse(token[1]), int.Parse(token[2]), int.Parse(token[3]), int.Parse(token[4]));
                pList.Add(p);
            }

            //Grid에 process 출력
            dataGridView1.Rows.Clear();
            string[] row = { "", "", "", "" };
            foreach (Process p in pList)
            {
                row[0] = p.processID.ToString();
                row[1] = p.arriveTime.ToString();
                row[2] = p.burstTime.ToString();
                row[3] = p.priority.ToString();

                dataGridView1.Rows.Add(row);
            }

            //arriveTime으로 정렬
            pList.Sort(delegate (Process x, Process y)
            {
                if (x.arriveTime > y.arriveTime) return 1;
                else if (x.arriveTime < y.arriveTime) return -1;
                else
                {
                    return x.processID.CompareTo(y.processID);
                }
                //return x.arriveTime.CompareTo(y.arriveTime);
            });

            readFile = true;
        }

        private string SelectFilePath()
        {
            openFileDialog1.Filter = "텍스트파일|*.txt";
            return (openFileDialog1.ShowDialog() == DialogResult.OK) ? openFileDialog1.FileName : null;
        }

        private void Run_Click(object sender, EventArgs e)
        {

            String text = textBox1.Text.ToString();
            int timeQuantum;
            if (!int.TryParse(text, out timeQuantum)) timeQuantum = 3;

            if (!readFile || !actionableStatus) return;
            String select = "";
            if (comboBox1.SelectedItem != null) select = comboBox1.SelectedItem.ToString();

            if (select.Equals("FCFS"))
            {
                //스케쥴러 실행
                resultList = fcfs.Run(pList, resultList);
                //결과출력
                dataGridView2.Rows.Clear();
                string[] row = { "", "", "", "", "" };
                double SumTAR = 0.0;
                int ContTAR = 0;
                double SumRES = 0.0;
                int ContRES = 0;
                double watingTime = 0.0;
                foreach (Result r in resultList)
                {
                    row[0] = r.processID.ToString();
                    row[1] = r.burstTime.ToString();
                    row[2] = r.waitingTime.ToString();
                    row[3] = r.responseTime.ToString();
                    row[4] = r.turnaroundTime.ToString();
                    watingTime += r.waitingTime;

                    if (r.responseTime == -1 || r.responseTime == -999) row[3] = " ";
                    else { SumRES += r.responseTime; ContRES++; }
                    if (r.turnaroundTime == -1 || r.turnaroundTime == -999) row[4] = " ";
                    else { SumTAR += r.turnaroundTime; ContTAR++; }
                    dataGridView2.Rows.Add(row);
                }
                TRTime.Text = "전체 실행시간: " + (resultList[resultList.Count - 1].startP + resultList[resultList.Count - 1].burstTime).ToString();
                avgRT.Text = "평균 대기시간: " + (watingTime / resultList.Count).ToString();
                label4.Text = "평균 turnaround time: " + (SumTAR / ContTAR).ToString();
                label5.Text = "평균 response time: " + (SumRES / ContRES).ToString();
                panel1.Invalidate();
            }

            if (select.Equals("SJF"))
            {
                //스케쥴러 실행
                resultList = SJF.Run(pList, resultList);
                //결과출력
                dataGridView2.Rows.Clear();
                string[] row = { "", "", "", "", "" };
                double SumTAR = 0.0;
                int ContTAR = 0;
                double SumRES = 0.0;
                int ContRES = 0;
                double watingTime = 0.0;

                foreach (Result r in resultList)
                {
                    row[0] = r.processID.ToString();
                    row[1] = r.burstTime.ToString();
                    row[2] = r.waitingTime.ToString();
                    row[3] = r.responseTime.ToString();
                    row[4] = r.turnaroundTime.ToString();
                    watingTime += r.waitingTime;

                    if (r.responseTime == -1 || r.responseTime == -999) row[3] = " ";
                    else { SumRES += r.responseTime; ContRES++; }
                    if (r.turnaroundTime == -1 || r.turnaroundTime == -999) row[4] = " ";
                    else { SumTAR += r.turnaroundTime; ContTAR++; }
                    dataGridView2.Rows.Add(row);
                }
                TRTime.Text = "전체 실행시간: " + (resultList[resultList.Count - 1].startP + resultList[resultList.Count - 1].burstTime).ToString();
                avgRT.Text = "평균 대기시간: " + (watingTime / resultList.Count).ToString();
                label4.Text = "평균 turnaround time: " +(SumTAR/ContTAR).ToString();
                label5.Text = "평균 response time: " + (SumRES / ContRES).ToString();
                panel1.Invalidate();
            }

            if (select.Equals("Priority"))
            {
                //스케쥴러 실행
                resultList = Priority.Run(pList, resultList);
                //결과출력
                dataGridView2.Rows.Clear();
                string[] row = { "", "", "", "", "" };
                double SumTAR = 0.0;
                int ContTAR = 0;
                double SumRES = 0.0;
                int ContRES = 0;
                double watingTime = 0.0;
                foreach (Result r in resultList)
                {
                    row[0] = r.processID.ToString();
                    row[1] = r.burstTime.ToString();
                    row[2] = r.waitingTime.ToString();
                    row[3] = r.responseTime.ToString();
                    row[4] = r.turnaroundTime.ToString();
                    watingTime += r.waitingTime;

                    if (r.responseTime == -1 || r.responseTime == -999) row[3] = " ";
                    else { SumRES += r.responseTime; ContRES++; }
                    if (r.turnaroundTime == -1 || r.turnaroundTime == -999) row[4] = " ";
                    else { SumTAR += r.turnaroundTime; ContTAR++; }
                    dataGridView2.Rows.Add(row);
                }
                TRTime.Text = "전체 실행시간: " + (resultList[resultList.Count - 1].startP + resultList[resultList.Count - 1].burstTime).ToString();
                avgRT.Text = "평균 대기시간: " + (watingTime / resultList.Count).ToString();
                label4.Text = "평균 turnaround time: " + (SumTAR / ContTAR).ToString();
                label5.Text = "평균 response time: " + (SumRES / ContRES).ToString();
                panel1.Invalidate();
            }

            if (select.Equals("RR"))
            {
                //스케쥴러 실행
                resultList = RR.Run(pList, resultList, timeQuantum);
                //결과출력
                dataGridView2.Rows.Clear();
                string[] row = { "", "", "", "","" };
                double SumTAR = 0.0;
                int ContTAR = 0;
                double SumRES = 0.0;
                int ContRES = 0;
                double watingTime = 0.0;
                foreach (Result r in resultList)
                {
                    row[0] = r.processID.ToString();
                    row[1] = r.burstTime.ToString();
                    row[2] = r.waitingTime.ToString();
                    row[3] = r.responseTime.ToString();
                    row[4] = r.turnaroundTime.ToString();

                    watingTime += r.waitingTime;

                    if (r.responseTime == -1 || r.responseTime == -999) row[3] = " ";
                    else { SumRES += r.responseTime; ContRES++; }
                    if (r.turnaroundTime == -1 || r.turnaroundTime == -999) row[4] = " ";
                    else { SumTAR += r.turnaroundTime; ContTAR++; }
                    dataGridView2.Rows.Add(row);
                }
                TRTime.Text = "전체 실행시간: " + (resultList[resultList.Count - 1].startP + resultList[resultList.Count - 1].burstTime).ToString();
                avgRT.Text = "평균 대기시간: " + (watingTime / resultList.Count).ToString();
                label4.Text = "평균 turnaround time: " + (SumTAR / ContTAR).ToString();
                label5.Text = "평균 response time: " + (SumRES / ContRES).ToString();
                panel1.Invalidate();
            }
        }


        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            int startPosition = 10;
            double waitingTime = 0.0;

            int resultListPosition = 0;
            foreach (Result r in resultList)
            {
                e.Graphics.DrawString("p" + r.processID.ToString(), Font, Brushes.Black, startPosition + (r.startP * 10), resultListPosition);
                e.Graphics.DrawRectangle(Pens.Red, startPosition + (r.startP * 10), resultListPosition + 20, r.burstTime * 10, 30);
                e.Graphics.DrawString(r.burstTime.ToString(), Font, Brushes.Black, startPosition + (r.startP * 10), resultListPosition + 60);
                e.Graphics.DrawString(r.waitingTime.ToString(), Font, Brushes.Black, startPosition + (r.startP * 10), resultListPosition + 80);
                waitingTime += (double)r.waitingTime;
            }
        }

        private void Button1_Click(object sender, EventArgs e)//Clear
        {
            pList.Clear();
            pView.Clear();
            resultList.Clear();
            pList = new List<Process>();
            pView = new List<Process>();
            resultList = new List<Result>();
            dataGridView1.Rows.Clear();
            dataGridView2.Rows.Clear();
            TRTime.Text = "전체 실행시간: ";
            avgRT.Text = "평균 대기시간: ";
            label4.Text = "평균 turnaround time: ";
            label5.Text = "평균 response time: ";
            panel1.Refresh();
            readFile = false;
            actionableStatus = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            pList = new List<Process>();
            pView = new List<Process>();
            resultList = new List<Result>();

            //입력창
            DataGridViewTextBoxColumn processColumn = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn arriveTimeColumn = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn burstTimeColumn = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn priorityColumn = new DataGridViewTextBoxColumn();

            processColumn.HeaderText = "프로세스";
            processColumn.Name = "process";
            arriveTimeColumn.HeaderText = "도착시간";
            arriveTimeColumn.Name = "arriveTime";
            burstTimeColumn.HeaderText = "실행시간";
            burstTimeColumn.Name = "burstTime";
            priorityColumn.HeaderText = "우선순위";
            priorityColumn.Name = "priority";

            dataGridView1.Columns.Add(processColumn);
            dataGridView1.Columns.Add(arriveTimeColumn);
            dataGridView1.Columns.Add(burstTimeColumn);
            dataGridView1.Columns.Add(priorityColumn);


            //결과창
            DataGridViewTextBoxColumn resultProcessColumn = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn resultBurstTimeColumn = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn resultWaitingTimeColumn = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn responseTimeColumn = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn turnarroundTimeColumn = new DataGridViewTextBoxColumn();


            resultProcessColumn.HeaderText = "프로세스";
            resultProcessColumn.Name = "process";
            resultBurstTimeColumn.HeaderText = "실행시간";
            resultBurstTimeColumn.Name = "resultBurstTimeColumn";
            resultWaitingTimeColumn.HeaderText = "대기시간";
            resultWaitingTimeColumn.Name = "waitingTime";
            responseTimeColumn.HeaderText = "응답시간";
            responseTimeColumn.Name = "responseTime";
            turnarroundTimeColumn.HeaderText = "turnarround";
            turnarroundTimeColumn.Name = "turnarroundTime";

            dataGridView2.Columns.Add(resultProcessColumn);
            dataGridView2.Columns.Add(resultBurstTimeColumn);
            dataGridView2.Columns.Add(resultWaitingTimeColumn);
            dataGridView2.Columns.Add(responseTimeColumn);
            dataGridView2.Columns.Add(turnarroundTimeColumn);
        }
    }
}
