using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataManager
{
    public static List<Island> AllIslands = new List<Island>();
    public static int MaxPicturePerIsland = 5;

    public static string[] AreasSentences = new string[]
{
    "Sentus were organized in caste.",
    "After the disater, all the areas were shatered.",
    "Each areas had a theme and a color to it.",
    "The differences between areas allowed for better understanding of everyone position."
};

    public static string[] OrangeSentences = new string[]
{
    "Merchant and diplomate were respected.",
    "The Orange area was meant to impress visitors.",
    "The area was organized for the pleasure of the eye.",
    "Alliance were made and broken here.",
    "Deals were made and broken here."
};

    public static string[] GreenSentences = new string[]
{
    "Food production was easy with new discoveries.",
    "Farmer were respected but not honored.",
    "The green zone was the land of food.",
    "Farmer had access to great technologies, but only for their work.",
};

    public static string[] GreySentences = new string[]
{
    "Sentus take all the resources from the lands.",
    "All the wonders come from the industrial district.",
    "The makers were highly respected and honored."
};

    public static string[] SnowSentences = new string[]
{
    "Experimentation on animals, plant or humans was tolerated.",
    "One of their weapon cause great disturbance in climate.",
    "Create and control tempest was a great discovery.",
    "One of the Sentus superiority was that their were able to construct for utility and fashion.",
    "Scientists were the most important caste."
};

    public static string[] SubmergedSentences = new string[]
{
    "A major part of population was spending their time entertaining.",
    "The population forgot our problem in the entertaining area.",
    "We were at the bottom of a civilization.",
    "Our installations were the best.",
    "The first complete virtual reality had just opened."
};

    public static string[] RemainsSentences = new string[]
{
    "Their civilisation is in ruin.",
    "They reaped what they sowed.",
    "it's impressive that they're left with so much remains.",
};

    public static string[] BoatSentences = new string[]
{
    "The Sentus protection destroy many ships.",
    "I am the only one that can traverse the storm barrier.",
    "There is a boat.",
    "The remain of a boat."

};

    public static string[] TreeSentences = new string[]
{
    "Sentus modified the trees in each region.",
    "Sentus change the colors of the trees.",
    "Every tree were adapted to his area.",
    "When the wood was refined, it was very resistant."
};

    public static string[] SubmergedPillarSentences = new string[]
{
    "There is some pillars.",
    "Pillars were often use as distributor.",
    "Sentus meet at place like that."
};

    public static string[] SciencePillarSentences = new string[]
{
    "There is some strange pillars.",
    "These pillars were use as sensors.",
    "All pillars were link to a master pillar.",
    "This pillar seems to have lost his link."
};

    public static string[] PlankSentences = new string[]
{
    "I's impressive that plank survive that long.",
    "Some Sentus building were constructed only with wood.",
    "Some plank lie here."
};

    public static string[] RuinSentences = new string[]
{
    "Ruins, ruins and more ruins.",
    "The only witness left of them.",
    "Sentus destroyed themselves."
};

    public static string[] WallSentences = new string[]
{
    "There are some walls here.",
    "The Sentus love to have private and personal space.",
    "Protection was assured with the new materials."
};

    public static string[] SlabSentences = new string[]
{
    "There are some slab here.",
    "The only witness left of them.",
    "Sentus destroyed themselves."
};

    public static string[] RockSentences = new string[]
{
    "There are some rock here.",
    "This was probably part of something bigger.",
    "The runes were incribed in rocks at the beginning."
};

    public static string[] StoneSentences = new string[]
{
    "There are stone here.",
    "Stone were used for alot of construction.",
    "Stone working were the first art mastered by the Sentus."
};

    public static string[] SandSentences = new string[]
{
    "There are sand here.",
    "Sand was not used anymore."
};

    public static string[] WonderSentences = new string[]
{
    "It seems to be an old wonder of the Sentus.",
    "The wonders of the Sentus resisted the pangs of time."
};

    public static string[] SnakeSentences = new string[]
{
    "The snake was one of the representation of knowledge.",
    "This structure is optized to harvest cold.",
    "This construction is the perfect mix of utility and fashion."
};

    public static string[] RingSentences = new string[]
{
    "This wonder is completly destroyed.",
    "This ring was build to impress visitors.",
    "This structure was build atop of the city, everyone can see it from afar."
};

    public static string[] OrcaSentences = new string[]
{
    "There is a beauitiful statue of an Orca.",
    "The Orca were created by the Sentus.",
    "Orca was the symbol of strenght."
};

    public static string[] WhiteBuildingSentences = new string[]
{
    "There is a beauitiful ruin of white building.",
    "These building were constructed to impress people.",
    "That was the house of diplomates."
};

    public static string[] MusicHoleSentences = new string[]
{
    "This particular stone seems to make music with wind.",
    "Stone like these were constructed to welcome visitors.",
    "Sentus were able to build musical instrument in buildings."
};

    public static string[] WindmillSentences = new string[]
{
    "This seems to be an old windmill.",
    "Windmill was used to harvest energie." 
};

    public static string[] RuneSentences = new string[]
{
    "Runes have a signification and combined they form words.",
    "Every runes engraved contain some power."
};

    public static string[] JoySentences = new string[]
{
    "Entertaining was central to the Sentus.",
    "Joy was the goal of most of the population.",
    "Joy was distilled in many forms."
};

    public static string[] KnowledgeSentences = new string[]
{
    "Knowledge becomes the most important thing to the Sentus.",
    "Education was mandatory for every citizens.",
    "Scientists were the keepers of knowledge."
};

    public static string[] SunSentences = new string[]
{
    "The Sun was the symbol of the farmers.",
    "During the day, clouds were ecarted for the crops.",
    "In the early stage of their civilisation the Sun was prayed.."
};

    public static string[] DaySentences = new string[]
{
    "The day of the citizens revolve around work.",
    "The Sentus could not control time"
};

    public static string[] BeginningSentences = new string[]
{
    "The civilisation of the Sentus begins in the north.",
    "Every begining has an End."
};

    public static string[] DestinySentences = new string[]
{
    "Sentus were destined to take over the world.",
    "Destiny has to be written." 
};

    public static string[] WealthSentences = new string[]
{
    "Wealth was important to the Sentus.",
    "Wealth is the symbol for the merchants.",
    "The Sentus loved to spread their wealth."
};

    public static string[] EyeSentences = new string[]
{
    "Sentus believed that the eyes are the reflect of souls.",
    "Sentus were able to restore view." 
};

    public static string[] JourneySentences = new string[]
{
    "Every journey of yound Sentus begin in the wealth area.",
    "In ancient time Sentus were great travelers."
};

    public static string[] BirdSentences = new string[]
{
    "Birds were modified for each areas.",
    "Birds were the symbol of freedom."
};

    public static string[] FireSentences = new string[]
{
    "Fire were used alot by the makers.",
    "People say they burned the land."
};

    public static string[] AshSentences = new string[]
{
    "Only ash remain after the passage of the makers.",
    "The ashes make the land fertile ."
};

    public static string[] IceSentences = new string[]
{
    "Cold was difficult to harvest.",
    "Ice was used in alot of drinks.",
    "The north was full of Ice"
};
}

public class Island
{
    public int ID;
    public bool DrawStarted = false;
    public string IslandFinalName = "Name the island";
    public Sprite Drawing;
    public Texture2D TextureDraw;
    public List<Sprite> Pictures = new List<Sprite>();
    public List<string> PicturesDescription = new List<string>();
    public List<string> PicturesNarratives = new List<string>();
    public string NarrativText = "Aucun texte chargé";
}

// Phrases de narration en fonction de leur ID
public enum SentenceType
{   Areas, Orange, Green, Grey, Snow, Submerged,
    Remains, Boat, Tree, SubmergedPillar, SciencePillar, Plank, Ruin, Wall, Slab, Rock, Stone, Sand,
    Wonder, Snake, Ring, Orca, WhiteBuilding, MusicHole, windmill,
    Rune, Joy, Knowledge, Sun, Day, Beginning, Destiny, Wealth, Eye, Journey, Bird, Fire, Ash, Ice
};

