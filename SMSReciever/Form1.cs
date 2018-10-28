using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using WindowsFormsApplication1.SMS;

namespace WindowsFormsApplication1
{
    public partial class Form1 :Form
    {
        //sleep time
        int milliseconds = 20000;

        public Form1()
        {
            InitializeComponent();
            DataClasses1DataContext dc = new DataClasses1DataContext();
            var q = from a in dc.GetTable<alarme>() orderby a.date descending select a;
            dataGridView.DataSource = q;
            //state();
        }
        private void Form1_Load(object sender, EventArgs e)
            {
            // TODO: This line of code loads data into the 'data.alarmes' table. You can move, or remove it, as needed.
            this.alarmesTableAdapter3.Fill(this.data.alarmes);
            // TODO: This line of code loads data into the 'projDBDataSet2.alarmes' table. You can move, or remove it, as needed.
            this.alarmesTableAdapter2.Fill(this.projDBDataSet2.alarmes);
            // TODO: This line of code loads data into the 'projDBDataSet1.alarmes' table. You can move, or remove it, as needed.
            this.alarmesTableAdapter1.Fill(this.projDBDataSet1.alarmes);
            // TODO: This line of code loads data into the 'projDBDataSet.alarmes' table. You can move, or remove it, as needed.
            this.alarmesTableAdapter.Fill(this.projDBDataSet.alarmes);
            Thread th = new Thread(function1);
            th.Start();
            Thread th2 = new Thread(function2);
            th2.Start();
        }
        public void function2()
        {
            while(true)
            {
                DataClasses1DataContext dc5 = new DataClasses1DataContext();
                var q = from a in dc5.GetTable<alarme>() select a;
                //sleep
                Thread.Sleep(milliseconds);
            }
        }
        public void function1()
        {
            #region Private Variables
            SerialPort port = new SerialPort();
            clsSMS objclsSMS = new clsSMS();
            ShortMessageCollection objShortMessageCollection = new ShortMessageCollection();
            #endregion
            try
            {
                //Open communication port 
                port = objclsSMS.OpenPort("COM6", 9600, 8, 300, 300);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            while (true)
            {
                // code de recepttion
                try
                {
                    //compter les SMS 
                    int uCountSMS = objclsSMS.CountSMSmessages(port);
                    //MessageBox.Show("receiving data from phone success");
                    if (uCountSMS > 0)
                    {
                        #region Command
                        string strCommand = "AT+CMGL=\"REC UNREAD\"";
                        //string strCommand = "AT+CMGL=\"ALL\"";
                        #endregion
                        // If SMS exist then read SMS
                        #region Read SMS
                        //.............................................. Read all SMS ....................................................
                        objShortMessageCollection = objclsSMS.ReadSMS(port, strCommand);
                        #endregion
                    }
                }
                catch (Exception ex){}
                //sleep
                Thread.Sleep(milliseconds);
            }
        }
        private void Form1_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == WindowState)
                Hide();
        }
        private void Form1_Close(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == WindowState)
                Hide();
        }
        private void notifyIcon1_MouseDoubleClick_1(object sender, MouseEventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            DataClasses1DataContext dc = new DataClasses1DataContext();
            var q =
                from a in dc.GetTable<alarme>() orderby a.date descending
                select a;
            dataGridView.DataSource = q;
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }
        private void CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
           
        }

        //public void state()
        //{
        //    for (int i = 0; i < dataGridView.Rows.Count; i++)
        //    {
        //        if ((Convert.ToString(dataGridView.Rows[i].Cells[4].Value) == "'alarm'*'"))
        //        {
        //            dataGridView.Rows[i].Cells[1].Style.BackColor = Color.Red;
        //        }        
        //    }
        //}
        //public DataTable alarme()
        //{
        //    //DataTable dtcl = new DataTable();
        //    //SqlConnection con = new SqlConnection("Data Source=DESKTOP-NMRO30H\\SQLEXPRESS2017;Initial Catalog=ProjDB;Integrated Security=True");
        //    //con.Open();
        //    //SqlCommand cmdRech = new SqlCommand("select * from alarmes where text LIKE 'alarme%' ", con);
        //    //SqlDataAdapter da = new SqlDataAdapter(cmdRech);
        //    //da.Fill(dtcl);
        //    //con.Close();
        //    //return dtcl;
        //}

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            DataClasses1DataContext dc = new DataClasses1DataContext();

            var q =
                from a in dc.GetTable<alarme>() orderby a.date descending
                select a;

            dataGridView.DataSource = q;
        }
        private void toolStripButton2_Click_1(object sender, EventArgs e)
        {
            //dataGridView.DataSource = alarme();
            DataClasses1DataContext dc = new DataClasses1DataContext();

            var q =
                from a in dc.GetTable<alarme>()
                where (a.text.Substring(0,6)=="alarme")
                orderby a.date descending
                select a;

            dataGridView.DataSource = q;
        }
        private void dataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            for (int i=0;i<dataGridView.Rows.Count;i++)
            {
                if (Convert.ToString(dataGridView.Rows[i].Cells[3].Value).Substring(0,6)=="alarme" ||
                    Convert.ToString(dataGridView.Rows[i].Cells[3].Value).Substring(0, 6) == "Alarme")
                {
                    dataGridView.Rows[i].Cells[5].Style.BackColor = System.Drawing.Color.Red;
                }
                else 
                {

                    dataGridView.Rows[i].Cells[5].Style.BackColor = System.Drawing.Color.Green;
                }
            }
        }
        public void supprimer()
        {
            
            SqlConnection con = new SqlConnection("Data Source=DESKTOP-NMRO30H\\SQLEXPRESS2017;Initial Catalog=ProjDB;Integrated Security=True");

            int i = new int();
            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                if (Convert.ToBoolean(row.Cells[0].Value)==true)
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("Delete From alarmes Where ID = '" + row.Cells[1].Value.ToString() + "'", con);
                    cmd.ExecuteNonQuery();
                    con.Close();
                    i = 1;
                }
            }
            if (i==1)
            {
                MessageBox.Show("Supprimé avec succee");
            }
        }
        public void display()
        {
            dataGridView.DataSource = displaydata();
        }
        public DataTable displaydata()
        {
            SqlConnection con = new SqlConnection("Data Source=DESKTOP-NMRO30H\\SQLEXPRESS2017;Initial Catalog=ProjDB;Integrated Security=True");
            con.Open();
            SqlDataAdapter sdf = new SqlDataAdapter(" select * from alarmes ", con);
            DataTable sd = new DataTable();
            sdf.Fill(sd);         
            con.Close();
            return sd;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection("Data Source=DESKTOP-NMRO30H\\SQLEXPRESS2017;Initial Catalog=ProjDB;Integrated Security=True");
            con.Open();
            SqlDataAdapter sdf = new SqlDataAdapter(" select * from alarmes where Date between'" + dateTimePicker1.Value.ToString() + "' and '" + dateTimePicker2.Value.ToString()  + "'", con);
            DataTable sd = new DataTable();
            sdf.Fill(sd);
            dataGridView.DataSource = sd;
            con.Close();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Voulez Vous supprimer", "question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                supprimer();
                display();
            }
        }
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            //dataGridView.DataSource = notification();
            DataClasses1DataContext dc = new DataClasses1DataContext();
            var q =
                from a in dc.GetTable<alarme>()
                where (a.text.Substring(0, 6) != "alarme")
                orderby a.date descending
                select a;
            dataGridView.DataSource = q;
        }

        //public DataTable notification()
        //{

        //    DataTable dtcl = new DataTable();
        //    SqlConnection con = new SqlConnection("Data Source=DESKTOP-NMRO30H\\SQLEXPRESS2017;Initial Catalog=ProjDB;Integrated Security=True");
        //    con.Open();


        //    SqlCommand cmdRech = new SqlCommand("select * from alarmes where text LIKE 'not%'", con);

        //    SqlDataAdapter da = new SqlDataAdapter(cmdRech);
        //    da.Fill(dtcl);

        //    con.Close();
        //    return dtcl;


        //}

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            exportgridtopdf(dataGridView,"test");
        }
        public void exportgridtopdf (DataGridView dgw ,string filename )
        {
            BaseFont bf = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1250, BaseFont.EMBEDDED);
            PdfPTable pdftable = new PdfPTable(dgw.Columns.Count);
            pdftable.DefaultCell.Padding = 3;
            pdftable.WidthPercentage = 100;
            pdftable.HorizontalAlignment = Element.ALIGN_LEFT;
            pdftable.DefaultCell.BorderWidth = 1;
            iTextSharp.text.Font text = new iTextSharp.text.Font(bf,10,iTextSharp.text.Font.NORMAL);
            foreach (DataGridViewColumn column in dgw.Columns)
            {
                PdfPCell cell = new PdfPCell(new Phrase(column.HeaderText, text));
                cell.BackgroundColor = new iTextSharp.text.Color(240, 240, 240);
                pdftable.AddCell(cell);
            }
            foreach (DataGridViewRow row in dgw.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {   
                    try
                    {
                        pdftable.AddCell(new Phrase(cell.Value.ToString(), text));
                    }
                    catch(Exception e)
                    {

                    }
                }
            }

            var savefiledialoge = new SaveFileDialog();
            savefiledialoge.FileName = filename;
            savefiledialoge.DefaultExt = ".pdf";
            if(savefiledialoge.ShowDialog()==DialogResult.OK)
            {
                using (FileStream stream = new FileStream(savefiledialoge.FileName, FileMode.Create))
                {
                    Document pdfdoc = new Document(PageSize.A4, 10f, 10f,10f,0f);
                    PdfWriter.GetInstance(pdfdoc, stream);
                    pdfdoc.Open();
                    pdfdoc.Add(pdftable);
                    pdfdoc.Close();
                    stream.Close();
                }
            }
        }
        private void dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
