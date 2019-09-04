using ElectronicObserver.Utility.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data
{
    public struct VisibleFits
    {
        public int Firepower { get; }
        public int Torpedo { get; }
        public int AA { get; }
        public int ASW { get; }
        public int Evasion { get; }
        public int Armor { get; }
        public int LoS { get; }

        public VisibleFits(int firepower = 0, int torpedo = 0, int aa = 0, int asw = 0,
            int evasion = 0, int armor = 0, int los = 0)
        {
            Firepower = firepower;
            Torpedo = torpedo;
            AA = aa;
            ASW = asw;
            Evasion = evasion;
            Armor = armor;
            LoS = los;
        }

        public static VisibleFits operator +(VisibleFits a, VisibleFits b)
        {
            return new VisibleFits
            (
                firepower: a.Firepower + b.Firepower,
                torpedo: a.Torpedo + b.Torpedo,
                aa: a.AA + b.AA,
                asw: a.ASW + b.ASW,
                evasion: a.Evasion + b.Evasion,
                armor: a.Armor + b.Armor,
                los: a.LoS + b.LoS
            );
        }

        public static VisibleFits operator -(VisibleFits a, VisibleFits b)
        {
            return new VisibleFits
            (
                firepower: a.Firepower - b.Firepower,
                torpedo: a.Torpedo - b.Torpedo,
                aa: a.AA - b.AA,
                asw: a.ASW - b.ASW,
                evasion: a.Evasion - b.Evasion,
                armor: a.Armor - b.Armor,
                los: a.LoS - b.LoS
            );
        }
    }

    // how to not SQL
    public class FitBonusCustom
    {
        private IShipDataCustom _ship;
        private VisibleFits _visibleFits;

        public VisibleFits VisibleFit =>
            new VisibleFits(_firepower, _torpedo, _aa, _asw, _evasion, _armor, _los);



        private int _firepower;
        private int _torpedo;
        private int _aa;
        private int _asw;
        private int _evasion;
        private int _armor;
        private int _los;
        private int _accuracy;

        public int Firepower
        {
            get => _firepower;
            set => _firepower = value;
        }

        public int Torpedo
        {
            get => _torpedo;
            set => _torpedo = value;
        }

        public int AA
        {
            get => _aa;
            set => _aa = value;
        }

        public int ASW
        {
            get => _asw;
            set => _asw = value;
        }

        public int Evasion
        {
            get => _evasion;
            set => _evasion = value;
        }

        public int Armor
        {
            get => _armor;
            set => _armor = value;
        }

        public int LoS
        {
            get => _los;
            set => _los = value;
        }

        public int Accuracy
        {
            get => _accuracy;
            set => _accuracy = value;
        }

        // todo find a better way for this
        public FitBonusCustom(VisibleFits synergies)
        {
            _visibleFits = synergies;

            Firepower = _visibleFits.Firepower;
            Torpedo = _visibleFits.Torpedo;
            AA = _visibleFits.AA;
            ASW = _visibleFits.ASW;
            Evasion = _visibleFits.Evasion;
            Armor = _visibleFits.Armor;
            LoS = _visibleFits.LoS;

            Accuracy = 0;
        }

        public FitBonusCustom(IShipDataCustom ship, IEquipmentDataCustom equip)
        {
            _ship = ship;

            _visibleFits = VisibleFitBonus(equip);

            Firepower = _visibleFits.Firepower;
            Torpedo = _visibleFits.Torpedo;
            AA = _visibleFits.AA;
            ASW = _visibleFits.ASW;
            Evasion = _visibleFits.Evasion;
            Armor = _visibleFits.Armor;
            LoS = _visibleFits.LoS;

            Accuracy = GetAccuracy(equip);
        }

        private VisibleFits VisibleFitBonus(IEquipmentDataCustom equip) => _ship.ShipClass switch
        {
            ShipClasses.Kongou => VisibleFitsKongou(equip),
            ShipClasses.QueenElizabeth => VisibleFitsWarspite(equip),
            ShipClasses.Iowa => VisibleFitsBurger(equip),
            ShipClasses.Ise when _ship.ShipID == 553 || _ship.ShipID == 554 => VisibleFitsIse(equip), // Ise k2 class
            ShipClasses.Nelson => VisibleFitsNelson(equip),
            ShipClasses.Nagato => VisibleFitsNagato(equip),

            _ when _ship.ShipType == ShipTypes.AviationBattleship => VisibleFitsAviationBattleship(equip),

            _ => new VisibleFits()
        };

        /// <summary>
        /// 149 - Kongou k2     <br/>
        /// 150 - Hiei k2       <br/>
        /// 151 - Haruna k2     <br/>
        /// 152 - Kirishima k2  <br/>
        /// 591 - Kongou k2c    <br/>
        /// _   - kai Kongou class
        /// </summary>
        private VisibleFits VisibleFitsKongou(IEquipmentDataCustom equip) => (equip.ID, _ship.ShipID) switch
        {
            // dazzle
            (104, 149) => new VisibleFits(firepower: 2),
            (104, 150) => new VisibleFits(firepower: 1),
            (104, 152) => new VisibleFits(firepower: 1),
            (104, 151) => new VisibleFits(firepower: 2, aa: 1, evasion: 2),

            // dazzle kai
            (289, 149) => new VisibleFits(firepower: 2, aa: 2),
            (289, 150) => new VisibleFits(firepower: 1),
            (289, 152) => new VisibleFits(firepower: 1),
            (289, 151) => new VisibleFits(firepower: 2, aa: 2, evasion: 2),

            // 35.6 kai
            (328, 591) => new VisibleFits(firepower: 3, torpedo: 1, evasion: 1),
            (328, 149) => new VisibleFits(firepower: 2, evasion: 1),
            (328, 150) => new VisibleFits(firepower: 2, evasion: 1),
            (328, 152) => new VisibleFits(firepower: 2, evasion: 1),
            (328, 151) => new VisibleFits(firepower: 2, evasion: 1),
            (328, _) => new VisibleFits(firepower: 1, evasion: 1),

            // 35.6 k2
            (329, 591) => new VisibleFits(firepower: 4, torpedo: 2, aa: 1, evasion: 1),
            (329, 149) => new VisibleFits(firepower: 3, aa: 1, evasion: 1),
            (329, 150) => new VisibleFits(firepower: 3, aa: 1, evasion: 1),
            (329, 152) => new VisibleFits(firepower: 3, aa: 1, evasion: 1),
            (329, 151) => new VisibleFits(firepower: 3, aa: 1, evasion: 1),
            (329, _) => new VisibleFits(firepower: 1, evasion: 1),

            // T13 kai
            (106, 151) => new VisibleFits(firepower: 1, aa: 2, evasion: 3, armor: 1),

            // sub torp
            (67, 151) => new VisibleFits(torpedo: -5),

            // Kamikaze torp
            (174, 591) => new VisibleFits(torpedo: 6, evasion: 3),

            // nelson gun
            (298, 149) => new VisibleFits(firepower: 1, evasion: -3, armor: 1),
            (298, 150) => new VisibleFits(firepower: 1, evasion: -3, armor: 1),
            (298, 152) => new VisibleFits(firepower: 1, evasion: -3, armor: 1),
            (298, 151) => new VisibleFits(firepower: 1, evasion: -3, armor: 1),

            // afct
            (299, 149) => new VisibleFits(firepower: 1, evasion: -3, armor: 1),
            (299, 150) => new VisibleFits(firepower: 1, evasion: -3, armor: 1),
            (299, 152) => new VisibleFits(firepower: 1, evasion: -3, armor: 1),
            (299, 151) => new VisibleFits(firepower: 1, evasion: -3, armor: 1),

            // fcr
            (300, 149) => new VisibleFits(firepower: 1, evasion: -3, armor: 1),
            (300, 150) => new VisibleFits(firepower: 1, evasion: -3, armor: 1),
            (300, 152) => new VisibleFits(firepower: 1, evasion: -3, armor: 1),
            (300, 151) => new VisibleFits(firepower: 1, evasion: -3, armor: 1),

            _ => new VisibleFits()
        };

        private VisibleFits VisibleFitsWarspite(IEquipmentDataCustom equip) => equip.ID switch
        {
            // nelson gun (afct, fcr)
            298 => new VisibleFits(firepower: 2, evasion: -2, armor: 1),
            299 => new VisibleFits(firepower: 2, evasion: -2, armor: 1),
            300 => new VisibleFits(firepower: 2, evasion: -2, armor: 1),

            // brit rocket
            301 => new VisibleFits(firepower: 2, evasion: 1, armor: 1),

            _ => new VisibleFits()
        };

        private VisibleFits VisibleFitsBurger(IEquipmentDataCustom equip) => equip.ID switch
        {
            // gfcs mk37 
            307 => new VisibleFits(firepower: 1, aa: 1, evasion: 1),

            // sg initial
            315 => new VisibleFits(firepower: 2, evasion: 3, los: 4),

            _ => new VisibleFits()
        };

        /// <summary>
        /// 553 - Ise k2    <br/>
        /// 554 - Hyuuga k2
        /// </summary>
        private VisibleFits VisibleFitsIse(IEquipmentDataCustom equip) => (equip.ID, _ship.ShipID) switch
        {
            // 35.6 kai (ni)
            (328, _) => new VisibleFits(firepower: 1),
            (329, _) => new VisibleFits(firepower: 1),

            // 41k2
            (318, 553) => new VisibleFits(firepower: 2, aa: 2, evasion: 2),
            (318, 554) => new VisibleFits(firepower: 3, aa: 2, evasion: 2),

            // 41k2 triple
            (236, 553) => new VisibleFits(firepower: 3, aa: 2, evasion: 1),
            (236, 554) => new VisibleFits(firepower: 3, aa: 2, evasion: 2),

            (24, _) => new VisibleFits(firepower: 2), // Suisei
            (57, _) => new VisibleFits(firepower: 2), // 12A
            (111, _) => new VisibleFits(firepower: 2), // 601
            (100, _) => new VisibleFits(firepower: 4), // Egusa
            (291, _) => new VisibleFits(firepower: 6, evasion: 1), // 22 634
            (292, _) => new VisibleFits(firepower: 8, aa: 1, evasion: 2), // 22 634 skilled
            (319, _) => new VisibleFits(firepower: 7, aa: 3, evasion: 2), // 12 634 T3
            (320, 553) => new VisibleFits(firepower: 2), // 12 31
            (320, 554) => new VisibleFits(firepower: 4), // 12 31

            // Zuiun
            (79, _) => new VisibleFits(firepower: 3), // 634
            (81, _) => new VisibleFits(firepower: 3), // 12 634
            (237, _) => new VisibleFits(firepower: 4, evasion: 2), // 634 skilled
            (322, _) => new VisibleFits(firepower: 5, aa: 2, asw: 1, evasion: 2), // k2 634
            (323, _) => new VisibleFits(firepower: 6, aa: 3, asw: 2, evasion: 3), // k2 634 skilled

            // gyro kai
            (324, 553) => new VisibleFits(asw: 1, evasion: 1),
            (324, 554) => new VisibleFits(asw: 2, evasion: 1),

            // gyro k2
            (325, 553) => new VisibleFits(asw: 1, evasion: 1),
            (325, 554) => new VisibleFits(asw: 2, evasion: 1),

            // S-51J
            (326, 553) => new VisibleFits(firepower: 2, asw: 1, evasion: 2),
            (326, 554) => new VisibleFits(firepower: 3, asw: 2, evasion: 3),

            // S-51J
            (327, 553) => new VisibleFits(firepower: 1, asw: 3, evasion: 1),
            (327, 554) => new VisibleFits(firepower: 2, asw: 4, evasion: 2),

            // T2 recon
            (61, 553) when equip.Level == 10 => new VisibleFits(firepower: 5, evasion: 2, armor: 1, los: 3),
            (61, 553) when equip.Level > 5 => new VisibleFits(firepower: 4, evasion: 2, armor: 1, los: 2),
            (61, 553) when equip.Level > 3 => new VisibleFits(firepower: 4, evasion: 2, armor: 1, los: 1),
            (61, 553) when equip.Level > 1 => new VisibleFits(firepower: 3, evasion: 2, armor: 1, los: 1),
            (61, 553) => new VisibleFits(firepower: 3, evasion: 2, armor: 1),

            (61, 554) when equip.Level == 10 => new VisibleFits(firepower: 5, evasion: 3, armor: 3, los: 3),
            (61, 554) when equip.Level > 5 => new VisibleFits(firepower: 4, evasion: 3, armor: 3, los: 2),
            (61, 554) when equip.Level > 3 => new VisibleFits(firepower: 4, evasion: 3, armor: 3, los: 1),
            (61, 554) when equip.Level > 1 => new VisibleFits(firepower: 3, evasion: 3, armor: 3, los: 1),
            (61, 554) => new VisibleFits(firepower: 3, evasion: 3, armor: 3),

            _ => new VisibleFits()
        };

        /// <summary>
        /// 82  - Ise kai       <br/>
        /// 87  - Hyuuga kai    <br/>
        /// 411 - Fusou k2      <br/>
        /// 412 - Yamashiro k2  <br/>
        /// _   - Fusou kai (ni) class, Ise kai class
        /// </summary>
        private VisibleFits VisibleFitsAviationBattleship(IEquipmentDataCustom equip) => (equip.ID, _ship.ShipID) switch
        {
            // 35.6 kai
            (328, 411) => new VisibleFits(firepower: 1),
            (328, 412) => new VisibleFits(firepower: 1),

            // 35.6 kai ni
            (329, _) => new VisibleFits(firepower: 1),

            // 41k2
            (318, 82) => new VisibleFits(firepower: 2, aa: 2, evasion: 2),
            (318, 87) => new VisibleFits(firepower: 2, aa: 2, evasion: 2),
            (318, 411) => new VisibleFits(firepower: 1),
            (318, 412) => new VisibleFits(firepower: 1),

            // 41k2 triple
            (236, 82) => new VisibleFits(firepower: 2, aa: 2, evasion: 1),
            (236, 87) => new VisibleFits(firepower: 2, aa: 2, evasion: 1),
            (236, 411) => new VisibleFits(firepower: 1),
            (236, 412) => new VisibleFits(firepower: 1),

            // Zuiun (634)
            (79, 82) => new VisibleFits(firepower: 2),
            (79, 87) => new VisibleFits(firepower: 2),
            (79, 411) => new VisibleFits(firepower: 2),
            (79, 412) => new VisibleFits(firepower: 2),

            // Zuiun 12 (634)
            (81, 82) => new VisibleFits(firepower: 2),
            (81, 87) => new VisibleFits(firepower: 2),
            (81, 411) => new VisibleFits(firepower: 2),
            (81, 412) => new VisibleFits(firepower: 2),

            // Zuiun (634 skilled)
            (237, 82) => new VisibleFits(firepower: 3, evasion: 1),
            (237, 87) => new VisibleFits(firepower: 3, evasion: 1),
            (237, 411) => new VisibleFits(firepower: 2),
            (237, 412) => new VisibleFits(firepower: 2),

            _ => new VisibleFits()
        };

        private VisibleFits VisibleFitsNelson(IEquipmentDataCustom equip) => equip.ID switch
        {
            // nelson gun (afct, fcr)
            298 => new VisibleFits(firepower: 2, armor: 1),
            299 => new VisibleFits(firepower: 2, armor: 1),
            300 => new VisibleFits(firepower: 2, armor: 1),

            // brit rocket
            301 => new VisibleFits(firepower: 2, evasion: 1, armor: 1),

            // burger twin (mk1, mk5, mk8)
            330 => new VisibleFits(firepower: 2),
            331 => new VisibleFits(firepower: 2),
            332 => new VisibleFits(firepower: 2),

            _ => new VisibleFits()
        };

        /// <summary>
        /// 541 - Nagato k2 <br/>
        /// 573 - Mutsu k2  <br/>
        /// _   - Nagato kai class
        /// </summary>
        private VisibleFits VisibleFitsNagato(IEquipmentDataCustom equip) => (equip.ID, _ship.ShipID) switch
        {
            // 41k2
            (318, 541) => new VisibleFits(firepower: 3, aa: 2, evasion: 1),
            (318, 573) => new VisibleFits(firepower: 3, aa: 2, evasion: 1),

            // burger twin mk1
            (330, 541) => new VisibleFits(firepower: 2),
            (330, 573) => new VisibleFits(firepower: 2),
            (330, _) => new VisibleFits(firepower: 1),

            // burger twin mk5
            (331, 541) => new VisibleFits(firepower: 2),
            (331, 573) => new VisibleFits(firepower: 2),
            (331, _) => new VisibleFits(firepower: 1),

            // burger twin mk8
            (332, 541) => new VisibleFits(firepower: 2),
            (332, 573) => new VisibleFits(firepower: 2),
            (332, _) => new VisibleFits(firepower: 1),

            // T13 kai
            (106, 541) => new VisibleFits(firepower: 1, aa: 2, evasion: 3, armor: 1),

            _ => new VisibleFits()
        };















        private int GetAccuracy(IEquipmentDataCustom equip) => _ship.ShipClass switch
        {
            ShipClasses.Gangut => AccuracyGangut(equip),
            ShipClasses.Kongou => AccuracyKongou(equip),
            ShipClasses.Bismarck => AccuracyBismarck(equip),
            ShipClasses.VVeneto => AccuracyBismarck(equip),
            ShipClasses.Iowa => AccuracyBurger(equip),
            ShipClasses.Richelieu => AccuracyBaguette(equip),
            ShipClasses.QueenElizabeth => AccuracyWarspite(equip),
            ShipClasses.Nelson => AccuracyNelson(equip),
            ShipClasses.Yamato => AccuracyYamato(equip),

            ShipClasses.Nagato when _ship.ShipID == 541 => AccuracyNagato(equip),
            ShipClasses.Nagato when _ship.ShipID == 573 => AccuracyMutsu(equip),
            ShipClasses.Ise when _ship.ShipID == 553 || _ship.ShipID == 554 => AccuracyIse(equip), // Ise kai ni class

            _ when _ship.ShipType == ShipTypes.AviationBattleship => AccuracyAviationBattleship(equip),
            _ when _ship.ShipType == ShipTypes.Battleship => AccuracyBattleship(equip),

            _ => 0
        };

        private int AccuracyGangut(IEquipmentDataCustom equip) => equip.FitCategory switch
        {
            // (_) when equip.ID == 330 || equip.ID == 331 || equip.ID == 332 => null, // Colorado guns
            FitCategories.smallBBGun when equip.ID == 231 || equip.ID == 232 => 10, // Gangut guns
            FitCategories.smallBBGun => 7,
            FitCategories.baguetteBBGun => 0,
            FitCategories.pastaBBGun => 1,
            FitCategories.nelsonBBGun => -8,
            FitCategories.burgerBBGun => -3,
            FitCategories.mediumBBGun => -10,
            FitCategories.largeBBGun when equip.ID == 117 => -7, // p46
            FitCategories.largeBBGun => -18,

            _ => 0
        };

        private int AccuracyKongou(IEquipmentDataCustom equip) => equip.FitCategory switch
        {
            FitCategories.smallBBGun => 7,
            FitCategories.baguetteBBGun => 0,
            FitCategories.pastaBBGun => -2,
            FitCategories.nelsonBBGun when equip.ID == 299 => -6, // afct
            FitCategories.nelsonBBGun when equip.ID == 300 => -8, // fcr
            FitCategories.nelsonBBGun => -5,
            FitCategories.burgerBBGun when equip.ID == 183 => -6, // gfcs
            FitCategories.burgerBBGun => -5,
            FitCategories.mediumBBGun => -5,
            FitCategories.largeBBGun when equip.ID == 117 => -7, // p46
            FitCategories.largeBBGun => -10,

            _ => 0
        };

        /// <summary>
        /// Bisko and pastas
        /// </summary>
        private int AccuracyBismarck(IEquipmentDataCustom equip) => equip.FitCategory switch
        {
            FitCategories.smallBBGun => 4,
            FitCategories.baguetteBBGun => -2,
            FitCategories.pastaBBGun => 1,
            FitCategories.nelsonBBGun => -5,
            FitCategories.burgerBBGun when equip.ID == 183 => -4, // gfcs
            FitCategories.burgerBBGun => -5,
            FitCategories.mediumBBGun => -5,
            FitCategories.largeBBGun when equip.ID == 117 => -7, // p46
            FitCategories.largeBBGun => -10,

            _ => 0
        };

        private int AccuracyBurger(IEquipmentDataCustom equip) => equip.FitCategory switch
        {
            FitCategories.smallBBGun => 4,
            FitCategories.baguetteBBGun => -2,
            FitCategories.pastaBBGun => -2,
            FitCategories.nelsonBBGun => -5,
            FitCategories.burgerBBGun when equip.ID == 183 && _ship.IsMarried => 5, // gfcs
            FitCategories.burgerBBGun when equip.ID == 183 => -2, // gfcs
            FitCategories.burgerBBGun when _ship.IsMarried => -3,
            FitCategories.burgerBBGun => -5,
            FitCategories.mediumBBGun => -5,
            FitCategories.largeBBGun when equip.ID == 117 => -7, // p46
            FitCategories.largeBBGun => -10,

            _ => 0
        };

        private int AccuracyBaguette(IEquipmentDataCustom equip) => equip.FitCategory switch
        {
            FitCategories.smallBBGun => 4,
            FitCategories.baguetteBBGun => 4,
            FitCategories.pastaBBGun => -2,
            FitCategories.nelsonBBGun when equip.ID == 299 => -14, // afct
            FitCategories.nelsonBBGun when equip.ID == 300 => -8, // fcr
            FitCategories.nelsonBBGun => -7,
            FitCategories.burgerBBGun => -5,
            FitCategories.mediumBBGun => -5,
            FitCategories.largeBBGun when equip.ID == 117 => -7, // p46
            FitCategories.largeBBGun => -10,

            _ => 0
        };

        private int AccuracyWarspite(IEquipmentDataCustom equip) => equip.FitCategory switch
        {
            FitCategories.smallBBGun when equip.ID == 190 || equip.ID == 192 => 8, // Warspite guns
            FitCategories.smallBBGun => 6,
            FitCategories.baguetteBBGun => 0,
            FitCategories.pastaBBGun => 1,
            FitCategories.nelsonBBGun when equip.ID == 300 => 3, // fcr
            FitCategories.nelsonBBGun => 5,
            FitCategories.burgerBBGun => -2,
            FitCategories.mediumBBGun => 2,
            FitCategories.largeBBGun when equip.ID == 117 => -8, // p46
            FitCategories.largeBBGun => -11,

            _ => 0
        };

        private int AccuracyNelson(IEquipmentDataCustom equip) => equip.FitCategory switch
        {
            FitCategories.smallBBGun => 0,
            FitCategories.baguetteBBGun => 0,
            FitCategories.pastaBBGun => 0,
            FitCategories.nelsonBBGun => 5,
            FitCategories.burgerBBGun => 0,
            FitCategories.mediumBBGun when equip.ID == 318 => 0, // 41k2
            FitCategories.mediumBBGun => 3,
            FitCategories.largeBBGun => -2,

            _ => 0
        };

        /// <summary>
        /// Ise kai class, Fusou kai (ni) class
        /// </summary>
        private int AccuracyAviationBattleship(IEquipmentDataCustom equip) => equip.FitCategory switch
        {
            FitCategories.smallBBGun => 4,
            FitCategories.baguetteBBGun => 0,
            FitCategories.pastaBBGun => 2,
            FitCategories.nelsonBBGun => 0,
            FitCategories.burgerBBGun => 0,
            FitCategories.mediumBBGun => 2,
            FitCategories.largeBBGun when equip.ID == 117 => -3, // p46
            FitCategories.largeBBGun when equip.ID == 276 => -4, // 46kai
            FitCategories.largeBBGun => -7,

            _ => 0
        };

        /// <summary>
        /// Ise kai ni class
        /// </summary>
        private int AccuracyIse(IEquipmentDataCustom equip) => equip.FitCategory switch
        {
            FitCategories.smallBBGun => 4,
            FitCategories.baguetteBBGun => 0,
            FitCategories.pastaBBGun => 2,
            FitCategories.nelsonBBGun => 2,
            FitCategories.burgerBBGun => 2,
            FitCategories.mediumBBGun when equip.ID == 318 => 6, //41k2
            FitCategories.mediumBBGun => 4,
            FitCategories.largeBBGun when equip.ID == 117 => -3, // p46
            FitCategories.largeBBGun when equip.ID == 276 => -4, // 46kai
            FitCategories.largeBBGun => -7,

            _ => 0
        };

        /// <summary>
        /// Ise class, Fusou class, Nagato (kai) class
        /// </summary>
        private int AccuracyBattleship(IEquipmentDataCustom equip) => equip.FitCategory switch
        {
            FitCategories.smallBBGun => 2,
            FitCategories.baguetteBBGun => 0,
            FitCategories.pastaBBGun => 2,
            FitCategories.nelsonBBGun => 3,
            FitCategories.burgerBBGun => 2,
            FitCategories.mediumBBGun => 2,
            FitCategories.largeBBGun when equip.ID == 117 => -3, // p46
            FitCategories.largeBBGun when equip.ID == 276 => -4, // 46kai
            FitCategories.largeBBGun => -7,
            FitCategories.veryLargeBBGun => 0, // lol

            _ => 0
        };

        /// <summary>
        /// Nagato kai ni
        /// </summary>
        private int AccuracyNagato(IEquipmentDataCustom equip) => equip.FitCategory switch
        {
            FitCategories.smallBBGun => 2,
            FitCategories.baguetteBBGun => 0,
            FitCategories.pastaBBGun => 2,
            FitCategories.nelsonBBGun when equip.ID == 299 => 4, // afct
            FitCategories.nelsonBBGun => 2,
            FitCategories.burgerBBGun => 0,
            FitCategories.mediumBBGun when equip.ID == 318 => 6, //41k2
            FitCategories.mediumBBGun => 4,
            FitCategories.largeBBGun when equip.ID == 117 => -2, // p46
            FitCategories.largeBBGun when equip.ID == 276 => -6, // 46kai
            FitCategories.largeBBGun => -4,
            FitCategories.veryLargeBBGun => -5,

            _ => 0
        };

        /// <summary>
        /// Mutsu kai ni
        /// </summary>
        private int AccuracyMutsu(IEquipmentDataCustom equip) => equip.FitCategory switch
        {
            FitCategories.smallBBGun => 2,
            FitCategories.baguetteBBGun => 0,
            FitCategories.pastaBBGun => 2,
            FitCategories.nelsonBBGun => 2,
            FitCategories.burgerBBGun => 0,
            FitCategories.mediumBBGun when equip.ID == 318 => 6, //41k2
            FitCategories.mediumBBGun => 5,
            FitCategories.largeBBGun when equip.ID == 117 => -2, // p46
            FitCategories.largeBBGun when equip.ID == 276 => -6, // 46kai
            FitCategories.largeBBGun => -4,
            FitCategories.veryLargeBBGun => -8,

            _ => 0
        };

        private int AccuracyYamato(IEquipmentDataCustom equip) => equip.FitCategory switch
        {
            FitCategories.smallBBGun => 0,
            FitCategories.baguetteBBGun => 0,
            FitCategories.pastaBBGun => 0,
            FitCategories.nelsonBBGun => 0,
            FitCategories.burgerBBGun => 0,
            FitCategories.mediumBBGun => 0,
            FitCategories.largeBBGun when equip.ID == 276 => 7, // 46kai
            FitCategories.largeBBGun => 3,
            FitCategories.veryLargeBBGun => 0,

            _ => 0
        };
    }
}
