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
        public MainWindow()
        {
            InitializeComponent();
        }

        public MainWindow(string dbPath)
        {
            InitializeComponent();

            MasterDataContext masterDb = new MasterDataContext(dbPath);
            UserDataContext userDb = new UserDataContext(dbPath);

            masterDb.Database.Migrate();
            userDb.Database.Migrate();

            Dictionary<ShipID, ShipDataCustom> masterShipData =
                (from ship in masterDb.MasterShipData where ship.ShipId < 1500 && ship.ShipName != null select ship)
                .ToDictionary
                (
                    g => (ShipID) g.ShipId,
                    g => new ShipDataCustom(g)
                );

            IEnumerable<ShipDataCustom> userShips =
                (from ship in userDb.UserShipData select ship)
                .AsEnumerable()
                .Select(s => new ShipDataCustom(s));

            foreach (ShipDataCustom ship in userShips)
            {
                ShipID baseShipId = GetBaseShip(ship).ShipID;

                if (masterShipData[baseShipId].Level < ship.Level)
                {
                    masterShipData[baseShipId].Level = ship.Level;
                }
            }

            List<ShipDataCustom> playerShips = masterShipData.Values
                .Where(s => s.RemodelBeforeShipId == 0)
                .OrderBy(s => s.SortID)
                .ToList();

            IEnumerable<IGrouping<ShipTypeGroup, ShipDataCustom>> groups = playerShips
                .GroupBy(s => s.ShipType.ToGroup())
                .OrderBy(s => s.Key);

            foreach (IGrouping<ShipTypeGroup, ShipDataCustom> group in groups)
            {
                ShipTypeGroupContainer.Children.Add(new ShipTypeGroupControl {Group = group});
            }

            ShipTypeGroupContainer.Children.Add(new ColorFilterContainerControl{Ships = playerShips});

            ShipDataCustom GetBaseShip(ShipDataCustom ship)
            {
                while (masterShipData[ship.ShipID].RemodelBeforeShipId != 0)
                {
                    ship = masterShipData[masterShipData[ship.ShipID].RemodelBeforeShipId];
                } 

                return ship;
            }
        }
    }
}
