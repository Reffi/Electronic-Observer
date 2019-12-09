using System;

namespace ElectronicObserverTypes
{
    public interface IEquipmentData
    {
        int Level { get; }
        int AircraftLevel { get; }
        IEquipmentDataMaster MasterEquipment { get; }
    }

    public interface IEquipmentDataMaster
    {
        int EquipmentID { get; }
        string Name { get; }
        int Firepower { get; }
        int Torpedo { get; }
        int Bomber { get; }
        int Accuracy { get; }
        int Evasion { get; }
        int ASW { get; }
        int LOS { get; }
        int Armor { get; }
        int AA { get; }
        int Range { get; }
        EquipmentTypes CategoryType { get; }
    }

    public class EquipmentDataCustom : IShellingDamageAttackerEquipment, ICarrierShellingDamageEquipment,
        IAswDamageAttackerEquipment, INightDamageAttackerEquipment, ICarrierNightDamageEquipment,
        IShellingAccuracyEquipment, IAswAccuracyEquipment, INightAccuracyEquipment, IEvasionEquipment,
        ITorpedoEvasionEquipment, IAntiInstallationEquipment
    {

        public EquipID EquipID { get; set; }
        [Obsolete("use EquipID")]
        public int ID { get; }
        public string Name { get; set; }


        public int BaseFirepower { get; set; }

        public int BaseTorpedo { get; set; }

        public int BaseAA { get; set; }

        public int BaseArmor { get; set; }

        public int BaseASW { get; set; }

        public int BaseEvasion { get; set; }

        public int BaseLoS { get; set; }

        public int BaseAccuracy { get; set; }

        public int BaseBombing { get; set; }


        public int Level { get; set; }

        public int Proficiency { get; set; }


        public FitBonusCustom CurrentFitBonus { get; set; } = new FitBonusCustom();







        public int Range { get; set; }
        private int BaseNightPower => BaseFirepower + BaseTorpedo;

        public double Firepower => BaseFirepower + UpgradeFirepower;
        public double Torpedo => BaseTorpedo + UpgradeTorpedo;
        public double Bombing => BaseBombing;
        public double Accuracy => BaseAccuracy + UpgradeAccuracy;
        public double AswAccuracy => BaseAccuracy + BaseAswAccuracy + UpgradeAswAccuracy;
        public double NightPower => BaseNightPower + UpgradeNightPower;
        public double ASW => BaseASW + UpgradeASW;
        public double Evasion => BaseEvasion; // + UpgradeEvasion;
        public double TorpedoEvasion => UpgradeTorpedoEvasion;


        public FitCategories FitCategory { get; private set; }

        public EquipmentDataCustom() { }

        public EquipmentDataCustom(IEquipmentData equip) : this(equip?.MasterEquipment)
        {
            Level = equip?.Level ?? default;
            Proficiency = equip?.AircraftLevel ?? default;
        }

        public EquipmentDataCustom(IEquipmentDataMaster equip)
        {
            Level = default;
            ID = equip?.EquipmentID ?? default;
            EquipID = (EquipID) ID;
            Name = equip?.Name ?? "";
            Proficiency = default;

            BaseFirepower = equip?.Firepower ?? default;
            BaseTorpedo = equip?.Torpedo ?? default;
            BaseBombing = equip?.Bomber ?? default;
            BaseAccuracy = equip?.Accuracy ?? default;
            BaseEvasion = equip?.Evasion ?? default;
            BaseASW = equip?.ASW ?? default;
            BaseLoS = equip?.LOS ?? default;
            BaseArmor = equip?.Armor ?? default;
            BaseAA = equip?.AA ?? default;
            Range = equip?.Range ?? default;

            CategoryType = equip.CategoryType;

            FitCategory = GetFitCategory(this);
        }


        private double UpgradeFirepower => CategoryType switch
        {
            EquipmentTypes.MainGunSmall => SqrtUpgrade(),
            EquipmentTypes.MainGunMedium => SqrtUpgrade(),
            EquipmentTypes.APShell => SqrtUpgrade(),
            EquipmentTypes.AADirector => SqrtUpgrade(),
            EquipmentTypes.Searchlight => SqrtUpgrade(),
            EquipmentTypes.SearchlightLarge => SqrtUpgrade(),
            EquipmentTypes.AAGun => SqrtUpgrade(),
            EquipmentTypes.LandingCraft => SqrtUpgrade(),
            EquipmentTypes.SpecialAmphibiousTank => SqrtUpgrade(),

            EquipmentTypes.MainGunLarge => SqrtUpgrade(1.5),
            EquipmentTypes.MainGunLarge2 => SqrtUpgrade(1.5),

            EquipmentTypes.SecondaryGun
            when ID == 10 || // 12.7cm連装高角砲
                 ID == 66 || // 8cm高角砲
                 ID == 220 || // 8cm高角砲改+増設機銃
                 ID == 275 // 10cm連装高角砲改+増設機銃
            => LinearUpgrade(0.2),

            EquipmentTypes.SecondaryGun
            when ID == 12 || // 15.5cm三連装副砲
                 ID == 234 // 15.5cm三連装副砲改
            => LinearUpgrade(0.3),

            EquipmentTypes.SecondaryGun => SqrtUpgrade(),

            EquipmentTypes.Sonar => SqrtUpgrade(0.75),
            EquipmentTypes.SonarLarge => SqrtUpgrade(0.75),

            EquipmentTypes.DepthCharge when IsDepthChargeProjector => SqrtUpgrade(0.75),

            _ => 0
        };

        private double UpgradeTorpedo => CategoryType switch
        {
            EquipmentTypes.Torpedo => LinearUpgrade(1.2),
            EquipmentTypes.AAGun => LinearUpgrade(1.2),
            EquipmentTypes.SubmarineTorpedo => LinearUpgrade(1.2),

            _ => 0
        };

        private double UpgradeASW => CategoryType switch
        {
            EquipmentTypes.DepthCharge => SqrtUpgrade(2 / 3.0),
            EquipmentTypes.Sonar => SqrtUpgrade(2 / 3.0),

            _ => 0
        };

        public double UpgradeNightPower => CategoryType switch
        {
            EquipmentTypes.MainGunSmall => SqrtUpgrade(),
            EquipmentTypes.MainGunMedium => SqrtUpgrade(),
            EquipmentTypes.MainGunLarge => SqrtUpgrade(),
            EquipmentTypes.MainGunLarge2 => SqrtUpgrade(),
            EquipmentTypes.APShell => SqrtUpgrade(),
            EquipmentTypes.AADirector => SqrtUpgrade(),
            EquipmentTypes.Searchlight => SqrtUpgrade(),
            EquipmentTypes.SearchlightLarge => SqrtUpgrade(),
            EquipmentTypes.Torpedo => SqrtUpgrade(),
            EquipmentTypes.SubmarineTorpedo => SqrtUpgrade(),
            EquipmentTypes.LandingCraft => SqrtUpgrade(),
            EquipmentTypes.SpecialAmphibiousTank => SqrtUpgrade(),

            EquipmentTypes.SecondaryGun
            when ID == 10 || // 12.7cm連装高角砲
                 ID == 66 || // 8cm高角砲
                 ID == 220 || // 8cm高角砲改+増設機銃
                 ID == 275 // 10cm連装高角砲改+増設機銃
            => LinearUpgrade(0.2),

            EquipmentTypes.SecondaryGun
            when ID == 12 || // 15.5cm三連装副砲
                 ID == 234 // 15.5cm三連装副砲改
            => LinearUpgrade(0.3),

            EquipmentTypes.SecondaryGun => SqrtUpgrade(),

            EquipmentTypes.CarrierBasedFighter => SqrtUpgrade(),
            EquipmentTypes.CarrierBasedTorpedo => SqrtUpgrade(),
            EquipmentTypes.CarrierBasedBomber => SqrtUpgrade(),

            _ => 0
        };

        private double UpgradeAccuracy => CategoryType switch
        {
            EquipmentTypes.RadarSmall when IsSurfaceRadar => SqrtUpgrade(1.7),
            EquipmentTypes.RadarLarge when IsSurfaceRadar => SqrtUpgrade(1.7),
            EquipmentTypes.RadarLarge2 when IsSurfaceRadar => SqrtUpgrade(1.7),

            EquipmentTypes.RadarSmall => SqrtUpgrade(),
            EquipmentTypes.RadarLarge => SqrtUpgrade(),
            EquipmentTypes.RadarLarge2 => SqrtUpgrade(),

            EquipmentTypes.MainGunSmall => SqrtUpgrade(),
            EquipmentTypes.MainGunMedium => SqrtUpgrade(),
            EquipmentTypes.MainGunLarge => SqrtUpgrade(),
            EquipmentTypes.MainGunLarge2 => SqrtUpgrade(),
            EquipmentTypes.SecondaryGun => SqrtUpgrade(),
            EquipmentTypes.APShell => SqrtUpgrade(),
            EquipmentTypes.AADirector => SqrtUpgrade(),
            EquipmentTypes.Searchlight => SqrtUpgrade(),
            EquipmentTypes.SearchlightLarge => SqrtUpgrade(),

            _ => 0
        };

        private double UpgradeAswAccuracy => CategoryType switch
        {
            _ when IsSonar => SqrtUpgrade(1.3),

            _ => 0
        };

        private int BaseAswAccuracy => IsSonar ? BaseASW * 2 : 0;

        public double UpgradeTorpedoEvasion => CategoryType switch
        {
            _ when IsSonar => SqrtUpgrade(1.5),

            _ => 0
        };



        private double LinearUpgrade(double constant) => constant * Level;
        private double SqrtUpgrade(double constant = 1) => constant * Math.Sqrt(Level);

        private FitCategories GetFitCategory(EquipmentDataCustom equip) => equip.ID switch
        {
            231 => FitCategories.smallBBGun, // 30.5
            232 => FitCategories.smallBBGun, // kai

            7 => FitCategories.smallBBGun, // 35.6
            103 => FitCategories.smallBBGun, // p
            104 => FitCategories.smallBBGun, // dazzle
            289 => FitCategories.smallBBGun, // dazzle kai
            328 => FitCategories.smallBBGun, // kai
            329 => FitCategories.smallBBGun, // ni

            76 => FitCategories.smallBBGun, // 38 (Bisko)
            114 => FitCategories.smallBBGun, // kai

            190 => FitCategories.smallBBGun, // 38.1 (Warspite)
            192 => FitCategories.smallBBGun, // kai

            133 => FitCategories.pastaBBGun, // 381
            137 => FitCategories.pastaBBGun, // kai

            245 => FitCategories.baguetteBBGun, // 38 quad
            246 => FitCategories.baguetteBBGun, // kai

            161 => FitCategories.burgerBBGun, // burger
            183 => FitCategories.burgerBBGun, // gfcs

            298 => FitCategories.nelsonBBGun, // Nelson
            299 => FitCategories.nelsonBBGun, // afct
            300 => FitCategories.nelsonBBGun, // fcr

            8 => FitCategories.mediumBBGun, // 41
            105 => FitCategories.mediumBBGun, // p
            236 => FitCategories.mediumBBGun, // kai
            318 => FitCategories.mediumBBGun, // ni
            290 => FitCategories.mediumBBGun, // triple

            9 => FitCategories.largeBBGun, // 46
            117 => FitCategories.largeBBGun, // p
            276 => FitCategories.largeBBGun, // kai

            281 => FitCategories.veryLargeBBGun, // 51
            128 => FitCategories.veryLargeBBGun, // p

            _ => FitCategories.Unknown
        };

        public bool IsSurfaceRadar => IsRadar && BaseLoS >= 5;
        public bool IsAirRadar => IsRadar && BaseAA >= 2;

        public bool IsSonar => IsSmallSonar || IsLargeSonar;
        public bool IsSmallSonar => CategoryType == EquipmentTypes.Sonar;
        public bool IsLargeSonar => CategoryType == EquipmentTypes.SonarLarge;

        /// <summary> 爆雷かどうか(投射機は含まない) </summary>
        public bool IsDepthCharge => EquipID == EquipID.DepthCharge_Type95DepthCharge || 
                                     EquipID == EquipID.DepthCharge_Type2DepthCharge;

        /// <summary> 爆雷投射機かどうか(爆雷は含まない) </summary>
        public bool IsDepthChargeProjector => CategoryType == EquipmentTypes.DepthCharge && !IsDepthCharge;

        public EquipmentTypes CategoryType { get; }

        public bool CountsForAswDamage => CategoryType switch
        {
            EquipmentTypes.CarrierBasedBomber => true,
            EquipmentTypes.CarrierBasedTorpedo => true,
            EquipmentTypes.SeaplaneBomber => true,
            EquipmentTypes.Sonar => true,
            EquipmentTypes.DepthCharge => true,
            EquipmentTypes.Autogyro => true,
            EquipmentTypes.ASPatrol => true,
            EquipmentTypes.SonarLarge => true,

            _ => false
        };

        public bool IsMainGun =>
            CategoryType == EquipmentTypes.MainGunSmall ||
            CategoryType == EquipmentTypes.MainGunMedium ||
            CategoryType == EquipmentTypes.MainGunLarge ||
            CategoryType == EquipmentTypes.MainGunLarge2;

        public bool IsSecondaryGun => CategoryType == EquipmentTypes.SecondaryGun;

        public bool IsGun => IsMainGun || IsSecondaryGun;

        public bool IsRadar =>
            CategoryType == EquipmentTypes.RadarSmall ||
            CategoryType == EquipmentTypes.RadarLarge ||
            CategoryType == EquipmentTypes.RadarLarge2;

        public bool IsApShell => CategoryType == EquipmentTypes.APShell;

        public bool IsTorpedo =>
            CategoryType == EquipmentTypes.Torpedo || CategoryType == EquipmentTypes.SubmarineTorpedo;

        public bool IsAntiSubmarineAircraft => BaseASW > 0 && (CategoryType switch
        {
            EquipmentTypes.CarrierBasedBomber => true,
            EquipmentTypes.CarrierBasedTorpedo => true,
            EquipmentTypes.SeaplaneBomber => true,
            EquipmentTypes.Autogyro => true,
            EquipmentTypes.ASPatrol => true,
            EquipmentTypes.FlyingBoat => true,
            EquipmentTypes.LandBasedAttacker => true,
            EquipmentTypes.JetBomber => true,
            EquipmentTypes.JetTorpedo => true,

            _ => false
        });

        public bool IsSpecialDepthChargeProjector =>
            ID == 287 || // 爆雷 三式爆雷投射機 集中配備
            ID == 288 || // 爆雷 試製15cm9連装対潜噴進砲
            ID == 346 || // 二式12cm迫撃砲改
            ID == 347; // 二式12cm迫撃砲改 集中配備

        public bool IsZuiun =>
            ID == 26 || // zuiun
            ID == 79 || // 634
            ID == 80 || // 12
            ID == 81 || // 12 634
            ID == 207 || // 631
            ID == 237 || // 634 skilled
            ID == 322 || // k2 634
            ID == 323; // k2 634 skilled

        public bool IsSwordfish =>
            ID == 242 || // Swordfish
            ID == 243 || // Swordfish Mk.II(熟練)
            ID == 244; // Swordfish Mk.III(熟練)

        public bool IsNightAviationPersonnel =>
            ID == 258 || // 夜間作戦航空要員
            ID == 259; // 夜間作戦航空要員+熟練甲板員

        public bool IsNightAircraft => IsNightFighter ||
                                       IsNightBomber ||
                                       IsNightAttacker;

        public bool IsNightCapableAircraft => IsNightCapableBomber ||
                                              IsNightCapableAttacker;

        public bool IsNightCapableBomber => ID == 154 || // 零戦62型(爆戦/岩井隊)
                                            ID == 320; // 彗星一二型(三一号光電管爆弾搭載機)

        public bool IsNightCapableAttacker => IsSwordfish;

        public bool IsNightFighter => ID == 254 || // F6F-3N
                                      ID == 255 || // F6F-5N
                                      ID == 338 || // 烈風改二戊型
                                      ID == 339; // 烈風改二戊型(一航戦/熟練)

        public bool IsNightBomber => false;

        public bool IsNightAttacker => ID == 257 || // TBM-3D
                                       ID == 344 || // 九七式艦攻改 試製三号戊型(空六号電探改装備機)
                                       ID == 345; // 九七式艦攻改(熟練) 試製三号戊型(空六号電探改装備機)

        /// <summary>
        /// WG42 (Wurfgerät 42)
        /// </summary>
        public bool IsWG => ID == 126;

        /// <summary>
        /// 艦載型 四式20cm対地噴進砲 <br />
        /// 四式20cm対地噴進砲 集中配備
        /// </summary>
        public bool IsAntiInstallationRocket => ID == 348 ||
                                                ID == 349;

        /// <summary>
        /// 二式12cm迫撃砲改 <br />
        /// 二式12cm迫撃砲改 集中配備
        /// </summary>
        public bool IsMortar => ID == 346 ||
                                ID == 347;

        /// <summary>
        /// 大発動艇
        /// </summary>
        public bool IsDaihatsu => ID == 68;

        /// <summary>
        /// 大発動艇(八九式中戦車&amp;陸戦隊)
        /// </summary>
        public bool IsDaihatsuTank => ID == 166;

        /// <summary>
        /// 特大発動艇
        /// </summary>
        public bool IsTokuDaihatsu => ID == 193;

        /// <summary>
        /// 特大発動艇+戦車第11連隊
        /// </summary>
        public bool IsTokuDaihatsuTank => ID == 230;

        /// <summary>
        /// M4A1 DD
        /// </summary>
        public bool IsAmericanDaihatsuTank => ID == 355;

        public EquipmentDataCustom Clone()
        {
            return (EquipmentDataCustom)MemberwiseClone();
        }
    }
}
