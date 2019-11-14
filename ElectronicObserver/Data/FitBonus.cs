// ReSharper disable ArgumentsStyleLiteral
// ReSharper disable ArgumentsStyleNamedExpression
using ElectronicObserver.Utility.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
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

        public static VisibleFits operator +(VisibleFits a, VisibleFits b) => new VisibleFits
        (
            firepower: a.Firepower + b.Firepower,
            torpedo: a.Torpedo + b.Torpedo,
            aa: a.AA + b.AA,
            asw: a.ASW + b.ASW,
            evasion: a.Evasion + b.Evasion,
            armor: a.Armor + b.Armor,
            los: a.LoS + b.LoS
        );

        public static VisibleFits operator -(VisibleFits a, VisibleFits b) => new VisibleFits
        (
            firepower: a.Firepower - b.Firepower,
            torpedo: a.Torpedo - b.Torpedo,
            aa: a.AA - b.AA,
            asw: a.ASW - b.ASW,
            evasion: a.Evasion - b.Evasion,
            armor: a.Armor - b.Armor,
            los: a.LoS - b.LoS
        );

        public static VisibleFits operator *(int a, VisibleFits b) => new VisibleFits
        (
            firepower: a * b.Firepower,
            torpedo: a * b.Torpedo,
            aa: a * b.AA,
            asw: a * b.ASW,
            evasion: a * b.Evasion,
            armor: a * b.Armor,
            los: a * b.LoS
        );
    }

    // how to not SQL
    public class FitBonusCustom
    {
        private IShipDataCustom Ship { get; }

        public VisibleFits VisibleFit =>
            new VisibleFits(Firepower, Torpedo, AA, ASW, Evasion, Armor, LoS);


        public int Firepower { get; set; }
        public int Torpedo { get; set; }
        public int AA { get; set; }
        public int ASW { get; set; }
        public int Evasion { get; set; }
        public int Armor { get; set; }
        public int LoS { get; set; }
        public int Accuracy { get; set; }

        public FitBonusCustom() : this(new VisibleFits())
        {
        }

        // todo find a better way for this
        public FitBonusCustom(VisibleFits synergies)
        {
            VisibleFits visibleFits = synergies;

            Firepower = visibleFits.Firepower;
            Torpedo = visibleFits.Torpedo;
            AA = visibleFits.AA;
            ASW = visibleFits.ASW;
            Evasion = visibleFits.Evasion;
            Armor = visibleFits.Armor;
            LoS = visibleFits.LoS;

            Accuracy = 0;
        }

        public FitBonusCustom(ShipDataCustom ship, EquipmentDataCustom equip)
        {
            Ship = ship;

            VisibleFits visibleFits = VisibleFitBonus(equip);

            Firepower = visibleFits.Firepower;
            Torpedo = visibleFits.Torpedo;
            AA = visibleFits.AA;
            ASW = visibleFits.ASW;
            Evasion = visibleFits.Evasion;
            Armor = visibleFits.Armor;
            LoS = visibleFits.LoS;

            Accuracy = GetAccuracy(equip);
        }

        private VisibleFits VisibleFitBonus(EquipmentDataCustom equip) => Ship.ShipClass switch
        {
            // why R# dying in here?
            ShipClasses.Kamikaze => VisibleFitsKamikaze(equip),
            ShipClasses.Mutsuki => VisibleFitsMutsuki(equip),
            ShipClasses.Fubuki => VisibleFitsFubuki(equip),
            ShipClasses.Ayanami => VisibleFitsAyanami(equip),
            ShipClasses.Akatsuki => VisibleFitsAkatsuki(equip),
            ShipClasses.Hatsuharu => VisibleFitsHatsuharu(equip),
            ShipClasses.Shiratsuyu => VisibleFitsShiratsuyu(equip),
            ShipClasses.Asashio => VisibleFitsAsashio(equip),
            ShipClasses.Kagerou => VisibleFitsKagerou(equip),
            ShipClasses.Yuugumo => VisibleFitsYuugumo(equip),
            ShipClasses.Akizuki => VisibleFitsAkizuki(equip),
            ShipClasses.Shimakaze => VisibleFitsShimakaze(equip),
            ShipClasses.Z1 => VisibleFitsZ1(equip),
            ShipClasses.Maestrale => VisibleFitsMaestrale(equip),
            ShipClasses.Fletcher => VisibleFitsAmericanDD(equip),
            ShipClasses.JohnCButler => VisibleFitsAmericanDD(equip),
            ShipClasses.J => VisibleFitsJ(equip),
            ShipClasses.Tashkent => VisibleFitsTashkent(equip),

            ShipClasses.Shimushu => VisibleFitsShimushu(equip),
            ShipClasses.Etorofu => VisibleFitsEtorofu(equip),
            // todo Mikura
            ShipClasses.Hiburi => VisibleFitsHiburi(equip),

            ShipClasses.Tenryuu => new VisibleFits(), // todo VisibleFitsTenryuu(equip),
            ShipClasses.Kuma => VisibleFitsKuma(equip),
            ShipClasses.Nagara => VisibleFitsNagara(equip),
            ShipClasses.Sendai => VisibleFitsSendai(equip),
            ShipClasses.Yuubari => VisibleFitsYuubari(equip),
            ShipClasses.Agano => VisibleFitsAgano(equip),
            ShipClasses.Ooyodo => VisibleFitsOoyodo(equip),
            // todo Luigi di Savoia Duca Degli Abruzzi
            ShipClasses.Gotland => VisibleFitsGotland(equip),

            ShipClasses.Furutaka => VisibleFitsFurutaka(equip),
            ShipClasses.Aoba => VisibleFitsAoba(equip),
            ShipClasses.Myoukou => new VisibleFits(), // todo VisibleFitsMyoukou(equip),
            ShipClasses.Takao => new VisibleFits(), // todo VisibleFitsTakao(equip),
            ShipClasses.Mogami => VisibleFitsMogami(equip),
            ShipClasses.Tone => new VisibleFits(), // todo VisibleFitsTone(equip),
            ShipClasses.AdmiralHipper => new VisibleFits(), // todo VisibleFitsAdmiralHipper(equip),
            ShipClasses.Zara => new VisibleFits(), // todo VisibleFitsZara(equip),

            ShipClasses.Kongou => VisibleFitsKongou(equip),
            ShipClasses.Ise when Ship.ShipID == ShipID.IseKaiNi || Ship.ShipID == ShipID.HyuugaKaiNi =>
            VisibleFitsIse(equip),
            _ when Ship.ShipType == ShipTypes.AviationBattleship => VisibleFitsAviationBattleship(equip),
            ShipClasses.Nagato => VisibleFitsNagato(equip),
            // todo Yamato
            // todo Bisko
            // todo VV
            ShipClasses.Iowa => VisibleFitsIowa(equip),
            // todo Colorado
            ShipClasses.QueenElizabeth => VisibleFitsWarspite(equip),
            ShipClasses.Nelson => VisibleFitsNelson(equip),
            // todo Richelieu
            // todo Gangut

            ShipClasses.Akagi => VisibleFitsAkagi(equip),
            ShipClasses.Kaga => VisibleFitsKaga(equip),
            ShipClasses.Souryuu => VisibleFitsSouryuu(equip),
            ShipClasses.Hiryuu => VisibleFitsHiryuu(equip),
            ShipClasses.Shoukaku => VisibleFitsShoukaku(equip),
            ShipClasses.Unryuu => new VisibleFits(), // todo VisibleFitsUnryuu(equip),
            ShipClasses.Taihou => VisibleFitsTaihou(equip),
            ShipClasses.GrafZeppelin => VisibleFitsGrafZeppelin(equip),
            ShipClasses.Aquila => VisibleFitsAquila(equip),
            ShipClasses.Lexington => VisibleFitsLexington(equip),
            ShipClasses.Essex => VisibleFitsEssex(equip),
            ShipClasses.ArkRoyal => VisibleFitsArkRoyal(equip),

            ShipClasses.Houshou => VisibleFitsHoushou(equip),
            ShipClasses.Ryuujou => VisibleFitsRyuujou(equip),
            ShipClasses.Shouhou => VisibleFitsShouhou(equip),
            ShipClasses.Hiyou => VisibleFitsHiyou(equip),
            ShipClasses.KasugaMaru => VisibleFitsTaiyou(equip),
            ShipClasses.Taiyou => VisibleFitsTaiyou(equip),
            ShipClasses.Casablanca => VisibleFitsCasablanca(equip),

            ShipClasses.神威型 => new VisibleFits(), // todo VisibleFitsKamoi(equip),
            ShipClasses.千歳型 => new VisibleFits(), // todo VisibleFitsChitose(equip),
            ShipClasses.瑞穂型 => new VisibleFits(), // todo VisibleFitsMizuho(equip),
            ShipClasses.日進型 => VisibleFitsNisshin(equip),
            ShipClasses.秋津洲型 => new VisibleFits(), // todo VisibleFitsAkitsushima(equip),
            ShipClasses.大鯨型 => VisibleFitsTaigei(equip),
            ShipClasses.改風早型 => new VisibleFits(), // todo VisibleFitsKazahaya(equip),
            ShipClasses.工作艦 => new VisibleFits(), // todo VisibleFitsRepairShip ??
            ShipClasses.香取型 => VisibleFitsKatori(equip),
            ShipClasses.巡潜甲型改二 => new VisibleFits(), // todo VisibleFitsI13(equip),
            ShipClasses.潜特型伊400型潜水艦 => new VisibleFits(), // todo VisibleFitsI400(equip),
            ShipClasses.海大VI型 => new VisibleFits(), // todo VisibleFitsI168(equip),
            ShipClasses.巡潜3型 => new VisibleFits(), // todo VisibleFitsI7(equip),
            ShipClasses.巡潜乙型 => new VisibleFits(), // todo VisibleFitsI15(equip),
            ShipClasses.巡潜乙型改二 => new VisibleFits(), // todo VisibleFitsI54(equip),
            ShipClasses.三式潜航輸送艇 => new VisibleFits(), // todo VisibleFitsMaruyu(equip),
            ShipClasses.特種船丙型 => new VisibleFits(), // todo VisibleFitsAkitsuMaru ??
            ShipClasses.UボートIXC型 => new VisibleFits(), // todo VisibleFitsIXC


            _ => new VisibleFits()
        };

        private int GetAccuracy(EquipmentDataCustom equip) => Ship.ShipClass switch
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

            ShipClasses.Nagato when Ship.ID == 541 => AccuracyNagato(equip),
            ShipClasses.Nagato when Ship.ID == 573 => AccuracyMutsu(equip),
            ShipClasses.Ise when Ship.ID == 553 || Ship.ID == 554 => AccuracyIse(equip), // Ise kai ni class

            _ when Ship.ShipType == ShipTypes.AviationBattleship => AccuracyAviationBattleship(equip),
            _ when Ship.ShipType == ShipTypes.Battleship => AccuracyBattleship(equip),

            _ => 0
        };

        #region BB visible fits

        /// <summary>
        /// 149 - Kongou k2     <br/>
        /// 150 - Hiei k2       <br/>
        /// 151 - Haruna k2     <br/>
        /// 152 - Kirishima k2  <br/>
        /// 591 - Kongou k2c    <br/>
        /// _   - kai Kongou class
        /// </summary>
        private VisibleFits VisibleFitsKongou(EquipmentDataCustom equip) => (equip.ID, ShipID: Ship.ID) switch
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
            (67, _) => new VisibleFits(torpedo: -5),

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

        private VisibleFits VisibleFitsWarspite(EquipmentDataCustom equip) => equip.ID switch
        {
            // nelson gun (afct, fcr)
            298 => new VisibleFits(firepower: 2, evasion: -2, armor: 1),
            299 => new VisibleFits(firepower: 2, evasion: -2, armor: 1),
            300 => new VisibleFits(firepower: 2, evasion: -2, armor: 1),

            // brit rocket
            301 => new VisibleFits(firepower: 2, evasion: 1, armor: 1),

            _ => new VisibleFits()
        };

        private VisibleFits VisibleFitsIowa(EquipmentDataCustom equip) => equip.ID switch
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
        private VisibleFits VisibleFitsIse(EquipmentDataCustom equip) => (equip.ID, ShipID: Ship.ID) switch
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
        private VisibleFits VisibleFitsAviationBattleship(EquipmentDataCustom equip) => (equip.ID, ShipID: Ship.ID) switch
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

        private VisibleFits VisibleFitsNelson(EquipmentDataCustom equip) => equip.ID switch
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
        private VisibleFits VisibleFitsNagato(EquipmentDataCustom equip) => (equip.ID, ShipID: Ship.ID) switch
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

        /// <summary>
        /// 601  - Colorado      <br/>
        /// 1496 - Colorado kai
        /// </summary>
        private VisibleFits VisibleFitsColorado(EquipmentDataCustom equip) => (equip.ID, ShipID: Ship.ID) switch
        {
            // burger twin mk1
            (330, _) => new VisibleFits(firepower: 1),

            // burger twin mk5
            (331, _) => new VisibleFits(firepower: 2, evasion: 1),

            // burger twin mk8
            (332, 601) => new VisibleFits(firepower: 1),
            (332, 1496) => new VisibleFits(firepower: 2, aa: 1, evasion: 1),

            // gfcs mk37 
            (307, _) => new VisibleFits(firepower: 1, aa: 1, evasion: 1),

            // sg initial
            (315, _) => new VisibleFits(firepower: 2, evasion: 3, los: 4),

            _ => new VisibleFits()
        };

        #endregion

        #region BB synergy

        /// <summary>
        /// 149 - Kongou k2     <br/>
        /// 150 - Hiei k2       <br/>
        /// 151 - Haruna k2     <br/>
        /// 152 - Kirishima k2  <br/>
        /// 591 - Kongou k2c    <br/>
        /// </summary>
        private VisibleFits SynergyKongou(ShipDataCustom ship)
        {
            VisibleFits synergy = new VisibleFits();

            // dazzle kai
            if (ship.Equipment.Any(eq => eq?.ID == 289))
            {
                synergy += ship.ID switch
                {
                    149 when ship.HasSurfaceRadar => new VisibleFits(firepower: 2),
                    151 when ship.HasSurfaceRadar => new VisibleFits(firepower: 2, evasion: 2),

                    _ => new VisibleFits()
                };
            }

            // sanshiki
            if (ship.Equipment.Any(eq => eq?.ID == 35))
            {
                synergy += ship.ID switch
                {
                    149 => new VisibleFits(firepower: 1, aa: 1),
                    591 => new VisibleFits(firepower: 1, aa: 1),
                    150 => new VisibleFits(aa: 1),
                    151 => new VisibleFits(aa: 1, evasion: 1),
                    152 => new VisibleFits(firepower: 1),

                    _ => new VisibleFits()
                };
            }

            // sanshiki kai
            if (ship.Equipment.Any(eq => eq?.ID == 317))
            {
                synergy += ship.ID switch
                {
                    149 => new VisibleFits(firepower: 3, aa: 3),
                    591 => new VisibleFits(firepower: 3, aa: 3),
                    150 => new VisibleFits(firepower: 2, aa: 2),
                    151 => new VisibleFits(firepower: 2, aa: 2, evasion: 1),
                    152 => new VisibleFits(firepower: 3, aa: 2),

                    _ => new VisibleFits(firepower: 1, aa: 1)
                };
            }

            // searchlight
            if (ship.Equipment.Any(eq => eq?.ID == 74))
            {
                VisibleFits fit = new VisibleFits(firepower: 2, evasion: -1);

                synergy += ship.ID switch
                {
                    86 => fit,
                    210 => fit,
                    150 => fit,

                    85 => fit,
                    212 => fit,
                    152 => fit,

                    _ => new VisibleFits()
                };
            }

            // large searchlight
            if (ship.Equipment.Any(eq => eq?.ID == 140))
            {
                VisibleFits fit = new VisibleFits(firepower: 3, evasion: -2);

                synergy += ship.ID switch
                {
                    86 => fit,
                    210 => fit,
                    150 => fit,

                    85 => fit,
                    212 => fit,
                    152 => fit,

                    _ => new VisibleFits()
                };
            }

            return synergy;
        }

        /// <summary>
        /// 82  - Ise kai       <br/>
        /// 87  - Hyuuga kai    <br/>
        /// 553 - Ise k2        <br/>
        /// 554 - Hyuuga k2
        /// </summary>
        private VisibleFits SynergyIse(ShipDataCustom ship)
        {
            VisibleFits synergy = new VisibleFits();

            // 41k2 triple
            if (ship.Equipment.Any(eq => eq?.ID == 290))
            {
                synergy += ship.ID switch
                {
                    82 when ship.HasAirRadar => new VisibleFits(aa: 2, evasion: 3),
                    87 when ship.HasAirRadar => new VisibleFits(aa: 2, evasion: 3),
                    553 when ship.HasAirRadar => new VisibleFits(aa: 2, evasion: 3),
                    554 when ship.HasAirRadar => new VisibleFits(aa: 2, evasion: 3),

                    _ => new VisibleFits()
                };

                // 41k2
                if (ship.Equipment.Any(eq => eq?.ID == 318))
                {
                    synergy += ship.ID switch
                    {
                        82 => new VisibleFits(evasion: 2, armor: 1),
                        87 => new VisibleFits(evasion: 2, armor: 1),
                        553 => new VisibleFits(evasion: 2, armor: 1),
                        554 => new VisibleFits(firepower: 1, evasion: 2, armor: 1),

                        _ => new VisibleFits()
                    };
                }
            }

            return synergy;
        }

        /// <summary>
        /// 541 - Nagato k2 <br/>
        /// 573 - Mutsu k2  <br/>
        /// </summary>
        private VisibleFits SynergyNagato(ShipDataCustom ship)
        {
            if (ship.ID != 541 && ship.ID != 571)
            {
                return new VisibleFits();
            }

            VisibleFits synergy = new VisibleFits();

            // 41k2 triple && 41k2
            if (ship.Equipment.Any(eq => eq?.ID == 290) && ship.Equipment.Any(eq => eq?.ID == 318))
            {
                synergy += ship.ID switch
                {
                    541 => new VisibleFits(firepower: 2, evasion: 2, armor: 1),
                    573 => new VisibleFits(firepower: 2, evasion: 2, armor: 1),

                    _ => new VisibleFits()
                };
            }

            // sanshiki kai
            if (ship.Equipment.Any(eq => eq?.ID == 317))
            {
                synergy += ship.ID switch
                {
                    541 => new VisibleFits(firepower: 1, aa: 2),
                    573 => new VisibleFits(firepower: 2, aa: 2, evasion: 1),

                    _ => new VisibleFits()
                };
            }

            return synergy;
        }

        #endregion

        #region BB accuracy

        private int AccuracyGangut(EquipmentDataCustom equip) => equip.FitCategory switch
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

        private int AccuracyKongou(EquipmentDataCustom equip) => equip.FitCategory switch
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
        private int AccuracyBismarck(EquipmentDataCustom equip) => equip.FitCategory switch
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

        private int AccuracyBurger(EquipmentDataCustom equip) => equip.FitCategory switch
        {
            FitCategories.smallBBGun => 4,
            FitCategories.baguetteBBGun => -2,
            FitCategories.pastaBBGun => -2,
            FitCategories.nelsonBBGun => -5,
            FitCategories.burgerBBGun when equip.ID == 183 && Ship.IsMarried => 5, // gfcs
            FitCategories.burgerBBGun when equip.ID == 183 => -2, // gfcs
            FitCategories.burgerBBGun when Ship.IsMarried => -3,
            FitCategories.burgerBBGun => -5,
            FitCategories.mediumBBGun => -5,
            FitCategories.largeBBGun when equip.ID == 117 => -7, // p46
            FitCategories.largeBBGun => -10,

            _ => 0
        };

        private int AccuracyBaguette(EquipmentDataCustom equip) => equip.FitCategory switch
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

        private int AccuracyWarspite(EquipmentDataCustom equip) => equip.FitCategory switch
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

        private int AccuracyNelson(EquipmentDataCustom equip) => equip.FitCategory switch
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
        private int AccuracyAviationBattleship(EquipmentDataCustom equip) => equip.FitCategory switch
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
        private int AccuracyIse(EquipmentDataCustom equip) => equip.FitCategory switch
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
        private int AccuracyBattleship(EquipmentDataCustom equip) => equip.FitCategory switch
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
        private int AccuracyNagato(EquipmentDataCustom equip) => equip.FitCategory switch
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
        private int AccuracyMutsu(EquipmentDataCustom equip) => equip.FitCategory switch
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

        private int AccuracyYamato(EquipmentDataCustom equip) => equip.FitCategory switch
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

        #endregion


        #region DD visible fits

        private VisibleFits VisibleFitsKamikaze(EquipmentDataCustom equip) => (equip.EquipID, Ship.ShipID) switch
        {
            (EquipID.SmallCalibreMainGun_12cmSingleGunKai2, _) => new VisibleFits(firepower: 2, aa: 1, evasion: 3),
            (EquipID.SmallCalibreMainGun_12_7cmSingleHighangleGun_LateModel, _) when equip.Level > 6 => new VisibleFits(firepower: 1, aa: 1),

            (EquipID.SmallCalibreMainGun_5inchSingleGunMk_30Kai_GFCSMk_37, _) => new VisibleFits(firepower: 1),

            (EquipID.Sonar_Type3ActiveSONAR, ShipID.Kamikaze) => new VisibleFits(firepower: 1, evasion: 2, asw: 3),
            (EquipID.Sonar_Type3ActiveSONAR, ShipID.KamikazeKai) => new VisibleFits(firepower: 1, evasion: 2, asw: 3),
            (EquipID.Sonar_Type3ActiveSONAR, ShipID.Harukaze) => new VisibleFits(firepower: 1, evasion: 2, asw: 3),
            (EquipID.Sonar_Type3ActiveSONAR, ShipID.HarukazeKai) => new VisibleFits(firepower: 1, evasion: 2, asw: 3),

            (EquipID.Torpedo_53cmBow_OxygenTorpedo, _) => new VisibleFits(torpedo: -5),

            _ => new VisibleFits()
        };

        private VisibleFits VisibleFitsMutsuki(EquipmentDataCustom equip) => (equip.EquipID, Ship.ShipID) switch
        {
            (EquipID.SmallCalibreMainGun_12cmSingleGunKai2, _) => new VisibleFits(firepower: 2, aa: 1, evasion: 3),
            (EquipID.SmallCalibreMainGun_12_7cmSingleHighangleGun_LateModel, _) when equip.Level > 6 => new VisibleFits(firepower: 1, aa: 1),

            (EquipID.SmallCalibreMainGun_5inchSingleGunMk_30Kai_GFCSMk_37, _) => new VisibleFits(firepower: 1),

            (EquipID.Torpedo_53cmBow_OxygenTorpedo, _) => new VisibleFits(torpedo: -5),

            _ => new VisibleFits()
        };

        private VisibleFits VisibleFitsFubuki(EquipmentDataCustom equip) => (equip.EquipID, Ship.ShipID) switch
        {
            (EquipID.SmallCalibreMainGun_12_7cmTwinGunModelA, _) => new VisibleFits(evasion: 2),
            (EquipID.SmallCalibreMainGun_12_7cmTwinGunModelAKai2, _) => new VisibleFits(firepower: 1),
            (EquipID.SmallCalibreMainGun_12_7cmTwinGunModelAKai3_WartimeModification_AAFD, _) => new VisibleFits(firepower: 2, aa: 2),

            (EquipID.SmallCalibreMainGun_5inchSingleGunMk_30Kai_GFCSMk_37, _) => new VisibleFits(firepower: 1),

            (EquipID.Torpedo_53cmBow_OxygenTorpedo, _) => new VisibleFits(torpedo: -5),

            _ => new VisibleFits()
        };

        private VisibleFits VisibleFitsAyanami(EquipmentDataCustom equip) => (equip.EquipID, Ship.ShipID) switch
        {
            (EquipID.SmallCalibreMainGun_12_7cmTwinGunModelA, _) => new VisibleFits(evasion: 1),
            (EquipID.SmallCalibreMainGun_12_7cmTwinGunModelAKai2, _) => new VisibleFits(firepower: 1),
            (EquipID.SmallCalibreMainGun_12_7cmTwinGunModelAKai3_WartimeModification_AAFD, _) => new VisibleFits(firepower: 2, aa: 2),
            
            (EquipID.SmallCalibreMainGun_12_7cmTwinGunModelBKai2, _) => new VisibleFits(aa: 1),
            (EquipID.SmallCalibreMainGun_12_7cmTwinGunModelBKai4_WartimeModification_AAFD, _) => new VisibleFits(firepower: 1),

            (EquipID.SmallCalibreMainGun_5inchSingleGunMk_30Kai_GFCSMk_37, _) => new VisibleFits(firepower: 1),

            (EquipID.SmallRadar_Type13AirRADARKai, ShipID.UshioKaiNi) => new VisibleFits(firepower: 1, aa:2, evasion:3, armor:1),

            (EquipID.Sonar_Type3ActiveSONAR, ShipID.Ushio) => new VisibleFits(evasion: 2, asw: 2),
            (EquipID.Sonar_Type3ActiveSONAR, ShipID.UshioKai) => new VisibleFits( evasion: 2, asw: 2),
            (EquipID.Sonar_Type3ActiveSONAR, ShipID.UshioKaiNi) => new VisibleFits( evasion: 2, asw:2),

            (EquipID.Torpedo_53cmBow_OxygenTorpedo, _) => new VisibleFits(torpedo: -5),

            _ => new VisibleFits()
        };

        private VisibleFits VisibleFitsAkatsuki(EquipmentDataCustom equip) => (equip.EquipID, Ship.ShipID) switch
        {
            (EquipID.SmallCalibreMainGun_12_7cmTwinGunModelA, _) => new VisibleFits(evasion: 1),
            (EquipID.SmallCalibreMainGun_12_7cmTwinGunModelAKai2, _) => new VisibleFits(firepower: 1),
            (EquipID.SmallCalibreMainGun_12_7cmTwinGunModelAKai3_WartimeModification_AAFD, _) => new VisibleFits(firepower: 2, aa: 2),
            
            (EquipID.SmallCalibreMainGun_12_7cmTwinGunModelBKai2, _) => new VisibleFits(aa: 1),
            (EquipID.SmallCalibreMainGun_12_7cmTwinGunModelBKai4_WartimeModification_AAFD, _) => new VisibleFits(firepower: 1),

            (EquipID.SmallCalibreMainGun_5inchSingleGunMk_30Kai_GFCSMk_37, _) => new VisibleFits(firepower: 1),

            (EquipID.SmallCalibreMainGun_130mmB13TwinGun, ShipID.Vérnyj) => new VisibleFits(firepower:2, armor:1),

            (EquipID.SmallRadar_Type13AirRADARKai, ShipID.Hibiki) => new VisibleFits(aa: 1, evasion: 3, armor: 1),
            (EquipID.SmallRadar_Type13AirRADARKai, ShipID.HibikiKai) => new VisibleFits(aa: 1, evasion: 3, armor: 1),
            (EquipID.SmallRadar_Type13AirRADARKai, ShipID.Vérnyj) => new VisibleFits(aa: 1, evasion: 3, armor: 1),

            (EquipID.Torpedo_533mmTripleTorpedo, ShipID.Vérnyj) => new VisibleFits(firepower:1, torpedo:3, armor:1),

            (EquipID.Sonar_Type3ActiveSONAR, ShipID.Ikazuchi) => new VisibleFits(evasion: 2, asw: 2),
            (EquipID.Sonar_Type3ActiveSONAR, ShipID.IkazuchiKai) => new VisibleFits(evasion: 2, asw: 2),

            (EquipID.Torpedo_53cmBow_OxygenTorpedo, _) => new VisibleFits(torpedo: -5),

            _ => new VisibleFits()
        };

        private VisibleFits VisibleFitsHatsuharu(EquipmentDataCustom equip) => (equip.EquipID, Ship.ShipID) switch
        {
            (EquipID.SmallCalibreMainGun_12_7cmTwinGunModelBKai2, _) => new VisibleFits(aa: 1),
            (EquipID.SmallCalibreMainGun_12_7cmTwinGunModelBKai4_WartimeModification_AAFD, _) => new VisibleFits(firepower: 1, evasion: 1),

            (EquipID.SmallCalibreMainGun_5inchSingleGunMk_30Kai_GFCSMk_37, _) => new VisibleFits(firepower: 1),

            (EquipID.SmallRadar_Type13AirRADARKai, ShipID.HatsushimoKaiNi) => new VisibleFits(firepower: 1, aa: 2, evasion: 3, armor: 1),

            (EquipID.Torpedo_53cmBow_OxygenTorpedo, _) => new VisibleFits(torpedo: -5),

            _ => new VisibleFits()
        };

        private VisibleFits VisibleFitsShiratsuyu(EquipmentDataCustom equip) => (equip.EquipID, Ship.ShipID) switch
        {
            (EquipID.SmallCalibreMainGun_12_7cmTwinGunModelBKai2, ShipID.ShiratsuyuKai) => new VisibleFits(evasion: 1),
            (EquipID.SmallCalibreMainGun_12_7cmTwinGunModelBKai2, ShipID.ShiratsuyuKaiNi) => new VisibleFits(evasion: 1),
            (EquipID.SmallCalibreMainGun_12_7cmTwinGunModelBKai2, ShipID.MurasameKaiNi) => new VisibleFits(evasion: 1),
            (EquipID.SmallCalibreMainGun_12_7cmTwinGunModelBKai2, ShipID.ShigureKaiNi) => new VisibleFits(firepower: 1),
            (EquipID.SmallCalibreMainGun_12_7cmTwinGunModelBKai2, ShipID.YuudachiKaiNi) => new VisibleFits(firepower: 1, torpedo: 1, aa:1, evasion:2),
            (EquipID.SmallCalibreMainGun_12_7cmTwinGunModelBKai2, ShipID.KawakazeKaiNi) => new VisibleFits(evasion:2),

            (EquipID.SmallCalibreMainGun_12_7cmTwinGunModelBKai4_WartimeModification_AAFD, ShipID.ShiratsuyuKaiNi) => new VisibleFits(firepower:2,evasion: 2),
            (EquipID.SmallCalibreMainGun_12_7cmTwinGunModelBKai4_WartimeModification_AAFD, ShipID.MurasameKaiNi) => new VisibleFits(firepower: 1,aa:1, evasion: 2),
            (EquipID.SmallCalibreMainGun_12_7cmTwinGunModelBKai4_WartimeModification_AAFD, ShipID.ShigureKaiNi) => new VisibleFits(firepower: 2, aa:1,evasion:1),
            (EquipID.SmallCalibreMainGun_12_7cmTwinGunModelBKai4_WartimeModification_AAFD, ShipID.YuudachiKaiNi) => new VisibleFits(firepower: 2, torpedo: 1, evasion: 1),
            (EquipID.SmallCalibreMainGun_12_7cmTwinGunModelBKai4_WartimeModification_AAFD, ShipID.KawakazeKaiNi) => new VisibleFits(firepower: 1,evasion: 2),
            (EquipID.SmallCalibreMainGun_12_7cmTwinGunModelBKai4_WartimeModification_AAFD, ShipID.UmikazeKaiNi) => new VisibleFits(firepower: 1,evasion: 2),
            (EquipID.SmallCalibreMainGun_12_7cmTwinGunModelBKai4_WartimeModification_AAFD, _) => new VisibleFits(firepower: 1),

            (EquipID.SmallCalibreMainGun_12_7cmTwinGunModelCKai2, ShipID.ShigureKaiNi) => new VisibleFits(firepower: 1, evasion: 1),
            (EquipID.SmallCalibreMainGun_12_7cmTwinGunModelCKai2, _) => new VisibleFits(firepower: 1),
            
            (EquipID.SmallCalibreMainGun_5inchSingleGunMk_30Kai_GFCSMk_37, _) => new VisibleFits(firepower: 1),

            (EquipID.SmallRadar_Type13AirRADARKai, ShipID.ShigureKaiNi) => new VisibleFits(firepower: 1, aa: 2, evasion: 3, armor: 1),

            (EquipID.Sonar_Type3ActiveSONAR, ShipID.Shigure) => new VisibleFits(firepower: 1, evasion: 2, asw: 3),
            (EquipID.Sonar_Type3ActiveSONAR, ShipID.ShigureKai) => new VisibleFits(firepower: 1, evasion: 2, asw: 3),
            (EquipID.Sonar_Type3ActiveSONAR, ShipID.ShigureKaiNi) => new VisibleFits(firepower: 1, evasion: 2, asw: 3),
            (EquipID.Sonar_Type3ActiveSONAR, ShipID.Yamakaze) => new VisibleFits(firepower: 1, evasion: 2, asw: 3),
            (EquipID.Sonar_Type3ActiveSONAR, ShipID.YamakazeKai) => new VisibleFits(firepower: 1, evasion: 2, asw: 3),


            (EquipID.Torpedo_53cmBow_OxygenTorpedo, _) => new VisibleFits(torpedo: -5),

            _ => new VisibleFits()
        };

        private VisibleFits VisibleFitsAsashio(EquipmentDataCustom equip) => (equip.EquipID, Ship.ShipID) switch
        {
            (EquipID.SmallCalibreMainGun_12_7cmTwinGunModelCKai2, _) => new VisibleFits(firepower: 1),

            (EquipID.SmallRadar_Type13AirRADARKai, ShipID.Kasumi) => new VisibleFits(aa: 2, evasion: 2, armor: 1),
            (EquipID.SmallRadar_Type13AirRADARKai, ShipID.KasumiKai) => new VisibleFits(aa: 2, evasion: 2, armor: 1),
            (EquipID.SmallRadar_Type13AirRADARKai, ShipID.KasumiKaiNi) => new VisibleFits(aa: 2, evasion: 2, armor: 1),
            (EquipID.SmallRadar_Type13AirRADARKai, ShipID.KasumiKaiNiB) => new VisibleFits(aa: 2, evasion: 2, armor: 1),

            (EquipID.Sonar_Type3ActiveSONAR, ShipID.Yamagumo) => new VisibleFits(evasion: 2, asw: 2),
            (EquipID.Sonar_Type3ActiveSONAR, ShipID.YamagumoKai) => new VisibleFits(evasion: 2, asw: 2),

            (EquipID.Torpedo_53cmBow_OxygenTorpedo, _) => new VisibleFits(torpedo: -5),

            _ => new VisibleFits()
        };

        private VisibleFits VisibleFitsKagerou(EquipmentDataCustom equip) => (equip.EquipID, Ship.ShipID) switch
        {
            (EquipID.SmallCalibreMainGun_12_7cmTwinGunModelCKai2, ShipID.YukikazeKai) => new VisibleFits(firepower: 1,
                evasion: 1),
            (EquipID.SmallCalibreMainGun_12_7cmTwinGunModelCKai2, ShipID.IsokazeBKai) => new VisibleFits(firepower: 1,
                evasion: 1),
            (EquipID.SmallCalibreMainGun_12_7cmTwinGunModelCKai2, ShipID.KagerouKaiNi) => new VisibleFits(),
            (EquipID.SmallCalibreMainGun_12_7cmTwinGunModelCKai2, ShipID.ShiranuiKaiNi) => new VisibleFits(),
            (EquipID.SmallCalibreMainGun_12_7cmTwinGunModelCKai2, ShipID.KuroshioKaiNi) => new VisibleFits(),
            (EquipID.SmallCalibreMainGun_12_7cmTwinGunModelCKai2, _) => new VisibleFits(firepower: 1),

            (EquipID.SmallCalibreMainGun_12_7cmTwinGunModelDKai2, ShipID.KagerouKaiNi) => new VisibleFits(),
            (EquipID.SmallCalibreMainGun_12_7cmTwinGunModelDKai2, ShipID.ShiranuiKaiNi) => new VisibleFits(),
            (EquipID.SmallCalibreMainGun_12_7cmTwinGunModelDKai2, ShipID.KuroshioKaiNi) => new VisibleFits(),
            (EquipID.SmallCalibreMainGun_12_7cmTwinGunModelDKai2, _) => new VisibleFits(firepower: 1, evasion: 1),

            (EquipID.SmallCalibreMainGun_5inchSingleGunMk_30Kai_GFCSMk_37, _) => new VisibleFits(firepower: 1),

            (EquipID.Searchlight_Searchlight, ShipID.Akigumo) => new VisibleFits(firepower: 1),
            (EquipID.Searchlight_Searchlight, ShipID.AkigumoKai) => new VisibleFits(firepower: 1),
            (EquipID.Searchlight_Searchlight, ShipID.Yukikaze) => new VisibleFits(aa: 1),
            (EquipID.Searchlight_Searchlight, ShipID.YukikazeKai) => new VisibleFits(aa: 1),

            (EquipID.SmallRadar_Type13AirRADARKai, ShipID.Yukikaze) => new VisibleFits(aa: 2, evasion: 2, armor: 1),
            (EquipID.SmallRadar_Type13AirRADARKai, ShipID.YukikazeKai) => new VisibleFits(aa: 2, evasion: 2, armor: 1),
            (EquipID.SmallRadar_Type13AirRADARKai, ShipID.Isokaze) => new VisibleFits(aa: 2, evasion: 2, armor: 1),
            (EquipID.SmallRadar_Type13AirRADARKai, ShipID.IsokazeKai) => new VisibleFits(aa: 2, evasion: 2, armor: 1),
            (EquipID.SmallRadar_Type13AirRADARKai, ShipID.IsokazeBKai) => new VisibleFits(aa: 2, evasion: 2, armor: 1),
            (EquipID.SmallRadar_Type13AirRADARKai, ShipID.Hamakaze) => new VisibleFits(aa: 2, evasion: 2, armor: 1),
            (EquipID.SmallRadar_Type13AirRADARKai, ShipID.HamakazeKai) => new VisibleFits(aa: 2, evasion: 2, armor: 1),
            (EquipID.SmallRadar_Type13AirRADARKai, ShipID.HamakazeBKai) => new VisibleFits(aa: 2, evasion: 2, armor: 1),

            (EquipID.Sonar_Type3ActiveSONAR, ShipID.Maikaze) => new VisibleFits(firepower: 1, evasion: 2, asw: 3),
            (EquipID.Sonar_Type3ActiveSONAR, ShipID.MaikazeKai) => new VisibleFits(firepower: 1, evasion: 2, asw: 3),

            (EquipID.Sonar_Type3ActiveSONAR, ShipID.Isokaze) => new VisibleFits(evasion: 2, asw: 2),
            (EquipID.Sonar_Type3ActiveSONAR, ShipID.IsokazeKai) => new VisibleFits(evasion: 2, asw: 2),
            (EquipID.Sonar_Type3ActiveSONAR, ShipID.IsokazeBKai) => new VisibleFits(evasion: 2, asw: 2),
            (EquipID.Sonar_Type3ActiveSONAR, ShipID.Hamakaze) => new VisibleFits(evasion: 2, asw: 2),
            (EquipID.Sonar_Type3ActiveSONAR, ShipID.HamakazeKai) => new VisibleFits(evasion: 2, asw: 2),
            (EquipID.Sonar_Type3ActiveSONAR, ShipID.HamakazeBKai) => new VisibleFits(evasion: 2, asw: 2),

            (EquipID.Torpedo_53cmBow_OxygenTorpedo, _) => new VisibleFits(torpedo: -5),

            _ => new VisibleFits()
        };

        private VisibleFits VisibleFitsYuugumo(EquipmentDataCustom equip) => (equip.EquipID, Ship.ShipID) switch
        {
            (EquipID.SmallCalibreMainGun_12_7cmTwinGunModelDKai2, ShipID.YuugumoKaiNi) => new VisibleFits(firepower: 3, evasion: 1),
            (EquipID.SmallCalibreMainGun_12_7cmTwinGunModelDKai2, ShipID.MakigumoKaiNi) => new VisibleFits(firepower: 3, evasion: 1),
            (EquipID.SmallCalibreMainGun_12_7cmTwinGunModelDKai2, ShipID.KazagumoKaiNi) => new VisibleFits(firepower: 3, evasion: 1),
            (EquipID.SmallCalibreMainGun_12_7cmTwinGunModelDKai2, ShipID.NaganamiKaiNi) => new VisibleFits(firepower: 3, evasion: 1),
            (EquipID.SmallCalibreMainGun_12_7cmTwinGunModelDKai2, ShipID.AsashimoKaiNi) => new VisibleFits(firepower: 3, evasion: 1),
            (EquipID.SmallCalibreMainGun_12_7cmTwinGunModelDKai2, _) => new VisibleFits(firepower: 2, evasion: 1),

            (EquipID.SmallCalibreMainGun_5inchSingleGunMk_30Kai_GFCSMk_37, _) => new VisibleFits(firepower: 1),

            (EquipID.SmallRadar_Type13AirRADARKai, ShipID.Asashimo) => new VisibleFits(aa: 2, evasion: 2, armor: 1),
            (EquipID.SmallRadar_Type13AirRADARKai, ShipID.AsashimoKai) => new VisibleFits(aa: 2, evasion: 2, armor: 1),
            (EquipID.SmallRadar_Type13AirRADARKai, ShipID.AsashimoKaiNi) => new VisibleFits(aa: 2, evasion: 2, armor: 1),

            (EquipID.Sonar_Type3ActiveSONAR, ShipID.Asashimo) => new VisibleFits(firepower: 1, evasion: 2, asw: 3),
            (EquipID.Sonar_Type3ActiveSONAR, ShipID.AsashimoKai) => new VisibleFits(firepower: 1, evasion: 2, asw: 3),
            (EquipID.Sonar_Type3ActiveSONAR, ShipID.AsashimoKaiNi) => new VisibleFits(firepower: 1, evasion: 2, asw: 3),

            (EquipID.Sonar_Type3ActiveSONAR, ShipID.Kishinami) => new VisibleFits(evasion: 2, asw: 2),
            (EquipID.Sonar_Type3ActiveSONAR, ShipID.KishinamiKai) => new VisibleFits(evasion: 2, asw: 2),

            (EquipID.Torpedo_53cmBow_OxygenTorpedo, _) => new VisibleFits(torpedo: -5),

            _ => new VisibleFits()
        };

        private VisibleFits VisibleFitsAkizuki(EquipmentDataCustom equip) => (equip.EquipID, Ship.ShipID) switch
        {
            (EquipID.SmallCalibreMainGun_5inchSingleGunMk_30Kai_GFCSMk_37, _) => new VisibleFits(firepower: 1),

            (EquipID.Torpedo_61cmQuintuple_OxygenTorpedo, _) => new VisibleFits(torpedo: 1),
            (EquipID.Torpedo_Prototype61cmSextuple_OxygenTorpedo, _) => new VisibleFits(torpedo: 1),

            (EquipID.SmallRadar_Type13AirRADARKai, ShipID.Suzutsuki) => new VisibleFits(aa: 2, evasion: 2, armor: 1),
            (EquipID.SmallRadar_Type13AirRADARKai, ShipID.SuzutsukiKai) => new VisibleFits(aa: 2, evasion: 2, armor: 1),

            (EquipID.Torpedo_53cmBow_OxygenTorpedo, _) => new VisibleFits(torpedo: -5),

            _ => new VisibleFits()
        };

        private VisibleFits VisibleFitsShimakaze(EquipmentDataCustom equip) => (equip.EquipID, Ship.ShipID) switch
        {
            (EquipID.SmallCalibreMainGun_12_7cmTwinGunModelDKai2, _) => new VisibleFits(firepower: 2, evasion: 1),
            (EquipID.SmallCalibreMainGun_5inchSingleGunMk_30Kai_GFCSMk_37, _) => new VisibleFits(firepower: 1),

            (EquipID.Torpedo_61cmQuintuple_OxygenTorpedo, _) => new VisibleFits(torpedo: 1),

            (EquipID.Torpedo_53cmBow_OxygenTorpedo, _) => new VisibleFits(torpedo: -5),

            _ => new VisibleFits()
        };

        private VisibleFits VisibleFitsZ1(EquipmentDataCustom equip) => (equip.EquipID, Ship.ShipID) switch
        {
            (EquipID.SmallCalibreMainGun_5inchSingleGunMk_30Kai_GFCSMk_37, _) => new VisibleFits(firepower: 1),

            (EquipID.Torpedo_53cmBow_OxygenTorpedo, _) => new VisibleFits(torpedo: -5),

            _ => new VisibleFits()
        };

        private VisibleFits VisibleFitsMaestrale(EquipmentDataCustom equip) => (equip.EquipID, Ship.ShipID) switch
        {
            (EquipID.SmallCalibreMainGun_5inchSingleGunMk_30Kai_GFCSMk_37, _) => new VisibleFits(firepower: 1),

            (EquipID.Torpedo_53cmBow_OxygenTorpedo, _) => new VisibleFits(torpedo: -5),

            _ => new VisibleFits()
        };

        private VisibleFits VisibleFitsAmericanDD(EquipmentDataCustom equip) => (equip.EquipID, Ship.ShipID) switch
        {
            (EquipID.SmallCalibreMainGun_5inchSingleGunMk_30Kai, _) => new VisibleFits(firepower: 2, aa: 2, evasion:1, armor:1),
            (EquipID.SmallCalibreMainGun_5inchSingleGunMk_30Kai_GFCSMk_37, _) => new VisibleFits(firepower: 2, aa: 1, evasion:1),

            (EquipID.Torpedo_533mmQuintupleTorpedo_InitialModel, _) => new VisibleFits(firepower: 1, torpedo: 3),

            (EquipID.SmallRadar_GFCSMk_37, _) => new VisibleFits(firepower: 1, aa: 1, evasion: 1),

            (EquipID.SmallRadar_SGRadar_InitialModel, _) => new VisibleFits(firepower: 3, evasion: 3, los:4),

            (EquipID.Torpedo_53cmBow_OxygenTorpedo, _) => new VisibleFits(torpedo: -5),

            _ => new VisibleFits()
        };

        private VisibleFits VisibleFitsJ(EquipmentDataCustom equip) => (equip.EquipID, Ship.ShipID) switch
        {
            (EquipID.SmallCalibreMainGun_5inchSingleGunMk_30Kai_GFCSMk_37, _) => new VisibleFits(firepower: 1),

            (EquipID.AntiAircraftMachineGun_20tube7inchUPRocketLaunchers, _) => new VisibleFits(aa: 2, evasion:1, armor:1),

            (EquipID.Torpedo_53cmBow_OxygenTorpedo, _) => new VisibleFits(torpedo: -5),

            _ => new VisibleFits()
        };

        private VisibleFits VisibleFitsTashkent(EquipmentDataCustom equip) => (equip.EquipID, Ship.ShipID) switch
        {
            (EquipID.SmallCalibreMainGun_5inchSingleGunMk_30Kai_GFCSMk_37, _) => new VisibleFits(firepower: 1),

            (EquipID.SmallCalibreMainGun_130mmB13TwinGun, _) => new VisibleFits(firepower: 2, armor: 1),

            (EquipID.Torpedo_533mmTripleTorpedo, _) => new VisibleFits(firepower: 1, torpedo: 3, armor: 1),

            (EquipID.Torpedo_53cmBow_OxygenTorpedo, _) => new VisibleFits(torpedo: -5),

            _ => new VisibleFits()
        };

        #endregion


        #region DD synergy

        private VisibleFits SynergyKamikaze(ShipDataCustom ship)
        {
            VisibleFits synergy = new VisibleFits();

            if (ship.Equipment.Any(eq => eq?.EquipID == EquipID.SmallCalibreMainGun_12cmSingleGunKai2))
            {
                synergy += ship.ID switch
                {
                    _ when ship.HasSurfaceRadar => new VisibleFits(firepower: 2, torpedo: 1, evasion: 3),

                    _ => new VisibleFits()
                };

                synergy += ship.Equipment.Count(eq => eq?.EquipID == EquipID.Torpedo_53cmTwinTorpedo) switch
                {
                    0 => new VisibleFits(),
                    1 => new VisibleFits(firepower: 2, torpedo: 4),
                    _ => new VisibleFits(firepower: 3, torpedo: 7)
                };
            }

            if (ship.Equipment.Any(eq => eq?.EquipID == EquipID.SmallCalibreMainGun_12_7cmSingleHighangleGun_LateModel && eq.Level > 6))
            {
                synergy += ship.ID switch
                {
                    _ when ship.HasSurfaceRadar => new VisibleFits(firepower: 2, evasion: 3),

                    _ => new VisibleFits()
                };
            }

            return synergy;
        }

        private VisibleFits SynergyMutsuki(ShipDataCustom ship)
        {
            VisibleFits synergy = new VisibleFits();

            if (ship.Equipment.Any(eq => eq?.EquipID == EquipID.SmallCalibreMainGun_12cmSingleGunKai2))
            {
                synergy += ship.ID switch
                {
                    _ when ship.HasSurfaceRadar => new VisibleFits(firepower: 2, torpedo: 1, evasion: 3),

                    _ => new VisibleFits()
                };

                synergy += ship.Equipment.Count(eq => eq?.EquipID == EquipID.Torpedo_53cmTwinTorpedo) switch
                {
                    0 => new VisibleFits(),
                    1 => new VisibleFits(firepower: 2, torpedo: 4),
                    _ => new VisibleFits(firepower: 3, torpedo: 7)
                };
            }

            if (ship.Equipment.Any(eq => eq?.EquipID == EquipID.SmallCalibreMainGun_12_7cmSingleHighangleGun_LateModel && eq.Level > 6))
            {
                synergy += ship.ID switch
                {
                    _ when ship.HasSurfaceRadar => new VisibleFits(firepower: 2, evasion: 3),

                    _ => new VisibleFits()
                };
            }

            return synergy;
        }

        private VisibleFits SynergyFubuki(ShipDataCustom ship)
        {
            VisibleFits synergy = new VisibleFits();

            if (ship.Equipment.Any(eq => eq?.EquipID == EquipID.SmallCalibreMainGun_12_7cmTwinGunModelAKai2))
            {
                synergy += ship.ShipID switch
                {
                    _ when ship.HasSurfaceRadar => new VisibleFits(firepower: 3, torpedo: 1, evasion: 2),

                    _ => new VisibleFits()
                };

                synergy += SpecialDestroyerTypeATripleTorpedo(ship);
            }

            if (ship.Equipment.Any(eq => eq?.EquipID == EquipID.SmallCalibreMainGun_12_7cmTwinGunModelAKai3_WartimeModification_AAFD))
            {
                synergy += ship.ShipID switch
                {
                    _ when ship.HasSurfaceRadar => new VisibleFits(firepower: 3, torpedo: 1, evasion: 2),

                    _ => new VisibleFits()
                };

                synergy += ship.ShipID switch
                {
                    _ when ship.HasAirRadar => new VisibleFits(aa: 6),

                    _ => new VisibleFits()
                };

                synergy += SpecialDestroyerTypeATripleTorpedo(ship);
            }

            switch (ship.ShipID)
            {
                case ShipID.FubukiKaiNi:
                case ShipID.MurakumoKaiNi:
                    synergy += TripleTorpedoLM(ship);
                    break;
            }

            return synergy;
        }

        /// <summary>
        /// </summary>
        private VisibleFits SynergyAyanami(ShipDataCustom ship)
        {
            VisibleFits synergy = new VisibleFits();

            if (ship.Equipment.Any(eq => eq?.EquipID == EquipID.SmallCalibreMainGun_12_7cmTwinGunModelAKai2))
            {
                synergy += ship.ShipID switch
                {
                    _ when ship.HasSurfaceRadar => new VisibleFits(firepower: 3, torpedo: 1, evasion: 2),

                    _ => new VisibleFits()
                };

                synergy += SpecialDestroyerTypeATripleTorpedo(ship);
            }

            if (ship.Equipment.Any(eq => eq?.EquipID == EquipID.SmallCalibreMainGun_12_7cmTwinGunModelAKai3_WartimeModification_AAFD))
            {
                synergy += ship.ShipID switch
                {
                    _ when ship.HasSurfaceRadar => new VisibleFits(firepower: 3, torpedo: 1, evasion: 2),

                    _ => new VisibleFits()
                };

                synergy += ship.ShipID switch
                {
                    _ when ship.HasAirRadar => new VisibleFits(aa: 6),

                    _ => new VisibleFits()
                };

                synergy += SpecialDestroyerTypeATripleTorpedo(ship);
            }

            if (ship.Equipment.Any(eq => eq?.EquipID == EquipID.SmallCalibreMainGun_12_7cmTwinGunModelBKai4_WartimeModification_AAFD))
            {
                synergy += ship.ShipID switch
                {
                    _ when ship.HasSurfaceRadar => new VisibleFits(firepower: 1, torpedo: 2, evasion: 2),

                    _ => new VisibleFits()
                };

                synergy += ship.ShipID switch
                {
                    _ when ship.HasAirRadar => new VisibleFits(aa: 5),

                    _ => new VisibleFits()
                };

                if (ship.Equipment.Any(eq => eq?.EquipID == EquipID.Torpedo_61cmTriple_OxygenTorpedoMountLateModel))
                {
                    synergy += new VisibleFits(firepower: 1, torpedo: 3);
                }
            }

            switch (ship.ShipID)
            {
                case ShipID.AyanamiKaiNi:
                case ShipID.UshioKaiNi:
                    synergy += TripleTorpedoLM(ship);
                    break;
            }

            return synergy;
        }

        private VisibleFits SynergyAkatsuki(ShipDataCustom ship)
        {
            VisibleFits synergy = new VisibleFits();

            if (ship.Equipment.Any(eq => eq?.EquipID == EquipID.SmallCalibreMainGun_12_7cmTwinGunModelAKai2))
            {
                synergy += ship.ShipID switch
                {
                    _ when ship.HasSurfaceRadar => new VisibleFits(firepower: 3, torpedo: 1, evasion: 2),

                    _ => new VisibleFits()
                };

                synergy += SpecialDestroyerTypeATripleTorpedo(ship);
            }

            if (ship.Equipment.Any(eq => eq?.EquipID == EquipID.SmallCalibreMainGun_12_7cmTwinGunModelAKai3_WartimeModification_AAFD))
            {
                synergy += ship.ShipID switch
                {
                    _ when ship.HasSurfaceRadar => new VisibleFits(firepower: 3, torpedo: 1, evasion: 2),

                    _ => new VisibleFits()
                };

                synergy += ship.ShipID switch
                {
                    _ when ship.HasAirRadar => new VisibleFits(aa: 6),

                    _ => new VisibleFits()
                };

                synergy += SpecialDestroyerTypeATripleTorpedo(ship);
            }

            if (ship.Equipment.Any(eq => eq?.EquipID == EquipID.SmallCalibreMainGun_12_7cmTwinGunModelBKai4_WartimeModification_AAFD))
            {
                synergy += ship.ShipID switch
                {
                    _ when ship.HasSurfaceRadar => new VisibleFits(firepower: 1, torpedo: 2, evasion: 2),

                    _ => new VisibleFits()
                };

                synergy += ship.ShipID switch
                {
                    _ when ship.HasAirRadar => new VisibleFits(aa: 5),

                    _ => new VisibleFits()
                };

                if (ship.Equipment.Any(eq => eq?.EquipID == EquipID.Torpedo_61cmTriple_OxygenTorpedoMountLateModel))
                {
                    synergy += new VisibleFits(firepower: 1, torpedo: 3);
                }
            }

            switch (ship.ShipID)
            {
                case ShipID.AkatsukiKaiNi:
                case ShipID.Vérnyj:
                    synergy += TripleTorpedoLM(ship);
                    break;
            }

            if (ship.Equipment.Any(eq => eq?.EquipID == EquipID.Searchlight_Searchlight))
            {
                synergy += ship.ShipID switch
                {
                    ShipID.Akatsuki => new VisibleFits(firepower: 2, evasion: -1),
                    ShipID.AkatsukiKai => new VisibleFits(firepower: 2, evasion: -1),
                    ShipID.AkatsukiKaiNi => new VisibleFits(firepower: 2, evasion: -1),

                    _ => new VisibleFits()
                };
            }

            return synergy;
        }

        private VisibleFits SynergyHatsuharu(ShipDataCustom ship)
        {
            VisibleFits synergy = new VisibleFits();

            if (ship.Equipment.Any(eq => eq?.EquipID == EquipID.SmallCalibreMainGun_12_7cmTwinGunModelBKai4_WartimeModification_AAFD))
            {
                synergy += ship.ShipID switch
                {
                    _ when ship.HasSurfaceRadar => new VisibleFits(firepower: 1, torpedo: 2, evasion: 2),

                    _ => new VisibleFits()
                };

                synergy += ship.ShipID switch
                {
                    _ when ship.HasAirRadar => new VisibleFits(aa: 5),

                    _ => new VisibleFits()
                };

                if (ship.Equipment.Any(eq => eq?.EquipID == EquipID.Torpedo_61cmTriple_OxygenTorpedoMountLateModel))
                {
                    synergy += new VisibleFits(firepower: 1, torpedo: 3);
                }
            }

            switch (ship.ShipID)
            {
                case ShipID.HatsuharuKaiNi:
                case ShipID.HatsushimoKaiNi:
                    synergy += TripleTorpedoLM(ship);
                    break;
            }

            return synergy;
        }

        private VisibleFits SynergyShiratsuyu(ShipDataCustom ship)
        {
            VisibleFits synergy = new VisibleFits();

            // 12.7Ck2
            if (ship.Equipment.Any(eq => eq?.ID == 266))
            {
                synergy += ship.ID switch
                {
                    _ when ship.HasSurfaceRadar => new VisibleFits(firepower: 1, torpedo: 3, evasion: 1),

                    _ => new VisibleFits()
                };
            }

            if (ship.Equipment.Any(eq => eq?.EquipID == EquipID.SmallCalibreMainGun_12_7cmTwinGunModelBKai4_WartimeModification_AAFD))
            {
                synergy += ship.ShipID switch
                {
                    _ when ship.HasSurfaceRadar => new VisibleFits(firepower: 1, torpedo: 3, evasion: 2),

                    _ => new VisibleFits()
                };

                synergy += ship.ShipID switch
                {
                    _ when ship.HasAirRadar => new VisibleFits(aa: 6),

                    _ => new VisibleFits()
                };

                if (ship.Equipment.Any(eq => eq?.EquipID == EquipID.Torpedo_61cmQuadruple_OxygenTorpedoMountLateModel))
                {
                    synergy += new VisibleFits(firepower: 1, torpedo: 3);
                }
            }

            switch (ship.ShipID)
            {
                case ShipID.ShiratsuyuKaiNi:
                case ShipID.ShigureKaiNi:
                case ShipID.MurasameKaiNi:
                case ShipID.YuudachiKaiNi:
                case ShipID.UmikazeKaiNi:
                case ShipID.KawakazeKaiNi:
                    synergy += QuadrupleTorpedoLM(ship);
                    break;
            }

            return synergy;
        }

        private VisibleFits SynergyAsashio(ShipDataCustom ship)
        {
            VisibleFits synergy = new VisibleFits();

            // 12.7Ck2
            if (ship.Equipment.Any(eq => eq?.ID == 266))
            {
                synergy += ship.ID switch
                {
                    _ when ship.HasSurfaceRadar => new VisibleFits(firepower: 1, torpedo: 3, evasion: 1),

                    _ => new VisibleFits()
                };
            }

            switch (ship.ShipID)
            {
                case ShipID.AsashioKaiNi:
                case ShipID.AsashioKaiNiD:
                case ShipID.OoshioKaiNi:
                case ShipID.MichishioKaiNi:
                case ShipID.ArashioKaiNi:
                case ShipID.ArareKaiNi:
                case ShipID.KasumiKaiNi:
                case ShipID.KasumiKaiNiB:
                    synergy += QuadrupleTorpedoLM(ship);
                    break;
            }

            return synergy;
        }

        private VisibleFits SynergyKagerou(ShipDataCustom ship)
        {
            VisibleFits synergy = new VisibleFits();

            if (ship.Equipment.Any(eq => eq?.EquipID == EquipID.SmallCalibreMainGun_12_7cmTwinGunModelCKai2))
            {
                if (ship.ShipID == ShipID.KagerouKaiNi || ship.ShipID == ShipID.ShiranuiKaiNi ||
                    ship.ShipID == ShipID.KuroshioKaiNi)
                {
                    synergy += ship.Equipment.Count(eq => eq?.EquipID == EquipID.SmallCalibreMainGun_12_7cmTwinGunModelCKai2) switch
                    {
                        1 => new VisibleFits(firepower: 2),
                        2 => new VisibleFits(firepower: 5),
                        _ => new VisibleFits(firepower: 6),
                    };
                }

                synergy += ship.ShipID switch
                {
                    _ when ship.HasSurfaceRadar => new VisibleFits(firepower: 2, torpedo: 3, evasion: 1),

                    _ => new VisibleFits()
                };
            }

            if (ship.Equipment.Any(eq => eq?.EquipID == EquipID.SmallCalibreMainGun_12_7cmTwinGunModelDKai2))
            {
                if (ship.ShipID == ShipID.KagerouKaiNi || ship.ShipID == ShipID.ShiranuiKaiNi ||
                    ship.ShipID == ShipID.KuroshioKaiNi)
                {
                    synergy += ship.Equipment.Count(eq => eq?.EquipID == EquipID.SmallCalibreMainGun_12_7cmTwinGunModelDKai2) switch
                    {
                        1 => new VisibleFits(firepower: 2, evasion:1),
                        2 => new VisibleFits(firepower: 3, evasion:2),
                        _ => new VisibleFits(firepower: 4, evasion: 3),
                    };
                }
            }

            switch (ship.ShipID)
            {
                case ShipID.KagerouKaiNi:
                case ShipID.ShiranuiKaiNi:
                case ShipID.KuroshioKaiNi:
                    synergy += QuadOxy(ship);
                    synergy += QuadrupleTorpedoLM(ship);
                    break;
            }

            return synergy;

            VisibleFits QuadOxy(ShipDataCustom ship) =>
                ship.Equipment.Count(eq => eq?.EquipID == EquipID.Torpedo_61cmQuadruple_OxygenTorpedo) switch
                {
                    0 => new VisibleFits(),
                    1 => new VisibleFits(torpedo: 2),
                    _ => 2 * new VisibleFits(torpedo: 2)
                };
        }

        private VisibleFits SynergyYuugumo(ShipDataCustom ship)
        {
            VisibleFits synergy = new VisibleFits();

            if (ship.Equipment.Any(eq => eq?.EquipID == EquipID.SmallCalibreMainGun_12_7cmTwinGunModelDKai2))
            {
                synergy += ship.ShipID switch
                {
                    ShipID.YuugumoKaiNi when ship.HasSurfaceRadar => new VisibleFits(firepower: 3, torpedo: 4, evasion: 3),
                    ShipID.MakigumoKaiNi when ship.HasSurfaceRadar => new VisibleFits(firepower: 3, torpedo: 4, evasion: 3),
                    ShipID.KazagumoKaiNi when ship.HasSurfaceRadar => new VisibleFits(firepower: 3, torpedo: 4, evasion: 3),
                    ShipID.NaganamiKaiNi when ship.HasSurfaceRadar => new VisibleFits(firepower: 3, torpedo: 4, evasion: 3),
                    ShipID.AsashimoKaiNi when ship.HasSurfaceRadar => new VisibleFits(firepower: 3, torpedo: 4, evasion: 3),

                    _ when ship.HasSurfaceRadar => new VisibleFits(firepower: 2, torpedo: 3, evasion: 1),

                    _ => new VisibleFits()
                };
            }

            switch (ship.ShipID)
            {
                case ShipID.YuugumoKaiNi:
                case ShipID.MakigumoKaiNi:
                case ShipID.KazagumoKaiNi:
                case ShipID.NaganamiKaiNi:
                case ShipID.AsashimoKaiNi:
                    synergy += QuadrupleTorpedoLM(ship);
                    break;
            }

            return synergy;
        }

        private VisibleFits SynergyShimakaze(ShipDataCustom ship)
        {
            VisibleFits synergy = new VisibleFits();

            if (ship.Equipment.Any(eq => eq?.EquipID == EquipID.SmallCalibreMainGun_12_7cmTwinGunModelDKai2))
            {
                synergy += ship.ShipID switch
                {
                    ShipID.ShimakazeKai when ship.HasSurfaceRadar => new VisibleFits(firepower: 1, torpedo:3, evasion: 2),

                    _ => new VisibleFits()
                };
            }

            return synergy;
        }

        #endregion






        #region DE visible fits

        /// <summary>
        /// </summary>
        private VisibleFits VisibleFitsShimushu(EquipmentDataCustom equip) => (equip.EquipID, Ship.ShipID) switch
        {
            (EquipID.SmallCalibreMainGun_12cmSingleGunKai2, _) => new VisibleFits(firepower: 1, aa: 1, evasion: 2),
            (EquipID.SmallCalibreMainGun_12_7cmSingleHighangleGun_LateModel, _) when equip.Level > 6 => new VisibleFits(firepower: 1, aa: 1),

            (EquipID.SmallCalibreMainGun_5inchSingleGunMk_30Kai_GFCSMk_37, _) => new VisibleFits(aa:1,evasion:1 ),

            _ => new VisibleFits()
        };

        /// <summary>
        /// </summary>
        private VisibleFits VisibleFitsEtorofu(EquipmentDataCustom equip) => (equip.EquipID, Ship.ShipID) switch
        {
            (EquipID.SmallCalibreMainGun_12cmSingleGunKai2, _) => new VisibleFits(firepower: 1, aa: 1, evasion: 2),
            (EquipID.SmallCalibreMainGun_12_7cmSingleHighangleGun_LateModel, _) when equip.Level > 6 => new VisibleFits(firepower: 1, aa: 1),

            (EquipID.SmallCalibreMainGun_5inchSingleGunMk_30Kai_GFCSMk_37, _) => new VisibleFits(aa: 1, evasion: 1),

            _ => new VisibleFits()
        };

        /// <summary>
        /// </summary>
        private VisibleFits VisibleFitsMikura(EquipmentDataCustom equip) => (equip.EquipID, Ship.ShipID) switch
        {
            (EquipID.SmallCalibreMainGun_12_7cmSingleHighangleGun_LateModel, _) when equip.Level > 6 => new VisibleFits(firepower: 1, aa: 1),

            (EquipID.SmallCalibreMainGun_5inchSingleGunMk_30Kai_GFCSMk_37, _) => new VisibleFits(aa: 1, evasion: 1),

            _ => new VisibleFits()
        };

        /// <summary>
        /// </summary>
        private VisibleFits VisibleFitsHiburi(EquipmentDataCustom equip) => (equip.EquipID, ShipID: Ship.ID) switch
        {
            (EquipID.SmallCalibreMainGun_12_7cmSingleHighangleGun_LateModel, _) when equip.Level > 6 => new VisibleFits(firepower: 1, aa: 1),

            (EquipID.SmallCalibreMainGun_5inchSingleGunMk_30Kai_GFCSMk_37, _) => new VisibleFits(aa: 1, evasion: 1),

            _ => new VisibleFits()
        };


        #endregion

        #region DE synergy

        /// <summary>
        /// </summary>
        private VisibleFits SynergyShimushu(ShipDataCustom ship)
        {
            VisibleFits synergy = new VisibleFits();

            // 12k2 single
            if (ship.Equipment.Any(eq => eq?.ID == 293))
            {
                synergy += ship.ID switch
                {
                    _ when ship.HasSurfaceRadar => new VisibleFits(firepower: 2, torpedo: 1, evasion: 3),

                    _ => new VisibleFits()
                };
            }

            // 12.7 LM
            if (ship.Equipment.Any(eq => eq?.ID == 229 && eq.Level > 6))
            {
                synergy += ship.ID switch
                {
                    _ when ship.HasSurfaceRadar => new VisibleFits(firepower: 1, evasion: 4),

                    _ => new VisibleFits()
                };
            }

            return synergy;
        }

        /// <summary>
        /// </summary>
        private VisibleFits SynergyEtorofu(ShipDataCustom ship)
        {
            VisibleFits synergy = new VisibleFits();

            // 12k2 single
            if (ship.Equipment.Any(eq => eq?.ID == 293))
            {
                synergy += ship.ID switch
                {
                    _ when ship.HasSurfaceRadar => new VisibleFits(firepower: 2, torpedo: 1, evasion: 3),

                    _ => new VisibleFits()
                };
            }

            // 12.7 LM
            if (ship.Equipment.Any(eq => eq?.ID == 229 && eq.Level > 6))
            {
                synergy += ship.ID switch
                {
                    _ when ship.HasSurfaceRadar => new VisibleFits(firepower: 1, evasion: 4),

                    _ => new VisibleFits()
                };
            }

            return synergy;
        }

        /// <summary>
        /// </summary>
        private VisibleFits SynergyMikura(ShipDataCustom ship)
        {
            VisibleFits synergy = new VisibleFits();

            // 12.7 LM
            if (ship.Equipment.Any(eq => eq?.ID == 229 && eq.Level > 6))
            {
                synergy += ship.ID switch
                {
                    _ when ship.HasSurfaceRadar => new VisibleFits(firepower: 1, evasion: 4),

                    _ => new VisibleFits()
                };
            }

            return synergy;
        }

        /// <summary>
        /// </summary>
        private VisibleFits SynergyHiburi(ShipDataCustom ship)
        {
            VisibleFits synergy = new VisibleFits();

            // 12.7 LM
            if (ship.Equipment.Any(eq => eq?.ID == 229 && eq.Level > 6))
            {
                synergy += ship.ID switch
                {
                    _ when ship.HasSurfaceRadar => new VisibleFits(firepower: 1, evasion: 4),

                    _ => new VisibleFits()
                };
            }

            return synergy;
        }

        #endregion






        #region CL visible fits

        private VisibleFits VisibleFitsKuma(EquipmentDataCustom equip) => (equip.EquipID, Ship.ShipID) switch
        {
            (EquipID.MediumCalibreMainGun_Bofors15_2cmTwinGunModel1930, _) => new VisibleFits(firepower: 1, aa: 1),

            (EquipID.Torpedo_61cmQuintuple_OxygenTorpedo, ShipID.KitakamiKai) => new VisibleFits(torpedo:1),
            (EquipID.Torpedo_61cmQuintuple_OxygenTorpedo, ShipID.KitakamiKaiNi) => new VisibleFits(torpedo:1),
            (EquipID.Torpedo_61cmQuintuple_OxygenTorpedo, ShipID.OoiKai) => new VisibleFits(torpedo:1),
            (EquipID.Torpedo_61cmQuintuple_OxygenTorpedo, ShipID.OoiKaiNi) => new VisibleFits(torpedo:1),
            (EquipID.Torpedo_61cmQuintuple_OxygenTorpedo, ShipID.KisoKaiNi) => new VisibleFits(torpedo:1),

            (EquipID.SeaplaneRecon_S9Osprey, _) => new VisibleFits(firepower: 1, asw: 1, evasion: 1),

            (EquipID.Torpedo_53cmBow_OxygenTorpedo, _) => new VisibleFits(torpedo: -5),

            _ => new VisibleFits()
        };

        private VisibleFits VisibleFitsNagara(EquipmentDataCustom equip) => (equip.EquipID, Ship.ShipID) switch
        {
            (EquipID.SmallCalibreMainGun_12_7cmSingleHighangleGun_LateModel, ShipID.YuraKaiNi) when equip.Level > 6 => new VisibleFits(firepower: 2, aa: 3),
            (EquipID.SmallCalibreMainGun_12_7cmSingleHighangleGun_LateModel, ShipID.KinuKaiNi) when equip.Level > 6 => new VisibleFits(firepower: 2, aa: 2),

            (EquipID.MediumCalibreMainGun_Bofors15_2cmTwinGunModel1930, _) => new VisibleFits(firepower: 1, aa: 1),

            (EquipID.SeaplaneRecon_S9Osprey, _) => new VisibleFits(firepower: 1, asw: 1, evasion: 1),

            (EquipID.Torpedo_53cmBow_OxygenTorpedo, _) => new VisibleFits(torpedo: -5),

            _ => new VisibleFits()
        };

        private VisibleFits VisibleFitsSendai(EquipmentDataCustom equip) => (equip.EquipID, Ship.ShipID) switch
        {
            (EquipID.MediumCalibreMainGun_Bofors15_2cmTwinGunModel1930, _) => new VisibleFits(firepower: 1, aa: 1),

            (EquipID.SeaplaneRecon_S9Osprey, _) => new VisibleFits(firepower: 1, asw: 1, evasion: 1),

            (EquipID.Torpedo_53cmBow_OxygenTorpedo, _) => new VisibleFits(torpedo: -5),

            _ => new VisibleFits()
        };

        private VisibleFits VisibleFitsYuubari(EquipmentDataCustom equip) => (equip.EquipID, Ship.ShipID) switch
        {
            (EquipID.SmallCalibreMainGun_130mmB13TwinGun, _) => new VisibleFits(firepower: 2, armor: 1),
            
            (EquipID.MediumCalibreMainGun_14cmTwinGun, _) => new VisibleFits(firepower: 1),
            (EquipID.MediumCalibreMainGun_14cmTwinGunKai, _) => new VisibleFits(firepower: 2, aa:1,evasion:1),

            (EquipID.Torpedo_53cmBow_OxygenTorpedo, _) => new VisibleFits(torpedo: -5),

            _ => new VisibleFits()
        };

        private VisibleFits VisibleFitsAgano(EquipmentDataCustom equip) => (equip.EquipID, Ship.ShipID) switch
        {
            (EquipID.MediumCalibreMainGun_Bofors15_2cmTwinGunModel1930, _) => new VisibleFits(firepower: 1, aa: 1),

            (EquipID.SeaplaneRecon_S9Osprey, _) => new VisibleFits(firepower: 1, asw: 1, evasion: 1),

            (EquipID.SmallRadar_Type13AirRADARKai, ShipID.Yahagi) => new VisibleFits(aa: 2, evasion: 2, armor: 1),
            (EquipID.SmallRadar_Type13AirRADARKai, ShipID.YahagiKai) => new VisibleFits(aa: 2, evasion: 2, armor: 1),

            (EquipID.Torpedo_53cmBow_OxygenTorpedo, _) => new VisibleFits(torpedo: -5),

            _ => new VisibleFits()
        };

        private VisibleFits VisibleFitsOoyodo(EquipmentDataCustom equip) => (equip.EquipID, Ship.ShipID) switch
        {
            (EquipID.SmallRadar_Type13AirRADARKai, ShipID.Ooyodo) => new VisibleFits(aa: 1, evasion: 3, armor: 1),
            (EquipID.SmallRadar_Type13AirRADARKai, ShipID.OoyodoKai) => new VisibleFits(aa: 3, evasion: 3, armor: 1),

            (EquipID.Torpedo_53cmBow_OxygenTorpedo, _) => new VisibleFits(torpedo: -5),

            _ => new VisibleFits()
        };

        private VisibleFits VisibleFitsItalianCL(EquipmentDataCustom equip) => (equip.EquipID, Ship.ShipID) switch
        {
            (EquipID.MediumCalibreMainGun_152mm55三連装速射砲, _) => new VisibleFits(firepower: 1, aa: 1, evasion:1),
            (EquipID.MediumCalibreMainGun_152mm55三連装速射砲改, _) => new VisibleFits(firepower: 2, aa: 1, evasion:1),

            (EquipID.Torpedo_53cmBow_OxygenTorpedo, _) => new VisibleFits(torpedo: -5),

            _ => new VisibleFits()
        };

        private VisibleFits VisibleFitsGotland(EquipmentDataCustom equip) => (equip.EquipID, Ship.ShipID) switch
        {
            (EquipID.MediumCalibreMainGun_Bofors15_2cmTwinGunModel1930, _) => new VisibleFits(firepower: 1, aa: 2, evasion:1),

            (EquipID.MediumCalibreMainGun_152mm55三連装速射砲改, _) => new VisibleFits(firepower: 1, aa: 1, evasion: 1),

            (EquipID.SeaplaneRecon_S9Osprey, _) => new VisibleFits(firepower: 1, asw: 2, evasion: 2),

            (EquipID.Torpedo_53cmBow_OxygenTorpedo, _) => new VisibleFits(torpedo: -5),

            _ => new VisibleFits()
        };

        private VisibleFits VisibleFitsKatori(EquipmentDataCustom equip) => (equip.EquipID, Ship.ShipID) switch
        {
            (EquipID.MediumCalibreMainGun_14cmTwinGun, _) => new VisibleFits(firepower: 1),
            (EquipID.MediumCalibreMainGun_14cmTwinGunKai, _) => new VisibleFits(firepower: 2, evasion: 1),

            (EquipID.SmallRadar_Type13AirRADARKai, ShipID.Kashima) => new VisibleFits(aa: 1, evasion: 3, armor: 1),
            (EquipID.SmallRadar_Type13AirRADARKai, ShipID.KashimaKai) => new VisibleFits(aa: 1, evasion: 3, armor: 1),

            (EquipID.Torpedo_53cmBow_OxygenTorpedo, _) => new VisibleFits(torpedo: -5),

            _ => new VisibleFits()
        };

        #endregion



        #region CL synergy

        private VisibleFits SynergyKuma(ShipDataCustom ship)
        {
            VisibleFits synergy = new VisibleFits();

            if (ship.Equipment.Any(eq => eq?.EquipID == EquipID.Bulge_Medium_ArcticCamouflage__ArcticEquipment))
            {
                synergy += ship.ShipID switch
                {
                    ShipID.TamaKai => new VisibleFits(evasion: 7, armor: 2),
                    ShipID.TamaKaiNi => new VisibleFits(evasion: 7, armor: 2),
                    ShipID.KisoKai => new VisibleFits(evasion: 7, armor: 2),
                    ShipID.KisoKaiNi => new VisibleFits(evasion: 7, armor: 2),

                    _ => new VisibleFits()
                };
            }

            return synergy;
        }

        private VisibleFits SynergyNagara(ShipDataCustom ship)
        {
            VisibleFits synergy = new VisibleFits();

            if (ship.Equipment.Any(eq => eq?.EquipID == EquipID.SmallCalibreMainGun_12_7cmSingleHighangleGun_LateModel && eq.Level > 6))
            {
                synergy += ship.ShipID switch
                {
                    ShipID.YuraKaiNi when ship.HasSurfaceRadar => new VisibleFits(firepower: 3, evasion:2),
                    ShipID.KinuKaiNi when ship.HasSurfaceRadar => new VisibleFits(firepower: 3, evasion: 2),

                    _ => new VisibleFits()
                };
            }

            return synergy;
        }

        private VisibleFits SynergySendai(ShipDataCustom ship)
        {
            VisibleFits synergy = new VisibleFits();

            if (ship.Equipment.Any(eq => eq?.EquipID == EquipID.Searchlight_Searchlight))
            {
                synergy += ship.ShipID switch
                {
                    ShipID.Jintsuu => new VisibleFits(firepower: 2, torpedo: 2, evasion: -1),
                    ShipID.JintsuuKai => new VisibleFits(firepower: 2, torpedo: 2, evasion: -1),
                    ShipID.JintsuuKaiNi => new VisibleFits(firepower: 2, torpedo: 2, evasion: -1),

                    _ => new VisibleFits()
                };
            }

            return synergy;
        }

        #endregion






        #region CA visible fits

        private VisibleFits VisibleFitsFurutaka(EquipmentDataCustom equip) => (equip.EquipID, Ship.ShipID) switch
        {
            (EquipID.MediumCalibreMainGun_20_3cm_No_2TwinGun, ShipID.FurutakaKaiNi) => new VisibleFits(firepower: 1),
            (EquipID.MediumCalibreMainGun_20_3cm_No_2TwinGun, ShipID.KakoKaiNi) => new VisibleFits(firepower: 1),

            (EquipID.Torpedo_53cmBow_OxygenTorpedo, _) => new VisibleFits(torpedo: -5),

            _ => new VisibleFits()
        };

        private VisibleFits VisibleFitsAoba(EquipmentDataCustom equip) => (equip.EquipID, Ship.ShipID) switch
        {
            (EquipID.MediumCalibreMainGun_20_3cm_No_2TwinGun, ShipID.KinugasaKai) => new VisibleFits(firepower: 1),
            (EquipID.MediumCalibreMainGun_20_3cm_No_2TwinGun, ShipID.KinugasaKaiNi) => new VisibleFits(firepower: 2, evasion: 1),
            (EquipID.MediumCalibreMainGun_20_3cm_No_2TwinGun, ShipID.AobaKai) => new VisibleFits(firepower: 1, aa:1),

            (EquipID.Torpedo_53cmBow_OxygenTorpedo, _) => new VisibleFits(torpedo: -5),

            _ => new VisibleFits()
        };

        private VisibleFits VisibleFitsMogami(EquipmentDataCustom equip) => (equip.EquipID, Ship.ShipID) switch
        {
            (EquipID.CarrierDiveBomber_彗星一二型_三一号光電管爆弾搭載機, ShipID.SuzuyaCVLKaiNi) => new VisibleFits(firepower: 4),
            (EquipID.CarrierDiveBomber_彗星一二型_三一号光電管爆弾搭載機, ShipID.KumanoCVLKaiNi) => new VisibleFits(firepower: 4),

            (EquipID.Torpedo_53cmBow_OxygenTorpedo, _) => new VisibleFits(torpedo: -5),

            _ => new VisibleFits()
        };

        #endregion



        #region CA synergy

        private VisibleFits SynergyFurutaka(ShipDataCustom ship)
        {
            VisibleFits synergy = new VisibleFits();

            if (ship.Equipment.Any(eq => eq?.EquipID == EquipID.MediumCalibreMainGun_20_3cm_No_2TwinGun))
            {
                synergy += ship.ShipID switch
                {
                    _ when ship.HasSurfaceRadar => new VisibleFits(firepower: 3, torpedo:2, evasion: 2),

                    _ => new VisibleFits()
                };
            }

            return synergy;
        }

        private VisibleFits SynergyAoba(ShipDataCustom ship)
        {
            VisibleFits synergy = new VisibleFits();

            if (ship.Equipment.Any(eq => eq?.EquipID == EquipID.MediumCalibreMainGun_20_3cm_No_2TwinGun))
            {
                synergy += ship.ShipID switch
                {
                    _ when ship.HasSurfaceRadar => new VisibleFits(firepower: 3, torpedo: 2, evasion: 2),

                    _ => new VisibleFits()
                };

                synergy += ship.ShipID switch
                {
                    ShipID.Aoba when ship.HasAirRadar => new VisibleFits(aa:5, evasion: 2),
                    ShipID.AobaKai when ship.HasAirRadar => new VisibleFits(aa:5, evasion: 2),

                    _ => new VisibleFits()
                };
            }

            return synergy;
        }

        private VisibleFits SynergyTakao(ShipDataCustom ship)
        {
            VisibleFits synergy = new VisibleFits();

            if (ship.Equipment.Any(eq => eq?.EquipID == EquipID.Searchlight_Searchlight))
            {
                synergy += ship.ShipID switch
                {
                    ShipID.Choukai => new VisibleFits(firepower: 2, evasion: -1),
                    ShipID.ChoukaiKai => new VisibleFits(firepower: 2, evasion: -1),
                    ShipID.ChoukaiKaiNi => new VisibleFits(firepower: 2, evasion: -1),

                    _ => new VisibleFits()
                };
            }

            return synergy;
        }

        private VisibleFits SynergyMogami(ShipDataCustom ship)
        {
            VisibleFits synergy = new VisibleFits();

            if (ship.Equipment.Any(eq => eq?.EquipID == EquipID.CarrierRecon_Type2ReconAircraft))
            {
                int level = ship.Equipment
                    .Where(eq => eq?.EquipID == EquipID.CarrierRecon_Type2ReconAircraft)
                    .Max(eq => eq.Level);

                synergy += ship.ShipID switch
                {
                    ShipID.SuzuyaCVLKaiNi when level == 10 => new VisibleFits(firepower: 3, los: 4),
                    ShipID.SuzuyaCVLKaiNi when level > 5 => new VisibleFits(firepower: 2, los: 3),
                    ShipID.SuzuyaCVLKaiNi when level > 3 => new VisibleFits(firepower: 2, los: 2),
                    ShipID.SuzuyaCVLKaiNi when level > 1 => new VisibleFits(firepower: 1, los: 2),
                    ShipID.SuzuyaCVLKaiNi when level == 1 => new VisibleFits(firepower: 1, los: 1),

                    ShipID.KumanoCVLKaiNi when level == 10 => new VisibleFits(firepower: 3, los: 4),
                    ShipID.KumanoCVLKaiNi when level > 5 => new VisibleFits(firepower: 2, los: 3),
                    ShipID.KumanoCVLKaiNi when level > 3 => new VisibleFits(firepower: 2, los: 2),
                    ShipID.KumanoCVLKaiNi when level > 1 => new VisibleFits(firepower: 1, los: 2),
                    ShipID.KumanoCVLKaiNi when level == 1 => new VisibleFits(firepower: 1, los: 1),

                    _ => new VisibleFits()
                };
            }

            return synergy;
        }

        #endregion








        #region CV visible fits

        private VisibleFits VisibleFitsAkagi(EquipmentDataCustom equip) => (equip.EquipID, Ship.ShipID) switch
        {
            (EquipID.CarrierFighter_烈風改二, ShipID.AkagiKai) => new VisibleFits(firepower: 1,aa: 1, evasion: 1),
            (EquipID.CarrierFighter_烈風改二, ShipID.AkagiKaiNi) => new VisibleFits(firepower: 1, aa: 2, evasion: 1),
            (EquipID.CarrierFighter_烈風改二, ShipID.AkagiKaiNiE) => new VisibleFits(firepower: 1, aa: 2, evasion: 1),

            (EquipID.CarrierFighter_烈風改_試製艦載型, ShipID.AkagiKai) => new VisibleFits(aa: 1, evasion: 1),
            (EquipID.CarrierFighter_烈風改_試製艦載型, ShipID.AkagiKaiNi) => new VisibleFits(aa: 2, evasion: 1),
            (EquipID.CarrierFighter_烈風改_試製艦載型, ShipID.AkagiKaiNiE) => new VisibleFits(aa: 2, evasion: 1),

            (EquipID.CarrierFighter_烈風改二戊型, ShipID.AkagiKai) => new VisibleFits(firepower: 1, aa:1, evasion:2),
            (EquipID.CarrierFighter_烈風改二戊型, ShipID.AkagiKaiNi) => new VisibleFits(firepower: 1, aa:2, evasion:3),
            (EquipID.CarrierFighter_烈風改二戊型, ShipID.AkagiKaiNiE) => new VisibleFits(firepower: 4, aa:3, evasion:4),

            (EquipID.CarrierFighter_烈風改二戊型_一航戦熟練, ShipID.AkagiKai) => new VisibleFits(firepower: 1, aa: 2, evasion: 2),
            (EquipID.CarrierFighter_烈風改二戊型_一航戦熟練, ShipID.AkagiKaiNi) => new VisibleFits(firepower: 1, aa: 3, evasion: 4),
            (EquipID.CarrierFighter_烈風改二戊型_一航戦熟練, ShipID.AkagiKaiNiE) => new VisibleFits(firepower: 6, aa: 4, evasion: 5),

            (EquipID.CarrierTorpedoBomber_Ryuusei, ShipID.AkagiKai) => new VisibleFits(firepower: 1),
            (EquipID.CarrierTorpedoBomber_Ryuusei, ShipID.AkagiKaiNi) => new VisibleFits(firepower: 1, evasion:1),
            (EquipID.CarrierTorpedoBomber_Ryuusei, ShipID.AkagiKaiNiE) => new VisibleFits(firepower: 2, evasion:1),

            (EquipID.CarrierTorpedoBomber_RyuuseiKai, ShipID.AkagiKai) => new VisibleFits(firepower: 1),
            (EquipID.CarrierTorpedoBomber_RyuuseiKai, ShipID.AkagiKaiNi) => new VisibleFits(firepower: 1, evasion: 1),
            (EquipID.CarrierTorpedoBomber_RyuuseiKai, ShipID.AkagiKaiNiE) => new VisibleFits(firepower: 2, evasion: 1),

            (EquipID.CarrierTorpedoBomber_流星改_一航戦, ShipID.AkagiKai) => new VisibleFits(firepower: 1),
            (EquipID.CarrierTorpedoBomber_流星改_一航戦, ShipID.AkagiKaiNi) => new VisibleFits(firepower: 2, aa:1, evasion: 1),
            (EquipID.CarrierTorpedoBomber_流星改_一航戦, ShipID.AkagiKaiNiE) => new VisibleFits(firepower: 3, aa:2, evasion: 2),

            (EquipID.CarrierTorpedoBomber_流星改_一航戦熟練, ShipID.AkagiKai) => new VisibleFits(firepower: 2),
            (EquipID.CarrierTorpedoBomber_流星改_一航戦熟練, ShipID.AkagiKaiNi) => new VisibleFits(firepower: 3, aa: 2, evasion: 1),
            (EquipID.CarrierTorpedoBomber_流星改_一航戦熟練, ShipID.AkagiKaiNiE) => new VisibleFits(firepower: 5, aa: 3, evasion: 3),

            (EquipID.CarrierTorpedoBomber_九七式艦攻改試製三号戊型_空六号電探改装備機, ShipID.AkagiKaiNiE) => new VisibleFits(firepower: 3),
            (EquipID.CarrierTorpedoBomber_九七式艦攻改_熟練試製三号戊型_空六号電探改装備機, ShipID.AkagiKaiNiE) => new VisibleFits(firepower: 3, evasion: 1),


            _ => new VisibleFits()
        };

        private VisibleFits VisibleFitsKaga(EquipmentDataCustom equip) => (equip.EquipID, Ship.ShipID) switch
        {
            (EquipID.CarrierFighter_烈風改二, ShipID.KagaKai) => new VisibleFits(firepower: 1, aa: 1, evasion: 1),
            (EquipID.CarrierFighter_烈風改_試製艦載型, ShipID.KagaKai) => new VisibleFits(aa: 1, evasion: 1),
            (EquipID.CarrierFighter_烈風改二戊型, ShipID.KagaKai) => new VisibleFits(firepower: 1, aa: 1, evasion: 2),
            (EquipID.CarrierFighter_烈風改二戊型_一航戦熟練, ShipID.KagaKai) => new VisibleFits(firepower: 1, aa: 2, evasion: 2),

            (EquipID.CarrierTorpedoBomber_Ryuusei, ShipID.KagaKai) => new VisibleFits(firepower: 1),
            (EquipID.CarrierTorpedoBomber_RyuuseiKai, ShipID.KagaKai) => new VisibleFits(firepower: 1),
            (EquipID.CarrierTorpedoBomber_流星改_一航戦, ShipID.KagaKai) => new VisibleFits(firepower: 1),
            (EquipID.CarrierTorpedoBomber_流星改_一航戦熟練, ShipID.KagaKai) => new VisibleFits(firepower: 2),


            _ => new VisibleFits()
        };

        private VisibleFits VisibleFitsSouryuu(EquipmentDataCustom equip) => (equip.EquipID, Ship.ShipID) switch
        {
            (EquipID.CarrierDiveBomber_彗星一二型_三一号光電管爆弾搭載機, ShipID.SouryuuKaiNi) => new VisibleFits(firepower: 3),

            _ => new VisibleFits()
        };

        private VisibleFits VisibleFitsHiryuu(EquipmentDataCustom equip) => (equip.EquipID, Ship.ShipID) switch
        {
            (EquipID.CarrierDiveBomber_彗星一二型_三一号光電管爆弾搭載機, ShipID.HiryuuKaiNi) => new VisibleFits(firepower: 3),

            _ => new VisibleFits()
        };

        private VisibleFits VisibleFitsShoukaku(EquipmentDataCustom equip) => (equip.EquipID, Ship.ShipID) switch
        {
            (EquipID.CarrierTorpedoBomber_流星改_一航戦, ShipID.ShoukakuKaiNi) => new VisibleFits(firepower: 1),
            (EquipID.CarrierTorpedoBomber_流星改_一航戦, ShipID.ShoukakuKaiNiA) => new VisibleFits(firepower: 1),

            (EquipID.CarrierTorpedoBomber_流星改_一航戦, ShipID.ZuikakuKaiNi) => new VisibleFits(firepower: 1),
            (EquipID.CarrierTorpedoBomber_流星改_一航戦, ShipID.ZuikakuKaiNiA) => new VisibleFits(firepower: 1),

            (EquipID.CarrierTorpedoBomber_流星改_一航戦熟練, ShipID.ShoukakuKaiNi) => new VisibleFits(firepower: 1),
            (EquipID.CarrierTorpedoBomber_流星改_一航戦熟練, ShipID.ShoukakuKaiNiA) => new VisibleFits(firepower: 1),

            (EquipID.CarrierTorpedoBomber_流星改_一航戦熟練, ShipID.ZuikakuKaiNi) => new VisibleFits(firepower: 1),
            (EquipID.CarrierTorpedoBomber_流星改_一航戦熟練, ShipID.ZuikakuKaiNiA) => new VisibleFits(firepower: 1),

            _ => new VisibleFits()
        };

        private VisibleFits VisibleFitsTaihou(EquipmentDataCustom equip) => (equip.EquipID, Ship.ShipID) switch
        {
            (EquipID.CarrierTorpedoBomber_Ryuusei, ShipID.TaihouKai) => new VisibleFits(firepower: 1),
            (EquipID.CarrierTorpedoBomber_RyuuseiKai, ShipID.TaihouKai) => new VisibleFits(firepower: 1),

            _ => new VisibleFits()
        };

        private VisibleFits VisibleFitsGrafZeppelin(EquipmentDataCustom equip) => (equip.EquipID, Ship.ShipID) switch
        {
            (EquipID.CarrierFighter_Re_2005Kai, _) => new VisibleFits(aa: 1, evasion: 2),

            (EquipID.CarrierDiveBomber_Ju87CKai2_KMX, _) => new VisibleFits(firepower: 1, evasion: 1),
            (EquipID.CarrierDiveBomber_Ju87CKai2_KMXSkilled, _) => new VisibleFits(firepower: 1, evasion: 1),

            _ => new VisibleFits()
        };

        private VisibleFits VisibleFitsAquila(EquipmentDataCustom equip) => (equip.EquipID, Ship.ShipID) switch
        {
            (EquipID.CarrierFighter_Re_2001ORKai, _) => new VisibleFits(firepower: 1, aa: 2, evasion:3),
            (EquipID.CarrierFighter_Re_2005Kai, _) => new VisibleFits(aa: 1, evasion:2),

            (EquipID.CarrierDiveBomber_Re_2001CB改, _) => new VisibleFits(firepower: 4, aa: 1, evasion:1),

            (EquipID.CarrierDiveBomber_Ju87CKai2_KMX, _) => new VisibleFits(firepower: 1, evasion: 1),
            (EquipID.CarrierDiveBomber_Ju87CKai2_KMXSkilled, _) => new VisibleFits(firepower: 1, evasion: 1),

            (EquipID.CarrierTorpedoBomber_Re_2001GKai, _) => new VisibleFits(firepower: 3, aa: 1, evasion: 1),

            _ => new VisibleFits()
        };

        private VisibleFits VisibleFitsLexington(EquipmentDataCustom equip) => (equip.EquipID, Ship.ShipID) switch
        {
            (EquipID.SmallRadar_GFCSMk_37, _) => new VisibleFits(firepower: 1, aa: 1, evasion: 1),

            (EquipID.SmallRadar_SGRadar_InitialModel, _) => new VisibleFits(firepower: 2, evasion: 3, los: 4),

            _ => new VisibleFits()
        };

        private VisibleFits VisibleFitsEssex(EquipmentDataCustom equip) => (equip.EquipID, Ship.ShipID) switch
        {
            (EquipID.SmallRadar_GFCSMk_37, _) => new VisibleFits(firepower: 1, aa: 1, evasion: 1),

            (EquipID.SmallRadar_SGRadar_InitialModel, _) => new VisibleFits(firepower: 2, evasion: 3, los: 4),

            _ => new VisibleFits()
        };

        private VisibleFits VisibleFitsArkRoyal(EquipmentDataCustom equip) => (equip.EquipID, Ship.ShipID) switch
        {
            (EquipID.AntiAircraftMachineGun_20tube7inchUPRocketLaunchers, _) => new VisibleFits(aa: 2, evasion: 1, armor: 1),

            _ => new VisibleFits()
        };

        private VisibleFits VisibleFitsHoushou(EquipmentDataCustom equip) => (equip.EquipID, Ship.ShipID) switch
        {
            (EquipID.CarrierFighter_Type96Fighter, _) => new VisibleFits(firepower: 1, aa: 1, asw:1, evasion: 2),
            (EquipID.CarrierFighter_Type96FighterKai, _) => new VisibleFits(firepower: 1, aa: 3, asw: 2, evasion:3),

            _ => new VisibleFits()
        };

        private VisibleFits VisibleFitsRyuujou(EquipmentDataCustom equip) => (equip.EquipID, Ship.ShipID) switch
        {
            (EquipID.CarrierFighter_Type96Fighter, _) => new VisibleFits(aa: 1, evasion: 1),
            (EquipID.CarrierFighter_Type96FighterKai, _) => new VisibleFits(aa: 1, asw: 2, evasion: 1),

            _ => new VisibleFits()
        };

        private VisibleFits VisibleFitsShouhou(EquipmentDataCustom equip) => (equip.EquipID, Ship.ShipID) switch
        {
            (EquipID.CarrierFighter_Type96Fighter, _) => new VisibleFits(aa: 1, evasion: 1),
            (EquipID.CarrierFighter_Type96FighterKai, _) => new VisibleFits(aa: 1, asw: 2, evasion: 1),

            (EquipID.CarrierTorpedoBomber_九七式艦攻改試製三号戊型_空六号電探改装備機, ShipID.ShouhouKai) => new VisibleFits(firepower: 2, asw: 1),
            (EquipID.CarrierTorpedoBomber_九七式艦攻改試製三号戊型_空六号電探改装備機, ShipID.ZuihouKaiNi) => new VisibleFits(firepower: 2, asw: 2),
            (EquipID.CarrierTorpedoBomber_九七式艦攻改試製三号戊型_空六号電探改装備機, ShipID.ZuihouKaiNiB) => new VisibleFits(firepower: 2, asw: 2),

            (EquipID.CarrierTorpedoBomber_九七式艦攻改_熟練試製三号戊型_空六号電探改装備機, ShipID.ShouhouKai) => new VisibleFits(firepower: 3, evasion: 1, asw: 1),
            (EquipID.CarrierTorpedoBomber_九七式艦攻改_熟練試製三号戊型_空六号電探改装備機, ShipID.ZuihouKaiNi) => new VisibleFits(firepower: 3, evasion: 1, asw: 1),
            (EquipID.CarrierTorpedoBomber_九七式艦攻改_熟練試製三号戊型_空六号電探改装備機, ShipID.ZuihouKaiNiB) => new VisibleFits(firepower: 3, evasion: 3, asw: 2),


            _ => new VisibleFits()
        };

        private VisibleFits VisibleFitsHiyou(EquipmentDataCustom equip) => (equip.EquipID, Ship.ShipID) switch
        {
            (EquipID.CarrierFighter_Type96Fighter, _) => new VisibleFits(aa: 1, evasion: 1),
            (EquipID.CarrierFighter_Type96FighterKai, _) => new VisibleFits(aa: 1, asw: 2, evasion: 1),

            _ => new VisibleFits()
        };

        /// <summary>
        /// Kasuga Maru, Taiyou
        /// </summary>
        private VisibleFits VisibleFitsTaiyou(EquipmentDataCustom equip) => (equip.EquipID, Ship.ShipID) switch
        {
            (EquipID.CarrierFighter_Type96Fighter, _) => new VisibleFits(firepower: 1, aa: 1, asw: 2, evasion: 1),
            (EquipID.CarrierFighter_Type96FighterKai, _) => new VisibleFits(firepower: 1, aa: 2, asw: 4, evasion: 2),

            (EquipID.CarrierDiveBomber_Ju87CKai2_KMX, ShipID.KasugaMaru) => new VisibleFits(asw: 1, evasion: 1),
            (EquipID.CarrierDiveBomber_Ju87CKai2_KMX, ShipID.Taiyou) => new VisibleFits(asw: 1, evasion: 1),
            (EquipID.CarrierDiveBomber_Ju87CKai2_KMX, ShipID.TaiyouKai) => new VisibleFits(asw: 1, evasion: 1),
            (EquipID.CarrierDiveBomber_Ju87CKai2_KMX, ShipID.TaiyouKaiNi) => new VisibleFits(asw: 1, evasion: 1),

            (EquipID.CarrierDiveBomber_Ju87CKai2_KMXSkilled, ShipID.Shinyou) => new VisibleFits(asw: 3, evasion:2),
            (EquipID.CarrierDiveBomber_Ju87CKai2_KMXSkilled, ShipID.ShinyouKai) => new VisibleFits(asw: 3, evasion:2),
            (EquipID.CarrierDiveBomber_Ju87CKai2_KMXSkilled, ShipID.ShinyouKaiNi) => new VisibleFits(asw: 3, evasion:2),

            (EquipID.CarrierTorpedoBomber_Type97TorpedoBomber_931AirGroupSkilled, _) => new VisibleFits(asw: 1, evasion: 1),

            _ => new VisibleFits()
        };

        private VisibleFits VisibleFitsCasablanca(EquipmentDataCustom equip) => (equip.EquipID, Ship.ShipID) switch
        {
            (EquipID.CarrierFighter_Type96Fighter, _) => new VisibleFits(aa: 1, evasion: 1),
            (EquipID.CarrierFighter_Type96FighterKai, _) => new VisibleFits(aa: 1, asw:2, evasion: 1),

            (EquipID.SmallRadar_GFCSMk_37, _) => new VisibleFits(firepower: 1, aa: 1, evasion: 1),

            (EquipID.SmallRadar_SGRadar_InitialModel, _) => new VisibleFits(firepower: 2, evasion: 3, los: 4),

            _ => new VisibleFits()
        };

        #endregion


        #region CV synergy

        private VisibleFits SynergyAkagi(ShipDataCustom ship)
        {
            VisibleFits synergy = new VisibleFits();

            if (ship.Equipment.Any(eq => eq?.EquipID == EquipID.CarrierTorpedoBomber_Type97TorpedoBomber_MurataSquadron))
            {
                synergy += ship.ShipID switch
                {
                    ShipID.AkagiKai => new VisibleFits(firepower: 3),
                    ShipID.AkagiKaiNi => new VisibleFits(firepower: 3),
                    ShipID.AkagiKaiNiE => new VisibleFits(firepower: 3),

                    _ => new VisibleFits()
                };
            }

            if (ship.Equipment.Any(eq => eq?.EquipID == EquipID.CarrierTorpedoBomber_TenzanModel12_MurataSquadron))
            {
                synergy += ship.ShipID switch
                {
                    ShipID.AkagiKai => new VisibleFits(firepower: 3),
                    ShipID.AkagiKaiNi => new VisibleFits(firepower: 3),
                    ShipID.AkagiKaiNiE => new VisibleFits(firepower: 3),

                    _ => new VisibleFits()
                };
            }

            if (ship.Equipment.Any(eq => eq?.EquipID == EquipID.CarrierRecon_Type2ReconAircraft))
            {
                synergy += Type2ReconAircraft(ship);
            }

            return synergy;
        }

        private VisibleFits SynergyKaga(ShipDataCustom ship)
        {
            VisibleFits synergy = new VisibleFits();

            if (ship.Equipment.Any(eq => eq?.EquipID == EquipID.CarrierTorpedoBomber_Type97TorpedoBomber_MurataSquadron))
            {
                synergy += ship.ShipID switch
                {
                    ShipID.KagaKai => new VisibleFits(firepower: 2),

                    _ => new VisibleFits()
                };
            }

            if (ship.Equipment.Any(eq => eq?.EquipID == EquipID.CarrierTorpedoBomber_TenzanModel12_MurataSquadron))
            {
                synergy += ship.ShipID switch
                {
                    ShipID.KagaKai => new VisibleFits(firepower: 2),

                    _ => new VisibleFits()
                };
            }

            if (ship.Equipment.Any(eq => eq?.EquipID == EquipID.CarrierRecon_Type2ReconAircraft))
            {
                synergy += Type2ReconAircraft(ship);
            }

            return synergy;
        }

        private VisibleFits SynergySouryuu(ShipDataCustom ship)
        {
            VisibleFits synergy = new VisibleFits();

            if (ship.Equipment.Any(eq => eq?.EquipID == EquipID.CarrierDiveBomber_Type99DiveBomber_EgusaSquadron))
            {
                synergy += new VisibleFits(firepower:4);
            }

            if (ship.Equipment.Any(eq => eq?.EquipID == EquipID.CarrierDiveBomber_Suisei_EgusaSquadron))
            {
                synergy += ship.ShipID switch
                {
                    ShipID.SouryuuKaiNi => new VisibleFits(firepower: 6),

                    _ => new VisibleFits()
                };
            }

            if (ship.Equipment.Any(eq => eq?.EquipID == EquipID.CarrierTorpedoBomber_Type97TorpedoBomber_TomonagaSquadron))
            {
                synergy += new VisibleFits(firepower: 1);
            }

            if (ship.Equipment.Any(eq => eq?.EquipID == EquipID.CarrierTorpedoBomber_TenzanModel12_TomonagaSquadron))
            {
                synergy += ship.ShipID switch
                {
                    ShipID.SouryuuKaiNi => new VisibleFits(firepower: 3),

                    _ => new VisibleFits()
                };
            }

            if (ship.Equipment.Any(eq => eq?.EquipID == EquipID.CarrierRecon_Type2ReconAircraft))
            {
                int level = ship.Equipment
                    .Where(eq => eq?.EquipID == EquipID.CarrierRecon_Type2ReconAircraft)
                    .Max(eq => eq.Level);

                synergy += ship.ShipID switch
                {
                    ShipID.SouryuuKaiNi when level == 10 => new VisibleFits(firepower: 6, los: 7),
                    ShipID.SouryuuKaiNi when level > 7 => new VisibleFits(firepower: 5, los: 6),
                    ShipID.SouryuuKaiNi when level > 5 => new VisibleFits(firepower: 4, los: 5),
                    ShipID.SouryuuKaiNi when level > 3 => new VisibleFits(firepower: 4, los: 4),
                    ShipID.SouryuuKaiNi when level > 1 => new VisibleFits(firepower: 3, los: 4),
                    ShipID.SouryuuKaiNi when level == 1 => new VisibleFits(firepower: 3, los: 3),

                    _ => Type2ReconAircraft(ship)
                };
            }

            return synergy;
        }

        private VisibleFits SynergyHiryuu(ShipDataCustom ship)
        {
            VisibleFits synergy = new VisibleFits();

            if (ship.Equipment.Any(eq => eq?.EquipID == EquipID.CarrierDiveBomber_Type99DiveBomber_EgusaSquadron))
            {
                synergy += new VisibleFits(firepower: 1);
            }

            if (ship.Equipment.Any(eq => eq?.EquipID == EquipID.CarrierDiveBomber_Suisei_EgusaSquadron))
            {
                synergy += ship.ShipID switch
                {
                    ShipID.HiryuuKaiNi => new VisibleFits(firepower: 3),

                    _ => new VisibleFits()
                };
            }

            if (ship.Equipment.Any(eq => eq?.EquipID == EquipID.CarrierTorpedoBomber_Type97TorpedoBomber_TomonagaSquadron))
            {
                synergy += new VisibleFits(firepower: 3);
            }

            if (ship.Equipment.Any(eq => eq?.EquipID == EquipID.CarrierTorpedoBomber_TenzanModel12_TomonagaSquadron))
            {
                synergy += ship.ShipID switch
                {
                    ShipID.HiryuuKaiNi => new VisibleFits(firepower: 7),

                    _ => new VisibleFits()
                };
            }

            if (ship.Equipment.Any(eq => eq?.EquipID == EquipID.CarrierRecon_Type2ReconAircraft))
            {
                int level = ship.Equipment
                    .Where(eq => eq?.EquipID == EquipID.CarrierRecon_Type2ReconAircraft)
                    .Max(eq => eq.Level);

                synergy += ship.ShipID switch
                {
                    ShipID.HiryuuKaiNi when level == 10 => new VisibleFits(firepower: 4, los: 5),
                    ShipID.HiryuuKaiNi when level > 5 => new VisibleFits(firepower: 3, los: 4),
                    ShipID.HiryuuKaiNi when level > 3 => new VisibleFits(firepower: 3, los: 3),
                    ShipID.HiryuuKaiNi when level > 1 => new VisibleFits(firepower: 2, los: 3),
                    ShipID.HiryuuKaiNi when level == 1 => new VisibleFits(firepower: 2, los: 2),

                    _ => Type2ReconAircraft(ship)
                };
            }

            return synergy;
        }

        private VisibleFits SynergyShoukaku(ShipDataCustom ship)
        {
            VisibleFits synergy = new VisibleFits();

            if (ship.Equipment.Any(eq => eq?.EquipID == EquipID.CarrierTorpedoBomber_Type97TorpedoBomber_MurataSquadron))
            {
                synergy += ship.ShipID switch
                {
                    ShipID.Shoukaku => new VisibleFits(firepower: 2),
                    ShipID.ShoukakuKai => new VisibleFits(firepower: 2),
                    ShipID.ShoukakuKaiNi => new VisibleFits(firepower: 2),
                    ShipID.ShoukakuKaiNiA => new VisibleFits(firepower: 2),

                    ShipID.Zuikaku => new VisibleFits(firepower: 1),
                    ShipID.ZuikakuKai => new VisibleFits(firepower: 1),
                    ShipID.ZuikakuKaiNi => new VisibleFits(firepower: 1),
                    ShipID.ZuikakuKaiNiA => new VisibleFits(firepower: 1),

                    _ => new VisibleFits()
                };
            }

            if (ship.Equipment.Any(eq => eq?.EquipID == EquipID.CarrierTorpedoBomber_TenzanModel12_MurataSquadron))
            {
                synergy += ship.ShipID switch
                {
                    ShipID.Shoukaku => new VisibleFits(firepower: 2),
                    ShipID.ShoukakuKai => new VisibleFits(firepower: 2),
                    ShipID.ShoukakuKaiNi => new VisibleFits(firepower: 4),
                    ShipID.ShoukakuKaiNiA => new VisibleFits(firepower: 4),

                    ShipID.Zuikaku => new VisibleFits(firepower: 1),
                    ShipID.ZuikakuKai => new VisibleFits(firepower: 1),
                    ShipID.ZuikakuKaiNi => new VisibleFits(firepower: 2),
                    ShipID.ZuikakuKaiNiA => new VisibleFits(firepower: 2),

                    _ => new VisibleFits()
                };
            }

            if (ship.Equipment.Any(eq => eq?.EquipID == EquipID.CarrierRecon_Type2ReconAircraft))
            {
                synergy += Type2ReconAircraft(ship);
            }

            if (ship.Equipment.Any(eq => eq?.EquipID == EquipID.CarrierRecon_PrototypeKeiun))
            {
                synergy += ship.ShipID switch
                {
                    ShipID.ShoukakuKaiNiA => PrototypeKeiun(ship),
                    ShipID.ZuikakuKaiNiA => PrototypeKeiun(ship),

                    _ => new VisibleFits()
                };
            }

            return synergy;
        }

        private VisibleFits SynergyUnryuu(ShipDataCustom ship)
        {
            VisibleFits synergy = new VisibleFits();

            if (ship.Equipment.Any(eq => eq?.EquipID == EquipID.CarrierRecon_Type2ReconAircraft))
            {
                synergy += Type2ReconAircraft(ship);
            }

            return synergy;
        }

        private VisibleFits SynergyTaihou(ShipDataCustom ship)
        {
            VisibleFits synergy = new VisibleFits();

            if (ship.Equipment.Any(eq => eq?.EquipID == EquipID.CarrierRecon_Type2ReconAircraft))
            {
                synergy += Type2ReconAircraft(ship);
            }

            if (ship.Equipment.Any(eq => eq?.EquipID == EquipID.CarrierRecon_PrototypeKeiun))
            {
                synergy += PrototypeKeiun(ship);
            }

            return synergy;
        }

        private VisibleFits SynergyGrafZepelin(ShipDataCustom ship)
        {
            VisibleFits synergy = new VisibleFits();

            if (ship.Equipment.Any(eq => eq?.EquipID == EquipID.CarrierRecon_Type2ReconAircraft))
            {
                synergy += Type2ReconAircraft(ship);
            }

            return synergy;
        }

        private VisibleFits SynergyAquila(ShipDataCustom ship)
        {
            VisibleFits synergy = new VisibleFits();

            if (ship.Equipment.Any(eq => eq?.EquipID == EquipID.CarrierRecon_Type2ReconAircraft))
            {
                synergy += Type2ReconAircraft(ship);
            }

            return synergy;
        }

        private VisibleFits SynergySaratoga(ShipDataCustom ship)
        {
            VisibleFits synergy = new VisibleFits();

            if (ship.Equipment.Any(eq => eq?.EquipID == EquipID.CarrierRecon_Type2ReconAircraft))
            {
                synergy += Type2ReconAircraft(ship);
            }

            if (ship.Equipment.Any(eq => eq?.EquipID == EquipID.CarrierRecon_PrototypeKeiun))
            {
                synergy += ship.ShipID switch
                {
                    ShipID.SaratogaMkIIMod2 => PrototypeKeiun(ship),

                    _ => new VisibleFits()
                };
            }

            return synergy;
        }

        private VisibleFits SynergyIntrepid(ShipDataCustom ship)
        {
            VisibleFits synergy = new VisibleFits();

            if (ship.Equipment.Any(eq => eq?.EquipID == EquipID.CarrierRecon_Type2ReconAircraft))
            {
                synergy += Type2ReconAircraft(ship);
            }

            return synergy;
        }

        private VisibleFits SynergyArkRoyal(ShipDataCustom ship)
        {
            VisibleFits synergy = new VisibleFits();

            if (ship.Equipment.Any(eq => eq?.EquipID == EquipID.CarrierRecon_Type2ReconAircraft))
            {
                synergy += Type2ReconAircraft(ship);
            }

            return synergy;
        }

        private VisibleFits SynergyHoushou(ShipDataCustom ship)
        {
            VisibleFits synergy = new VisibleFits();

            if (ship.Equipment.Any(eq => eq?.EquipID == EquipID.CarrierRecon_Type2ReconAircraft))
            {
                synergy += Type2ReconAircraft(ship);
            }

            return synergy;
        }

        private VisibleFits SynergyRyuujou(ShipDataCustom ship)
        {
            VisibleFits synergy = new VisibleFits();

            if (ship.Equipment.Any(eq => eq?.EquipID == EquipID.CarrierTorpedoBomber_Type97TorpedoBomber_MurataSquadron))
            {
                synergy += ship.ShipID switch
                {
                    ShipID.RyuujouKaiNi => new VisibleFits(firepower: 1),

                    _ => new VisibleFits()
                };
            }

            if (ship.Equipment.Any(eq => eq?.EquipID == EquipID.CarrierTorpedoBomber_TenzanModel12_MurataSquadron))
            {
                synergy += ship.ShipID switch
                {
                    ShipID.RyuujouKaiNi => new VisibleFits(firepower: 1),

                    _ => new VisibleFits()
                };
            }

            if (ship.Equipment.Any(eq => eq?.EquipID == EquipID.CarrierRecon_Type2ReconAircraft))
            {
                synergy += Type2ReconAircraft(ship);
            }

            return synergy;
        }

        private VisibleFits SynergyShouhou(ShipDataCustom ship)
        {
            VisibleFits synergy = new VisibleFits();

            if (ship.Equipment.Any(eq => eq?.EquipID == EquipID.CarrierRecon_Type2ReconAircraft))
            {
                int level = ship.Equipment
                    .Where(eq => eq?.EquipID == EquipID.CarrierRecon_Type2ReconAircraft)
                    .Max(eq => eq.Level);

                synergy += ship.ShipID switch
                {
                    ShipID.ZuihouKaiNiB when level == 10 => new VisibleFits(firepower: 3, los: 4),
                    ShipID.ZuihouKaiNiB when level > 5 => new VisibleFits(firepower: 2, los: 3),
                    ShipID.ZuihouKaiNiB when level > 3 => new VisibleFits(firepower: 2, los: 2),
                    ShipID.ZuihouKaiNiB when level > 1 => new VisibleFits(firepower: 1, los: 2),
                    ShipID.ZuihouKaiNiB when level == 1 => new VisibleFits(firepower: 1, los: 1),

                    _ => new VisibleFits()
                };
            }

            return synergy;
        }

        private VisibleFits SynergyHiyou(ShipDataCustom ship)
        {
            VisibleFits synergy = new VisibleFits();

            if (ship.Equipment.Any(eq => eq?.EquipID == EquipID.CarrierRecon_Type2ReconAircraft))
            {
                synergy += Type2ReconAircraft(ship);
            }

            return synergy;
        }

        private VisibleFits SynergyTaiyou(ShipDataCustom ship)
        {
            VisibleFits synergy = new VisibleFits();

            if (ship.Equipment.Any(eq => eq?.EquipID == EquipID.CarrierRecon_Type2ReconAircraft))
            {
                synergy += Type2ReconAircraft(ship);
            }

            return synergy;
        }

        private VisibleFits SynergyCasablanca(ShipDataCustom ship)
        {
            VisibleFits synergy = new VisibleFits();

            if (ship.Equipment.Any(eq => eq?.EquipID == EquipID.CarrierRecon_Type2ReconAircraft))
            {
                synergy +=  Type2ReconAircraft(ship);
            }

            return synergy;
        }

        #endregion










        #region AUX visible fits

        private VisibleFits VisibleFitsNisshin(EquipmentDataCustom equip) => (equip.EquipID, Ship.ShipID) switch
        {
            (EquipID.MediumCalibreMainGun_14cmTwinGun, _) => new VisibleFits(firepower: 2, torpedo: 1),
            (EquipID.MediumCalibreMainGun_14cmTwinGunKai, _) => new VisibleFits(firepower: 3, torpedo:2, aa: 1, evasion: 1),

            _ => new VisibleFits()
        };

        private VisibleFits VisibleFitsTaigei(EquipmentDataCustom equip) => (equip.EquipID, Ship.ShipID) switch
        {
            (EquipID.CarrierTorpedoBomber_九七式艦攻改試製三号戊型_空六号電探改装備機, ShipID.RyuuhouKai) => new VisibleFits(firepower: 4, asw: 1),
            (EquipID.CarrierTorpedoBomber_九七式艦攻改_熟練試製三号戊型_空六号電探改装備機, ShipID.RyuuhouKai) => new VisibleFits(firepower: 5, evasion:2, asw: 1),

            _ => new VisibleFits()
        };



        #endregion



        #region AUX synergy

        private VisibleFits SynergyChitose(ShipDataCustom ship)
        {
            VisibleFits synergy = new VisibleFits();

            if (ship.Equipment.Any(eq => eq?.EquipID == EquipID.CarrierRecon_Type2ReconAircraft))
            {
                synergy += ship.ShipID switch
                {
                    ShipID.ChitoseCVL => Type2ReconAircraft(ship),
                    ShipID.ChitoseCVLKai => Type2ReconAircraft(ship),
                    ShipID.ChitoseCVLKaiNi => Type2ReconAircraft(ship),

                    ShipID.ChiyodaCVL => Type2ReconAircraft(ship),
                    ShipID.ChiyodaCVLKai => Type2ReconAircraft(ship),
                    ShipID.ChiyodaCVLKaiNi => Type2ReconAircraft(ship),

                    _ => new VisibleFits()
                };
            }

            return synergy;
        }

        private VisibleFits SynergyTaigei(ShipDataCustom ship)
        {
            VisibleFits synergy = new VisibleFits();

            if (ship.Equipment.Any(eq => eq?.EquipID == EquipID.CarrierRecon_Type2ReconAircraft))
            {
                synergy += ship.ShipID switch
                {
                    ShipID.Ryuuhou => Type2ReconAircraft(ship),
                    ShipID.RyuuhouKai => Type2ReconAircraft(ship),

                    _ => new VisibleFits()
                };
            }

            return synergy;
        }

        #endregion








        #region helper functions

        /// <summary>
        /// Fubuki, Ayanami, Akatsuki <br/>
        ///
        /// used for synergy between type A guns and triple torpedoes
        /// </summary>
        private VisibleFits SpecialDestroyerTypeATripleTorpedo(ShipDataCustom ship)
        {
            if (ship.Equipment.Any(eq => eq?.EquipID == EquipID.Torpedo_61cmTriple_OxygenTorpedoMountLateModel))
            {
                return ship.Equipment.Count(eq => eq?.EquipID == EquipID.Torpedo_61cmTripleTorpedo ||
                                                  eq?.EquipID == EquipID.Torpedo_61cmTriple_OxygenTorpedo ||
                                                  eq?.EquipID ==
                                                  EquipID.Torpedo_61cmTriple_OxygenTorpedoMountLateModel) switch
                {
                    0 => new VisibleFits(),
                    1 => new VisibleFits(firepower: 1, torpedo: 4),
                    _ => new VisibleFits(firepower: 2, torpedo: 6)
                };
            }

            return ship.Equipment.Count(eq => eq?.EquipID == EquipID.Torpedo_61cmTripleTorpedo ||
                                              eq?.EquipID == EquipID.Torpedo_61cmTriple_OxygenTorpedo) switch
            {
                0 => new VisibleFits(),
                1 => new VisibleFits(firepower: 1, torpedo: 3),
                _ => new VisibleFits(firepower: 2, torpedo: 5)
            };
        }

        /// <summary>
        /// Fubuki k2, Ayanami k2, Akatsuki k2, Hatsuharu k2
        /// </summary>
        private VisibleFits TripleTorpedoLM(ShipDataCustom ship)
        {
            int fpBonus = ship.Equipment
                .Where(eq => eq?.EquipID == EquipID.Torpedo_61cmTriple_OxygenTorpedoMountLateModel && eq.Level == 10)
                .Take(2)
                .Count();

            return ship.Equipment.Count(eq => eq?.EquipID == EquipID.Torpedo_61cmTriple_OxygenTorpedoMountLateModel)
                switch
                {
                    0 => new VisibleFits(),
                    1 => new VisibleFits(firepower: fpBonus, torpedo: 2, evasion: 1),
                    _ => 2 * new VisibleFits(firepower: fpBonus, torpedo: 2, evasion: 1)
                };
        }

        /// <summary>
        /// Shiratsuyu k2, Asashio k2, Kagerou k2, Yuugumo k2
        /// </summary>
        private VisibleFits QuadrupleTorpedoLM(ShipDataCustom ship)
        {
            int fpBonus = ship.Equipment
                .Where(eq => eq?.EquipID == EquipID.Torpedo_61cmTriple_OxygenTorpedoMountLateModel && eq.Level == 10)
                .Take(2)
                .Count();

            int tpBonus = 0;
            if (ship.ShipClass == ShipClasses.Kagerou)
            {
                tpBonus = ship.Equipment
                    .Where(eq => eq?.EquipID == EquipID.Torpedo_61cmQuadruple_OxygenTorpedoMountLateModel && eq.Level > 4)
                    .Take(2)
                    .Count();
            }

            return ship.Equipment.Count(eq => eq?.EquipID == EquipID.Torpedo_61cmTriple_OxygenTorpedoMountLateModel)
                switch
                {
                    0 => new VisibleFits(),
                    1 => new VisibleFits(firepower: fpBonus, torpedo: 2 + tpBonus, evasion: 1),
                    _ => 2 * new VisibleFits(firepower: fpBonus, torpedo: 2 + tpBonus, evasion: 1)
                };
        }

        /// <summary>
        /// for all ships without a specific bonus
        ///
        /// todo: could subtract this value from all specific ones and then always do this + specific
        /// </summary>
        private VisibleFits Type2ReconAircraft(ShipDataCustom ship)
        {
            VisibleFits synergy = new VisibleFits();

            if (ship.Equipment.Any(eq => eq?.EquipID == EquipID.CarrierRecon_Type2ReconAircraft))
            {
                int level = ship.Equipment
                    .Where(eq => eq?.EquipID == EquipID.CarrierRecon_Type2ReconAircraft)
                    .Max(eq => eq.Level);

                synergy += ship.ShipID switch
                {
                    _ when level == 10 => new VisibleFits(firepower: 2, los: 3),
                    _ when level > 5 => new VisibleFits(firepower: 1, los: 2),
                    _ when level > 3 => new VisibleFits(firepower: 1, los: 1),
                    _ when level > 1 => new VisibleFits(los: 1),

                    _ => new VisibleFits()
                };
            }

            return synergy;
        }

        /// <summary>
        /// todo: the upgrade bonus is the same for all carrier recons?
        /// </summary>
        private VisibleFits PrototypeKeiun(ShipDataCustom ship)
        {
            VisibleFits synergy = new VisibleFits();

            if (ship.Equipment.Any(eq => eq?.EquipID == EquipID.CarrierRecon_PrototypeKeiun))
            {
                int level = ship.Equipment
                    .Where(eq => eq?.EquipID == EquipID.CarrierRecon_PrototypeKeiun)
                    .Max(eq => eq.Level);

                synergy += ship.ShipID switch
                {
                    _ when level == 10 => new VisibleFits(firepower: 2, los: 3),
                    _ when level > 5 => new VisibleFits(firepower: 1, los: 2),
                    _ when level > 3 => new VisibleFits(firepower: 1, los: 1),
                    _ when level > 1 => new VisibleFits(los: 1),

                    _ => new VisibleFits()
                };
            }

            return synergy;
        }

        #endregion
    }
}
