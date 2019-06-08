using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using WheelsScraper;

namespace Databox.Libs.EdgeInfo
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

        private void textEditFTPLogin_EditValueChanged(object sender, EventArgs e)
        {
            ExtSett.FTPLogin = textEditFTPLogin.Text;
        }

        private void textEditFTPPassword_EditValueChanged(object sender, EventArgs e)
        {
            ExtSett.FTPPassword = textEditFTPPassword.Text;
        }

        private void textEditFTPHost_EditValueChanged(object sender, EventArgs e)
        {
            ExtSett.FTPHost = textEditFTPHost.Text;
        }

        private void ucExtSettings_Load(object sender, EventArgs e)
        {
            textEditFTPLogin.Text = ExtSett.FTPLogin;
            textEditFTPPassword.Text = ExtSett.FTPPassword;
            textEditFTPHost.Text = ExtSett.FTPHost;
        }

        private void buttonEditSupplierFilePath_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            string filePath = GetFilePath();

            if (!string.IsNullOrEmpty(filePath))
                buttonEditSupplierFilePath.Text = filePath;
            else
                throw new Exception("You must select file");
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

        private void buttonEditSupplierFileSkip_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            string fileSkip = GetFilePath();

            if (!string.IsNullOrEmpty(fileSkip))
                buttonEditSupplierFileSkip.Text = fileSkip;
            else
                throw new Exception("You must select file");
        }

        private void buttonEditSupplierZeroItems_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            string supplierZeroFile = GetFilePath();

            if (!string.IsNullOrEmpty(supplierZeroFile))
                buttonEditSupplierZeroItems.Text = supplierZeroFile;
            else
                throw new Exception("You must select file");
        }
    }
}
