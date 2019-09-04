using ElectronicObserver.Data;
using ElectronicObserver.Resource;
using ElectronicObserver.Utility.Data;
using ElectronicObserver.Window.Support;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ElectronicObserver.Window.Dialog
{
    public partial class DialogShipSimulation : Form
    {
        int DefaultShipID = -1;

        public DialogShipSimulation()
        {
            InitializeComponent();

            Icon = ResourceManager.ImageToIcon(ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormEquipmentList]);


            ClientSize = new Size(1600, 900);
        }

        public DialogShipSimulation(int shipID) : this()
        {
            DefaultShipID = shipID;
        }

        private void DialogShipSimulation_Load(object sender, EventArgs e)
        {
            DialogShipSimulationWpf wpfApp = new DialogShipSimulationWpf(DefaultShipID);
            elementHost1.Child = wpfApp;
        }
    }
}