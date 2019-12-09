// ReSharper disable ArgumentsStyleLiteral
// ReSharper disable ArgumentsStyleNamedExpression
using System.Linq;

namespace ElectronicObserverTypes
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

        public static bool operator ==(VisibleFits a, VisibleFits b)
        {
            return a.Firepower == b.Firepower &&
                   a.Torpedo == b.Torpedo &&
                   a.AA == b.AA &&
                   a.ASW == b.ASW &&
                   a.Evasion == b.Evasion &&
                   a.Armor == b.Armor &&
                   a.LoS == b.LoS;
        }

        public static bool operator !=(VisibleFits a, VisibleFits b) => !(a == b);
    }

    // how to not SQL
    public class FitBonusCustom
    {
        private ShipDataCustom Ship { get; }

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

        // todo synergy and fit bonus should be different classes
        /// <summary>
        /// this is for synergy
        /// </summary>
        public FitBonusCustom(ShipDataCustom ship)
        {
            Ship = ship;

            VisibleFits visibleFits = SynergyBonus(ship);

            Firepower = visibleFits.Firepower;
            Torpedo = visibleFits.Torpedo;
            AA = visibleFits.AA;
            ASW = visibleFits.ASW;
            Evasion = visibleFits.Evasion;
            Armor = visibleFits.Armor;
            LoS = visibleFits.LoS;
        }

        /// <summary>
        /// this is for fit bonus
        /// </summary>
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
            ShipClasses.Mikura => VisibleFitsMikura(equip),
            ShipClasses.Hiburi => VisibleFitsHiburi(equip),

            ShipClasses.Tenryuu => new VisibleFits(), // todo VisibleFitsTenryuu(equip),
            ShipClasses.Kuma => VisibleFitsKuma(equip),
            ShipClasses.Nagara => VisibleFitsNagara(equip),
            ShipClasses.Sendai => VisibleFitsSendai(equip),
            ShipClasses.Yuubari => VisibleFitsYuubari(equip),
            ShipClasses.Agano => VisibleFitsAgano(equip),
            ShipClasses.Ooyodo => VisibleFitsOoyodo(equip),
            ShipClasses.Abruzzi => VisibleFitsAbruzzi(equip),
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
            ShipClasses.Colorado => VisibleFitsColorado(equip),
            ShipClasses.QueenElizabeth => VisibleFitsQueenElizabeth(equip),
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

            ShipClasses.Kamoi => new VisibleFits(), // todo VisibleFitsKamoi(equip),
            ShipClasses.Chitose => new VisibleFits(), // todo VisibleFitsChitose(equip),
            ShipClasses.Mizuho => new VisibleFits(), // todo VisibleFitsMizuho(equip),
            ShipClasses.Nisshin => VisibleFitsNisshin(equip),
            ShipClasses.Akitsushima => new VisibleFits(), // todo VisibleFitsAkitsushima(equip),
            ShipClasses.Taigei => VisibleFitsTaigei(equip),
            ShipClasses.Ryuuhou => VisibleFitsTaigei(equip),
            ShipClasses.Kazahaya => new VisibleFits(), // todo VisibleFitsKazahaya(equip),
            ShipClasses.工作艦 => new VisibleFits(), // todo VisibleFitsRepairShip ??
            ShipClasses.Katori => VisibleFitsKatori(equip),
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

        private VisibleFits SynergyBonus(ShipDataCustom ship) => ship.ShipClass switch
        {
            // why R# dying in here?
            ShipClasses.Kamikaze => SynergyKamikaze(ship),
            ShipClasses.Mutsuki => SynergyMutsuki(ship),
            ShipClasses.Fubuki => SynergyFubuki(ship),
            ShipClasses.Ayanami => SynergyAyanami(ship),
            ShipClasses.Akatsuki => SynergyAkatsuki(ship),
            ShipClasses.Hatsuharu => SynergyHatsuharu(ship),
            ShipClasses.Shiratsuyu => SynergyShiratsuyu(ship),
            ShipClasses.Asashio => SynergyAsashio(ship),
            ShipClasses.Kagerou => SynergyKagerou(ship),
            ShipClasses.Yuugumo => SynergyYuugumo(ship),
            // ShipClasses.Akizuki => SynergyAkizuki(ship),
            ShipClasses.Shimakaze => SynergyShimakaze(ship),
            // ShipClasses.Z1 => SynergyZ1(ship),
            // ShipClasses.Maestrale => SynergyMaestrale(ship),
            // ShipClasses.Fletcher => SynergyAmericanDD(ship),
            // ShipClasses.JohnCButler => SynergyAmericanDD(ship),
            // ShipClasses.J => SynergyJ(ship),
            // ShipClasses.Tashkent => SynergyTashkent(ship),

            ShipClasses.Shimushu => SynergyShimushu(ship),
            ShipClasses.Etorofu => SynergyEtorofu(ship),
            ShipClasses.Mikura => SynergyMikura(ship),
            ShipClasses.Hiburi => SynergyHiburi(ship),

            ShipClasses.Tenryuu => new VisibleFits(), // todo SynergyTenryuu(ship),
            ShipClasses.Kuma => SynergyKuma(ship),
            ShipClasses.Nagara => SynergyNagara(ship),
            ShipClasses.Sendai => SynergySendai(ship),
            // ShipClasses.Yuubari => SynergyYuubari(ship),
            // ShipClasses.Agano => SynergyAgano(ship),
            // ShipClasses.Ooyodo => SynergyOoyodo(ship),
            // ShipClasses.Abruzzi => Synergy
            // ShipClasses.Gotland => SynergyGotland(ship),

            ShipClasses.Furutaka => SynergyFurutaka(ship),
            ShipClasses.Aoba => SynergyAoba(ship),
            ShipClasses.Myoukou => new VisibleFits(), // todo SynergyMyoukou(ship),
            ShipClasses.Takao => SynergyTakao(ship),
            ShipClasses.Mogami => SynergyMogami(ship),
            ShipClasses.Tone => new VisibleFits(), // todo SynergyTone(ship),
            ShipClasses.AdmiralHipper => new VisibleFits(), // todo SynergyAdmiralHipper(ship),
            ShipClasses.Zara => new VisibleFits(), // todo SynergyZara(ship),

            ShipClasses.Kongou => SynergyKongou(ship),
            ShipClasses.Ise when Ship.ShipID == ShipID.IseKaiNi || Ship.ShipID == ShipID.HyuugaKaiNi =>
            SynergyIse(ship),
            // _ when Ship.ShipType == ShipTypes.AviationBattleship => SynergyAviationBattleship(ship),
            ShipClasses.Nagato => SynergyNagato(ship),
            // todo Yamato
            // todo Bisko
            // todo VV
            // ShipClasses.Iowa => SynergyIowa(ship),
            // ShipClasses.Colorado
            // ShipClasses.QueenElizabeth => SynergyQueenElizabeth(ship),
            // ShipClasses.Nelson => SynergyNelson(ship),
            // todo Richelieu
            // todo Gangut

            ShipClasses.Akagi => SynergyAkagi(ship),
            ShipClasses.Kaga => SynergyKaga(ship),
            ShipClasses.Souryuu => SynergySouryuu(ship),
            ShipClasses.Hiryuu => SynergyHiryuu(ship),
            ShipClasses.Shoukaku => SynergyShoukaku(ship),
            ShipClasses.Unryuu => SynergyUnryuu(ship),
            ShipClasses.Taihou => SynergyTaihou(ship),
            ShipClasses.GrafZeppelin => SynergyGrafZeppelin(ship),
            ShipClasses.Aquila => SynergyAquila(ship),
            ShipClasses.Lexington => SynergyLexington(ship),
            ShipClasses.Essex => SynergyEssex(ship),
            ShipClasses.ArkRoyal => SynergyArkRoyal(ship),

            ShipClasses.Houshou => SynergyHoushou(ship),
            ShipClasses.Ryuujou => SynergyRyuujou(ship),
            ShipClasses.Shouhou => SynergyShouhou(ship),
            ShipClasses.Hiyou => SynergyHiyou(ship),
            ShipClasses.KasugaMaru => SynergyTaiyou(ship),
            ShipClasses.Taiyou => SynergyTaiyou(ship),
            ShipClasses.Casablanca => SynergyCasablanca(ship),

            ShipClasses.Kamoi => new VisibleFits(), // todo SynergyKamoi(ship),
            ShipClasses.Chitose => SynergyChitose(ship),
            ShipClasses.Mizuho => new VisibleFits(), // todo SynergyMizuho(ship),
            // ShipClasses.日進型 => SynergyNisshin(ship),
            ShipClasses.Akitsushima => new VisibleFits(), // todo SynergyAkitsushima(ship),
            ShipClasses.Taigei => SynergyTaigei(ship),
            ShipClasses.Ryuuhou => SynergyTaigei(ship),
            ShipClasses.Kazahaya => new VisibleFits(), // todo SynergyKazahaya(ship),
            ShipClasses.工作艦 => new VisibleFits(), // todo SynergyRepairShip ??
            // ShipClasses.香取型 => SynergyKatori(ship),
            ShipClasses.巡潜甲型改二 => new VisibleFits(), // todo SynergyI13(ship),
            ShipClasses.潜特型伊400型潜水艦 => new VisibleFits(), // todo SynergyI400(ship),
            ShipClasses.海大VI型 => new VisibleFits(), // todo SynergyI168(ship),
            ShipClasses.巡潜3型 => new VisibleFits(), // todo SynergyI7(ship),
            ShipClasses.巡潜乙型 => new VisibleFits(), // todo SynergyI15(ship),
            ShipClasses.巡潜乙型改二 => new VisibleFits(), // todo SynergyI54(ship),
            ShipClasses.三式潜航輸送艇 => new VisibleFits(), // todo SynergyMaruyu(ship),
            ShipClasses.特種船丙型 => new VisibleFits(), // todo SynergyAkitsuMaru ??
            ShipClasses.UボートIXC型 => new VisibleFits(), // todo SynergyIXC

            _ => new VisibleFits()
        };

        private int GetAccuracy(EquipmentDataCustom equip) => Ship.ShipClass switch
        {
            ShipClasses.Gangut => AccuracyGangut(equip),
            ShipClasses.Kongou => AccuracyKongou(equip),
            ShipClasses.Bismarck => AccuracyBismarck(equip),
            ShipClasses.VVeneto => AccuracyBismarck(equip),
            ShipClasses.Iowa => AccuracyIowa(equip),
            ShipClasses.Richelieu => AccuracyRichelieu(equip),
            ShipClasses.QueenElizabeth => AccuracyQueenElizabeth(equip),
            ShipClasses.Nelson => AccuracyNelson(equip),
            ShipClasses.Yamato => AccuracyYamato(equip),

            ShipClasses.Nagato when Ship.ShipID == ShipID.NagatoKaiNi => AccuracyNagato(equip),
            ShipClasses.Nagato when Ship.ShipID == ShipID.MutsuKaiNi => AccuracyMutsu(equip),
            ShipClasses.Ise when Ship.ShipID == ShipID.IseKaiNi || 
                                 Ship.ShipID == ShipID.HyuugaKaiNi => AccuracyIse(equip),

            _ when Ship.ShipType == ShipTypes.AviationBattleship => AccuracyAviationBattleship(equip),
            _ when Ship.ShipType == ShipTypes.Battleship => AccuracyBattleship(equip),

            _ => 0
        };

        #region BB visible fits

        private VisibleFits VisibleFitsKongou(EquipmentDataCustom equip) => (equip.EquipID, Ship.ShipID) switch
        {
            (EquipID.LargeCalibreMainGun_35_6cmTwinGun_DazzleCamouflage, ShipID.KongouKaiNi) => new VisibleFits(firepower: 2),
            (EquipID.LargeCalibreMainGun_35_6cmTwinGun_DazzleCamouflage, ShipID.HieiKaiNi) => new VisibleFits(firepower: 1),
            (EquipID.LargeCalibreMainGun_35_6cmTwinGun_DazzleCamouflage, ShipID.KirishimaKaiNi) => new VisibleFits(firepower: 1),
            (EquipID.LargeCalibreMainGun_35_6cmTwinGun_DazzleCamouflage, ShipID.HarunaKaiNi) => new VisibleFits(firepower: 2, aa: 1, evasion: 2),

            (EquipID.LargeCalibreMainGun_35_6cmTripleGunKai_DazzleCamouflage, ShipID.KongouKaiNi) => new VisibleFits(firepower: 2, aa: 2),
            (EquipID.LargeCalibreMainGun_35_6cmTripleGunKai_DazzleCamouflage, ShipID.HieiKaiNi) => new VisibleFits(firepower: 1),
            (EquipID.LargeCalibreMainGun_35_6cmTripleGunKai_DazzleCamouflage, ShipID.KirishimaKaiNi) => new VisibleFits(firepower: 1),
            (EquipID.LargeCalibreMainGun_35_6cmTripleGunKai_DazzleCamouflage, ShipID.HarunaKaiNi) => new VisibleFits(firepower: 2, aa: 2, evasion: 2),

            (EquipID.LargeCalibreMainGun_35_6cm連装砲改, ShipID.KongouKaiNiC) => new VisibleFits(firepower: 3, torpedo: 1, evasion: 1),
            (EquipID.LargeCalibreMainGun_35_6cm連装砲改, ShipID.KongouKaiNi) => new VisibleFits(firepower: 2, evasion: 1),
            (EquipID.LargeCalibreMainGun_35_6cm連装砲改, ShipID.HieiKaiNi) => new VisibleFits(firepower: 2, evasion: 1),
            (EquipID.LargeCalibreMainGun_35_6cm連装砲改, ShipID.KirishimaKaiNi) => new VisibleFits(firepower: 2, evasion: 1),
            (EquipID.LargeCalibreMainGun_35_6cm連装砲改, ShipID.HarunaKaiNi) => new VisibleFits(firepower: 2, evasion: 1),
            (EquipID.LargeCalibreMainGun_35_6cm連装砲改, _) => new VisibleFits(firepower: 1, evasion: 1),

            (EquipID.LargeCalibreMainGun_35_6cm連装砲改二, ShipID.KongouKaiNiC) => new VisibleFits(firepower: 4, torpedo: 2, aa: 1, evasion: 1),
            (EquipID.LargeCalibreMainGun_35_6cm連装砲改二, ShipID.KongouKaiNi) => new VisibleFits(firepower: 3, aa: 1, evasion: 1),
            (EquipID.LargeCalibreMainGun_35_6cm連装砲改二, ShipID.HieiKaiNi) => new VisibleFits(firepower: 3, aa: 1, evasion: 1),
            (EquipID.LargeCalibreMainGun_35_6cm連装砲改二, ShipID.KirishimaKaiNi) => new VisibleFits(firepower: 3, aa: 1, evasion: 1),
            (EquipID.LargeCalibreMainGun_35_6cm連装砲改二, ShipID.HarunaKaiNi) => new VisibleFits(firepower: 3, aa: 1, evasion: 1),
            (EquipID.LargeCalibreMainGun_35_6cm連装砲改二, _) => new VisibleFits(firepower: 1, evasion: 1),

            (EquipID.SmallRadar_Type13AirRADARKai, ShipID.HarunaKaiNi) => new VisibleFits(firepower: 1, aa: 2, evasion: 3, armor: 1),

            (EquipID.Torpedo_53cmBow_OxygenTorpedo, _) => new VisibleFits(torpedo: -5),

            (EquipID.Torpedo_53cmTwinTorpedo, ShipID.KongouKaiNiC) => new VisibleFits(torpedo: 6, evasion: 3),

            (EquipID.LargeCalibreMainGun_16inchMk_ITripleGun, ShipID.KongouKaiNi) => new VisibleFits(firepower: 1, evasion: -3, armor: 1),
            (EquipID.LargeCalibreMainGun_16inchMk_ITripleGun, ShipID.HieiKaiNi) => new VisibleFits(firepower: 1, evasion: -3, armor: 1),
            (EquipID.LargeCalibreMainGun_16inchMk_ITripleGun, ShipID.KirishimaKaiNi) => new VisibleFits(firepower: 1, evasion: -3, armor: 1),
            (EquipID.LargeCalibreMainGun_16inchMk_ITripleGun, ShipID.HarunaKaiNi) => new VisibleFits(firepower: 1, evasion: -3, armor: 1),

            (EquipID.LargeCalibreMainGun_16inchMk_ITripleGun_AFCTKai, ShipID.KongouKaiNi) => new VisibleFits(firepower: 1, evasion: -3, armor: 1),
            (EquipID.LargeCalibreMainGun_16inchMk_ITripleGun_AFCTKai, ShipID.HieiKaiNi) => new VisibleFits(firepower: 1, evasion: -3, armor: 1),
            (EquipID.LargeCalibreMainGun_16inchMk_ITripleGun_AFCTKai, ShipID.KirishimaKaiNi) => new VisibleFits(firepower: 1, evasion: -3, armor: 1),
            (EquipID.LargeCalibreMainGun_16inchMk_ITripleGun_AFCTKai, ShipID.HarunaKaiNi) => new VisibleFits(firepower: 1, evasion: -3, armor: 1),

            (EquipID.LargeCalibreMainGun_16inchMk_ITripleGunKai_FCRType284, ShipID.KongouKaiNi) => new VisibleFits(firepower: 1, evasion: -3, armor: 1),
            (EquipID.LargeCalibreMainGun_16inchMk_ITripleGunKai_FCRType284, ShipID.HieiKaiNi) => new VisibleFits(firepower: 1, evasion: -3, armor: 1),
            (EquipID.LargeCalibreMainGun_16inchMk_ITripleGunKai_FCRType284, ShipID.KirishimaKaiNi) => new VisibleFits(firepower: 1, evasion: -3, armor: 1),
            (EquipID.LargeCalibreMainGun_16inchMk_ITripleGunKai_FCRType284, ShipID.HarunaKaiNi) => new VisibleFits(firepower: 1, evasion: -3, armor: 1),

            _ => new VisibleFits()
        };

        private VisibleFits VisibleFitsQueenElizabeth(EquipmentDataCustom equip) => (equip.EquipID, Ship.ShipID) switch
        {
            (EquipID.LargeCalibreMainGun_16inchMk_ITripleGun, _) => new VisibleFits(firepower: 2, evasion: -2, armor: 1),
            (EquipID.LargeCalibreMainGun_16inchMk_ITripleGun_AFCTKai, _) => new VisibleFits(firepower: 2, evasion: -2, armor: 1),
            (EquipID.LargeCalibreMainGun_16inchMk_ITripleGunKai_FCRType284, _) => new VisibleFits(firepower: 2, evasion: -2, armor: 1),

            (EquipID.AntiAircraftMachineGun_20tube7inchUPRocketLaunchers, _) => new VisibleFits(firepower: 2, evasion: 1, armor: 1),

            _ => new VisibleFits()
        };

        private VisibleFits VisibleFitsIowa(EquipmentDataCustom equip) => (equip.EquipID, Ship.ShipID) switch
        {
            (EquipID.SmallRadar_GFCSMk_37, _) => new VisibleFits(firepower: 1, aa: 1, evasion: 1),

            (EquipID.SmallRadar_SGRadar_InitialModel, _) => new VisibleFits(firepower: 2, evasion: 3, los: 4),

            _ => new VisibleFits()
        };

        private VisibleFits VisibleFitsIse(EquipmentDataCustom equip) => (equip.EquipID, Ship.ShipID) switch
        {
            (EquipID.LargeCalibreMainGun_35_6cm連装砲改, _) => new VisibleFits(firepower: 1),
            (EquipID.LargeCalibreMainGun_35_6cm連装砲改二, _) => new VisibleFits(firepower: 1),

            (EquipID.LargeCalibreMainGun_41cm連装砲改二, ShipID.IseKaiNi) => new VisibleFits(firepower: 2, aa: 2, evasion: 2),
            (EquipID.LargeCalibreMainGun_41cm連装砲改二, ShipID.HyuugaKaiNi) => new VisibleFits(firepower: 3, aa: 2, evasion: 2),

            (EquipID.LargeCalibreMainGun_41cmTripleGunKai2, ShipID.IseKaiNi) => new VisibleFits(firepower: 3, aa: 2, evasion: 1),
            (EquipID.LargeCalibreMainGun_41cmTripleGunKai2, ShipID.HyuugaKaiNi) => new VisibleFits(firepower: 3, aa: 2, evasion: 2),

            (EquipID.CarrierDiveBomber_Suisei, _) => new VisibleFits(firepower: 2),
            (EquipID.CarrierDiveBomber_SuiseiModel12A, _) => new VisibleFits(firepower: 2),
            (EquipID.CarrierDiveBomber_Suisei_601AirGroup, _) => new VisibleFits(firepower: 2),
            (EquipID.CarrierDiveBomber_Suisei_EgusaSquadron, _) => new VisibleFits(firepower: 4),
            (EquipID.CarrierDiveBomber_SuiseiModel22_634AirGroup, _) => new VisibleFits(firepower: 6, evasion: 1),
            (EquipID.CarrierDiveBomber_SuiseiModel22_634AirGroupSkilled, _) => new VisibleFits(firepower: 8, aa: 1, evasion: 2),
            (EquipID.CarrierDiveBomber_彗星一二型_六三四空三号爆弾搭載機, _) => new VisibleFits(firepower: 7, aa: 3, evasion: 2),
            (EquipID.CarrierDiveBomber_彗星一二型_三一号光電管爆弾搭載機, ShipID.IseKaiNi) => new VisibleFits(firepower: 2),
            (EquipID.CarrierDiveBomber_彗星一二型_三一号光電管爆弾搭載機, ShipID.HyuugaKaiNi) => new VisibleFits(firepower: 4),

            (EquipID.SeaplaneBomber_Zuiun_634AirGroup, _) => new VisibleFits(firepower: 3),
            (EquipID.SeaplaneBomber_ZuiunModel12_634AirGroup, _) => new VisibleFits(firepower: 3),
            (EquipID.SeaplaneBomber_Zuiun_634AirGroupSkilled, _) => new VisibleFits(firepower: 4, evasion: 2),
            (EquipID.SeaplaneBomber_瑞雲改二_六三四空, _) => new VisibleFits(firepower: 5, aa: 2, asw: 1, evasion: 2),
            (EquipID.SeaplaneBomber_瑞雲改二_六三四空熟練, _) => new VisibleFits(firepower: 6, aa: 3, asw: 2, evasion: 3),

            (EquipID.Autogyro_オ号観測機改, ShipID.IseKaiNi) => new VisibleFits(asw: 1, evasion: 1),
            (EquipID.Autogyro_オ号観測機改, ShipID.HyuugaKaiNi) => new VisibleFits(asw: 2, evasion: 1),

            (EquipID.Autogyro_オ号観測機改二, ShipID.IseKaiNi) => new VisibleFits(asw: 1, evasion: 1),
            (EquipID.Autogyro_オ号観測機改二, ShipID.HyuugaKaiNi) => new VisibleFits(asw: 2, evasion: 1),

            (EquipID.Autogyro_S51J, ShipID.IseKaiNi) => new VisibleFits(firepower: 2, asw: 1, evasion: 2),
            (EquipID.Autogyro_S51J, ShipID.HyuugaKaiNi) => new VisibleFits(firepower: 3, asw: 2, evasion: 3),

            (EquipID.Autogyro_S51J改, ShipID.IseKaiNi) => new VisibleFits(firepower: 1, asw: 3, evasion: 1),
            (EquipID.Autogyro_S51J改, ShipID.HyuugaKaiNi) => new VisibleFits(firepower: 2, asw: 4, evasion: 2),

            _ => new VisibleFits()
        };

        private VisibleFits VisibleFitsAviationBattleship(EquipmentDataCustom equip) => (equip.EquipID, Ship.ShipID) switch
        {
            (EquipID.LargeCalibreMainGun_35_6cm連装砲改, ShipID.FusouKaiNi) => new VisibleFits(firepower: 1),
            (EquipID.LargeCalibreMainGun_35_6cm連装砲改, ShipID.YamashiroKaiNi) => new VisibleFits(firepower: 1),

            (EquipID.LargeCalibreMainGun_35_6cm連装砲改二, _) => new VisibleFits(firepower: 1),

            (EquipID.LargeCalibreMainGun_41cm連装砲改二, ShipID.IseKai) => new VisibleFits(firepower: 2, aa: 2, evasion: 2),
            (EquipID.LargeCalibreMainGun_41cm連装砲改二, ShipID.HyuugaKai) => new VisibleFits(firepower: 2, aa: 2, evasion: 2),
            (EquipID.LargeCalibreMainGun_41cm連装砲改二, ShipID.FusouKaiNi) => new VisibleFits(firepower: 1),
            (EquipID.LargeCalibreMainGun_41cm連装砲改二, ShipID.YamashiroKaiNi) => new VisibleFits(firepower: 1),

            (EquipID.LargeCalibreMainGun_41cmTripleGunKai2, ShipID.IseKai) => new VisibleFits(firepower: 2, aa: 2, evasion: 1),
            (EquipID.LargeCalibreMainGun_41cmTripleGunKai2, ShipID.HyuugaKai) => new VisibleFits(firepower: 2, aa: 2, evasion: 1),
            (EquipID.LargeCalibreMainGun_41cmTripleGunKai2, ShipID.FusouKaiNi) => new VisibleFits(firepower: 1),
            (EquipID.LargeCalibreMainGun_41cmTripleGunKai2, ShipID.YamashiroKaiNi) => new VisibleFits(firepower: 1),

            (EquipID.SeaplaneBomber_Zuiun_634AirGroup, ShipID.IseKai) => new VisibleFits(firepower: 2),
            (EquipID.SeaplaneBomber_Zuiun_634AirGroup, ShipID.HyuugaKai) => new VisibleFits(firepower: 2),
            (EquipID.SeaplaneBomber_Zuiun_634AirGroup, ShipID.FusouKaiNi) => new VisibleFits(firepower: 2),
            (EquipID.SeaplaneBomber_Zuiun_634AirGroup, ShipID.YamashiroKaiNi) => new VisibleFits(firepower: 2),

            (EquipID.SeaplaneBomber_ZuiunModel12_634AirGroup, ShipID.IseKai) => new VisibleFits(firepower: 2),
            (EquipID.SeaplaneBomber_ZuiunModel12_634AirGroup, ShipID.HyuugaKai) => new VisibleFits(firepower: 2),
            (EquipID.SeaplaneBomber_ZuiunModel12_634AirGroup, ShipID.FusouKaiNi) => new VisibleFits(firepower: 2),
            (EquipID.SeaplaneBomber_ZuiunModel12_634AirGroup, ShipID.YamashiroKaiNi) => new VisibleFits(firepower: 2),

            (EquipID.SeaplaneBomber_Zuiun_634AirGroupSkilled, ShipID.IseKai) => new VisibleFits(firepower: 3, evasion: 1),
            (EquipID.SeaplaneBomber_Zuiun_634AirGroupSkilled, ShipID.HyuugaKai) => new VisibleFits(firepower: 3, evasion: 1),
            (EquipID.SeaplaneBomber_Zuiun_634AirGroupSkilled, ShipID.FusouKaiNi) => new VisibleFits(firepower: 2),
            (EquipID.SeaplaneBomber_Zuiun_634AirGroupSkilled, ShipID.YamashiroKaiNi) => new VisibleFits(firepower: 2),

            _ => new VisibleFits()
        };

        private VisibleFits VisibleFitsNelson(EquipmentDataCustom equip) => (equip.EquipID, Ship.ShipID) switch
        {
            (EquipID.LargeCalibreMainGun_16inchMk_ITripleGun, _) => new VisibleFits(firepower: 2, armor: 1),
            (EquipID.LargeCalibreMainGun_16inchMk_ITripleGun_AFCTKai, _) => new VisibleFits(firepower: 2, armor: 1),
            (EquipID.LargeCalibreMainGun_16inchMk_ITripleGunKai_FCRType284, _) => new VisibleFits(firepower: 2, armor: 1),

            (EquipID.AntiAircraftMachineGun_20tube7inchUPRocketLaunchers, _) => new VisibleFits(firepower: 2, evasion: 1, armor: 1),

            (EquipID.LargeCalibreMainGun_16inchMk_I連装砲, _) => new VisibleFits(firepower: 2),
            (EquipID.LargeCalibreMainGun_16inchMk_V連装砲, _) => new VisibleFits(firepower: 2),
            (EquipID.LargeCalibreMainGun_16inchMk_VIII連装砲改, _) => new VisibleFits(firepower: 2),

            _ => new VisibleFits()
        };

        private VisibleFits VisibleFitsNagato(EquipmentDataCustom equip) => (equip.EquipID, Ship.ShipID) switch
        {
            (EquipID.LargeCalibreMainGun_41cm連装砲改二, ShipID.NagatoKaiNi) => new VisibleFits(firepower: 3, aa: 2, evasion: 1),
            (EquipID.LargeCalibreMainGun_41cm連装砲改二, ShipID.MutsuKaiNi) => new VisibleFits(firepower: 3, aa: 2, evasion: 1),

            (EquipID.LargeCalibreMainGun_16inchMk_I連装砲, ShipID.NagatoKaiNi) => new VisibleFits(firepower: 2),
            (EquipID.LargeCalibreMainGun_16inchMk_I連装砲, ShipID.MutsuKaiNi) => new VisibleFits(firepower: 2),
            (EquipID.LargeCalibreMainGun_16inchMk_I連装砲, _) => new VisibleFits(firepower: 1),

            (EquipID.LargeCalibreMainGun_16inchMk_V連装砲, ShipID.NagatoKaiNi) => new VisibleFits(firepower: 2),
            (EquipID.LargeCalibreMainGun_16inchMk_V連装砲, ShipID.MutsuKaiNi) => new VisibleFits(firepower: 2),
            (EquipID.LargeCalibreMainGun_16inchMk_V連装砲, _) => new VisibleFits(firepower: 1),

            (EquipID.LargeCalibreMainGun_16inchMk_VIII連装砲改, ShipID.NagatoKaiNi) => new VisibleFits(firepower: 2),
            (EquipID.LargeCalibreMainGun_16inchMk_VIII連装砲改, ShipID.MutsuKaiNi) => new VisibleFits(firepower: 2),
            (EquipID.LargeCalibreMainGun_16inchMk_VIII連装砲改, _) => new VisibleFits(firepower: 1),

            (EquipID.SmallRadar_Type13AirRADARKai, ShipID.NagatoKaiNi) => new VisibleFits(firepower: 1, aa: 2, evasion: 3, armor: 1),

            _ => new VisibleFits()
        };

        private VisibleFits VisibleFitsColorado(EquipmentDataCustom equip) => (equip.EquipID, Ship.ShipID) switch
        {
            (EquipID.LargeCalibreMainGun_16inchMk_I連装砲, _) => new VisibleFits(firepower: 1),

            (EquipID.LargeCalibreMainGun_16inchMk_V連装砲, _) => new VisibleFits(firepower: 2, evasion: 1),

            (EquipID.LargeCalibreMainGun_16inchMk_VIII連装砲改, ShipID.Colorado) => new VisibleFits(firepower: 1),
            (EquipID.LargeCalibreMainGun_16inchMk_VIII連装砲改, ShipID.Colorado改) => new VisibleFits(firepower: 2, aa: 1, evasion: 1),

            (EquipID.SmallRadar_GFCSMk_37, _) => new VisibleFits(firepower: 1, aa: 1, evasion: 1),

            (EquipID.SmallRadar_SGRadar_InitialModel, _) => new VisibleFits(firepower: 2, evasion: 3, los: 4),

            _ => new VisibleFits()
        };

        #endregion

        #region BB synergy

        private VisibleFits SynergyKongou(ShipDataCustom ship)
        {
            VisibleFits synergy = new VisibleFits();

            if (ship.Equipment.Any(eq => eq?.EquipID == EquipID.LargeCalibreMainGun_35_6cmTripleGunKai_DazzleCamouflage))
            {
                synergy += ship.ShipID switch
                {
                    ShipID.KongouKaiNi when ship.HasSurfaceRadar => new VisibleFits(firepower: 2),
                    ShipID.HarunaKaiNi when ship.HasSurfaceRadar => new VisibleFits(firepower: 2, evasion: 2),

                    _ => new VisibleFits()
                };
            }

            if (ship.Equipment.Any(eq => eq?.EquipID == EquipID.AntiAircraftShell_Type3Shell))
            {
                synergy += ship.ShipID switch
                {
                    ShipID.KongouKaiNi => new VisibleFits(firepower: 1, aa: 1),
                    ShipID.KongouKaiNiC => new VisibleFits(firepower: 1, aa: 1),
                    ShipID.HieiKaiNi => new VisibleFits(aa: 1),
                    ShipID.HarunaKaiNi => new VisibleFits(aa: 1, evasion: 1),
                    ShipID.KirishimaKaiNi => new VisibleFits(firepower: 1),

                    _ => new VisibleFits()
                };
            }

            if (ship.Equipment.Any(eq => eq?.EquipID == EquipID.AntiAircraftShell_三式弾改))
            {
                synergy += ship.ShipID switch
                {
                    ShipID.KongouKaiNi => new VisibleFits(firepower: 3, aa: 3),
                    ShipID.KongouKaiNiC => new VisibleFits(firepower: 3, aa: 3),
                    ShipID.HieiKaiNi => new VisibleFits(firepower: 2, aa: 2),
                    ShipID.HarunaKaiNi => new VisibleFits(firepower: 2, aa: 2, evasion: 1),
                    ShipID.KirishimaKaiNi => new VisibleFits(firepower: 3, aa: 2),

                    _ => new VisibleFits(firepower: 1, aa: 1)
                };
            }

            if (ship.Equipment.Any(eq => eq?.EquipID == EquipID.Searchlight_Searchlight))
            {
                VisibleFits fit = new VisibleFits(firepower: 2, evasion: -1);

                synergy += ship.ShipID switch
                {
                    ShipID.Hiei => fit,
                    ShipID.HieiKai => fit,
                    ShipID.HieiKaiNi => fit,

                    ShipID.Kirishima => fit,
                    ShipID.KirishimaKai => fit,
                    ShipID.KirishimaKaiNi => fit,

                    _ => new VisibleFits()
                };
            }

            if (ship.Equipment.Any(eq => eq?.EquipID == EquipID.LargeSearchlight_Type96150cmSearchlight))
            {
                VisibleFits fit = new VisibleFits(firepower: 3, evasion: -2);

                synergy += ship.ShipID switch
                {
                    ShipID.Hiei => fit,
                    ShipID.HieiKai => fit,
                    ShipID.HieiKaiNi => fit,

                    ShipID.Kirishima => fit,
                    ShipID.KirishimaKai => fit,
                    ShipID.KirishimaKaiNi => fit,

                    _ => new VisibleFits()
                };
            }

            return synergy;
        }

        private VisibleFits SynergyIse(ShipDataCustom ship)
        {
            VisibleFits synergy = new VisibleFits();

            if (ship.Equipment.Any(eq => eq?.EquipID == EquipID.LargeCalibreMainGun_41cmTripleGunKai2))
            {
                synergy += ship.ShipID switch
                {
                    ShipID.IseKai when ship.HasAirRadar => new VisibleFits(aa: 2, evasion: 3),
                    ShipID.HyuugaKai when ship.HasAirRadar => new VisibleFits(aa: 2, evasion: 3),
                    ShipID.IseKaiNi when ship.HasAirRadar => new VisibleFits(aa: 2, evasion: 3),
                    ShipID.HyuugaKaiNi when ship.HasAirRadar => new VisibleFits(aa: 2, evasion: 3),

                    _ => new VisibleFits()
                };

                if (ship.Equipment.Any(eq => eq?.EquipID == EquipID.LargeCalibreMainGun_41cm連装砲改二))
                {
                    synergy += ship.ShipID switch
                    {
                        ShipID.IseKai => new VisibleFits(evasion: 2, armor: 1),
                        ShipID.HyuugaKai => new VisibleFits(evasion: 2, armor: 1),
                        ShipID.IseKaiNi => new VisibleFits(evasion: 2, armor: 1),
                        ShipID.HyuugaKaiNi => new VisibleFits(firepower: 1, evasion: 2, armor: 1),

                        _ => new VisibleFits()
                    };
                }
            }

            if (ship.Equipment.Any(eq => eq?.CategoryType == EquipmentTypes.CarrierBasedRecon ||
                                         eq?.CategoryType == EquipmentTypes.CarrierBasedRecon2))
            {
                synergy += ship.ShipID switch
                {
                    ShipID.IseKaiNi => new VisibleFits(firepower: 3, evasion:2, armor: 1),

                    ShipID.HyuugaKaiNi => new VisibleFits(firepower: 3, evasion: 3, armor: 3),

                    _ => new VisibleFits()
                };

                synergy += SynergyCarrierReconUpgrade(ship);
            }

            return synergy;
        }

        private VisibleFits SynergyNagato(ShipDataCustom ship)
        {
            VisibleFits synergy = new VisibleFits();

            if (ship.Equipment.Any(eq => eq?.EquipID == EquipID.LargeCalibreMainGun_41cmTripleGunKai2) && 
                ship.Equipment.Any(eq => eq?.EquipID == EquipID.LargeCalibreMainGun_41cm連装砲改二))
            {
                synergy += ship.ShipID switch
                {
                    ShipID.NagatoKaiNi => new VisibleFits(firepower: 2, evasion: 2, armor: 1),
                    ShipID.MutsuKaiNi => new VisibleFits(firepower: 2, evasion: 2, armor: 1),

                    _ => new VisibleFits()
                };
            }

            if (ship.Equipment.Any(eq => eq?.EquipID == EquipID.AntiAircraftShell_三式弾改))
            {
                synergy += ship.ShipID switch
                {
                    ShipID.NagatoKaiNi => new VisibleFits(firepower: 1, aa: 2),
                    ShipID.MutsuKaiNi => new VisibleFits(firepower: 2, aa: 2, evasion: 1),

                    _ => new VisibleFits()
                };
            }

            return synergy;
        }

        #endregion

        #region BB accuracy

        private int AccuracyGangut(EquipmentDataCustom equip) => equip.FitCategory switch
        {
            FitCategories.smallBBGun when equip.EquipID == EquipID.LargeCalibreMainGun_30_5cmTripleGun || 
                                          equip.EquipID == EquipID.LargeCalibreMainGun_30_5cmTripleGunKai => 10,
            FitCategories.smallBBGun => 7,
            FitCategories.baguetteBBGun => 0,
            FitCategories.pastaBBGun => 1,
            FitCategories.nelsonBBGun => -8,
            FitCategories.burgerBBGun => -3,
            FitCategories.mediumBBGun => -10,
            FitCategories.largeBBGun when equip.EquipID == EquipID.LargeCalibreMainGun_Prototype46cmTwinGun => -7,
            FitCategories.largeBBGun => -18,

            _ => 0
        };

        private int AccuracyKongou(EquipmentDataCustom equip) => equip.FitCategory switch
        {
            FitCategories.smallBBGun => 7,
            FitCategories.baguetteBBGun => 0,
            FitCategories.pastaBBGun => -2,
            FitCategories.nelsonBBGun when equip.EquipID == EquipID.LargeCalibreMainGun_16inchMk_ITripleGun_AFCTKai => -6,
            FitCategories.nelsonBBGun when equip.EquipID == EquipID.LargeCalibreMainGun_16inchMk_ITripleGunKai_FCRType284 => -8,
            FitCategories.nelsonBBGun => -5,
            FitCategories.burgerBBGun when equip.EquipID == EquipID.LargeCalibreMainGun_16inchTripleGunMk_7_GFCS => -6,
            FitCategories.burgerBBGun => -5,
            FitCategories.mediumBBGun => -5,
            FitCategories.largeBBGun when equip.EquipID == EquipID.LargeCalibreMainGun_Prototype46cmTwinGun => -7,
            FitCategories.largeBBGun => -10,

            _ => 0
        };

        private int AccuracyBismarck(EquipmentDataCustom equip) => equip.FitCategory switch
        {
            FitCategories.smallBBGun => 4,
            FitCategories.baguetteBBGun => -2,
            FitCategories.pastaBBGun => 1,
            FitCategories.nelsonBBGun => -5,
            FitCategories.burgerBBGun when equip.EquipID == EquipID.LargeCalibreMainGun_16inchTripleGunMk_7_GFCS => -4,
            FitCategories.burgerBBGun => -5,
            FitCategories.mediumBBGun => -5,
            FitCategories.largeBBGun when equip.EquipID == EquipID.LargeCalibreMainGun_Prototype46cmTwinGun => -7,
            FitCategories.largeBBGun => -10,

            _ => 0
        };

        private int AccuracyIowa(EquipmentDataCustom equip) => equip.FitCategory switch
        {
            FitCategories.smallBBGun => 4,
            FitCategories.baguetteBBGun => -2,
            FitCategories.pastaBBGun => -2,
            FitCategories.nelsonBBGun => -5,
            FitCategories.burgerBBGun when equip.EquipID == EquipID.LargeCalibreMainGun_16inchTripleGunMk_7_GFCS && Ship.IsMarried => 5,
            FitCategories.burgerBBGun when equip.EquipID == EquipID.LargeCalibreMainGun_16inchTripleGunMk_7_GFCS => -2,
            FitCategories.burgerBBGun when Ship.IsMarried => -3,
            FitCategories.burgerBBGun => -5,
            FitCategories.mediumBBGun => -5,
            FitCategories.largeBBGun when equip.EquipID == EquipID.LargeCalibreMainGun_Prototype46cmTwinGun => -7,
            FitCategories.largeBBGun => -10,

            _ => 0
        };

        private int AccuracyRichelieu(EquipmentDataCustom equip) => equip.FitCategory switch
        {
            FitCategories.smallBBGun => 4,
            FitCategories.baguetteBBGun => 4,
            FitCategories.pastaBBGun => -2,
            FitCategories.nelsonBBGun when equip.EquipID == EquipID.LargeCalibreMainGun_16inchMk_ITripleGun_AFCTKai => -14,
            FitCategories.nelsonBBGun when equip.EquipID == EquipID.LargeCalibreMainGun_16inchMk_ITripleGunKai_FCRType284 => -8,
            FitCategories.nelsonBBGun => -7,
            FitCategories.burgerBBGun => -5,
            FitCategories.mediumBBGun => -5,
            FitCategories.largeBBGun when equip.EquipID == EquipID.LargeCalibreMainGun_Prototype46cmTwinGun => -7,
            FitCategories.largeBBGun => -10,

            _ => 0
        };

        private int AccuracyQueenElizabeth(EquipmentDataCustom equip) => equip.FitCategory switch
        {
            FitCategories.smallBBGun when equip.EquipID == EquipID.LargeCalibreMainGun_38_1cmMk_ITwinGun || 
                                          equip.EquipID == EquipID.LargeCalibreMainGun_38_1cmMk_INTwinGunKai => 8,
            FitCategories.smallBBGun => 6,
            FitCategories.baguetteBBGun => 0,
            FitCategories.pastaBBGun => 1,
            FitCategories.nelsonBBGun when equip.EquipID == EquipID.LargeCalibreMainGun_16inchMk_ITripleGunKai_FCRType284 => 3,
            FitCategories.nelsonBBGun => 5,
            FitCategories.burgerBBGun => -2,
            FitCategories.mediumBBGun => 2,
            FitCategories.largeBBGun when equip.EquipID == EquipID.LargeCalibreMainGun_Prototype46cmTwinGun => -8,
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
            FitCategories.mediumBBGun when equip.EquipID == EquipID.LargeCalibreMainGun_41cm連装砲改二 => 0,
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
            FitCategories.largeBBGun when equip.EquipID == EquipID.LargeCalibreMainGun_Prototype46cmTwinGun => -3,
            FitCategories.largeBBGun when equip.EquipID == EquipID.LargeCalibreMainGun_46cmTripleGunKai => -4,
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
            FitCategories.mediumBBGun when equip.EquipID == EquipID.LargeCalibreMainGun_41cm連装砲改二 => 6,
            FitCategories.mediumBBGun => 4,
            FitCategories.largeBBGun when equip.EquipID == EquipID.LargeCalibreMainGun_Prototype46cmTwinGun => -3,
            FitCategories.largeBBGun when equip.EquipID == EquipID.LargeCalibreMainGun_46cmTripleGunKai => -4,
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
            FitCategories.largeBBGun when equip.EquipID == EquipID.LargeCalibreMainGun_Prototype46cmTwinGun => -3,
            FitCategories.largeBBGun when equip.EquipID == EquipID.LargeCalibreMainGun_46cmTripleGunKai => -4,
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
            FitCategories.nelsonBBGun when equip.EquipID == EquipID.LargeCalibreMainGun_16inchMk_ITripleGun_AFCTKai => 4,
            FitCategories.nelsonBBGun => 2,
            FitCategories.burgerBBGun => 0,
            FitCategories.mediumBBGun when equip.EquipID == EquipID.LargeCalibreMainGun_41cm連装砲改二 => 6,
            FitCategories.mediumBBGun => 4,
            FitCategories.largeBBGun when equip.EquipID == EquipID.LargeCalibreMainGun_Prototype46cmTwinGun => -2,
            FitCategories.largeBBGun when equip.EquipID == EquipID.LargeCalibreMainGun_46cmTripleGunKai => -6,
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
            FitCategories.mediumBBGun when equip.EquipID == EquipID.LargeCalibreMainGun_41cm連装砲改二 => 6,
            FitCategories.mediumBBGun => 5,
            FitCategories.largeBBGun when equip.EquipID == EquipID.LargeCalibreMainGun_Prototype46cmTwinGun => -2,
            FitCategories.largeBBGun when equip.EquipID == EquipID.LargeCalibreMainGun_46cmTripleGunKai => -6,
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
            FitCategories.largeBBGun when equip.EquipID == EquipID.LargeCalibreMainGun_46cmTripleGunKai => 7,
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
                synergy += ship.ShipID switch
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
                synergy += ship.ShipID switch
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
                synergy += ship.ShipID switch
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
                synergy += ship.ShipID switch
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

            if (ship.Equipment.Any(eq => eq?.EquipID == EquipID.SmallCalibreMainGun_12_7cmTwinGunModelCKai2))
            {
                synergy += ship.ShipID switch
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

            if (ship.Equipment.Any(eq => eq?.EquipID == EquipID.SmallCalibreMainGun_12_7cmTwinGunModelCKai2))
            {
                synergy += ship.ShipID switch
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

        private VisibleFits VisibleFitsShimushu(EquipmentDataCustom equip) => (equip.EquipID, Ship.ShipID) switch
        {
            (EquipID.SmallCalibreMainGun_12cmSingleGunKai2, _) => new VisibleFits(firepower: 1, aa: 1, evasion: 2),
            (EquipID.SmallCalibreMainGun_12_7cmSingleHighangleGun_LateModel, _) when equip.Level > 6 => new VisibleFits(firepower: 1, aa: 1),

            (EquipID.SmallCalibreMainGun_5inchSingleGunMk_30Kai_GFCSMk_37, _) => new VisibleFits(aa:1,evasion:1 ),

            _ => new VisibleFits()
        };

        private VisibleFits VisibleFitsEtorofu(EquipmentDataCustom equip) => (equip.EquipID, Ship.ShipID) switch
        {
            (EquipID.SmallCalibreMainGun_12cmSingleGunKai2, _) => new VisibleFits(firepower: 1, aa: 1, evasion: 2),
            (EquipID.SmallCalibreMainGun_12_7cmSingleHighangleGun_LateModel, _) when equip.Level > 6 => new VisibleFits(firepower: 1, aa: 1),

            (EquipID.SmallCalibreMainGun_5inchSingleGunMk_30Kai_GFCSMk_37, _) => new VisibleFits(aa: 1, evasion: 1),

            _ => new VisibleFits()
        };

        private VisibleFits VisibleFitsMikura(EquipmentDataCustom equip) => (equip.EquipID, Ship.ShipID) switch
        {
            (EquipID.SmallCalibreMainGun_12_7cmSingleHighangleGun_LateModel, _) when equip.Level > 6 => new VisibleFits(firepower: 1, aa: 1),

            (EquipID.SmallCalibreMainGun_5inchSingleGunMk_30Kai_GFCSMk_37, _) => new VisibleFits(aa: 1, evasion: 1),

            _ => new VisibleFits()
        };

        private VisibleFits VisibleFitsHiburi(EquipmentDataCustom equip) => (equip.EquipID, Ship.ShipID) switch
        {
            (EquipID.SmallCalibreMainGun_12_7cmSingleHighangleGun_LateModel, _) when equip.Level > 6 => new VisibleFits(firepower: 1, aa: 1),

            (EquipID.SmallCalibreMainGun_5inchSingleGunMk_30Kai_GFCSMk_37, _) => new VisibleFits(aa: 1, evasion: 1),

            _ => new VisibleFits()
        };


        #endregion

        #region DE synergy

        private VisibleFits SynergyShimushu(ShipDataCustom ship)
        {
            VisibleFits synergy = new VisibleFits();

            if (ship.Equipment.Any(eq => eq?.EquipID == EquipID.SmallCalibreMainGun_12cmSingleGunKai2))
            {
                synergy += ship.ShipID switch
                {
                    _ when ship.HasSurfaceRadar => new VisibleFits(firepower: 2, torpedo: 1, evasion: 3),

                    _ => new VisibleFits()
                };
            }

            if (ship.Equipment.Any(eq => eq?.EquipID == EquipID.SmallCalibreMainGun_12_7cmSingleHighangleGun_LateModel && eq.Level > 6))
            {
                synergy += ship.ShipID switch
                {
                    _ when ship.HasSurfaceRadar => new VisibleFits(firepower: 1, evasion: 4),

                    _ => new VisibleFits()
                };
            }

            return synergy;
        }

        private VisibleFits SynergyEtorofu(ShipDataCustom ship)
        {
            VisibleFits synergy = new VisibleFits();

            if (ship.Equipment.Any(eq => eq?.EquipID == EquipID.SmallCalibreMainGun_12cmSingleGunKai2))
            {
                synergy += ship.ShipID switch
                {
                    _ when ship.HasSurfaceRadar => new VisibleFits(firepower: 2, torpedo: 1, evasion: 3),

                    _ => new VisibleFits()
                };
            }

            if (ship.Equipment.Any(eq => eq?.EquipID == EquipID.SmallCalibreMainGun_12_7cmSingleHighangleGun_LateModel && eq.Level > 6))
            {
                synergy += ship.ShipID switch
                {
                    _ when ship.HasSurfaceRadar => new VisibleFits(firepower: 1, evasion: 4),

                    _ => new VisibleFits()
                };
            }

            return synergy;
        }

        private VisibleFits SynergyMikura(ShipDataCustom ship)
        {
            VisibleFits synergy = new VisibleFits();

            if (ship.Equipment.Any(eq => eq?.EquipID == EquipID.SmallCalibreMainGun_12_7cmSingleHighangleGun_LateModel && eq.Level > 6))
            {
                synergy += ship.ShipID switch
                {
                    _ when ship.HasSurfaceRadar => new VisibleFits(firepower: 1, evasion: 4),

                    _ => new VisibleFits()
                };
            }

            return synergy;
        }

        private VisibleFits SynergyHiburi(ShipDataCustom ship)
        {
            VisibleFits synergy = new VisibleFits();

            if (ship.Equipment.Any(eq => eq?.EquipID == EquipID.SmallCalibreMainGun_12_7cmSingleHighangleGun_LateModel && eq.Level > 6))
            {
                synergy += ship.ShipID switch
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

        private VisibleFits VisibleFitsAbruzzi(EquipmentDataCustom equip) => (equip.EquipID, Ship.ShipID) switch
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

            if (ship.Equipment.Any(eq => eq?.CategoryType == EquipmentTypes.CarrierBasedRecon ||
                                         eq?.CategoryType == EquipmentTypes.CarrierBasedRecon2))
            {
                int level = ship.Equipment
                    .Where(eq => eq?.EquipID == EquipID.CarrierRecon_Type2ReconAircraft)
                    .Max(eq => eq.Level);

                synergy += ship.ShipID switch
                {
                    ShipID.SuzuyaCVLKaiNi when level > 0 => new VisibleFits(firepower: 1, los: 1),

                    ShipID.KumanoCVLKaiNi when level > 0 => new VisibleFits(firepower: 1, los: 1),

                    _ => new VisibleFits()
                };

                synergy += SynergyCarrierReconUpgrade(ship);
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

            if (ship.Equipment.Any(eq => eq?.CategoryType == EquipmentTypes.CarrierBasedRecon ||
                                         eq?.CategoryType == EquipmentTypes.CarrierBasedRecon2))
            {
                synergy += SynergyCarrierReconUpgrade(ship);
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

            if (ship.Equipment.Any(eq => eq?.CategoryType == EquipmentTypes.CarrierBasedRecon ||
                                         eq?.CategoryType == EquipmentTypes.CarrierBasedRecon2))
            {
                synergy += SynergyCarrierReconUpgrade(ship);
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

            if (ship.Equipment.Any(eq => eq?.CategoryType == EquipmentTypes.CarrierBasedRecon ||
                                         eq?.CategoryType == EquipmentTypes.CarrierBasedRecon2))
            {
                int level = ship.Equipment
                    .Where(eq => eq?.EquipID == EquipID.CarrierRecon_Type2ReconAircraft)
                    .Max(eq => eq.Level);

                synergy += ship.ShipID switch
                {
                    ShipID.SouryuuKaiNi when level > 7 => new VisibleFits(firepower: 4, los: 4),
                    ShipID.SouryuuKaiNi when level > 0 => new VisibleFits(firepower: 3, los: 3),

                    _ => new VisibleFits()
                };

                synergy += SynergyCarrierReconUpgrade(ship);
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

            if (ship.Equipment.Any(eq => eq?.CategoryType == EquipmentTypes.CarrierBasedRecon ||
                                         eq?.CategoryType == EquipmentTypes.CarrierBasedRecon2))
            {
                int level = ship.Equipment
                    .Where(eq => eq?.EquipID == EquipID.CarrierRecon_Type2ReconAircraft)
                    .Max(eq => eq.Level);

                synergy += ship.ShipID switch
                {
                    ShipID.HiryuuKaiNi when level > 0 => new VisibleFits(firepower: 2, los: 2),

                    _ => new VisibleFits()
                };

                synergy += SynergyCarrierReconUpgrade(ship);
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

            if (ship.Equipment.Any(eq => eq?.CategoryType == EquipmentTypes.CarrierBasedRecon ||
                                         eq?.CategoryType == EquipmentTypes.CarrierBasedRecon2))
            {
                synergy += SynergyCarrierReconUpgrade(ship);
            }

            return synergy;
        }

        private VisibleFits SynergyUnryuu(ShipDataCustom ship)
        {
            VisibleFits synergy = new VisibleFits();

            if (ship.Equipment.Any(eq => eq?.CategoryType == EquipmentTypes.CarrierBasedRecon ||
                                         eq?.CategoryType == EquipmentTypes.CarrierBasedRecon2))
            {
                synergy += SynergyCarrierReconUpgrade(ship);
            }

            return synergy;
        }

        private VisibleFits SynergyTaihou(ShipDataCustom ship)
        {
            VisibleFits synergy = new VisibleFits();

            if (ship.Equipment.Any(eq => eq?.CategoryType == EquipmentTypes.CarrierBasedRecon ||
                                         eq?.CategoryType == EquipmentTypes.CarrierBasedRecon2))
            {
                synergy += SynergyCarrierReconUpgrade(ship);
            }

            return synergy;
        }

        private VisibleFits SynergyGrafZeppelin(ShipDataCustom ship)
        {
            VisibleFits synergy = new VisibleFits();

            if (ship.Equipment.Any(eq => eq?.CategoryType == EquipmentTypes.CarrierBasedRecon ||
                                         eq?.CategoryType == EquipmentTypes.CarrierBasedRecon2))
            {
                synergy += SynergyCarrierReconUpgrade(ship);
            }

            return synergy;
        }

        private VisibleFits SynergyAquila(ShipDataCustom ship)
        {
            VisibleFits synergy = new VisibleFits();

            if (ship.Equipment.Any(eq => eq?.CategoryType == EquipmentTypes.CarrierBasedRecon ||
                                         eq?.CategoryType == EquipmentTypes.CarrierBasedRecon2))
            {
                synergy += SynergyCarrierReconUpgrade(ship);
            }

            return synergy;
        }

        private VisibleFits SynergyLexington(ShipDataCustom ship)
        {
            VisibleFits synergy = new VisibleFits();

            if (ship.Equipment.Any(eq => eq?.CategoryType == EquipmentTypes.CarrierBasedRecon ||
                                         eq?.CategoryType == EquipmentTypes.CarrierBasedRecon2))
            {
                synergy += SynergyCarrierReconUpgrade(ship);
            }

            return synergy;
        }

        private VisibleFits SynergyEssex(ShipDataCustom ship)
        {
            VisibleFits synergy = new VisibleFits();

            if (ship.Equipment.Any(eq => eq?.CategoryType == EquipmentTypes.CarrierBasedRecon ||
                                         eq?.CategoryType == EquipmentTypes.CarrierBasedRecon2))
            {
                synergy += SynergyCarrierReconUpgrade(ship);
            }

            return synergy;
        }

        private VisibleFits SynergyArkRoyal(ShipDataCustom ship)
        {
            VisibleFits synergy = new VisibleFits();

            if (ship.Equipment.Any(eq => eq?.CategoryType == EquipmentTypes.CarrierBasedRecon ||
                                         eq?.CategoryType == EquipmentTypes.CarrierBasedRecon2))
            {
                synergy += SynergyCarrierReconUpgrade(ship);
            }

            return synergy;
        }

        private VisibleFits SynergyHoushou(ShipDataCustom ship)
        {
            VisibleFits synergy = new VisibleFits();

            if (ship.Equipment.Any(eq => eq?.CategoryType == EquipmentTypes.CarrierBasedRecon ||
                                         eq?.CategoryType == EquipmentTypes.CarrierBasedRecon2))
            {
                synergy += SynergyCarrierReconUpgrade(ship);
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

            if (ship.Equipment.Any(eq => eq?.CategoryType == EquipmentTypes.CarrierBasedRecon ||
                                         eq?.CategoryType == EquipmentTypes.CarrierBasedRecon2))
            {
                synergy += SynergyCarrierReconUpgrade(ship);
            }

            return synergy;
        }

        private VisibleFits SynergyShouhou(ShipDataCustom ship)
        {
            VisibleFits synergy = new VisibleFits();

            if (ship.Equipment.Any(eq => eq?.CategoryType == EquipmentTypes.CarrierBasedRecon ||
                                         eq?.CategoryType == EquipmentTypes.CarrierBasedRecon2))
            {
                int level = ship.Equipment
                    .Where(eq => eq?.EquipID == EquipID.CarrierRecon_Type2ReconAircraft)
                    .Max(eq => eq.Level);

                synergy += ship.ShipID switch
                {
                    ShipID.ZuihouKaiNiB when level > 0 => new VisibleFits(firepower: 1, los: 1),

                    _ => new VisibleFits()
                };

                synergy += SynergyCarrierReconUpgrade(ship);
            }

            return synergy;
        }

        private VisibleFits SynergyHiyou(ShipDataCustom ship)
        {
            VisibleFits synergy = new VisibleFits();

            if (ship.Equipment.Any(eq => eq?.CategoryType == EquipmentTypes.CarrierBasedRecon ||
                                         eq?.CategoryType == EquipmentTypes.CarrierBasedRecon2))
            {
                synergy += SynergyCarrierReconUpgrade(ship);
            }

            return synergy;
        }

        private VisibleFits SynergyTaiyou(ShipDataCustom ship)
        {
            VisibleFits synergy = new VisibleFits();

            if (ship.Equipment.Any(eq => eq?.CategoryType == EquipmentTypes.CarrierBasedRecon ||
                                         eq?.CategoryType == EquipmentTypes.CarrierBasedRecon2))
            {
                synergy += SynergyCarrierReconUpgrade(ship);
            }

            return synergy;
        }

        private VisibleFits SynergyCasablanca(ShipDataCustom ship)
        {
            VisibleFits synergy = new VisibleFits();

            if (ship.Equipment.Any(eq => eq?.CategoryType == EquipmentTypes.CarrierBasedRecon ||
                                         eq?.CategoryType == EquipmentTypes.CarrierBasedRecon2))
            {
                synergy +=  SynergyCarrierReconUpgrade(ship);
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
                    ShipID.ChitoseCVL => SynergyCarrierReconUpgrade(ship),
                    ShipID.ChitoseCVLKai => SynergyCarrierReconUpgrade(ship),
                    ShipID.ChitoseCVLKaiNi => SynergyCarrierReconUpgrade(ship),

                    ShipID.ChiyodaCVL => SynergyCarrierReconUpgrade(ship),
                    ShipID.ChiyodaCVLKai => SynergyCarrierReconUpgrade(ship),
                    ShipID.ChiyodaCVLKaiNi => SynergyCarrierReconUpgrade(ship),

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
                    ShipID.Ryuuhou => SynergyCarrierReconUpgrade(ship),
                    ShipID.RyuuhouKai => SynergyCarrierReconUpgrade(ship),

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
        /// todo: the upgrade bonus is the same for all carrier recons?
        /// </summary>
        private VisibleFits SynergyCarrierReconUpgrade(ShipDataCustom ship)
        {
            VisibleFits synergy = new VisibleFits();

            if (ship.Equipment.Any(eq => eq?.CategoryType == EquipmentTypes.CarrierBasedRecon ||
                                         eq?.CategoryType == EquipmentTypes.CarrierBasedRecon2))
            {
                int level = ship.Equipment
                    .Where(eq => eq?.CategoryType == EquipmentTypes.CarrierBasedRecon ||
                                 eq?.CategoryType == EquipmentTypes.CarrierBasedRecon2)
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
