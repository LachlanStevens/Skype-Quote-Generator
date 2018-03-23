using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SkypeQuoteGenerator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void CopyButton_Click(object sender, EventArgs e)
        {
            DateTime selecteddate = DateBox.Value;
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            DateTime epochtime = new DateTime(1970, 1, 1, 12, 00, 00);
            var dataObject = new DataObject();
            //TimeSpan t = selecteddate - epoch.total;
            var j = Convert.ToInt64((selecteddate.ToUniversalTime() - epoch).TotalSeconds);
            //int secondsSinceEpoch = (int)t.TotalSeconds;
            var messagePlain = string.Format(
                "[{0:hh:mm:ss}] {1}: {2}",
                "",
                AuthorBox.Text,
                MessageBox.Text);
            var messageXml = string.Format(
                "<quote author=\"{0}\" timestamp=\"{1}\">{2}</quote>",
                AuthorBox.Text,
                j,
                MessageBox.Text);

            dataObject.SetData("System.String", messagePlain);
            dataObject.SetData("UnicodeText", messagePlain);
            dataObject.SetData("Text", messagePlain);
            dataObject.SetData("SkypeMessageFragment", new MemoryStream(Encoding.UTF8.GetBytes(messageXml)));
            dataObject.SetData("Locale", new MemoryStream(BitConverter.GetBytes(CultureInfo.CurrentCulture.LCID)));
            dataObject.SetData("OEMText", messagePlain);

            Clipboard.SetDataObject(dataObject, true);
            
        }

        
        public static DateTime ConvertFromUnixTimestamp(double timestamp)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return origin.AddSeconds(timestamp);
        }

        public static double ConvertToUnixTimestamp(DateTime date)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            TimeSpan diff = date.ToUniversalTime() - origin;
            return Math.Floor(diff.TotalSeconds);
        }

        public static void ReadFromClipBoard()
        {
            IDataObject iData = Clipboard.GetDataObject();

            // Determines whether the data is in a format you can use. 
            if (iData.GetDataPresent(DataFormats.Text))
            {
                // Yes it is, so display it in a text box.
                //MessageBox.Text = (String)iData.GetData(DataFormats.Text);
                IDataObject j = Clipboard.GetDataObject();
                string separator = ", ";
                String[] i = j.GetFormats();
                //MessageBox.Text = j.GetType().Name;
                //MessageBox.Text = string.Join(separator, i);
                var data = Clipboard.GetData("SkypeMessageFragment") as MemoryStream;
                StreamReader sr = new StreamReader(data);
                var str = sr.ReadToEnd();

                //MessageBox.Text = "SkypeMessageFragment: " + str;
            }
            else
            {
                // No it is not.
                //MessageBox.Text = "Could not retrieve data off the clipboard.";
            }
            //MessageBox.Show();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            MessageBox.Multiline = true;
            MessageBox.ScrollBars = ScrollBars.Vertical;
            MessageBox.WordWrap = true;
        }

        private void ResestTime_Click(object sender, EventArgs e)
        {
            DateBox.Value = DateTime.Now;
        }
    }
}
