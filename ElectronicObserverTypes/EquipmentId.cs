﻿// ReSharper disable InconsistentNaming
// ReSharper disable IdentifierTypo
namespace ElectronicObserverTypes;

public enum EquipmentId
{
	MainGunSmall_12cmSingleGun = 1,
	MainGunSmall_12_7cmTwinGun = 2,
	MainGunSmall_10cmTwinHighangleGun = 3,
	MainGunMedium_14cmSingleGun = 4,
	MainGunMedium_15_5cmTripleGun = 5,
	MainGunMedium_20_3cmTwinGun = 6,
	MainGunLarge_35_6cmTwinGun = 7,
	MainGunLarge_41cmTwinGun = 8,
	MainGunLarge_46cmTripleGun = 9,
	SecondaryGun_12_7cmTwinHighangleGun = 10,
	SecondaryGun_15_2cmSingleGun = 11,
	SecondaryGun_15_5cmTripleSecondaryGun = 12,
	Torpedo_61cmTripleTorpedo = 13,
	Torpedo_61cmQuadrupleTorpedo = 14,
	Torpedo_61cmQuadruple_OxygenTorpedo = 15,
	CarrierBasedTorpedo_Type97TorpedoBomber = 16,
	CarrierBasedTorpedo_Tenzan = 17,
	CarrierBasedTorpedo_Ryuusei = 18,
	CarrierBasedFighter_Type96Fighter = 19,
	CarrierBasedFighter_ZeroFighterModel21 = 20,
	CarrierBasedFighter_ZeroFighterModel52 = 21,
	CarrierBasedFighter_PrototypeReppuuLateModel = 22,
	CarrierBasedBomber_Type99DiveBomber = 23,
	CarrierBasedBomber_Suisei = 24,
	SeaplaneRecon_Type0ReconSeaplane = 25,
	SeaplaneBomber_Zuiun = 26,
	RadarSmall_Type13AirRADAR = 27,
	RadarSmall_Type22SurfaceRADAR = 28,
	RadarSmall_Type33SurfaceRADAR = 29,
	RadarLarge_Type21AirRADAR = 30,
	RadarLarge_Type32SurfaceRADAR = 31,
	RadarLarge_Type42AirRADAR = 32,
	Engine_ImprovedKanhonTurbine = 33,
	Engine_EnhancedKanhonBoiler = 34,
	AAShell_Type3Shell = 35,
	APShell_Type91APShell = 36,
	AAGun_7_7mmMachineGun = 37,
	AAGun_12_7mmSingleMachineGun = 38,
	AAGun_25mmTwinAutocannon = 39,
	AAGun_25mmTripleAutocannon = 40,
	MidgetSubmarine_TypeAKouhyouteki = 41,
	DamageControl_EmergencyRepairPersonnel = 42,
	DamageControl_EmergencyRepairGoddess = 43,
	DepthCharge_Type94DepthChargeProjector = 44,
	DepthCharge_Type3DepthChargeProjector = 45,
	Sonar_Type93PassiveSONAR = 46,
	Sonar_Type3ActiveSONAR = 47,
	MainGunSmall_12cmSingleHighangleGun = 48,
	AAGun_25mmSingleAutocannon = 49,
	MainGunMedium_20_3cm_No_3TwinGun = 50,
	AAGun_12cm30tubeRocketLauncher = 51,
	CarrierBasedTorpedo_RyuuseiKai = 52,
	CarrierBasedFighter_ReppuuModel11 = 53,
	CarrierBasedRecon_Saiun = 54,
	CarrierBasedFighter_ShidenKai2 = 55,
	CarrierBasedFighter_ShindenKai = 56,
	CarrierBasedBomber_SuiseiModel12A = 57,
	Torpedo_61cmQuintuple_OxygenTorpedo = 58,
	SeaplaneRecon_Type0ObservationSeaplane = 59,
	CarrierBasedBomber_ZeroFighterbomberModel62 = 60,
	CarrierBasedRecon_Type2ReconAircraft = 61,
	SeaplaneBomber_PrototypeSeiran = 62,
	MainGunSmall_12_7cmTwinGunModelBKai2 = 63,
	CarrierBasedBomber_Ju87CKai = 64,
	MainGunMedium_15_2cmTwinGun = 65,
	SecondaryGun_8cmHighangleGun = 66,
	Torpedo_53cmBow_OxygenTorpedo = 67,
	LandingCraft_DaihatsuLC = 68,
	Autogyro_KaTypeObservationAutogyro = 69,
	ASPatrol_Type3CommandLiaisonAircraft_ASW = 70,
	SecondaryGun_10cmTwinHighangleGun_Carriage = 71,
	ExtraArmorMedium_AntitorpedoBulge_M = 72,
	ExtraArmorLarge_AntitorpedoBulge_L = 73,
	Searchlight_Searchlight = 74,
	TransportContainer_Drum_Transport = 75,
	MainGunLarge_38cmTwinGun = 76,
	SecondaryGun_15cmTwinSecondaryGun = 77,
	MainGunSmall_12_7cmSingleGun = 78,
	SeaplaneBomber_Zuiun_634AirGroup = 79,
	SeaplaneBomber_ZuiunModel12 = 80,
	SeaplaneBomber_ZuiunModel12_634AirGroup = 81,
	CarrierBasedTorpedo_Type97TorpedoBomber_931AirGroup = 82,
	CarrierBasedTorpedo_Tenzan_931AirGroup = 83,
	AAGun_2cmFlakvierling38 = 84,
	AAGun_3_7cmFlaKM42 = 85,
	RepairFacility_ShipRepairFacility = 86,
	Engine_NewHighPressureTemperatureSteamBoiler = 87,
	RadarSmall_Type22SurfaceRADARKai4 = 88,
	RadarLarge_Type21AirRADARKai = 89,
	MainGunMedium_20_3cm_No_2TwinGun = 90,
	MainGunSmall_12_7cmTwinHighangle_LateModel = 91,
	AAGun_BiType40mmTwinAutocannon = 92,
	CarrierBasedTorpedo_Type97TorpedoBomber_TomonagaSquadron = 93,
	CarrierBasedTorpedo_TenzanModel12_TomonagaSquadron = 94,
	SubmarineTorpedo_Submarine53cmBowTorpedo_8tubes = 95,
	CarrierBasedFighter_ZeroFighterModel21_Skilled = 96,
	CarrierBasedBomber_Type99DiveBomber_Skilled = 97,
	CarrierBasedTorpedo_Type97TorpedoBomber_Skilled = 98,
	CarrierBasedBomber_Type99DiveBomber_EgusaSquadron = 99,
	CarrierBasedBomber_Suisei_EgusaSquadron = 100,
	StarShell_StarShell = 101,
	SeaplaneRecon_Type98ReconSeaplane_NightRecon = 102,
	MainGunLarge_Prototype35_6cmTripleGun = 103,
	MainGunLarge_35_6cmTwinGun_DazzleCamouflage = 104,
	MainGunLarge_Prototype41cmTripleGun = 105,
	RadarSmall_Type13AirRADARKai = 106,
	CommandFacility_FleetCommandFacility = 107,
	AviationPersonnel_SkilledCarrierbasedAircraftMaintenancePersonnel = 108,
	CarrierBasedFighter_ZeroFighterModel52C_601AirGroup = 109,
	CarrierBasedFighter_Reppuu_601AirGroup = 110,
	CarrierBasedBomber_Suisei_601AirGroup = 111,
	CarrierBasedTorpedo_Tenzan_601AirGroup = 112,
	CarrierBasedTorpedo_Ryuusei_601AirGroup = 113,
	MainGunLarge_38cmTwinGunKai = 114,
	SeaplaneRecon_Ar196Kai = 115,
	APShell_Type1APShell = 116,
	MainGunLarge_Prototype46cmTwinGun = 117,
	SeaplaneRecon_Shiun = 118,
	MainGunMedium_14cmTwinGun = 119,
	AADirector_Type91AAFD = 120,
	AADirector_Type94AAFD = 121,
	MainGunSmall_10cmTwinHighangleGun_AAFD = 122,
	MainGunMedium_SKC3420_3cmTwinGun = 123,
	RadarLarge_FuMO25RADAR = 124,
	Torpedo_61cmTriple_OxygenTorpedo = 125,
	Rocket_WG42_Wurfgerät42 = 126,
	SubmarineTorpedo_PrototypeFaTType95OxygenTorpedoKai = 127,
	MainGunLarge_Prototype51cmTwinGun = 128,
	SurfaceShipPersonnel_SkilledLookouts = 129,
	SecondaryGun_12_7cmHighangleGun_AAFD = 130,
	AAGun_25mmTripleAutocannon_CD = 131,
	SonarLarge_Type0PassiveSONAR = 132,
	MainGunLarge_381mm50TripleGun = 133,
	SecondaryGun_OTO152mmTripleRapidfireGun = 134,
	SecondaryGun_90mmSingleHighangleGun = 135,
	ExtraArmorLarge_PuglieseUnderwaterProtectionBulkhead = 136,
	MainGunLarge_381mm50TripleGunKai = 137,
	FlyingBoat_Type2LargeFlyingBoat = 138,
	MainGunMedium_15_2cmTwinGunKai = 139,
	SearchlightLarge_Type96150cmSearchlight = 140,
	RadarLarge_Type32SurfaceRADARKai = 141,
	RadarLarge_15mDuplexRangefinder_Type21AirRADARKai2 = 142,
	CarrierBasedTorpedo_Type97TorpedoBomber_MurataSquadron = 143,
	CarrierBasedTorpedo_TenzanModel12_MurataSquadron = 144,
	Ration_CombatRation = 145,
	Supplies_UnderwayReplenishment = 146,
	MainGunSmall_120mm50TwinGunMount = 147,
	CarrierBasedBomber_PrototypeNanzan = 148,
	Sonar_Type4PassiveSONAR = 149,
	Ration_CannedMackerel = 150,
	CarrierBasedRecon_PrototypeKeiun = 151,
	CarrierBasedFighter_ZeroFighterModel52_Skilled = 152,
	CarrierBasedFighter_ZeroFighterModel52C_wIwaiFlight = 153,
	CarrierBasedBomber_ZeroFighterbomberModel62_IwaiSquadron = 154,
	CarrierBasedFighter_ZeroFighterModel21_wIwamotoFlight = 155,
	CarrierBasedFighter_ZeroFighterModel52A_wIwamotoFlight = 156,
	CarrierBasedFighter_ZeroFighterModel53_IwamotoSquadron = 157,
	CarrierBasedFighter_Bf109TKai = 158,
	CarrierBasedFighter_Fw190TKai = 159,
	SecondaryGun_10_5cmTwinGun = 160,
	MainGunLarge_16inchTripleGunMk_7 = 161,
	MainGunMedium_203mm53TwinGun = 162,
	SeaplaneRecon_Ro_43ReconSeaplane = 163,
	SeaplaneFighter_Ro_44SeaplaneFighter = 164,
	SeaplaneFighter_Type2SeaplaneFighterKai = 165,
	LandingCraft_DaihatsuLC_Type89Tank_LandingForce = 166,
	SpecialAmphibiousTank_SpecialType2AmphibiousTank = 167,
	LandBasedAttacker_Type96AttackBomber = 168,
	LandBasedAttacker_Type1AttackBomber = 169,
	LandBasedAttacker_Type1AttackBomber_NonakaSquadron = 170,
	SeaplaneRecon_OS2U = 171,
	SecondaryGun_5inchTwinGunMk_28mod_2 = 172,
	AAGun_Bofors40mmQuadrupleAutocannon = 173,
	Torpedo_53cmTwinTorpedo = 174,
	Interceptor_Raiden = 175,
	Interceptor_Type3FighterHien = 176,
	Interceptor_Type3FighterHien_244thAirCombatGroup = 177,
	FlyingBoat_PBY5ACatalina = 178,
	Torpedo_Prototype61cmSextuple_OxygenTorpedo = 179,
	LandBasedAttacker_Type1AttackBomberModel22A = 180,
	CarrierBasedFighter_ZeroFighterModel32 = 181,
	CarrierBasedFighter_ZeroFighterModel32_Skilled = 182,
	MainGunLarge_16inchTripleGunMk_7_GFCS = 183,
	CarrierBasedFighter_Re_2001ORKai = 184,
	Interceptor_Type3FighterHienModel1D = 185,
	LandBasedAttacker_Type1AttackBomberModel34 = 186,
	LandBasedAttacker_Ginga = 187,
	CarrierBasedTorpedo_Re_2001GKai = 188,
	CarrierBasedFighter_Re_2005Kai = 189,
	MainGunLarge_38_1cmMk_ITwinGun = 190,
	AAGun_QF2pounderOctuplePompomGun = 191,
	MainGunLarge_38_1cmMk_INTwinGunKai = 192,
	LandingCraft_TokuDaihatsuLC = 193,
	SeaplaneBomber_Laté298B = 194,
	CarrierBasedBomber_SBDDauntless = 195,
	CarrierBasedTorpedo_TBDDevastator = 196,
	CarrierBasedFighter_F4F3Wildcat = 197,
	CarrierBasedFighter_F4F4Wildcat = 198,
	JetBomber_JetKeiunKai = 199,
	JetBomber_KikkaKai = 200,
	Interceptor_ShidenModel11 = 201,
	Interceptor_ShidenModel21ShidenKai = 202,
	ExtraArmorMedium_NewKanhonAntitorpedoBulge_M = 203,
	ExtraArmorLarge_NewKanhonAntitorpedoBulge_L = 204,
	CarrierBasedFighter_F6F3Hellcat = 205,
	CarrierBasedFighter_F6F5Hellcat = 206,
	SeaplaneBomber_Zuiun_631AirGroup = 207,
	SeaplaneBomber_Seiran_631AirGroup = 208,
	TransportMaterial_Saiun_Disassembled = 209,
	SubmarineEquipment_SubmarineRadar_Periscope = 210,
	SubmarineEquipment_SubmarineRadar_E27RadarDetector = 211,
	CarrierBasedRecon_Saiun_EastCarolineAirGroup = 212,
	SubmarineTorpedo_LateModelBowTorpedo_6tubes = 213,
	SubmarineTorpedo_SkilledSonarPersonnel_LateModelBowTorpedo_6tubes = 214,
	SeaplaneFighter_Ro_44SeaplaneFighterbis = 215,
	SeaplaneFighter_Type2SeaplaneFighterKai_Skilled = 216,
	SeaplaneFighter_KyoufuuKai = 217,
	Interceptor_Type4FighterHayate = 218,
	CarrierBasedBomber_ZeroFighterbomberModel63 = 219,
	SecondaryGun_8cmHighangleGunKai_MachineGun = 220,
	Interceptor_Type1FighterHayabusaModelII = 221,
	Interceptor_Type1FighterHayabusaModelIIIA = 222,
	Interceptor_Type1FighterHayabusaModelIIIA_54thSquadron = 223,
	LandBasedAttacker_Type1FighterHayabusaModelIIIKai_65thSquadron = 224,
	Interceptor_Type1FighterHayabusaModelII_64thSquadron = 225,
	DepthCharge_Type95DepthCharge = 226,
	DepthCharge_Type2DepthCharge = 227,
	CarrierBasedFighter_Type96FighterKai = 228,
	MainGunSmall_12_7cmSingleHighangleGun_LateModel = 229,
	LandingCraft_TokuDaihatsuLC_11thTankRegiment = 230,
	MainGunLarge_30_5cmTripleGun = 231,
	MainGunLarge_30_5cmTripleGunKai = 232,
	CarrierBasedBomber_F4U1DCorsair = 233,
	SecondaryGun_15_5cmTripleSecondaryGunKai = 234,
	MainGunMedium_15_5cmTripleGunKai = 235,
	MainGunLarge_41cmTripleGunKai = 236,
	SeaplaneBomber_Zuiun_634AirGroupSkilled = 237,
	SeaplaneRecon_Type0ReconSeaplaneModel11B = 238,
	SeaplaneRecon_Type0ReconSeaplaneModel11B_Skilled = 239,
	RadarSmall_Type22SurfaceRadarKai4_CalibratedLateModel = 240,
	Ration_CombatRation_SpecialOnigiri = 241,
	CarrierBasedTorpedo_Swordfish = 242,
	CarrierBasedTorpedo_SwordfishMk_II_Skilled = 243,
	CarrierBasedTorpedo_SwordfishMk_III_Skilled = 244,
	MainGunLarge_38cmQuadrupleGun = 245,
	MainGunLarge_38cmQuadrupleGunKai = 246,
	SecondaryGun_15_2cmTripleGun = 247,
	CarrierBasedBomber_Skua = 248,
	CarrierBasedFighter_Fulmar = 249,
	Interceptor_SpitfireMk_I = 250,
	Interceptor_SpitfireMk_V = 251,
	CarrierBasedFighter_SeafireMk_IIIKai = 252,
	Interceptor_SpitfireMk_IX_Skilled = 253,
	CarrierBasedFighter_F6F3NHellcat = 254,
	CarrierBasedFighter_F6F5NHellcat = 255,
	CarrierBasedTorpedo_TBFAvenger = 256,
	CarrierBasedTorpedo_TBM3DAvenger = 257,
	AviationPersonnel_NightOperationAviationPersonnel = 258,
	AviationPersonnel_NightOperationAviationPersonnel_SkilledCrew = 259,
	Sonar_Type124ASDIC = 260,
	Sonar_Type144147ASDIC = 261,
	Sonar_HFDF_Type144147ASDIC = 262,
	Interceptor_ShidenKai_343rdAirGroup301stFighterSquadron = 263,
	MainGunSmall_12_7cmTwinGunModelCKai2 = 266,
	MainGunSmall_12_7cmTwinGunModelDKai2 = 267,
	ExtraArmorMedium_ArcticCamouflage__ArcticEquipment = 268,
	LandBasedAttacker_PrototypeToukai = 269,
	LandBasedAttacker_Toukai_901AirGroup = 270,
	CarrierBasedFighter_ShidenKai4 = 271,
	CommandFacility_StrikingForceFCF = 272,
	CarrierBasedRecon_Saiun_4thRecon = 273,
	AAGun_12cm30tubeRocketLauncherKai2 = 274,
	SecondaryGun_10cmTwinHighangleGunKai_AdditionalMachineGuns = 275,
	MainGunLarge_46cmTripleGunKai = 276,
	CarrierBasedBomber_FM2Wildcat = 277,
	RadarLarge_SKRadar = 278,
	RadarLarge_SK_SGRadar = 279,
	MainGunSmall_QF4_7inchGunMk_XIIKai = 280,
	MainGunLarge_51cmTwinGun = 281,
	MainGunSmall_130mmB13TwinGun = 282,
	Torpedo_533mmTripleTorpedo = 283,
	MainGunSmall_5inchSingleGunMk_30 = 284,
	Torpedo_61cmTriple_OxygenTorpedoMountLateModel = 285,
	Torpedo_61cmQuadruple_OxygenTorpedoMountLateModel = 286,
	DepthCharge_Type3DepthChargeProjector_CD = 287,
	DepthCharge_Prototype15cm9tubeASWRocketLauncher = 288,
	MainGunLarge_35_6cmTripleGunKai_DazzleCamouflage = 289,
	MainGunLarge_41cmTripleGunKai2 = 290,
	CarrierBasedBomber_SuiseiModel22_634AirGroup = 291,
	CarrierBasedBomber_SuiseiModel22_634AirGroupSkilled = 292,
	MainGunSmall_12cmSingleGunKai2 = 293,
	MainGunSmall_12_7cmTwinGunModelAKai2 = 294,
	MainGunSmall_12_7cmTwinGunModelAKai3_WartimeModification_AAFD = 295,
	MainGunSmall_12_7cmTwinGunModelBKai4_WartimeModification_AAFD = 296,
	MainGunSmall_12_7cmTwinGunModelA = 297,
	MainGunLarge_16inchMk_ITripleGun = 298,
	MainGunLarge_16inchMk_ITripleGun_AFCTKai = 299,
	MainGunLarge_16inchMk_ITripleGunKai_FCRType284 = 300,
	AAGun_20tube7inchUPRocketLaunchers = 301,
	CarrierBasedTorpedo_Type97TorpedoBomber_931AirGroupSkilled = 302,
	MainGunMedium_Bofors15_2cmTwinGunModel1930 = 303,
	SeaplaneRecon_S9Osprey = 304,
	CarrierBasedBomber_Ju87CKai2_KMX = 305,
	CarrierBasedBomber_Ju87CKai2_KMXSkilled = 306,
	RadarSmall_GFCSMk_37 = 307,
	MainGunSmall_5inchSingleGunMk_30Kai_GFCSMk_37 = 308,
	MidgetSubmarine_TypeCKouhyouteki = 309,
	MainGunMedium_14cmTwinGunKai = 310,
	LandBasedRecon_Type2LandbasedReconAircraft = 311,
	LandBasedRecon_Type2LandbasedReconAircraft_Skilled = 312,
	MainGunSmall_5inchSingleGunMk_30Kai = 313,
	Torpedo_533mmQuintupleTorpedo_InitialModel = 314,
	RadarSmall_SGRadar_InitialModel = 315,
	CarrierBasedBomber_Re_2001CBKai = 316,
	AAShell_Type3ShellKai = 317,
	MainGunLarge_41cmTwinGunMountKaiNi = 318,
	CarrierBasedBomber_SuiseiModel12_634AirGroupwType3ClusterBombs = 319,
	CarrierBasedBomber_SuiseiModel12_wType31PhotoelectricFuzeBombs = 320,
	SeaplaneBomber_ZuiunKaiNi_634AirGroup = 322,
	SeaplaneBomber_ZuiunKaiNi_634AirGroupSkilled = 323,
	Autogyro_OTypeObservationAutogyroKai = 324,
	Autogyro_OTypeObservationAutogyroKaiNi = 325,
	Autogyro_S51J = 326,
	Autogyro_S51JKai = 327,
	MainGunLarge_35_6cmTwinGunMountKai = 328,
	MainGunLarge_35_6cmTwinGunMountKaiNi = 329,
	MainGunLarge_16inchMk_ITwinGunMount = 330,
	MainGunLarge_16inchMk_VTwinGunMount = 331,
	MainGunLarge_16inchMk_VIIITwinGunMountKai = 332,
	Interceptor_ReppuuKai = 333,
	Interceptor_ReppuuKai_352AirGroupSkilled = 334,
	CarrierBasedFighter_ReppuuKai_PrototypeCarrierbasedModel = 335,
	CarrierBasedFighter_ReppuuKaiNi = 336,
	CarrierBasedFighter_ReppuuKaiNi_CarDiv1Skilled = 337,
	CarrierBasedFighter_ReppuuKaiNiModelE = 338,
	CarrierBasedFighter_ReppuuKaiNiModelE_CarDiv1Skilled = 339,
	MainGunMedium_152mm55TripleRapidFireGunMount = 340,
	MainGunMedium_152mm55TripleRapidFireGunMountKai = 341,
	CarrierBasedTorpedo_RyuuseiKai_CarDiv1 = 342,
	CarrierBasedTorpedo_RyuuseiKai_CarDiv1Skilled = 343,
	CarrierBasedTorpedo_PrototypeType97TorpedoBomberKaiNo_3ModelE_wType6AirborneRadarKai = 344,
	CarrierBasedTorpedo_PrototypeType97TorpedoBomberKai_SkilledNo_3ModelE_wType6AirborneRadarKai = 345,
	DepthCharge_Type212cmMortarKai = 346,
	DepthCharge_Type212cmMortarKai_ConcentratedDeployment = 347,
	Rocket_ShipborneModelType420cmAntigroundRocketLauncher = 348,
	Rocket_Type420cmAntigroundRocketLauncher_ConcentratedDeployment = 349,
	Interceptor_Me163B = 350,
	Interceptor_PrototypeShuusui = 351,
	Interceptor_Shuusui = 352,
	CarrierBasedFighter_Fw190A5Kai_Skilled = 353,
	Interceptor_Fw190D9 = 354,
	LandingCraft_M4A1DD = 355,
	MainGunMedium_8inchTripleGunMountMk_9 = 356,
	MainGunMedium_8inchTripleGunMountMk_9mod_2 = 357,
	SecondaryGun_5inchSingleHighangleGunMountBattery = 358,
	MainGunMedium_6inchTwinRapidFireGunMountMk_XXI = 359,
	MainGunMedium_Bofors15cmTwinRapidFireGunMountMk_9Model1938 = 360,
	MainGunMedium_Bofors15cmTwinRapidFireGunMountMk_9Kai_SingleRapidFireGunMountMk_10KaiModel1938 = 361,
	MainGunMedium_5inchTwinDualpurposeGunMount_ConcentratedDeployment = 362,
	MainGunMedium_GFCSMk_37_5inchTwinDualpurposeGunMount_ConcentratedDeployment = 363,
	MidgetSubmarine_TypeDKouhyoutekiKai_KouryuuKai = 364,
	APShell_Type1ArmorPiercingShellKai = 365,
	MainGunSmall_12_7cmTwinGunModelDKai3 = 366,
	SeaplaneBomber_Swordfish_SeaplaneModel = 367,
	SeaplaneBomber_SwordfishMk_IIIKai_SeaplaneModel = 368,
	SeaplaneBomber_SwordfishMk_IIIKai_SeaplaneModelSkilled = 369,
	SeaplaneRecon_SwordfishMk_IIKai_ReconnaissanceSeaplaneModel = 370,
	SeaplaneRecon_FaireySeafoxKai = 371,
	CarrierBasedTorpedo_TenzanModel12A = 372,
	CarrierBasedTorpedo_TenzanModel12AKai_wType6AirborneRadarKai = 373,
	CarrierBasedTorpedo_TenzanModel12AKai_SkilledwType6AirborneRadarKai = 374,
	CarrierBasedFighter_XF5U = 375,
	Torpedo_533mmQuintupleTorpedoMount_LateModel = 376,
	DepthCharge_RUR4AWeaponAlphaKai = 377,
	DepthCharge_LightweightASWTorpedo_InitialTestModel = 378,
	MainGunSmall_12_7cmSingleHighangleGunKaiNi = 379,
	MainGunSmall_12_7cmTwinHighangleGunKaiNi = 380,
	MainGunLarge_16inchTripleGunMk_6 = 381,
	MainGunSmall_12cmSingleHighangleGunModelE = 382,
	SubmarineTorpedo_LateModel53cmBowTorpedo_8tubes = 383,
	SubmarineEquipment_LateModelSubmarineRadar_PassiveRadiolocator = 384,
	MainGunLarge_16inchTripleGunMk_6mod_2 = 385,
	MainGunMedium_6inchTripleRapidFireGunMountMk_16 = 386,
	MainGunMedium_6inchTripleRapidFireGunMountMk_16mod_2 = 387,
	LandBasedAttacker_Ginga_EgusaSquadron = 388,
	CarrierBasedTorpedo_TBM3W_3S = 389,
	MainGunLarge_16inchTripleGunMountMk_6_GFCS = 390,
	CarrierBasedBomber_Type99DiveBomberModel22 = 391,
	CarrierBasedBomber_Type99DiveBomberModel22_Skilled = 392,
	MainGunSmall_120mm50TwinGunMountmod_1936 = 393,
	MainGunSmall_120mm50TwinGunMountKaiA_mod_1937 = 394,
	HeavyBomber_Shinzan = 395,
	HeavyBomber_ShinzanKai = 396,
	MainGunSmall_LocallyModified12_7cmTwinHighangleGunMount = 397,
	MainGunSmall_LocallyModified10cmTwinHighangleGunMount = 398,
	MainGunMedium_6inchMk_XXIIITripleGunMount = 399,
	Torpedo_533mmTripleTorpedoMount_Model5339 = 400,
	LandBasedAttacker_Do17Z2 = 401,
	AviationPersonnel_ArcticGear_DeckPersonnel = 402,
	LandBasedAttacker_Type4HeavyBomberHiryuu = 403,
	LandBasedAttacker_Type4HeavyBomberHiryuu_Skilled = 404,
	LandBasedAttacker_Do217E5_Hs293InitialModel = 405,
	LandBasedAttacker_Do217K2_FritzX = 406,
	MainGunMedium_15_2cmTwinGunMountKaiNi = 407,
	LandingCraft_Soukoutei_ABClass = 408,
	LandingCraft_ArmedDaihatsu = 409,
	RadarLarge_Type21AirRadarKaiNi = 410,
	RadarLarge_Type42AirRadarKaiNi = 411,
	SurfaceShipPersonnel_TorpedoSquadronSkilledLookouts = 412,
	CommandFacility_EliteTorpedoSquadronCommandFacility = 413,
	SeaplaneRecon_SOCSeagull = 414,
	SeaplaneRecon_SO3CSeamewKai = 415,
	Interceptor_Type0FighterModel21_TainanAirGroup = 416,
	Interceptor_Type0FighterModel32_TainanAirGroup = 417,
	Interceptor_Type0FighterModel22_251AirGroup = 418,
	CarrierBasedBomber_SBD5 = 419,
	CarrierBasedBomber_SB2C3 = 420,
	CarrierBasedBomber_SB2C5 = 421,
	CarrierBasedFighter_FR1Fireball = 422,
	CarrierBasedRecon_Fulmar_ReconnaissanceFighterSkilled = 423,
	CarrierBasedTorpedo_BarracudaMk_II = 424,
	CarrierBasedTorpedo_BarracudaMk_III = 425,
	MainGunLarge_305mm46TwinGunMount = 426,
	MainGunLarge_305mm46TripleGunMount = 427,
	MainGunLarge_320mm44TwinGunMount = 428,
	MainGunLarge_320mm44TripleGunMount = 429,
	SecondaryGun_65mm64SingleRapidFireGunMountKai = 430,
	LandBasedAttacker_SM_79 = 431,
	LandBasedAttacker_SM_79bis = 432,
	LandBasedAttacker_SM_79bis_Skilled = 433,
	CarrierBasedFighter_CorsairMk_II = 434,
	CarrierBasedFighter_CorsairMk_II_Ace = 435,
	LandingCraft_DaihatsuLandingCraft_PanzerIINorthAfricanSpecification = 436,
	CarrierBasedFighter_PrototypeJinpuu = 437,
	Sonar_Type3ActiveSonarKai = 438,
	DepthCharge_Hedgehog_InitialModel = 439,
	SubmarineTorpedo_21inch6tubeBowTorpedoLauncher_InitialModel = 440,
	SubmarineTorpedo_21inch6tubeBowTorpedoLauncher_LateModel = 441,
	SubmarineTorpedo_Submarine4tubeSternTorpedoLauncher_InitialModel = 442,
	SubmarineTorpedo_Submarine4tubeSternTorpedoLauncher_LateModel = 443,
	LandBasedAttacker_Type4HeavyBomberHiryuu_No_1Model1AGuidedMissile = 444,
	Interceptor_Type2TwoseatFighterToryuu = 445,
	Interceptor_Type2TwoseatFighterToryuuModelC = 446,
	CarrierBasedBomber_Type0FighterModel64_TwoseatwKMX = 447,
	LandingCraft_TokuDaihatsuLandingCraft_Type1GunTank = 449,
	RadarSmall_Type13AirRadarKai_LateModel = 450,
	ASPatrol_Type3CommandLiaisonAircraftKai = 451,
	Interceptor_Ki96 = 452,
	LandBasedAttacker_Ki102B = 453,
	LandBasedAttacker_Ki102BKai_No_1Model1BGuidedMissile = 454,
	MainGunSmall_PrototypeLongbarrel12_7cmTwinGunMountModelAKai4 = 455,
	RadarSmall_SGRadar_LateModel = 456,
	SubmarineTorpedo_LateModelBowTorpedoMount_4tubes = 457,
	SubmarineEquipment_LateModelRadar_PassiveRadiolocator_SnorkelEquipment = 458,
	LandBasedAttacker_B25 = 459,
}
