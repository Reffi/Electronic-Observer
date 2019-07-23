using ElectronicObserver.Utility.Data;

namespace ElectronicObserver.Data
{
    public interface IEquipmentDataCustom
    {
        string Name { get; }
        int ID { get; }


        int BaseFirepower { get; set; }
        int BaseTorpedo { get; set; }
        int BaseAA { get; set; }
        int BaseArmor { get; set; }
        int BaseASW { get; set; }
        int BaseEvasion { get; set; }
        int BaseLoS { get; set; }
        int BaseAccuracy { get; set; }
        int BaseBombing { get; set; }


        


        int Range { get; set; }
        int Level { get; set; }
        int Proficiency { get; set; }
        double Firepower { get; }
        double Torpedo { get; }
        double Bombing { get; }
        double Accuracy { get; }
        double AswAccuracy { get; }
        double NightPower { get; }
        double ASW { get; }
        EquipmentTypes CategoryType { get; }
        FitCategories FitCategory { get; }
        bool CountsForAswDamage { get; }
        bool IsAntiSubmarineAircraft { get; }
        bool IsDepthCharge { get; }
        bool IsSpecialDepthChargeProjector { get; }
        bool IsZuiun { get; }

        FitBonusCustom CurrentFitBonus { get; set; }
    }
}