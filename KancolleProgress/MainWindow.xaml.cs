using System.Collections.Generic;
using System.Linq;
using System.Windows;
using ElectronicObserverDatabase.Models;
using ElectronicObserverTypes;
using KancolleProgress.Controls;
using Microsoft.EntityFrameworkCore;


namespace KancolleProgress
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<ShipTypeGroupControl> Controls { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            MasterDataContext masterDb = new MasterDataContext();
            UserDataContext userDb = new UserDataContext();

            masterDb.Database.Migrate();
            userDb.Database.Migrate();

            Dictionary<ShipID, ShipDataCustom> masterShipData =
                (from ship in masterDb.Ships where ship.ShipId < 1500 && ship.ShipName != null select ship)
                .ToDictionary
                (
                    g => (ShipID) g.ShipId,
                    g => new ShipDataCustom(g)
                );

            IEnumerable<ShipDataCustom> userShips =
                (from ship in userDb.UserShipData
                    select ship)
                .AsEnumerable()
                .Select(s => new ShipDataCustom(s));

            foreach (ShipDataCustom ship in userShips)
            {
                GetBaseShip(ship);
            }

            IEnumerable<IGrouping<ShipTypeGroup, ShipDataCustom>> groups = masterShipData.Values
                .Where(s => s.RemodelBeforeShipId == 0)
                .OrderBy(s => s.SortID)
                .GroupBy(s => s.ShipType.ToGroup())
                .OrderBy(s => s.Key);

            

            Controls = new List<ShipTypeGroupControl>();

            foreach (IGrouping<ShipTypeGroup, ShipDataCustom> group in groups)
            {
                ShipTypeGroupContainer.Children.Add(new ShipTypeGroupControl {Group = group});
            }

            ShipDataCustom GetBaseShip(ShipDataCustom ship)
            {
                while (masterShipData[ship.ShipID].RemodelBeforeShipId != 0)
                {
                    if (masterShipData[masterShipData[ship.ShipID].RemodelBeforeShipId].Level < ship.Level)
                    {
                        masterShipData[masterShipData[ship.ShipID].RemodelBeforeShipId].Level = ship.Level;
                    }
                    ship = masterShipData[masterShipData[ship.ShipID].RemodelBeforeShipId];
                }

                return ship;
            }
        }
    }
}
