#region using

using System.Windows.Forms;
using DevExpress.XtraEditors;
using WheelsScraper;

#endregion

namespace Databox.Libs.Terapeak
{
    public partial class ucExtSettings : XtraUserControl
    {
        public ExtSettings ExtSett
        {
            get { return (ExtSettings) Sett.SpecialSettings; }
        }

        ScraperSettings _sett;

        public ScraperSettings Sett
        {
            get { return _sett; }
            set
            {
                _sett = value;
                if (_sett != null) RefreshBindings();
            }
        }

        public ucExtSettings()
        {
            InitializeComponent();
            layoutControl1.Dock = DockStyle.Fill;
        }

        private void RefreshBindings()
        {
            bsSett.DataSource = ExtSett;
            bsSett.ResetBindings(false);
        }
    }
}
