using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElectronicObserver.Utility.Data;

namespace ElectronicObserver.Data
{
    public class Accuracy : IAccuracy
    {
        private IShipDataCustom _ship;
        private FleetDataCustom _fleet;
        private BattleDataCustom _battle;

        public bool IgnoreFit { get; set; }

        public Accuracy(IShipDataCustom ship, FleetDataCustom fleet = null, BattleDataCustom battle = null, 
            bool ignoreFit = false)
        {
            _ship = ship;
            _fleet = fleet;
            _battle = battle;
            IgnoreFit = ignoreFit;
        }


        public double DayShelling =>
            ((_fleet.BaseAccuracy(_ship) 
              + _ship.BaseAccuracy
              + _ship.Equipment.Where(eq => eq != null).Sum(eq => eq.Accuracy))
             * _fleet.ShellingAccuracyMod(_ship)
             * ConditionAccuracyMod
             + (IgnoreFit ? 0 : _ship.AccuracyFitBonus))
            * DayAttackKindAccuracyMod
            * ApAccuracyMod;

        public double ASW =>
            (_fleet.BaseAswAccuracy(_ship)
             + _ship.BaseAccuracy
             + _ship.Equipment.Where(eq => eq != null).Sum(eq => eq.AswAccuracy))
            * ConditionAccuracyMod
            * _fleet.AswAccuracyMod(_ship);

        public double Night =>
            ((_fleet.NightRecon ? 1.1 : 1)
             * (_fleet.BaseNightAccuracy(_ship) + (_fleet.Flare ? 5 : 0))
             + _ship.BaseAccuracy
             + _ship.Equipment.Where(eq => eq != null).Sum(eq => eq.Accuracy))
            * _fleet.NightAccuracyMod(_ship)
            * ConditionAccuracyMod
            * NightAttackKindAccuracyMod
            + (_fleet.Searchlight ? 7 : 0)
            + (IgnoreFit ? 0 : _ship.NightAccuracyFitBonus);








        private double ConditionAccuracyMod => _ship.Condition switch
        {
            int condition when condition > 52 => 1.2,
            int condition when condition > 32 => 1,
            int condition when condition > 22 => 0.8,
            _ => 0.5
        };

        private double DayAttackKindAccuracyMod => _battle.DayAttack switch
        {
            DayAttackKind.CutinMainRadar => 1.5,

            DayAttackKind.CutinMainSub => 1.3,
            DayAttackKind.CutinMainAP => 1.3,

            DayAttackKind.CutinMainMain => 1.2,

            DayAttackKind.DoubleShelling => 1.1,

            _ =>1
        };

        private double NightAttackKindAccuracyMod => _battle.NightAttack switch
        {
            NightAttackKind.CutinMainMain => 2,

            NightAttackKind.CutinTorpedoTorpedo => 1.65,

            NightAttackKind.CutinMainTorpedo => 1.5,
            NightAttackKind.CutinMainSub => 1.5,

            NightAttackKind.DoubleShelling => 1.1,

            NightAttackKind.NormalAttack => 1,

            _ => 1
        };
        

        private double ApAccuracyMod => CalculateApAccuracyMod();
        private double CalculateApAccuracyMod()
        {
            bool ap = false;
            bool main = false;
            bool sub = false;
            bool radar = false;

            foreach (IEquipmentDataCustom eq in _ship.Equipment.Where(eq => eq != null))
            {
                switch (eq.CategoryType)
                {
                    case EquipmentTypes.APShell:
                        ap = true;
                        break;

                    case EquipmentTypes.MainGunSmall:
                    case EquipmentTypes.MainGunMedium:
                    case EquipmentTypes.MainGunLarge:
                    case EquipmentTypes.MainGunLarge2:
                        main = true;
                        break;

                    case EquipmentTypes.SecondaryGun:
                        sub = true;
                        break;

                    case EquipmentTypes.RadarLarge:
                    case EquipmentTypes.RadarLarge2:
                    case EquipmentTypes.RadarSmall:
                        radar = true;
                        break;
                }
            }

            if (!ap || !main)
                return 1;

            if (radar)
                return sub ? 1.3 : 1.25;

            return sub ? 1.2 : 1.1;
        }
    }
}
