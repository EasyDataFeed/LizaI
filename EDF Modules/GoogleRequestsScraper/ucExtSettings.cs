using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using GoogleRequestsScraper.Helpers;
using WheelsScraper;

namespace Databox.Libs.GoogleRequestsScraper
{
    public partial class ucExtSettings : XtraUserControl
    {
        public ExtSettings ExtSett
        {
            get
            {
                return (ExtSettings)Sett.SpecialSettings;
            }
        }

        ScraperSettings _sett;
        public ScraperSettings Sett
        {
            get { return _sett; }
            set { _sett = value; if (_sett != null) RefreshBindings(); }
        }
        public ucExtSettings()
        {
            InitializeComponent();
        }

        protected void RefreshBindings()
        {
            bsSett.DataSource = ExtSett;
            bsSett.ResetBindings(false);
        }

        private string GetFilePath()
        {
            OpenFileDialog fileDiaog = new OpenFileDialog();
            fileDiaog.Filter = "Excel files (*.csv?) | *.csv?";
            if (fileDiaog.ShowDialog() == DialogResult.OK)
                return fileDiaog.FileName;
            else
                return null;
        }

        private void textBoxKeywordsForScrape_TextChanged(object sender, EventArgs e)
        {
            List<string> listKeywords = new List<string>();
            foreach (var item in textBoxKeywordsForScrape.Lines)
            {
                if (!listKeywords.Contains(item.Trim(), StringComparer.OrdinalIgnoreCase) && (item.Trim() != string.Empty))
                {
                    listKeywords.Add(item.Trim());
                }
            }
            ExtSett.KeywordsForScrape = listKeywords;
        }

        private void ucExtSettings_Load(object sender, EventArgs e)
        {
            if (ExtSett.KeywordsForScrape.Count != 0)
            {
                textBoxKeywordsForScrape.Clear();
                foreach (var item in ExtSett.KeywordsForScrape)
                {
                    textBoxKeywordsForScrape.AppendText(item + Environment.NewLine);
                }
            }
            cbeScanMethod.Properties.Items.AddRange(typeof(global::GoogleRequestsScraper.Enums.ScanMethod).GetEnumValues());
        }

        private void buttonEdit1_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            string fileSkip = GetFilePath();

            if (!string.IsNullOrEmpty(fileSkip))
                buttonEdit1.Text = fileSkip;
            else
                throw new Exception("You must select file");
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            GoogleDocApiHelper.GetGoogleService();
        }

        private void buttonEdit2_ButtonClick_1(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            string fileSkip = GetFilePath();

            if (!string.IsNullOrEmpty(fileSkip))
                buttonEdit2.Text = fileSkip;
            else
                throw new Exception("You must select file");
        }
    }
}
