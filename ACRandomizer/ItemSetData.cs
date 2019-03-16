using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACRandomizer
{
    class ItemSetData
    {
        private static readonly SetInfo[] SetInfoCabinArray =
        {
            new SetInfo("cabin rug",        "260D", "cabin"),
            new SetInfo("cabin wall",       "270D", "cabin"),
            new SetInfo("cabin wardrobe",   "101C", "cabin"),
            new SetInfo("cabin dresser",    "1048", "cabin"),
            new SetInfo("cabin clock",      "1304", "cabin"),
            new SetInfo("cabin bed",        "138C", "cabin"),
            new SetInfo("cabin couch",      "1390", "cabin"),
            new SetInfo("cabin armchair",   "1394", "cabin"),
            new SetInfo("cabin bookcase",   "1398", "cabin"),
            new SetInfo("cabin low table",  "139C", "cabin"),
            new SetInfo("cabin chair",      "1578", "cabin"),
            new SetInfo("cabin table",      "1590", "cabin"),
        };

        private static readonly SetInfo[] SetInfoClassicArray =
        {
            new SetInfo("classic carpet",   "2601", "classic"),
            new SetInfo("classic wall",     "2701", "classic"),
            new SetInfo("classic wardrobe", "1004", "classic"),
            new SetInfo("classic vanity",   "1060", "classic"),
            new SetInfo("classic hutch",    "10F4", "classic"),
            new SetInfo("classic chair",    "10F8", "classic"),
            new SetInfo("classic desk",     "10FC", "classic"),
            new SetInfo("classic table",    "1100", "classic"),
            new SetInfo("classic cabinet",  "1104", "classic"),
            new SetInfo("classic sofa",     "115C", "classic"),
            new SetInfo("classic clock",    "117C", "classic"),
            new SetInfo("classic bed",      "1214", "classic"),
        };

        private static readonly SetInfo[] SetInfoKiddieArray =
        {
            new SetInfo("kiddie carpet",    "2636", "kiddie"),
            new SetInfo("kiddie wall",      "2736", "kiddie"),
            new SetInfo("kiddie dresser",   "1070", "kiddie"),
            new SetInfo("kiddie bureau",    "1074", "kiddie"),
            new SetInfo("kiddie wardrobe",  "1078", "kiddie"),
            new SetInfo("kiddie clock",     "1ec0", "kiddie"),
            new SetInfo("kiddie bed",       "1EC4", "kiddie"),
            new SetInfo("kiddie table",     "1EC8", "kiddie"),
            new SetInfo("kiddie couch",     "1ECC", "kiddie"),
            new SetInfo("kiddie stereo",    "1ED0", "kiddie"),
            new SetInfo("kiddie chair",     "1ED4", "kiddie"),
            new SetInfo("kiddie bookcase",  "1ED8", "kiddie"),
        };

        private static readonly SetInfo[] SetInfoLovelyArray =
        {
            new SetInfo("lovely carpet",    "2607", "lovely"),
            new SetInfo("lovely wall",      "2707", "lovely"),
            new SetInfo("lovely dresser",   "104C", "lovely"),
            new SetInfo("lovely end table", "1160", "lovely"),
            new SetInfo("lovely armchair",  "1164", "lovely"),
            new SetInfo("lovely lamp",      "116C", "lovely"),
            new SetInfo("lovely kitchen",   "1170", "lovely"),
            new SetInfo("lovely chair",     "1174", "lovely"),
            new SetInfo("lovely bed",       "1178", "lovely"),
            new SetInfo("lovely vanity",    "11EC", "lovely"),
            new SetInfo("lovely table",     "121C", "lovely"),
        };

        private static readonly SetInfo[] SetInfoBoxingArray =
        {
            new SetInfo("boxing ring mat",  "2641", "boxing"),
            new SetInfo("ringside seating", "2741", "boxing"),
            new SetInfo("boxing barricade", "3338", "boxing"),
            new SetInfo("blue corner",      "3344", "boxing"),
            new SetInfo("neutral corner",   "333C", "boxing"),
            new SetInfo("red corner",       "3340", "boxing"),
            new SetInfo("boxing mat",       "3348", "boxing"),
            new SetInfo("ringside table",   "334C", "boxing"),
            new SetInfo("sandbag",          "3354", "boxing"),
            new SetInfo("speedbag",         "3350", "boxing"),
            new SetInfo("weight bench",     "3358", "boxing"),
            new SetInfo("judge's bell",     "33B8", "boxing"),
        };

        private static readonly SetInfo[] SetInfoSpaceArray =
        {
            new SetInfo("lunar surface",    "260F", "space"),
            new SetInfo("lunar horizon",    "270F", "space"),
            new SetInfo("lunar rover",      "14DC", "space"),
            new SetInfo("satellite",        "14E0", "space"),
            new SetInfo("flying saucer",    "14F0", "space"),
            new SetInfo("rocket",           "14FC", "space"),
            new SetInfo("Spaceman Sam",     "1500", "space"),
            new SetInfo("asteroid",         "1514", "space"),
            new SetInfo("lunar lander",     "1544", "space"),
            new SetInfo("space station",    "1570", "space"),
            new SetInfo("space shuttle",    "1580", "space"),
        };

        private static readonly SetInfo[] SetInfoWesternArray =
        {
            new SetInfo("western desert",   "2639", "western"),
            new SetInfo("western vista",    "2739", "western"),
            new SetInfo("tumbleweed",       "32B0", "western"),
            new SetInfo("cow skull",        "32B4", "western"),
            new SetInfo("saddle fence",     "32BC", "western"),
            new SetInfo("western fence",    "32C0", "western"),
            new SetInfo("watering trough",  "32C4", "western"),
            new SetInfo("covered wagon",    "32D4", "western"),
            new SetInfo("storefront",       "32D8", "western"),
            new SetInfo("desert cactus",    "3328", "western"),
            new SetInfo("wagon wheel",      "3330", "western"),
            new SetInfo("well",             "3334", "western"),
        };

        private static readonly SetInfo[] SetInfoGardenArray =
        {
            new SetInfo("mossy carpet",     "2609", "garden"),
            new SetInfo("mortar wall",      "2709", "garden"),
            new SetInfo("low lantern",      "1228", "garden"),
            new SetInfo("tall lantern",     "122C", "garden"),
            new SetInfo("pond lantern",     "1230", "garden"),
            new SetInfo("shrine lantern",   "1254", "garden"),
            new SetInfo("deer scare",       "13E0", "garden"),
            new SetInfo("garden pond",      "14F8", "garden"),
        };

        private static readonly SetInfo[] SetInfoRockGardenArray =
        {
            new SetInfo("sand garden",      "2610", "rockgarden"),
            new SetInfo("garden wall",      "2710", "rockgarden"),
            new SetInfo("garden stone",     "14A8", "rockgarden"),
            new SetInfo("standing stone",   "14AC", "rockgarden"),
            new SetInfo("mossy stone",      "14E4", "rockgarden"),
            new SetInfo("leaning stone",    "14E8", "rockgarden"),
            new SetInfo("dark stone",       "14EC", "rockgarden"),
            new SetInfo("stone couple",     "14F4", "rockgarden"),
        };

        private static readonly SetInfo[] SetInfoApatoArray =
        {
            new SetInfo("apato skull",      "1F04", "apato"),
            new SetInfo("apato tail",       "1F08", "apato"),
            new SetInfo("apato torso",      "1F0C", "apato"),
        };

        private static readonly SetInfo[] SetInfoAppleArray =
        {
            new SetInfo("apple TV",         "12F8", "apple"),
            new SetInfo("apple clock",      "1E44", "apple"),
        };

        private static readonly SetInfo[] SetInfoBearArray =
        {
            new SetInfo("Baby bear",        "2610", "bear"),
            new SetInfo("Mama bear",        "2710", "bear"),
            new SetInfo("Papa bear",        "14A8", "bear"),
        };

        private static readonly SetInfo[] SetInfoBonsaiArray =
        {
            new SetInfo("pine bonsai",      "13E4", "bonsai"),
            new SetInfo("mugho bonsai",     "13E8", "bonsai"),
            new SetInfo("ponderosa bonsai", "13F0", "bonsai"),
        };

        private static readonly SetInfo[] SetInfoCactusArray =
        {
            new SetInfo("tall cactus",      "120C", "cactus"),
            new SetInfo("round cactus",     "1210", "cactus"),
            new SetInfo("cactus",           "13D8", "cactus"),
        };

        private static readonly SetInfo[] SetInfoCitrusArray =
        {
            new SetInfo("orange chair",     "12EC", "citrus"),
            new SetInfo("lemon table",      "12F4", "citrus"),
            new SetInfo("grapefruit table", "13C0", "citrus"),
            new SetInfo("lime chair",       "13C4", "citrus"),
        };

        private static readonly SetInfo[] SetInfoDaffodilArray =
        {
            new SetInfo("daffodil table",   "1270", "daffodil"),
            new SetInfo("daffodil chair",   "1280", "daffodil"),
        };

        private static readonly SetInfo[] SetInfoDrumArray =
        {
            new SetInfo("conga drum",       "11D0", "drum"),
            new SetInfo("timpano drum",     "11F4", "drum"),
            new SetInfo("djimbe drum",      "133C", "drum"),
        };

        private static readonly SetInfo[] SetInfoFigureArray =
        {
            new SetInfo("Keiko figurine",   "1114", "figure"),
            new SetInfo("Yuki figurine",    "1118", "figure"),
            new SetInfo("Yoko figurine",    "111C", "figure"),
            new SetInfo("Emi figurine",     "1120", "figure"),
            new SetInfo("Maki figurine",    "1124", "figure"),
            new SetInfo("Naomi figurine",   "1128", "figure"),
            new SetInfo("Aiko figurine",    "1E38", "figure"),
        };

        private static readonly SetInfo[] SetInfoFroggyArray =
        {
            new SetInfo("froggy chair",     "10A4", "froggy"),
            new SetInfo("lily-pad table",   "10A8", "froggy"),
        };

        private static readonly SetInfo[] SetInfoGuitarArray =
        {
            new SetInfo("folk guitar",      "10D8", "guitar"),
            new SetInfo("country guitar",   "10DC", "guitar"),
            new SetInfo("rock guitar",      "10E0", "guitar"),
        };

        private static readonly SetInfo[] SetInfoIrisArray =
        {
            new SetInfo("iris table",       "1274", "iris"),
            new SetInfo("iris chair",       "1284", "iris"),
        };

        private static readonly SetInfo[] SetInfoMammothArray =
        {
            new SetInfo("mammoth skull",    "1F34", "mammoth"),
            new SetInfo("mammoth torso",    "1F3C", "mammoth"),
        };

        private static readonly SetInfo[] SetInfoMelonArray =
        {
            new SetInfo("watermelon chair", "1468", "melon"),
            new SetInfo("melon chair",      "146C", "melon"),
            new SetInfo("watermelon table", "1470", "melon"),
        };

        private static readonly SetInfo[] SetInfoOfficeArray =
        {
            new SetInfo("office locker",    "100C", "office"),
            new SetInfo("office desk",      "11BC", "office"),
            new SetInfo("office chair",     "1234", "office"),
        };

        private static readonly SetInfo[] SetInfoPearArray =
        {
            new SetInfo("pear wardrobe",    "1028", "pear"),
            new SetInfo("pear dresser",     "1058", "pear"),
        };

        private static readonly SetInfo[] SetInfoPineArray =
        {
            new SetInfo("pine table",       "1294", "pine"),
            new SetInfo("pine chair",       "1298", "pine"),
        };

        private static readonly SetInfo[] SetInfoPlesioArray =
        {
            new SetInfo("plesio skull",     "1F28", "plesio"),
            new SetInfo("plesio neck",      "1F2C", "plesio"),
            new SetInfo("plesio torso",     "1F30", "plesio"),
        };

        private static readonly SetInfo[] SetInfoStegoArray =
        {
            new SetInfo("stego skull",      "1F10", "stego"),
            new SetInfo("stego tail",       "1F14", "stego"),
            new SetInfo("stego torso",      "1F18", "stego"),
        };

        private static readonly SetInfo[] SetInfoStringsArray =
        {
            new SetInfo("violin",           "1480", "strings"),
            new SetInfo("bass",             "1484", "strings"),
            new SetInfo("cello",            "1488", "strings"),
        };

        private static readonly SetInfo[] SetInfoTRexArray =
        {
            new SetInfo("T-rex skull",      "1EF8", "T-rex"),
            new SetInfo("T-rex tail",       "1EFC", "T-rex"),
            new SetInfo("T-rex torso",      "1F00", "T-rex"),
        };

        private static readonly SetInfo[] SetInfoTriceraArray =
        {
            new SetInfo("tricera skull",    "1EEC", "tricera"),
            new SetInfo("tricera tail",     "1EF0", "tricera"),
            new SetInfo("tricera torso",    "1EF4", "tricera"),
        };

        private static readonly SetInfo[] SetInfoTulipArray =
        {
            new SetInfo("tulip table",      "126C", "tulip"),
            new SetInfo("tulip chair",      "127C", "tulip"),
        };

        private static readonly SetInfo[] SetInfoVaseArray =
        {
            new SetInfo("blue vase",        "1278", "vase"),
            new SetInfo("tea vase",         "129C", "vase"),
            new SetInfo("red vase",         "12A0", "vase"),
        };

        private static readonly SetInfo[] SetInfoWritingArray =
        {
            new SetInfo("writing desk",     "1110", "writing"),
            new SetInfo("globe",            "112C", "writing"),
            new SetInfo("writing chair",    "1194", "writing"),
        };
    }
}
