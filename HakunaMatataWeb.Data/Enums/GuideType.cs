using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HakunaMatataWeb.Data.Enums
{
    public enum GuideType
    {
        Character = 0,
        [Display(Name = "Public Dungeon")]
        PublicDungeon = 1,
        [Display(Name = "Group Dungeon")]
        GroupDungeon = 2,
        Trails = 3,
        Treasure = 4,        
        Game = 5,
        Quest = 6,
        Finance = 7,
        Achievements = 8,
        Crafting = 9
    }
}
