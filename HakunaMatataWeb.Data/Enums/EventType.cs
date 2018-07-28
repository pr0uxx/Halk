using HakunaMatataWeb.Utilities;
using System.ComponentModel.DataAnnotations;

namespace HakunaMatataWeb.Data.Enums
{
    public enum EventType
    {
        Gambling = 0,
        PvP = 1,
        PvE = 2,
        Social = 3,
        [Display(Name = "Mob Farming")]
        MobFarming = 4,
        [Display(Name ="Resource Farming")]
        ResourceFarming = 5,
        [Display(Name = "Skyshard Farming")]
        SkyshardFarming = 6,
        [Display(Name = "Vet Farming")]
        VetFarming = 7,
        [Display(Name = "Gear Farming")]
        GearFarming = 9
    }
}