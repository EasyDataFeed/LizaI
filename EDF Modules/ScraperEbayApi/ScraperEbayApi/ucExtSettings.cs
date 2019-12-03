using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using WheelsScraper;

namespace Databox.Libs.ScraperEbayApi
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

        private void buttonEditEbayItemsFilepath_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            string filePath = GetFilePath();

            if (!string.IsNullOrEmpty(filePath))
                buttonEditEbayItemsFilepath.Text = filePath;
            else
                throw new Exception("You must select file");
        }

        private void ucExtSettings_Load(object sender, EventArgs e)
        {
            radioGroupCompatibilityCheck.SelectedIndex = 1;
        }

        private void radioGroupCompatibilityCheck_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (radioGroupCompatibilityCheck.SelectedIndex != -1)
            {
                if (radioGroupCompatibilityCheck.Properties.Items[radioGroupCompatibilityCheck.SelectedIndex].Description == "Without Compatibility")
                {
                    ExtSett.VehicleCompatibility = false;
                }
                else
                {
                    ExtSett.VehicleCompatibility = true;
                }
            }
        }
    }
}
