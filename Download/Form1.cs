using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Download
{
    public partial class Form1 : Form
    {
        private SqlDataAdapter adp;

        private SqlCommand command;

        DataTable Dts = new DataTable();

        System.Data.DataSet dataSet = new System.Data.DataSet();

        System.Data.DataSet dataSet2 = new System.Data.DataSet();

        System.Data.DataSet dataSet3 = new System.Data.DataSet(); Int32 recCount = 0; int recCount2 = 0;

        public Form1()
        {
            InitializeComponent();
            timer1.Tick += timer1_Tick;
        }

        void timer1_Tick(object sender, EventArgs e)
        {
            doDowunload();
        }

        int doDowunload()
        {
            try
            {
                //    SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                {
                    connect.Open();

                    command = new SqlCommand("doGetStationInfo", connect) { CommandType = CommandType.StoredProcedure, CommandTimeout = 0 };

                    using (System.Data.DataSet ds = new System.Data.DataSet())
                    {
                        adp = new SqlDataAdapter(command);

                        adp.SelectCommand.CommandTimeout = 0;

                        adp.Fill(ds);

                        Dts = ds.Tables[0];

                        connect.Close();

                        if (ds.Tables[0].Rows.Count < 1)
                        {
                            //if (label7.InvokeRequired)
                            //{
                            //    label7.Invoke(new MethodInvoker(delegate { label7.Text = "Not Configured for this station"; }));
                            //    m_oWorker.CancelAsync();
                            //}
                            //else
                            //{
                            lblDownload.Text = "Not Configured for this station";
                            //m_oWorker.CancelAsync();
                            //}
                            return 0;
                        }
                        else
                        {

                            //label5.Text = string.Format("Receipt Data Download [ {0} ]", ds.Tables[0].Rows[0]["StationName"]);


                            switch (Program.intCode)
                            {
                                case 13://Akwa Ibom state
                                    using (var receiptAka = new AkwaIbomReceiptServices.ReceiptService())
                                    {
                                        dataSet = receiptAka.DownloadData(ds, Program.stateCode);
                                    }

                                    break;

                                case 20:
                                    using (var receiptDelta = new DeltaBir.ReceiptService())
                                    {
                                        dataSet = receiptDelta.DownloadData(Logic.GetMacAddress(), Program.stationCode);
                                    }

                                    if (dataSet.Tables[0].Rows[0]["returnCode"].ToString() == "-1")
                                    {
                                        textBox1.Text = string.Format("{0}...Error Occur During Data download.... Download", dataSet.Tables[0].Rows[0]["returnMessage"]);

                                        timer1.Stop();
                                        return 0;
                                    }

                                    break;
                                case 32://kogi state
                                    break;

                                case 37://ogun state

                                    using (var receiptsserv = new ReceiptServices.ReceiptService())
                                    {
                                        dataSet = receiptsserv.DownloadData(Logic.GetMacAddress(), Program.stationCode);
                                    }

                                    if (dataSet.Tables[0].Rows[0]["returnCode"].ToString() == "-1")
                                    {
                                        textBox1.Text = string.Format("{0}...Error Occur During Data download.... Insert Download", dataSet2.Tables[0].Rows[0]["returnMessage"]);
                                        //timer1.Stop();
                                        //timer1.Enabled = false;
                                        return 0;
                                    }
                                    break;

                                case 40://oyo state

                                    using (var receiptsServices = new OyoReceiptServices.ReceiptService())
                                    {
                                        dataSet = receiptsServices.DownloadData(ds, Program.stateCode);
                                    }

                                    break;

                                default:
                                    break;
                            }

                            if (dataSet.Tables.Count == 0)
                            {

                                timer1.Stop(); timer1.Enabled = false;
                                //btnStop.Visible = false; btnStart.Visible = true;
                                return 0;
                            }
                            else
                            {

                                if (dataSet.Tables[0].Rows.Count < 1)
                                {

                                    lblDownload.Text = String.Format("No More Records for the Station {0}", Program.stationName);


                                    timer1.Stop(); timer1.Enabled = false;
                                    //btnStop.Visible = false; btnStart.Visible = true;
                                    return 0;
                                }
                                else
                                {
                                    if (dataSet.Tables[1] != null && dataSet.Tables[1].Rows.Count < 1)
                                    {
                                        textBox1.Text = String.Format("No More Records for the Station {0}", Program.stationName);
                                        timer1.Stop(); timer1.Enabled = false;
                                        //btnStop.Visible = false; btnStart.Visible = true; btnStart.Enabled = true;
                                        return 0;
                                    }
                                    else
                                    {
                                        //switch case state code
                                        switch (Program.intCode)
                                        {
                                            //case 13://Akwa Ibom state

                                            //    break;

                                            //case 32://kogi state
                                            //    break;

                                            case 20:
                                                if (dataSet.Tables[0].Rows[0]["returnCode"].ToString() == "-1")
                                                {
                                                    textBox1.Text = string.Format("{0}.....Download Message", dataSet.Tables[0].Rows[0]["returnMessage"]);
                                                    //timer1.Enabled = false;
                                                    return 0;
                                                }
                                                else
                                                {
                                                    dataSet2 = InsertData(dataSet);


                                                    if (dataSet2.Tables[0].Rows[0]["returnCode"].ToString() == "-1")
                                                    {
                                                        textBox1.Text = string.Format("{0}...Error Occur During Data Insert After download.... Insert Download", dataSet2.Tables[0].Rows[0]["returnMessage"]);
                                                        //timer1.Stop();
                                                        //timer1.Enabled = false;
                                                        return 0;
                                                    }
                                                    else
                                                    {
                                                        dataSet3.Clear();

                                                        switch (Program.intCode)
                                                        {
                                                            case 13://Akwa Ibom state
                                                                using (var receiptAka = new AkwaIbomReceiptServices.ReceiptService())
                                                                {
                                                                    dataSet3 = receiptAka.DownloadDataUpdate(dataSet2, Program.stationCode);
                                                                }

                                                                break;
                                                            case 20:
                                                                using (var receiptDelta = new DeltaBir.ReceiptService())
                                                                {
                                                                    dataSet3 = receiptDelta.DownloadDataUpdate(dataSet2, Program.stationCode);
                                                                }
                                                                break;
                                                            case 32://kogi state
                                                                break;

                                                            case 37://ogun state

                                                                using (var receiptsserv = new ReceiptServices.ReceiptService())
                                                                {
                                                                    dataSet3 = receiptsserv.DownloadDataUpdate(dataSet2, Program.stationCode);
                                                                }
                                                                break;

                                                            case 40://oyo state

                                                                using (var receiptsServices = new OyoReceiptServices.ReceiptService())
                                                                {
                                                                    dataSet3 = receiptsServices.DownloadDataUpdate(dataSet2, Program.stationCode);
                                                                }
                                                                break;
                                                            default:
                                                                break;
                                                        }

                                                        //dataSet2 = receiptsserv.DownloadDataUpdate(dataSet2, Program.stationCode);

                                                        if (dataSet3.Tables[0].Rows[0]["returnCode"].ToString() == "00")
                                                        {
                                                            lblAll.Text = dataSet3.Tables[1].Rows[0]["ALLRecords"].ToString();

                                                            lblDownload.Text = dataSet3.Tables[2].Rows[0]["DownloadedRecords"].ToString();

                                                            lblRemain.Text = dataSet3.Tables[3].Rows[0]["RemainRecords"].ToString();

                                                            lblError.Text = dataSet3.Tables[4].Rows[0]["ErrorRecords"].ToString();

                                                            return 1;

                                                        }
                                                        else
                                                        {


                                                            textBox1.Text = string.Format("{0}...Download Data to Local Station...", dataSet3.Tables[0].Rows[0]["returnMessage"]);
                                                            //timer1.Enabled = false;
                                                            //btnStop.Visible = false;
                                                            //btnStart.Visible = true;
                                                            return 0;
                                                        }
                                                    }


                                                }
                                                break;
                                            case 37://ogun state

                                                if (dataSet.Tables[0].Rows[0]["returnCode"].ToString() == "-1")
                                                {
                                                    textBox1.Text = string.Format("{0}.....Download Message", dataSet.Tables[0].Rows[0]["returnMessage"]);
                                                    //timer1.Enabled = false;
                                                    return 0;
                                                }
                                                else
                                                {
                                                    dataSet2 = InsertData(dataSet);


                                                    if (dataSet2.Tables[0].Rows[0]["returnCode"].ToString() == "-1")
                                                    {
                                                        textBox1.Text = string.Format("{0}...Error Occur During Data Insert After download.... Insert Download", dataSet2.Tables[0].Rows[0]["returnMessage"]);
                                                        //timer1.Stop();
                                                        //timer1.Enabled = false;
                                                        return 0;
                                                    }
                                                    else
                                                    {
                                                        dataSet3.Clear();

                                                        switch (Program.intCode)
                                                        {
                                                            case 13://Akwa Ibom state
                                                                using (var receiptAka = new AkwaIbomReceiptServices.ReceiptService())
                                                                {
                                                                    dataSet3 = receiptAka.DownloadDataUpdate(dataSet2, Program.stationCode);
                                                                }

                                                                break;

                                                            case 32://kogi state
                                                                break;

                                                            case 37://ogun state

                                                                using (var receiptsserv = new ReceiptServices.ReceiptService())
                                                                {
                                                                    dataSet3 = receiptsserv.DownloadDataUpdate(dataSet2, Program.stationCode);
                                                                }
                                                                break;

                                                            case 40://oyo state

                                                                using (var receiptsServices = new OyoReceiptServices.ReceiptService())
                                                                {
                                                                    dataSet3 = receiptsServices.DownloadDataUpdate(dataSet2, Program.stationCode);
                                                                }
                                                                break;
                                                            default:
                                                                break;
                                                        }

                                                        //dataSet2 = receiptsserv.DownloadDataUpdate(dataSet2, Program.stationCode);

                                                        if (dataSet3.Tables[0].Rows[0]["returnCode"].ToString() == "00")
                                                        {
                                                            lblAll.Text = dataSet3.Tables[1].Rows[0]["ALLRecords"].ToString();

                                                            lblDownload.Text = dataSet3.Tables[2].Rows[0]["DownloadedRecords"].ToString();

                                                            lblRemain.Text = dataSet3.Tables[3].Rows[0]["RemainRecords"].ToString();

                                                            lblError.Text = dataSet3.Tables[4].Rows[0]["ErrorRecords"].ToString();

                                                            return 1;

                                                        }
                                                        else
                                                        {

                                                            //timer1.Stop();
                                                            textBox1.Text = string.Format("{0}...Download Data to Local Station...", dataSet3.Tables[0].Rows[0]["returnMessage"]);
                                                            //timer1.Enabled = false;
                                                            //btnStop.Visible = false;
                                                            //btnStart.Visible = true;
                                                            return 0;
                                                        }
                                                    }


                                                }
                                                break;

                                            //case 40://oyo state


                                            //    break;
                                            default:

                                                //if (dataSet.Tables[0].Rows[0]["returnCode"].ToString() == "-1")
                                                //{
                                                //    Common.setMessageBox(string.Format("{0}.....Download Message", dataSet.Tables[0].Rows[0]["returnMessage"]), Program.ApplicationName, 3); timer1.Stop();
                                                //    timer1.Enabled = false;
                                                //    return 0;
                                                //}
                                                //else
                                                //{
                                                dataSet2 = InsertData(dataSet);


                                                if (dataSet2.Tables[0].Rows[0]["returnCode"].ToString() == "-1")
                                                {
                                                    textBox1.Text = string.Format("{0}...Error Occur During Data Insert After download.... Insert Download", dataSet2.Tables[0].Rows[0]["returnMessage"]);
                                                    //timer1.Stop();
                                                    //timer1.Enabled = false;
                                                    return 0;
                                                }
                                                else
                                                {
                                                    dataSet3.Clear();

                                                    switch (Program.intCode)
                                                    {
                                                        case 13://Akwa Ibom state
                                                            using (var receiptAka = new AkwaIbomReceiptServices.ReceiptService())
                                                            {
                                                                dataSet3 = receiptAka.DownloadDataUpdate(dataSet2, Program.stationCode);
                                                            }

                                                            break;

                                                        case 32://kogi state
                                                            break;

                                                        case 37://ogun state

                                                            using (var receiptsserv = new ReceiptServices.ReceiptService())
                                                            {
                                                                dataSet3 = receiptsserv.DownloadDataUpdate(dataSet2, Program.stationCode);
                                                            }
                                                            break;

                                                        case 40://oyo state

                                                            using (var receiptsServices = new OyoReceiptServices.ReceiptService())
                                                            {
                                                                dataSet3 = receiptsServices.DownloadDataUpdate(dataSet2, Program.stationCode);
                                                            }
                                                            break;
                                                        default:
                                                            break;
                                                    }

                                                    //dataSet2 = receiptsserv.DownloadDataUpdate(dataSet2, Program.stationCode);

                                                    if (dataSet3.Tables[0].Rows[0]["returnCode"].ToString() == "00")
                                                    {
                                                        lblAll.Text = dataSet3.Tables[1].Rows[0]["ALLRecords"].ToString();

                                                        lblDownload.Text = dataSet3.Tables[2].Rows[0]["DownloadedRecords"].ToString();

                                                        lblRemain.Text = dataSet3.Tables[3].Rows[0]["RemainRecords"].ToString();

                                                        lblError.Text = dataSet3.Tables[4].Rows[0]["ErrorRecords"].ToString();

                                                        return 1;

                                                    }
                                                    else
                                                    {

                                                        textBox1.Text = string.Format("{0}...Download Data to Local Station...", dataSet3.Tables[0].Rows[0]["returnMessage"]);
                                                        //timer1.Enabled = false;
                                                        //btnStop.Visible = false;
                                                        //btnStart.Visible = true;
                                                        return 0;
                                                    }
                                                }


                                                //}
                                                break;
                                        }



                                    }


                                }
                            }

                            //}
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //timer1.Stop();
                textBox1.Text = String.Format("{0}----{1}...Do Download to Station,doDowunload", ex.Message, ex.StackTrace);
                return 0;
            }
            finally
            {
                //SplashScreenManager.CloseForm(false);
            }
        }

        public System.Data.DataSet InsertData(System.Data.DataSet dataSet)
        {
            System.Data.DataSet ds = new System.Data.DataSet();

            try
            {
                recCount = dataSet.Tables[0].Rows.Count;

                switch (Program.intCode)
                {
                    //case 13://Akwa Ibom state


                    //    break;

                    //case 32://kogi state
                    //    break;
                    case 20://delta state
                        using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                        {
                            connect.Open();

                            command = new SqlCommand("doInsertStationReceipt", connect) { CommandType = CommandType.StoredProcedure };
                            command.Parameters.Add(new SqlParameter("@tblCollectionReport_Temp", SqlDbType.Structured)).Value = dataSet.Tables[1];
                            command.CommandTimeout = 0;

                            //using (System.Data.DataSet dsf = new System.Data.DataSet())
                            //{
                            adp = new SqlDataAdapter(command);
                            adp.Fill(ds);
                            Dts = ds.Tables[0];
                            connect.Close();
                            //}
                        }
                        break;
                    case 37://ogun state
                        using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                        {
                            connect.Open();

                            command = new SqlCommand("doInsertStationReceipt", connect) { CommandType = CommandType.StoredProcedure };
                            command.Parameters.Add(new SqlParameter("@tblCollectionReport_Temp", SqlDbType.Structured)).Value = dataSet.Tables[1];
                            command.CommandTimeout = 0;

                            //using (System.Data.DataSet dsf = new System.Data.DataSet())
                            //{
                            adp = new SqlDataAdapter(command);
                            adp.Fill(ds);
                            Dts = ds.Tables[0];
                            connect.Close();
                            //}
                        }
                        break;

                    //case 40://oyo state

                    //    break;
                    default:
                        using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                        {
                            connect.Open();

                            command = new SqlCommand("doInsertStationReceipt", connect) { CommandType = CommandType.StoredProcedure };
                            command.Parameters.Add(new SqlParameter("@tblCollectionReport_Temp", SqlDbType.Structured)).Value = dataSet.Tables[0];
                            command.CommandTimeout = 0;

                            //using (System.Data.DataSet dsf = new System.Data.DataSet())
                            //{
                            adp = new SqlDataAdapter(command);
                            adp.Fill(ds);
                            Dts = ds.Tables[0];
                            connect.Close();
                            //}
                        }
                        break;
                }




                return ds;
            }
            catch (Exception ex)
            {
                textBox1.Text = String.Format("{0}----{1}...Insert Data Record to Station", ex.StackTrace, ex.Message);
                //timer1.Enabled = false;
                //btnStop.Visible = false;
                //btnStart.Visible = true;
                return ds;
            }
            return ds;
        }

    }
}
