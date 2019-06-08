using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using MarksJewelersSuppliersData.Helpers;
using WheelsScraper;

namespace Databox.Libs.MarksJewelersSuppliersData
{
    public partial class ucExtSettings : XtraUserControl
    {
        private const string FileDialogFilter = "Excel files (*.csv?) | *.csv?";

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
            layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
        }

        protected void RefreshBindings()
        {
            bsSett.DataSource = ExtSett;
            bsSett.ResetBindings(false);
        }

        private string GetFilePath()
        {
            OpenFileDialog fileDiaog = new OpenFileDialog();
            fileDiaog.Filter = FileDialogFilter;
            if (fileDiaog.ShowDialog() == DialogResult.OK)
                return fileDiaog.FileName;
            else
                return null;
        }

        private string GetFolderPath()
        {
            FolderBrowserDialog folderDiaog = new FolderBrowserDialog();
            folderDiaog.SelectedPath = FileHelper.GetSettingsPath();
            if (folderDiaog.ShowDialog() == DialogResult.OK)
                return folderDiaog.SelectedPath;
            else
                return null;
        }

        public List<string> GetAllFiles(string folderPath)
        {
            List<string> files = new List<string>();
            List<string> filesPath = new List<string>();

            try
            {
                foreach (string file in Directory.GetFiles(folderPath))
                {
                    files.Add(file);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return files;
        }

        private void buttonEdit1_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            string filePath = GetFilePath();

            if (!string.IsNullOrEmpty(filePath))
                buttonEdit1.Text = filePath;
            else
                throw new Exception("You must select file");
        }

        private void buttonEdit2_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            string filePath = GetFilePath();

            if (!string.IsNullOrEmpty(filePath))
                buttonEdit2.Text = filePath;
            else
                throw new Exception("You must select file");
        }

        private void buttonEdit3_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            ExtSett.TagHeuerFolderPath = GetFolderPath();
            string folderPath = ExtSett.TagHeuerFolderPath;

            ExtSett.GeneralImageList = GetAllFiles(folderPath);

            RefreshBindings();
        }
    }
}
