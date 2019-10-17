using System.ComponentModel.DataAnnotations;

namespace ElectronicObserver.Data
{
	public enum EquipmentTypes
	{
        Unknown = 0,

        /// <summary>小口径主砲</summary>
        [Display(ResourceType = typeof(Properties.EquipmentTypes), Name = "MainGunSmall")]
        MainGunSmall = 1,

        /// <summary>中口径主砲</summary>
        [Display(ResourceType = typeof(Properties.EquipmentTypes), Name = "MainGunMedium")]
        MainGunMedium = 2,

        /// <summary>大口径主砲</summary>
        [Display(ResourceType = typeof(Properties.EquipmentTypes), Name = "MainGunLarge")]
        MainGunLarge = 3,

        /// <summary>副砲</summary>
        [Display(ResourceType = typeof(Properties.EquipmentTypes), Name = "SecondaryGun")]
        SecondaryGun = 4,

        /// <summary>魚雷</summary>
        [Display(ResourceType = typeof(Properties.EquipmentTypes), Name = "Torpedo")]
        Torpedo = 5,

        /// <summary>艦上戦闘機</summary>
        [Display(ResourceType = typeof(Properties.EquipmentTypes), Name = "CarrierBasedFighter")]
        CarrierBasedFighter = 6,

        /// <summary>艦上爆撃機</summary>
        [Display(ResourceType = typeof(Properties.EquipmentTypes), Name = "CarrierBasedBomber")]
        CarrierBasedBomber = 7,

        /// <summary>艦上攻撃機</summary>
        [Display(ResourceType = typeof(Properties.EquipmentTypes), Name = "CarrierBasedTorpedo")]
        CarrierBasedTorpedo = 8,

        /// <summary>艦上偵察機</summary>
        [Display(ResourceType = typeof(Properties.EquipmentTypes), Name = "CarrierBasedRecon")]
        CarrierBasedRecon = 9,

        /// <summary>水上偵察機</summary>
        [Display(ResourceType = typeof(Properties.EquipmentTypes), Name = "SeaplaneRecon")]
        SeaplaneRecon = 10,

        /// <summary>水上爆撃機</summary>
        [Display(ResourceType = typeof(Properties.EquipmentTypes), Name = "SeaplaneBomber")]
        SeaplaneBomber = 11,

        /// <summary>小型電探</summary>
        [Display(ResourceType = typeof(Properties.EquipmentTypes), Name = "RadarSmall")]
        RadarSmall = 12,

        /// <summary>大型電探</summary>
        [Display(ResourceType = typeof(Properties.EquipmentTypes), Name = "RadarLarge")]
        RadarLarge = 13,

        /// <summary>ソナー</summary>
        [Display(ResourceType = typeof(Properties.EquipmentTypes), Name = "Sonar")]
        Sonar = 14,

        /// <summary>爆雷</summary>
        [Display(ResourceType = typeof(Properties.EquipmentTypes), Name = "DepthCharge")]
        DepthCharge = 15,

        /// <summary>追加装甲</summary>
        [Display(ResourceType = typeof(Properties.EquipmentTypes), Name = "ExtraArmor")]
        ExtraArmor = 16,

        /// <summary>機関部強化</summary>
        [Display(ResourceType = typeof(Properties.EquipmentTypes), Name = "Engine")]
        Engine = 17,

        /// <summary>対空強化弾</summary>
        [Display(ResourceType = typeof(Properties.EquipmentTypes), Name = "AAShell")]
        AAShell = 18,

        /// <summary>対艦強化弾</summary>
        [Display(ResourceType = typeof(Properties.EquipmentTypes), Name = "APShell")]
        APShell = 19,

        /// <summary>VT信管</summary>
        [Display(ResourceType = typeof(Properties.EquipmentTypes), Name = "VTFuse")]
        VTFuse = 20,

        /// <summary>対空機銃</summary>
        [Display(ResourceType = typeof(Properties.EquipmentTypes), Name = "AAGun")]
        AAGun = 21,

        /// <summary>特殊潜航艇</summary>
        [Display(ResourceType = typeof(Properties.EquipmentTypes), Name = "MidgetSubmarine")]
        MidgetSubmarine = 22,

        /// <summary>応急修理要員</summary>
        [Display(ResourceType = typeof(Properties.EquipmentTypes), Name = "DamageControl")]
        DamageControl = 23,

        /// <summary>上陸用舟艇</summary>
        [Display(ResourceType = typeof(Properties.EquipmentTypes), Name = "LandingCraft")]
        LandingCraft = 24,

        /// <summary>オートジャイロ</summary>
        [Display(ResourceType = typeof(Properties.EquipmentTypes), Name = "Autogyro")]
        Autogyro = 25,

        /// <summary>対潜哨戒機</summary>
        [Display(ResourceType = typeof(Properties.EquipmentTypes), Name = "ASPatrol")]
        ASPatrol = 26,

        /// <summary>追加装甲（中型）</summary>
        [Display(ResourceType = typeof(Properties.EquipmentTypes), Name = "ExtraArmorMedium")]
        ExtraArmorMedium = 27,

        /// <summary>追加装甲（大型）</summary>
        [Display(ResourceType = typeof(Properties.EquipmentTypes), Name = "ExtraArmorLarge")]
        ExtraArmorLarge = 28,

        /// <summary>探照灯</summary>
        [Display(ResourceType = typeof(Properties.EquipmentTypes), Name = "Searchlight")]
        Searchlight = 29,

        /// <summary>簡易輸送部材</summary>
        [Display(ResourceType = typeof(Properties.EquipmentTypes), Name = "TransportContainer")]
        TransportContainer = 30,

        /// <summary>艦艇修理施設</summary>
        [Display(ResourceType = typeof(Properties.EquipmentTypes), Name = "RepairFacility")]
        RepairFacility = 31,

        /// <summary>潜水艦魚雷</summary>
        [Display(ResourceType = typeof(Properties.EquipmentTypes), Name = "SubmarineTorpedo")]
        SubmarineTorpedo = 32,

        /// <summary>照明弾</summary>
        [Display(ResourceType = typeof(Properties.EquipmentTypes), Name = "StarShell")]
        StarShell = 33,

        /// <summary>司令部施設</summary>
        [Display(ResourceType = typeof(Properties.EquipmentTypes), Name = "CommandFacility")]
        CommandFacility = 34,

        /// <summary>航空要員</summary>
        [Display(ResourceType = typeof(Properties.EquipmentTypes), Name = "AviationPersonnel")]
        AviationPersonnel = 35,

        /// <summary>高射装置</summary>
        [Display(ResourceType = typeof(Properties.EquipmentTypes), Name = "AADirector")]
        AADirector = 36,

        /// <summary>対地装備</summary>
        [Display(ResourceType = typeof(Properties.EquipmentTypes), Name = "Rocket")]
        Rocket = 37,

        /// <summary>大口径主砲(II)</summary>
        [Display(ResourceType = typeof(Properties.EquipmentTypes), Name = "MainGunLarge2")]
        MainGunLarge2 = 38,

        /// <summary>水上艦要員</summary>
        [Display(ResourceType = typeof(Properties.EquipmentTypes), Name = "SurfaceShipPersonnel")]
        SurfaceShipPersonnel = 39,

        /// <summary>大型ソナー</summary>
        [Display(ResourceType = typeof(Properties.EquipmentTypes), Name = "SonarLarge")]
        SonarLarge = 40,

        /// <summary>大型飛行艇</summary>
        [Display(ResourceType = typeof(Properties.EquipmentTypes), Name = "FlyingBoat")]
        FlyingBoat = 41,

        /// <summary>大型探照灯</summary>
        [Display(ResourceType = typeof(Properties.EquipmentTypes), Name = "SearchlightLarge")]
        SearchlightLarge = 42,

        /// <summary>戦闘糧食</summary>
        [Display(ResourceType = typeof(Properties.EquipmentTypes), Name = "Ration")]
        Ration = 43,

        /// <summary>補給物資</summary>
        [Display(ResourceType = typeof(Properties.EquipmentTypes), Name = "Supplies")]
        Supplies = 44,

        /// <summary>水上戦闘機</summary>
        [Display(ResourceType = typeof(Properties.EquipmentTypes), Name = "SeaplaneFighter")]
        SeaplaneFighter = 45,

        /// <summary>特型内火艇</summary>
        [Display(ResourceType = typeof(Properties.EquipmentTypes), Name = "SpecialAmphibiousTank")]
        SpecialAmphibiousTank = 46,

        /// <summary>陸上攻撃機</summary>
        [Display(ResourceType = typeof(Properties.EquipmentTypes), Name = "LandBasedAttacker")]
        LandBasedAttacker = 47,

        /// <summary>局地戦闘機</summary>
        [Display(ResourceType = typeof(Properties.EquipmentTypes), Name = "Interceptor")]
        Interceptor = 48,

        /// <summary>陸上偵察機</summary>
        [Display(ResourceType = typeof(Properties.EquipmentTypes), Name = "LandBasedRecon")]
        LandBasedRecon = 49,

        /// <summary>輸送機材</summary>
        [Display(ResourceType = typeof(Properties.EquipmentTypes), Name = "TransportMaterial")]
        TransportMaterial = 50,

        /// <summary>潜水艦装備</summary>
        [Display(ResourceType = typeof(Properties.EquipmentTypes), Name = "SubmarineEquipment")]
        SubmarineEquipment = 51,

        /// <summary>噴式戦闘機</summary>
        [Display(ResourceType = typeof(Properties.EquipmentTypes), Name = "JetFighter")]
        JetFighter = 56,

        /// <summary>噴式戦闘爆撃機</summary>
        [Display(ResourceType = typeof(Properties.EquipmentTypes), Name = "JetBomber")]
        JetBomber = 57,

        /// <summary>噴式攻撃機</summary>
        [Display(ResourceType = typeof(Properties.EquipmentTypes), Name = "JetTorpedo")]
        JetTorpedo = 58,

        /// <summary>噴式索敵機</summary>
        [Display(ResourceType = typeof(Properties.EquipmentTypes), Name = "JetRecon")]
        JetRecon = 59,

        /// <summary>大型電探(II)</summary>
        [Display(ResourceType = typeof(Properties.EquipmentTypes), Name = "RadarLarge2")]
        RadarLarge2 = 93,

        /// <summary>艦上偵察機(II)</summary>
        [Display(ResourceType = typeof(Properties.EquipmentTypes), Name = "CarrierBasedRecon2")]
        CarrierBasedRecon2 = 94
    }

    public enum EquipmentTypeGroup
    {
        Fighters,
        Bombers,
        Recons,
        MainGuns,
        SecondaryGuns,
        Torpedoes,
        ASW,
        Radars,
        Transport,
        Food,
        LandPlanes,
        Other
    }
}
