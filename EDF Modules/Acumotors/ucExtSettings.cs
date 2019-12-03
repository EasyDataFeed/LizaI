using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using WheelsScraper;

namespace Databox.Libs.Acumotors
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

        private void radioGroupCompatibilityCheck_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (radioGroupPercentage.SelectedIndex != -1)
            {
                if (radioGroupPercentage.Properties.Items[radioGroupPercentage.SelectedIndex].Description == "Percentage for all brands")
                {
                    ExtSett.Percentage = false;
                    layoutControlItem3.Enabled = true;
                }
                else
                {
                    ExtSett.Percentage = true;
                    layoutControlItem3.Enabled = false;
                }
            }
        }

        private void ucExtSettings_Load(object sender, EventArgs e)
        {
            if (ExtSett.Percentage)
            {
                radioGroupPercentage.SelectedIndex = 1;
            }
            else
            {
                radioGroupPercentage.SelectedIndex = 0;
            }

            if (ExtSett.UseGoogleSheets)
            {
                layoutControlItem7.Enabled = true;
            }
            else
            {
                layoutControlItem7.Enabled = false;
            }
        }

        private void Export(object sender, EventArgs e)
        {
            if (ExtSett.UseGoogleSheets)
            {
                layoutControlItem7.Enabled = true;
            }
            else
            {
                layoutControlItem7.Enabled = false;
            }
        }
    }
}
