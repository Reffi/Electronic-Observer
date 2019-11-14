using System;
using System.Collections.Generic;
using ElectronicObserver.Utility.Data;

namespace ElectronicObserver.Data
{
    public interface IShipDataCustom : IBaseShipStats
    {
        string Name { get; }
        int SortID { get; }


        double AccuracyFitBonus { get; }
        double NightAccuracyFitBonus { get; }
        EquipmentDataCustom[] Equipment { get; set; }
        FitBonusCustom FitBonus { get; }


        int Firepower { get; }
        int Torpedo { get; }

        [Obsolete("use ShipID")]
        int ID { get; }
        ShipID ShipID { get; }
        ShipTypes ShipType { get; }
        ShipClasses ShipClass { get; }
        bool IsMarried { get; }
        bool IsInstallation { get; }
        bool IsSubmarine { get; }
        IEnumerable<int> EquippableCategories { get; }
        int EquipmentSlotCount { get; }
        bool IsExpansionSlotAvailable { get; }
        IEnumerable<DayAttackKind> DayAttacks { get; }
        IEnumerable<NightAttackKind> NightAttacks { get; }
        IEnumerable<DayAttackKind> AswAttacks { get; }
    }

    public interface IBaseShipStats
    {
        int Level { get; set; }
        int HP { get; set; }
        int BaseArmor { get; set; }
        int BaseEvasion { get; set; }
        int[] Aircraft { get; set; }
        int BaseSpeed { get; set; }
        int BaseRange { get; set; }
        double BaseAccuracy { get; }

        int Condition { get; set; }
        int BaseFirepower { get; set; }
        int BaseTorpedo { get; set; }
        int BaseAA { get; set; }
        int BaseASW { get; set; }
        int BaseLoS { get; set; }
        int BaseLuck { get; set; }
        int BaseNightPower { get; }
    }

    public interface IShipDamageData : IShipEquipmentData
    {
        int BaseFirepower { get; set; }
        int BaseTorpedo { get; set; }
        int BaseASW { get; }

        int Firepower { get; }
        int Torpedo { get; }
        int NightPower { get; }

        double EquipmentFirepower { get; }
        double EquipmentTorpedo { get; }
        double EquipmentBombing { get; }
        double EquipmentAswDamage { get; }
        double EquipmentNightPower { get; }

        ShipTypes ShipType { get; }
        IEnumerable<DayAttackKind> AswAttacks { get; }
    }

    public interface IShipEquipmentData
    {
        int MainGunCount { get; }
        int SecondaryGunCount { get; }
        int RadarCount { get; }
        int ApShellCount { get; }

        int SonarCount { get; }
        int SmallSonarCount { get; }
        int LargeSonarCount { get; }
        int DepthChargeCount { get; }
        int DepthChargeProjectorCount { get; }
        int SpecialDepthChargeProjectorCount { get; }
    }
}