using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using WheelsScraper;

namespace Databox.Libs.XmlCombine
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
            layoutControl1.Dock = DockStyle.Fill;
        }

        protected void RefreshBindings()
        {
            bsSett.DataSource = ExtSett;
            bsSett.ResetBindings(false);
        }

        private void buttonEditInputDirectory_Click(object sender, EventArgs e)
        {
            string directory = GetDirectory();

            if (!string.IsNullOrEmpty(directory))
                buttonEditInputDirectory.Text = directory;
            else
                throw new Exception("You must select directory");
        }

        private string GetDirectory()
        {
            FolderBrowserDialog browser = new FolderBrowserDialog();

            if (browser.ShowDialog() == DialogResult.OK)
                return  browser.SelectedPath; // prints path

            return null;
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

        private void buttonEditOutputDirectory_Click(object sender, EventArgs e)
        {
            string directory = GetDirectory();

            if (!string.IsNullOrEmpty(directory))
                buttonEditOutputDirectory.Text = directory;
            else
                throw new Exception("You must select directory");
        }

        private void buttonEditFilterFile_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            string filePath = GetFilePath();

            if (!string.IsNullOrEmpty(filePath))
                buttonEditFilterFile.Text = filePath;
            else
                throw new Exception("You must select file");


        }
    }
}
