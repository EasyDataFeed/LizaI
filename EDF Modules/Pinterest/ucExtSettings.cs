using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Pinterest.Enums;
using WheelsScraper;

namespace Databox.Libs.Pinterest
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

        private void Export(object sender, EventArgs e)
        {
            if (ExtSett.UseExistingExport)
            {
                layoutControlItem1.Enabled = true;
            }
            else
            {
                layoutControlItem1.Enabled = false;
            }
        }

        private void ucExtSettings_Load(object sender, EventArgs e)
        {
            if (ExtSett.UseExistingExport)
            {
                layoutControlItem1.Enabled = true;
            }
            else
            {
                layoutControlItem1.Enabled = false;
            }

            radioGroupDestCategory.SelectedIndex = (int) ExtSett.ActionType;
        }

        private void radioGroupDestCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (radioGroupDestCategory.SelectedIndex != -1)
            {
                if (radioGroupDestCategory.Properties.Items[radioGroupDestCategory.SelectedIndex].Description == "Brand")
                {
                    ExtSett.ActionType = ActionType.Brand;
                }
                else if (radioGroupDestCategory.Properties.Items[radioGroupDestCategory.SelectedIndex].Description == "Main Category")
                {
                    ExtSett.ActionType = ActionType.MainCategory;
                }
                else if (radioGroupDestCategory.Properties.Items[radioGroupDestCategory.SelectedIndex].Description == "Sub Category")
                {
                    ExtSett.ActionType = ActionType.SubCategory;
                }
            }
            else
            {
                radioGroupDestCategory.SelectedIndex = 1;
            }

            RefreshBindings();
        }
    }
}
