using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace ACRandomizer
{
    public partial class Form1 : Form
    {
        Random rnd = new Random();
        private static int scoreTypeTotal = 10;
        private static int itemSetArrayTotal = 20;
        private static int itemSetNormalTotal = 22;
        private static int itemDistTotal = 20;
        private static int eventHRAValue = 19;
        private static int fullitemSetTotal = 574;

        private static string outputLogFileLocation = "C:/Users/Brian/Desktop/AC Files/AC ISOs/Animal Crossing (USA).iso_dir_Edit";
        private static string outputLogFileName = "output.txt";
        private static string inputRELFileLocation = "C:/Users/Brian/Desktop/AC Files/AC ISOs/Animal Crossing (USA).iso_dir_Edit";
        private static string inputRELFileName = "foresta.rel";
        private static string outputRELFileLocation = "C:/Users/Brian/Desktop/AC Files/AC ISOs/Animal Crossing (USA).iso_dir_Edit";
        private static string outputRELFileName = "foresta.txt";
        long seed = 0;

        public Form1()
        {
            InitializeComponent();
            txtInputLocation.Text = "C:/Users/Brian/Desktop/AC Files/AC ISOs/Animal Crossing (USA).iso_dir_Edit";
            txtOutputLocation.Text = outputLogFileLocation + "/" + outputLogFileName;
        }

        private void btnGenerateROM_Click(object sender, EventArgs e)
        {
            seed = rnd.Next(0, 16777215) * 4294967296 + rnd.Next(0, 2147483647);
            string seedHex = seed.ToString("X14");
            txtSeed.Text = seedHex;

            byte[] fileBytes = File.ReadAllBytes(inputRELFileLocation + "/" + inputRELFileName);
            StringBuilder sbRELText = new StringBuilder();

            int setItemsNormalNeeded = 0, setItemsRemaining = 0;
            int counter = 0, pos = 0;
            long scoreType = Convert.ToUInt32(seedHex.Substring(0, 4), 16) % scoreTypeTotal;
            SetInfo[] itemSetArray = new SetInfo[itemSetArrayTotal];
            scoreType = 6;

            //Select 100K HRA score Option
            switch (scoreType)
            {
                case 0: for (int i = 0; i < SetInfoCabinArray.Length; i++) { itemSetArray[i] = SetInfoCabinArray[i]; } break;
                case 1: for (int i = 0; i < SetInfoClassicArray.Length; i++) { itemSetArray[i] = SetInfoCabinArray[i]; } break;
                case 2: for (int i = 0; i < SetInfoKiddieArray.Length; i++) { itemSetArray[i] = SetInfoKiddieArray[i]; } break;
                case 3: for (int i = 0; i < SetInfoLovelyArray.Length; i++) { itemSetArray[i] = SetInfoLovelyArray[i]; } break;
                case 4: for (int i = 0; i < SetInfoBoxingArray.Length; i++) { itemSetArray[i] = SetInfoBoxingArray[i]; } break;
                case 5: for (int i = 0; i < SetInfoWesternArray.Length; i++) { itemSetArray[i] = SetInfoWesternArray[i]; } break;
                case 6: for (int i = 0; i < SetInfoSpaceArray.Length; i++) { itemSetArray[i] = SetInfoSpaceArray[i]; }
                    setItemsNormalNeeded = 5; pos = SetInfoSpaceArray.Length; break;
                case 7: for (int i = 0; i < SetInfoGardenArray.Length; i++) { itemSetArray[i] = SetInfoGardenArray[i]; }
                    setItemsNormalNeeded = 11; pos = SetInfoGardenArray.Length; break;
                case 8: for (int i = 0; i < SetInfoRockGardenArray.Length; i++) { itemSetArray[i] = SetInfoRockGardenArray[i]; }
                    setItemsNormalNeeded = 12; pos = SetInfoRockGardenArray.Length; break;
                case 9: itemSetArray[0] = CarpetsArray[0]; itemSetArray[1] = WallpapersArray[0]; pos = 2; setItemsNormalNeeded = 16; break;
                default: break;
            }

            //Resize ItemSetArray
            counter = 0;
            for (int i = 0; i < itemSetArray.Length; i++)
            {
                if (itemSetArray[i] != null) { counter++; }
            }
            Array.Resize<SetInfo>(ref itemSetArray, counter + setItemsNormalNeeded);

            //Fill in Extra Sets, if applicable
            counter = 0;
            setItemsRemaining = setItemsNormalNeeded;
            int[] setItemCountArray = new int[itemSetArrayTotal];
            for (int i = 0; i < setItemCountArray.Length; i++)
            {
                setItemCountArray[i] = itemSetArrayTotal + 1;
            }

            while (setItemsRemaining > 0)
            {
                long setNumber = (Convert.ToUInt32(seedHex.Substring((1 + counter) % 8, 4), 16) + counter) % itemSetNormalTotal;
                int setItemCount = GetSetItemNormalLength(Convert.ToInt32(setNumber));
                
                if (setItemsRemaining - setItemCount >= 0 && setItemsRemaining - setItemCount != 1) {
                    bool isInItemSetArray = false;
                    for (int j = 0; j < setItemCountArray.Length; j++)
                    {
                        if (Convert.ToInt32(setNumber) == setItemCountArray[j] || Convert.ToInt32(setNumber) <= 4 && setItemCountArray[j] <= 4)
                        {
                            isInItemSetArray = true;
                        }
                    }
                    if (isInItemSetArray == false)
                    {
                        itemSetArray = SetItemSetArray(itemSetArray, setNumber, pos, setItemCount);
                        setItemCountArray[pos] = Convert.ToInt32(setNumber);
                        pos += setItemCount;
                        setItemsRemaining -= setItemCount;
                    }
                }
                counter++;
            }

            //Populate Location of Goal Items in World
            counter = Convert.ToInt32(Convert.ToUInt32(seedHex.Substring(6, 4), 16) + counter);
            EventInfo[] itemDistArray = new EventInfo[itemSetArray.Length];
            int[] eventArray = new int[itemDistTotal];
            eventArray = ShuffleArray(eventArray, counter, seedHex);

            for (int i = 0; i < itemDistArray.Length; i++) {
                if (i < 12 && eventArray[i] == eventHRAValue) //Logically switches House Model within sets, if applicable
                {
                    int switchValue = Convert.ToInt32(Convert.ToUInt32(seedHex.Substring(7, 4), 16)) % (itemDistTotal - 12) + 12;
                    eventArray[i] = eventArray[switchValue];
                    eventArray[switchValue] = eventHRAValue;
                }

                if (i == 0)
                {
                    itemDistArray[i] = EventInfoIndividualArray[9];
                }
                else if (i == 1)
                {
                    itemDistArray[i] = EventInfoIndividualArray[20];
                }
                else
                {
                    string eventGroup = GetEventName(eventArray[i]);
                    if (eventGroup == "holiday" || eventGroup == "festive" || eventGroup == "toy" || eventGroup == "vacation")
                    {
                        counter = Convert.ToInt32(Convert.ToUInt32(seedHex.Substring(3, 4), 16) + counter);
                        itemDistArray[i] = GetRandomEventFromName(eventGroup, counter, seedHex);
                    }
                    else
                    {
                        counter = 0;
                        EventInfo eventInfo = EventInfoIndividualArray[counter];
                        while (eventInfo.eventGroup.Trim() != eventGroup)
                        {
                            counter++;
                            eventInfo = EventInfoIndividualArray[counter];
                        }
                        itemDistArray[i] = EventInfoIndividualArray[counter];
                    }
                }
            }

            //Establish useable Furniture Pool
            string[] removedSetFurniturePool = new string[itemSetNormalTotal];
            for (int i = 0; i < removedSetFurniturePool.Length; i++)
            {
                string setItemNumber = GetRandomItemInItemSet(i, counter, seedHex);
                removedSetFurniturePool[i] = setItemNumber;
            }

            SetInfo[] useableFurniturePool = new SetInfo[UseableFurniturePoolArray.Length];
            for (int i = 0; i < UseableFurniturePoolArray.Length; i++)
            {
                bool isInItemSetArray = false;
                for (int j = 0; j < itemSetArray.Length; j++) {
                    if (itemSetArray[j].ItemNumber == UseableFurniturePoolArray[i].ItemNumber)
                    {
                        isInItemSetArray = true;
                    }
                }
                for (int j = 0; j < removedSetFurniturePool.Length; j++)
                {
                    if (removedSetFurniturePool[j] == UseableFurniturePoolArray[i].ItemNumber)
                    {
                        isInItemSetArray = true;
                    }
                }

                if (isInItemSetArray == false)
                {
                    useableFurniturePool[i] = UseableFurniturePoolArray[i];
                }
            }

            counter = 0;
            for (int i = 0; i <useableFurniturePool.Length; i++, counter++)
            {
                useableFurniturePool[counter] = useableFurniturePool[i];
                if (useableFurniturePool[i] == null)
                {
                    counter--;
                }
                
            }
            Array.Resize<SetInfo>(ref useableFurniturePool, counter);

            counter = Convert.ToInt32(Convert.ToUInt32(seedHex.Substring(3, 4), 16) + counter);
            int[] useableFurnitureArray = new int[useableFurniturePool.Length];
            useableFurnitureArray = ShuffleArray(useableFurnitureArray, counter, seedHex);
            SetInfo[] useableFurniturePoolShuffled = new SetInfo[useableFurniturePool.Length];
            for (int i = 0; i < useableFurniturePool.Length; i++)
            {
                useableFurniturePoolShuffled[i] = useableFurniturePool[useableFurnitureArray[i]];
            }

            //Establish useable Carpet and Wallpaper Pool (can group since both positionally align in tables)
            SetInfo[] useableCarpetPool = new SetInfo[CarpetsArray.Length];
            SetInfo[] useableWallpaperPool = new SetInfo[WallpapersArray.Length];
            for (int i = 0; i < CarpetsArray.Length; i++)
            {
                bool isInItemSetArray = false;
                for (int j = 0; j < itemSetArray.Length; j++)
                {
                    if (itemSetArray[j].ItemNumber == CarpetsArray[i].ItemNumber)
                    {
                        isInItemSetArray = true;
                    }
                }

                if (isInItemSetArray == false)
                {
                    useableCarpetPool[i] = CarpetsArray[i];
                    useableWallpaperPool[i] = WallpapersArray[i];
                }
            }

            counter = 0;
            for (int i = 0; i < useableCarpetPool.Length; i++, counter++)
            {
                useableCarpetPool[counter] = useableCarpetPool[i];
                useableWallpaperPool[counter] = useableWallpaperPool[i];
                if (useableCarpetPool[i] == null)
                {
                    counter--;
                }

            }
            Array.Resize<SetInfo>(ref useableCarpetPool, counter);

            counter = Convert.ToInt32(Convert.ToUInt32(seedHex.Substring(3, 4), 16) + counter);
            int[] useableCarpetArray = new int[useableCarpetPool.Length];
            useableCarpetArray = ShuffleArray(useableCarpetArray, counter, seedHex);
            SetInfo[] useableCarpetPoolShuffled = new SetInfo[useableCarpetPool.Length];
            counter = Convert.ToInt32(Convert.ToUInt32(seedHex.Substring(8, 4), 16) + counter);
            int[] useableWallpaperArray = new int[useableWallpaperPool.Length];
            useableWallpaperArray = ShuffleArray(useableWallpaperArray, counter, seedHex);
            SetInfo[] useableWallpaperPoolShuffled = new SetInfo[useableWallpaperPool.Length];
            for (int i = 0; i < useableCarpetPool.Length; i++)
            {
                useableCarpetPoolShuffled[i] = useableCarpetPool[useableCarpetArray[i]];
                useableWallpaperPoolShuffled[i] = useableWallpaperPool[useableWallpaperArray[i]];
            }

            //Establish organized item array
            string[] fullItemArrayName = new string[fullitemSetTotal];
            string[] fullItemArrayNumber = new string[fullitemSetTotal];
            string[] fullItemArrayAddress = new string[fullitemSetTotal];
            string[] itemDistAddressArray = new string[itemDistArray.Length];
            counter = 0;
            for (int i = 0; i < itemDistArray.Length; i++)
            {
                Tuple<int, int> duplicateInfo = GetDuplicatedItemsAndEventsByEventName(itemDistArray[i].eventGroup.Trim());
                //duplicateItemBoolArray[i] = duplicateInfo.Item1 <= 1 ? false : true;

                string lastAddress = "";
                int saveRandomNumber = 0;

                for (int j = 0; j < i; j++)
                {
                    if (itemDistArray[j].eventName == itemDistArray[i].eventName)
                    {
                        lastAddress = itemDistAddressArray[j];
                    }
                }

                for (int j = 0; j < duplicateInfo.Item1; j++)
                {
                    counter = Convert.ToInt32(Convert.ToUInt32(seedHex.Substring(5, 4), 16) + counter);
                    Tuple<string, int> eventAddressInfo = GetRandomAddressFromName(itemDistArray[i].eventAddress, itemDistArray[i].byteSize,
                        duplicateInfo.Item1, duplicateInfo.Item2, lastAddress, saveRandomNumber, counter, seedHex);
                    fullItemArrayName[itemDistArray[i].itemPosition + j + eventAddressInfo.Item2] = itemSetArray[i].ItemName;
                    fullItemArrayNumber[itemDistArray[i].itemPosition + j + eventAddressInfo.Item2] = itemSetArray[i].ItemNumber;
                    fullItemArrayAddress[itemDistArray[i].itemPosition + j + eventAddressInfo.Item2] = eventAddressInfo.Item1;
                    if (itemDistArray[i].eventGroup.Trim() == "common")
                    {
                        for (int k = 0; k < 2; k++)
                        {
                            fullItemArrayName[itemDistArray[i].itemPosition + j + eventAddressInfo.Item2 + (k + 1) * 103 + k] = itemSetArray[i].ItemName;
                            fullItemArrayNumber[itemDistArray[i].itemPosition + j + eventAddressInfo.Item2 + (k + 1) * 103 + k] = itemSetArray[i].ItemNumber;
                            fullItemArrayAddress[itemDistArray[i].itemPosition + j + eventAddressInfo.Item2 + (k + 1) * 103 + k] =
                            Convert.ToInt32(Convert.ToUInt32(eventAddressInfo.Item1.Substring(0, 8), 16) + (k + 1) * 208 + k * 4).ToString("X8");
                        }
                    }
                    lastAddress = fullItemArrayAddress[itemDistArray[i].itemPosition + j + eventAddressInfo.Item2];
                    saveRandomNumber = eventAddressInfo.Item2;
                }
                itemDistAddressArray[i] = lastAddress;
            }

            //Populate rest of address table with shuffled useable items
            int useableCarpetcounter = 0, useableWallpapercounter = 0, useableFurniturecounter = 0;
            for (int i = 0; i < fullItemArrayName.Length; i++)
            {
                if (String.IsNullOrEmpty(fullItemArrayName[i]))
                {
                    if (AddressArray[i].StartsWith("009086")) //Saharah Carpets
                    {
                        fullItemArrayName[i] = useableCarpetPoolShuffled[useableCarpetcounter].ItemName;
                        fullItemArrayNumber[i] = useableCarpetPoolShuffled[useableCarpetcounter].ItemNumber;
                        useableCarpetcounter++;
                    }
                    else if (AddressArray[i].StartsWith("00DA57")) //Wendell Wallpapers
                    {
                        fullItemArrayName[i] = useableWallpaperPoolShuffled[useableWallpapercounter].ItemName;
                        fullItemArrayNumber[i] = useableWallpaperPoolShuffled[useableWallpapercounter].ItemNumber;
                        useableWallpapercounter++;
                    }
                    else //Useable Furniture
                    {
                        fullItemArrayName[i] = useableFurniturePoolShuffled[useableFurniturecounter].ItemName;
                        fullItemArrayNumber[i] = useableFurniturePoolShuffled[useableFurniturecounter].ItemNumber;
                        useableFurniturecounter++;
                    }

                    fullItemArrayAddress[i] = AddressArray[i]; //"0";
                }

                if (useableCarpetcounter >= useableFurniturePoolShuffled.Length)
                {
                    useableCarpetcounter = 0;
                }
                if (useableWallpapercounter >= useableFurniturePoolShuffled.Length)
                {
                    useableWallpapercounter = 0;
                }
                if (useableFurniturecounter >= useableFurniturePoolShuffled.Length)
                {
                    useableFurniturecounter = 0;
                }
            }

            //Replace Bytes in REL file byte array
            int fullItemArrayCounter = 0;
            int modArrayCounter = 0;
            for (int i = 0; i < fileBytes.Length; i++)
            {
                if (i == Convert.ToInt32(Convert.ToUInt32(fullItemArrayAddress[fullItemArrayCounter].Substring(0, 8), 16)))
                {
                    sbRELText.Append(fullItemArrayNumber[fullItemArrayCounter]);
                    if (fullItemArrayCounter < fullItemArrayAddress.Length - 1)
                    {
                        fullItemArrayCounter++;
                    }
                    i++;
                }
                else if (i == Convert.ToInt32(Convert.ToUInt32(ModInfoArray[modArrayCounter].modAddress.Substring(0, 8), 16)))
                {
                    sbRELText.Append(ModInfoArray[modArrayCounter].modNumber);
                    if (modArrayCounter < ModInfoArray.Length - 1)
                    {
                        modArrayCounter++;
                    }
                    i++;
                }
                else
                {
                    sbRELText.Append(fileBytes[i].ToString("X2"));
                }
            }
            File.WriteAllText(outputRELFileLocation + "/" + outputRELFileName, sbRELText.ToString());

            //Output Information to Log File
            StringBuilder sbLogFile = new StringBuilder();

            sbLogFile.AppendLine("Goal Items:");
            for (int i = 0; i < itemSetArray.Length; i++)
            {
                sbLogFile.AppendLine(itemSetArray[i].ItemNumber + "\t" + itemSetArray[i].SetName + " " +
                    itemSetArray[i].ItemName + "\t" + itemDistArray[i].eventName);
            }
            sbLogFile.AppendLine("Extra Items:");
            for (int i = 0; i < useableFurniturePool.Length; i++)
            {
                sbLogFile.AppendLine(useableFurniturePool[i].ItemNumber + "\t" + useableFurniturePool[i].ItemName);
            }
            sbLogFile.AppendLine("Useable Items Pool:");
            for (int i = 0; i < useableFurniturePoolShuffled.Length; i++)
            {
                sbLogFile.AppendLine(i + "\t" + useableFurniturePoolShuffled[i].ItemNumber + "\t" + useableFurniturePoolShuffled[i].ItemName);
            }
            sbLogFile.AppendLine("Full Item Array:");
            for (int i = 0; i < fullItemArrayName.Length; i++)
            {
                sbLogFile.AppendLine(i + "\t" + fullItemArrayNumber[i] + "\t" + fullItemArrayName[i] + "\t" + fullItemArrayAddress[i]);
            }
            File.WriteAllText(txtOutputLocation.Text, sbLogFile.ToString());
            
            MessageBox.Show("Complete! Please see Log File");
        }



        private int[] ShuffleArray(int[] array, int counter, string seedHex)
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = i;
            }

            for (int i = 0; i < array.Length * 16; i++, counter++)
            {
                long setNumber1 = (Convert.ToUInt32(seedHex.Substring((1 + i * counter) % 8, 4), 16) + counter) % array.Length;
                long setNumber2 = (Convert.ToUInt32(seedHex.Substring((6 + i * counter) % 8, 4), 16) + counter) % array.Length;
                int temp = array[setNumber1];
                array[setNumber1] = array[setNumber2];
                array[setNumber2] = temp;
            }

            return array;
        }

        private SetInfo[] SetItemSetArray(SetInfo[] itemSetArray, long setNumber, int pos, int setItemCount)
        {
            switch (setNumber)
            {
                case 0: for (int i = pos; i < pos + setItemCount; i++) { itemSetArray[i] = SetInfoApatoArray[i - pos]; } break;
                case 1: for (int i = pos; i < pos + setItemCount; i++) { itemSetArray[i] = SetInfoPlesioArray[i - pos]; } break;
                case 2: for (int i = pos; i < pos + setItemCount; i++) { itemSetArray[i] = SetInfoStegoArray[i - pos]; } break;
                case 3: for (int i = pos; i < pos + setItemCount; i++) { itemSetArray[i] = SetInfoTRexArray[i - pos]; } break;
                case 4: for (int i = pos; i < pos + setItemCount; i++) { itemSetArray[i] = SetInfoTriceraArray[i - pos]; } break;
                case 5: for (int i = pos; i < pos + setItemCount; i++) { itemSetArray[i] = SetInfoAppleArray[i - pos]; } break;
                case 6: for (int i = pos; i < pos + setItemCount; i++) { itemSetArray[i] = SetInfoBearArray[i - pos]; } break;
                case 7: for (int i = pos; i < pos + setItemCount; i++) { itemSetArray[i] = SetInfoBonsaiArray[i - pos]; } break;
                case 8: for (int i = pos; i < pos + setItemCount; i++) { itemSetArray[i] = SetInfoCactusArray[i - pos]; } break;
                case 9: for (int i = pos; i < pos + setItemCount; i++) { itemSetArray[i] = SetInfoCitrusArray[i - pos]; } break;
                case 10: for (int i = pos; i < pos + setItemCount; i++) { itemSetArray[i] = SetInfoDaffodilArray[i - pos]; } break;
                case 11: for (int i = pos; i < pos + setItemCount; i++) { itemSetArray[i] = SetInfoFroggyArray[i - pos]; } break;
                case 12: for (int i = pos; i < pos + setItemCount; i++) { itemSetArray[i] = SetInfoGuitarArray[i - pos]; } break;
                case 13: for (int i = pos; i < pos + setItemCount; i++) { itemSetArray[i] = SetInfoIrisArray[i - pos]; } break;
                case 14: for (int i = pos; i < pos + setItemCount; i++) { itemSetArray[i] = SetInfoMammothArray[i - pos]; } break;
                case 15: for (int i = pos; i < pos + setItemCount; i++) { itemSetArray[i] = SetInfoMelonArray[i - pos]; } break;
                case 16: for (int i = pos; i < pos + setItemCount; i++) { itemSetArray[i] = SetInfoOfficeArray[i - pos]; } break;
                case 17: for (int i = pos; i < pos + setItemCount; i++) { itemSetArray[i] = SetInfoPearArray[i - pos]; } break;
                case 18: for (int i = pos; i < pos + setItemCount; i++) { itemSetArray[i] = SetInfoPineArray[i - pos]; } break;
                case 19: for (int i = pos; i < pos + setItemCount; i++) { itemSetArray[i] = SetInfoTulipArray[i - pos]; } break;
                case 20: for (int i = pos; i < pos + setItemCount; i++) { itemSetArray[i] = SetInfoVaseArray[i - pos]; } break;
                case 21: for (int i = pos; i < pos + setItemCount; i++) { itemSetArray[i] = SetInfoWritingArray[i - pos]; } break;
                default: break;
            }
            return itemSetArray;
        }

        private string GetRandomItemInItemSet(int setNumber, int counter, string seedHex)
        {
            string itemNumber = "";
            int setLength = GetSetItemNormalLength(setNumber);
            int setNumberRandom = Convert.ToInt32((Convert.ToUInt32(seedHex.Substring((5 + counter) % 9, 4), 16) + counter) % setLength);
            switch (setNumber)
            {
                case 0: itemNumber = SetInfoApatoArray[setNumberRandom].ItemNumber; break;
                case 1: itemNumber = SetInfoPlesioArray[setNumberRandom].ItemNumber; break;
                case 2: itemNumber = SetInfoStegoArray[setNumberRandom].ItemNumber; break;
                case 3: itemNumber = SetInfoTRexArray[setNumberRandom].ItemNumber; break;
                case 4: itemNumber = SetInfoTriceraArray[setNumberRandom].ItemNumber; break;
                case 5: itemNumber = SetInfoAppleArray[setNumberRandom].ItemNumber; break;
                case 6: itemNumber = SetInfoBearArray[setNumberRandom].ItemNumber; break;
                case 7: itemNumber = SetInfoBonsaiArray[setNumberRandom].ItemNumber; break;
                case 8: itemNumber = SetInfoCactusArray[setNumberRandom].ItemNumber; break;
                case 9: itemNumber = SetInfoCitrusArray[setNumberRandom].ItemNumber; break;
                case 10: itemNumber = SetInfoDaffodilArray[setNumberRandom].ItemNumber; break;
                case 11: itemNumber = SetInfoFroggyArray[setNumberRandom].ItemNumber; break;
                case 12: itemNumber = SetInfoGuitarArray[setNumberRandom].ItemNumber; break;
                case 13: itemNumber = SetInfoIrisArray[setNumberRandom].ItemNumber; break;
                case 14: itemNumber = SetInfoMammothArray[setNumberRandom].ItemNumber; break;
                case 15: itemNumber = SetInfoMelonArray[setNumberRandom].ItemNumber; break;
                case 16: itemNumber = SetInfoOfficeArray[setNumberRandom].ItemNumber; break;
                case 17: itemNumber = SetInfoPearArray[setNumberRandom].ItemNumber; break;
                case 18: itemNumber = SetInfoPineArray[setNumberRandom].ItemNumber; break;
                case 19: itemNumber = SetInfoTulipArray[setNumberRandom].ItemNumber; break;
                case 20: itemNumber = SetInfoVaseArray[setNumberRandom].ItemNumber; break;
                case 21: itemNumber = SetInfoWritingArray[setNumberRandom].ItemNumber; break;
                default: break;
            }
            counter++;
            return itemNumber;
        }

        private EventInfo GetRandomEventFromName(string eventName, int counter, string seedHex)
        {
            EventInfo eventInfo = null;
            int eventLength = GetEventLength(eventName);
            int setNumberRandom = Convert.ToInt32((Convert.ToUInt32(seedHex.Substring((5 + counter) % 9, 4), 16) + counter) % eventLength);
            switch (eventName)
            {
                case "holiday": eventInfo = EventInfoHolidayArray[setNumberRandom]; break;
                case "festive": eventInfo = EventInfoFestiveArray[setNumberRandom]; break;
                case "toy": eventInfo = EventInfoToyArray[setNumberRandom]; break;
                case "vacation": eventInfo = EventInfoVacationArray[setNumberRandom]; break;
                default: eventInfo = EventInfoIndividualArray[setNumberRandom]; break;
            }
            counter++;
            return eventInfo;
        }

        private Tuple<string, int> GetRandomAddressFromName(string eventAddress, int eventSize, int duplicateItems,
            int duplicateEvents, string lastAddress, int saveRandomNumber, int counter, string seedHex)
        {
            string addressInfo = "";
            int setNumberRandom = 0;
            if (String.IsNullOrEmpty(lastAddress))
            {
                setNumberRandom = Convert.ToInt32((Convert.ToUInt32(seedHex.Substring((2 + counter) % 9, 4), 16) + counter) %
                    (eventSize - (duplicateItems * duplicateEvents - 1)));
                addressInfo = (Convert.ToInt32(Convert.ToUInt32(eventAddress.Substring(0, 8), 16)) + setNumberRandom * 2).ToString("X8");
            }
            else
            {
                addressInfo = (Convert.ToInt32(Convert.ToUInt32(lastAddress.Substring(0, 8), 16)) + 2).ToString("X8");
                setNumberRandom = saveRandomNumber;
            }
            return new Tuple<string, int>(addressInfo, setNumberRandom);
        }

        private Tuple<int, int> GetDuplicatedItemsAndEventsByEventName(string eventName)
        {
            int duplicateItems = 1;
            int duplicateEvents = 1;
            switch (eventName)
            {
                case "wendell": duplicateItems = 6; break;
                case "gulliver": duplicateItems = 7; break;
                case "snowman": duplicateItems = 4; break;
                case "jingle": case "jack": case "igloo": case "summer": duplicateItems = 3; break;
                case "common": case "redd": duplicateItems = 2; duplicateEvents = 2; break;
                case "lottery": duplicateItems = 2; break;
                default: break;
            }

            return new Tuple<int, int>(duplicateItems, duplicateEvents);
        }

        private string GetEventName(int eventLocation)
        {
            string eventName = "";
            switch (eventLocation)
            {
                case 0: eventName = "holiday"; break;
                case 1: eventName = "holiday"; break;
                case 2: eventName = "toy"; break;
                case 3: eventName = "radio"; break;
                case 4: eventName = "vacation"; break;
                case 5: eventName = "franklin"; break;
                case 6: eventName = "post"; break;
                case 7: eventName = "snowman"; break;
                case 8: eventName = "common"; break;
                case 9: eventName = "common"; break;
                case 10: eventName = "festive"; break;
                case 11: eventName = "redd"; break;
                case 12: eventName = "redd"; break;
                case 13: eventName = "lottery"; break;
                case 14: eventName = "jack"; break;
                case 15: eventName = "jingle"; break;
                case 16: eventName = "gulliver"; break;
                case 17: eventName = "igloo"; break;
                case 18: eventName = "summer"; break;
                case 19: eventName = "hra"; break;
                default: eventName = "null"; break;
            }

            return eventName;
        }

        private int GetSetItemNormalLength(int setItemCount)
        {
            int itemCount = 0;
            switch (setItemCount)
            {
                case 0: itemCount = SetInfoApatoArray.Length; break;
                case 1: itemCount = SetInfoPlesioArray.Length; break;
                case 2: itemCount = SetInfoStegoArray.Length; break;
                case 3: itemCount = SetInfoTRexArray.Length; break;
                case 4: itemCount = SetInfoTriceraArray.Length; break;
                case 5: itemCount = SetInfoAppleArray.Length; break;
                case 6: itemCount = SetInfoBearArray.Length; break;
                case 7: itemCount = SetInfoBonsaiArray.Length; break;
                case 8: itemCount = SetInfoCactusArray.Length; break;
                case 9: itemCount = SetInfoCitrusArray.Length; break;
                case 10: itemCount = SetInfoDaffodilArray.Length; break;
                case 11: itemCount = SetInfoFroggyArray.Length; break;
                case 12: itemCount = SetInfoGuitarArray.Length; break;
                case 13: itemCount = SetInfoIrisArray.Length; break;
                case 14: itemCount = SetInfoMammothArray.Length; break;
                case 15: itemCount = SetInfoMelonArray.Length; break;
                case 16: itemCount = SetInfoOfficeArray.Length; break;
                case 17: itemCount = SetInfoPearArray.Length; break;
                case 18: itemCount = SetInfoPineArray.Length; break;
                case 19: itemCount = SetInfoTulipArray.Length; break;
                case 20: itemCount = SetInfoVaseArray.Length; break;
                case 21: itemCount = SetInfoWritingArray.Length; break;
                default: break;
            }
            return itemCount;
        }

        private int GetEventLength(string eventName)
        {
            int eventLength = 0;
            switch (eventName)
            {
                case "holiday": eventLength = EventInfoHolidayArray.Length; break;
                case "festive": eventLength = EventInfoFestiveArray.Length; break;
                case "toy": eventLength = EventInfoToyArray.Length; break;
                case "vacation": eventLength = EventInfoVacationArray.Length; break;
                default: eventLength = EventInfoIndividualArray.Length; break;
            }
            return eventLength;
        }

        private static readonly EventInfo[] EventInfoIndividualArray =
        {
            new EventInfo("Aerobics Radio  ", "0007CD5E", "radio           ",   7,      1),
            new EventInfo("House Model     ", "001620EE", "hra             ",   10,     1),  //Make sure hra stays at case 19!!
            new EventInfo("Manor Model     ", "0016211A", "hra             ",   11,     1),
            new EventInfo("Franklin Furn.  ", "002D9BE0", "franklin        ",   12,     12),
            new EventInfo("Tissue          ", "002E64BB", "post            ",   24,     1),
            new EventInfo("Piggy Bank      ", "002E64CB", "post            ",   25,     1),
            new EventInfo("Mailbox         ", "002E64DB", "post            ",   26,     1),
            new EventInfo("Post Model      ", "002E64EB", "post            ",   27,     1),
            new EventInfo("Snowman Furn.   ", "0031E770", "snowman         ",   52,     12),
            new EventInfo("Saharah Carpets ", "009086A0", "saharah         ",   64,     18),
            new EventInfo("Item List A Pool", "00975560", "common          ",   82,     103),
            new EventInfo("Item List B Pool", "00975630", "common          ",   185,    104),
            new EventInfo("Item List C Pool", "00975704", "common          ",   289,    103),
            new EventInfo("Crazy Redd Pool ", "009757D4", "redd            ",   392,    67),
            new EventInfo("Lottery Pool    ", "00975868", "lottery         ",   459,    36),
            new EventInfo("Spooky Furn.    ", "009758B4", "jack            ",   495,    10),
            new EventInfo("Jingle Furn.    ", "009758D0", "jingle          ",   505,    10),
            new EventInfo("Gulliver Furn.  ", "00975908", "gulliver        ",   517,    20),
            new EventInfo("Igloo Furn.     ", "0097598C", "igloo           ",   537,    9),
            new EventInfo("Sum. Camp. Furn.", "009759C4", "summer          ",   546,    10),
            new EventInfo("Wendell Walls   ", "00DA5710", "wendell         ",   556,    18),
        };

        private static readonly EventInfo[] EventInfoHolidayArray =
        {
            new EventInfo("Birthday Cake   ", "0007148A", "holiday         ",   0,      1),
            new EventInfo("Weed Model      ", "002EDA56", "holiday         ",   28,     1),
            new EventInfo("Tailor Model    ", "002EDA58", "holiday         ",   29,     1),
            new EventInfo("Super Tortimer  ", "002EDA5A", "holiday         ",   30,     1),
            new EventInfo("Lovely Phone    ", "002EDA5E", "holiday         ",   31,     1),
            new EventInfo("Market Model    ", "002EDA60", "holiday         ",   32,     1),
            new EventInfo("Pink Tree Model ", "002EDA62", "holiday         ",   33,     1),
            new EventInfo("Spring Medal    ", "002EDA64", "holiday         ",   34,     1),
            new EventInfo("Tree Model      ", "002EDA66", "holiday         ",   35,     1),
            new EventInfo("Dump Model      ", "002EDA68", "holiday         ",   36,     1),
            new EventInfo("Locomotive Model", "002EDA6A", "holiday         ",   37,     1),
            new EventInfo("Angler Trophy   ", "002EDA6C", "holiday         ",   38,     1),
            new EventInfo("Bottled Ship    ", "002EDA70", "holiday         ",   39,     1),
            new EventInfo("Bottle Rocket   ", "002EDA72", "holiday         ",   40,     1),
            new EventInfo("Telescope       ", "002EDA74", "holiday         ",   41,     1),
            new EventInfo("Moon            ", "002EDA76", "holiday         ",   42,     1),
            new EventInfo("Well Model      ", "002EDA78", "holiday         ",   43,     1),
            new EventInfo("Police Model    ", "002EDA7A", "holiday         ",   44,     1),
            new EventInfo("Autumn Medal    ", "002EDA7C", "holiday         ",   45,     1),
            new EventInfo("Katrina's Tent  ", "002EDA7E", "holiday         ",   46,     1),
            new EventInfo("Fishing Trophy  ", "002EDA80", "holiday         ",   47,     1),
            new EventInfo("Snowman	       ", "002EDA82", "holiday         ",   48,     1),
            new EventInfo("Shop Model      ", "002EDA84", "holiday         ",   49,     1),
            new EventInfo("Noisemaker	   ", "002EDA88", "holiday         ",   50,     1),
            new EventInfo("Cornucopia	   ", "002EDA8A", "holiday         ",   51,     1),
            new EventInfo("Jack O' Lantern ", "00975900", "holiday         ",   515,    1),
            new EventInfo("Jack-in-the-Box ", "00975902", "holiday         ",   516,    1),
        };

        private static readonly EventInfo[] EventInfoFestiveArray =
        {
            new EventInfo("Festive Tree    ", "00077AD2", "festive         ",   1,      1),
            new EventInfo("Big Festive Tree", "00077AD6", "festive         ",   2,      1),
            new EventInfo("Festive Candle  ", "00077AEA", "festive         ",   3,      1),
            new EventInfo("Festive Flag    ", "00077AEE", "festive         ",   4,      1),
        };

        private static readonly EventInfo[] EventInfoToyArray =
        {
            new EventInfo("Miniature Car   ", "0007C48A", "toy             ",   5,      1),
            new EventInfo("Dolly           ", "0007C492", "toy             ",   6,      1),
        };

        private static readonly EventInfo[] EventInfoVacationArray =
        {
            new EventInfo("Lighthouse Model", "0007D8AE", "vacation        ",   8,      1),
            new EventInfo("Chocolates      ", "0007D8C6", "vacation        ",   9,      1),
        };

        private static readonly SetInfo[] SetInfoCabinArray =
        {
            new SetInfo("cabin rug       ", "260D", "cabin           "),
            new SetInfo("cabin wall      ", "270D", "cabin           "),
            new SetInfo("cabin wardrobe  ", "101C", "cabin           "),
            new SetInfo("cabin dresser   ", "1048", "cabin           "),
            new SetInfo("cabin clock     ", "1304", "cabin           "),
            new SetInfo("cabin bed       ", "138C", "cabin           "),
            new SetInfo("cabin couch     ", "1390", "cabin           "),
            new SetInfo("cabin armchair  ", "1394", "cabin           "),
            new SetInfo("cabin bookcase  ", "1398", "cabin           "),
            new SetInfo("cabin low table ", "139C", "cabin           "),
            new SetInfo("cabin chair     ", "1578", "cabin           "),
            new SetInfo("cabin table     ", "1590", "cabin           "),
        };

        private static readonly SetInfo[] SetInfoClassicArray =
            {
            new SetInfo("classic carpet  ", "2601", "classic         "),
            new SetInfo("classic wall    ", "2701", "classic         "),
            new SetInfo("classic wardrobe", "1004", "classic         "),
            new SetInfo("classic vanity  ", "1060", "classic         "),
            new SetInfo("classic hutch   ", "10F4", "classic         "),
            new SetInfo("classic chair   ", "10F8", "classic         "),
            new SetInfo("classic desk    ", "10FC", "classic         "),
            new SetInfo("classic table   ", "1100", "classic         "),
            new SetInfo("classic cabinet ", "1104", "classic         "),
            new SetInfo("classic sofa    ", "115C", "classic         "),
            new SetInfo("classic clock   ", "117C", "classic         "),
            new SetInfo("classic bed     ", "1214", "classic         "),
        };

        private static readonly SetInfo[] SetInfoKiddieArray =
        {
            new SetInfo("kiddie carpet   ", "2636", "kiddie          "),
            new SetInfo("kiddie wall     ", "2736", "kiddie          "),
            new SetInfo("kiddie dresser  ", "1070", "kiddie          "),
            new SetInfo("kiddie bureau   ", "1074", "kiddie          "),
            new SetInfo("kiddie wardrobe ", "1078", "kiddie          "),
            new SetInfo("kiddie clock    ", "1ec0", "kiddie          "),
            new SetInfo("kiddie bed      ", "1EC4", "kiddie          "),
            new SetInfo("kiddie table    ", "1EC8", "kiddie          "),
            new SetInfo("kiddie couch    ", "1ECC", "kiddie          "),
            new SetInfo("kiddie stereo   ", "1ED0", "kiddie          "),
            new SetInfo("kiddie chair    ", "1ED4", "kiddie          "),
            new SetInfo("kiddie bookcase ", "1ED8", "kiddie          "),
        };

        private static readonly SetInfo[] SetInfoLovelyArray =
        {
            new SetInfo("lovely carpet   ", "2607", "lovely          "),
            new SetInfo("lovely wall     ", "2707", "lovely          "),
            new SetInfo("lovely armoire  ", "1020", "lovely          "),
            new SetInfo("lovely dresser  ", "104C", "lovely          "),
            new SetInfo("lovely end table", "1160", "lovely          "),
            new SetInfo("lovely armchair ", "1164", "lovely          "),
            new SetInfo("lovely lamp     ", "116C", "lovely          "),
            new SetInfo("lovely kitchen  ", "1170", "lovely          "),
            new SetInfo("lovely chair    ", "1174", "lovely          "),
            new SetInfo("lovely bed      ", "1178", "lovely          "),
            new SetInfo("lovely vanity   ", "11EC", "lovely          "),
            new SetInfo("lovely table    ", "121C", "lovely          "),
        };

        private static readonly SetInfo[] SetInfoBoxingArray =
        {
            new SetInfo("boxing ring mat ", "2641", "boxing          "),
            new SetInfo("ringside seating", "2741", "boxing          "),
            new SetInfo("boxing barricade", "3338", "boxing          "),
            new SetInfo("blue corner     ", "3344", "boxing          "),
            new SetInfo("neutral corner  ", "333C", "boxing          "),
            new SetInfo("red corner      ", "3340", "boxing          "),
            new SetInfo("boxing mat      ", "3348", "boxing          "),
            new SetInfo("ringside table  ", "334C", "boxing          "),
            new SetInfo("sandbag         ", "3354", "boxing          "),
            new SetInfo("speedbag        ", "3350", "boxing          "),
            new SetInfo("weight bench    ", "3358", "boxing          "),
            new SetInfo("judge's bell    ", "33B8", "boxing          "),
        };

        private static readonly SetInfo[] SetInfoSpaceArray =
        {
            new SetInfo("lunar surface   ", "260F", "space           "),
            new SetInfo("lunar horizon   ", "270F", "space           "),
            new SetInfo("lunar rover     ", "1544", "space           "),
            new SetInfo("satellite       ", "14E0", "space           "),
            new SetInfo("flying saucer   ", "14F0", "space           "),
            new SetInfo("rocket          ", "14FC", "space           "),
            new SetInfo("Spaceman Sam    ", "1500", "space           "),
            new SetInfo("asteroid        ", "1514", "space           "),
            new SetInfo("lunar lander    ", "14DC", "space           "),
            new SetInfo("space station   ", "1570", "space           "),
            new SetInfo("space shuttle   ", "1580", "space           "),
        };

        private static readonly SetInfo[] SetInfoWesternArray =
        {
            new SetInfo("western desert  ", "2639", "western         "),
            new SetInfo("western vista   ", "2739", "western         "),
            new SetInfo("tumbleweed      ", "32B0", "western         "),
            new SetInfo("cow skull       ", "32B4", "western         "),
            new SetInfo("saddle fence    ", "32BC", "western         "),
            new SetInfo("western fence   ", "32C0", "western         "),
            new SetInfo("watering trough ", "32C4", "western         "),
            new SetInfo("covered wagon   ", "32D4", "western         "),
            new SetInfo("storefront      ", "32D8", "western         "),
            new SetInfo("desert cactus   ", "3328", "western         "),
            new SetInfo("wagon wheel     ", "3330", "western         "),
            new SetInfo("well            ", "3334", "western         "),
        };

        private static readonly SetInfo[] SetInfoGardenArray =
        {
            new SetInfo("mossy carpet    ", "2609", "garden          "),
            new SetInfo("mortar wall     ", "2709", "garden          "),
            new SetInfo("low lantern     ", "1228", "garden          "),
            new SetInfo("tall lantern    ", "122C", "garden          "),
            new SetInfo("pond lantern    ", "1230", "garden          "),
            new SetInfo("shrine lantern  ", "1254", "garden          "),
            new SetInfo("deer scare      ", "13E0", "garden          "),
            new SetInfo("garden pond     ", "14F8", "garden          "),
        };

        private static readonly SetInfo[] SetInfoRockGardenArray =
        {
            new SetInfo("sand garden     ", "2610", "rockgarden      "),
            new SetInfo("garden wall     ", "2710", "rockgarden      "),
            new SetInfo("garden stone    ", "14A8", "rockgarden      "),
            new SetInfo("standing stone  ", "14AC", "rockgarden      "),
            new SetInfo("mossy stone     ", "14E4", "rockgarden      "),
            new SetInfo("leaning stone   ", "14E8", "rockgarden      "),
            new SetInfo("dark stone      ", "14EC", "rockgarden      "),
            new SetInfo("stone couple    ", "14F4", "rockgarden      "),
        };

        private static readonly SetInfo[] SetInfoApatoArray =
        {
            new SetInfo("apato skull     ", "1F04", "apato           "),
            new SetInfo("apato tail      ", "1F08", "apato           "),
            new SetInfo("apato torso     ", "1F0C", "apato           "),
        };

        private static readonly SetInfo[] SetInfoAppleArray =
        {
            new SetInfo("apple TV        ", "12F8", "apple           "),
            new SetInfo("apple clock     ", "1E44", "apple           "),
        };

        private static readonly SetInfo[] SetInfoBearArray =
        {
            new SetInfo("Baby bear       ", "10F0", "bear            "),
            new SetInfo("Mama bear       ", "10EC", "bear            "),
            new SetInfo("Papa bear       ", "10E8", "bear            "),
        };

        private static readonly SetInfo[] SetInfoBonsaiArray =
        {
            new SetInfo("pine bonsai     ", "13E4", "bonsai          "),
            new SetInfo("mugho bonsai    ", "13E8", "bonsai          "),
            new SetInfo("ponderosa bonsai", "13F0", "bonsai          "),
        };

        private static readonly SetInfo[] SetInfoCactusArray =
        {
            new SetInfo("tall cactus     ", "120C", "cactus          "),
            new SetInfo("round cactus    ", "1210", "cactus          "),
            new SetInfo("cactus          ", "13D8", "cactus          "),
        };

        private static readonly SetInfo[] SetInfoCitrusArray =
        {
            new SetInfo("orange chair    ", "12EC", "citrus          "),
            new SetInfo("lemon table     ", "12F4", "citrus          "),
            new SetInfo("grapefruit table", "13C0", "citrus          "),
            new SetInfo("lime chair      ", "13C4", "citrus          "),
        };

        private static readonly SetInfo[] SetInfoDaffodilArray =
        {
            new SetInfo("daffodil table  ", "1270", "daffodil        "),
            new SetInfo("daffodil chair  ", "1280", "daffodil        "),
        };

        private static readonly SetInfo[] SetInfoDrumArray =
        {
            new SetInfo("conga drum      ", "11D0", "drum            "),
            new SetInfo("timpano drum    ", "11F4", "drum            "),
            new SetInfo("djimbe drum     ", "133C", "drum            "),
        };

        private static readonly SetInfo[] SetInfoFigureArray =
        {
            new SetInfo("Keiko figurine  ", "1114", "figure          "),
            new SetInfo("Yuki figurine   ", "1118", "figure          "),
            new SetInfo("Yoko figurine   ", "111C", "figure          "),
            new SetInfo("Emi figurine    ", "1120", "figure          "),
            new SetInfo("Maki figurine   ", "1124", "figure          "),
            new SetInfo("Naomi figurine  ", "1128", "figure          "),
            new SetInfo("Aiko figurine   ", "1E38", "figure          "),
        };

        private static readonly SetInfo[] SetInfoFroggyArray =
        {
            new SetInfo("froggy chair    ", "10A4", "froggy          "),
            new SetInfo("lily-pad table  ", "10A8", "froggy          "),
        };

        private static readonly SetInfo[] SetInfoGuitarArray =
        {
            new SetInfo("folk guitar     ", "10D8", "guitar          "),
            new SetInfo("country guitar  ", "10DC", "guitar          "),
            new SetInfo("rock guitar     ", "10E0", "guitar          "),
        };

        private static readonly SetInfo[] SetInfoIrisArray =
        {
            new SetInfo("iris table      ", "1274", "iris            "),
            new SetInfo("iris chair      ", "1284", "iris            "),
        };

        private static readonly SetInfo[] SetInfoMammothArray =
        {
            new SetInfo("mammoth skull   ", "1F34", "mammoth         "),
            new SetInfo("mammoth torso   ", "1F38", "mammoth         "),
        };

        private static readonly SetInfo[] SetInfoMelonArray =
        {
            new SetInfo("watermelon chair", "1468", "melon           "),
            new SetInfo("melon chair     ", "146C", "melon           "),
            new SetInfo("watermelon table", "1470", "melon           "),
        };

        private static readonly SetInfo[] SetInfoOfficeArray =
        {
            new SetInfo("office locker   ", "100C", "office          "),
            new SetInfo("office desk     ", "11BC", "office          "),
            new SetInfo("office chair    ", "1234", "office          "),
        };

        private static readonly SetInfo[] SetInfoPearArray =
        {
            new SetInfo("pear wardrobe   ", "1028", "pear            "),
            new SetInfo("pear dresser    ", "1058", "pear            "),
        };

        private static readonly SetInfo[] SetInfoPineArray =
        {
            new SetInfo("pine table      ", "1294", "pine            "),
            new SetInfo("pine chair      ", "1298", "pine            "),
        };

        private static readonly SetInfo[] SetInfoPlesioArray =
        {
            new SetInfo("plesio skull    ", "1F28", "plesio          "),
            new SetInfo("plesio neck     ", "1F2C", "plesio          "),
            new SetInfo("plesio torso    ", "1F30", "plesio          "),
        };

        private static readonly SetInfo[] SetInfoStegoArray =
        {
            new SetInfo("stego skull     ", "1F10", "stego           "),
            new SetInfo("stego tail      ", "1F14", "stego           "),
            new SetInfo("stego torso     ", "1F18", "stego           "),
        };

        private static readonly SetInfo[] SetInfoStringsArray =
        {
            new SetInfo("violin          ", "1480", "strings         "),
            new SetInfo("bass            ", "1484", "strings         "),
            new SetInfo("cello           ", "1488", "strings         "),
        };

        private static readonly SetInfo[] SetInfoTRexArray =
        {
            new SetInfo("T-rex skull     ", "1EF8", "T-rex           "),
            new SetInfo("T-rex tail      ", "1EFC", "T-rex           "),
            new SetInfo("T-rex torso     ", "1F00", "T-rex           "),
        };

        private static readonly SetInfo[] SetInfoTriceraArray =
        {
            new SetInfo("tricera skull   ", "1EEC", "tricera         "),
            new SetInfo("tricera tail    ", "1EF0", "tricera         "),
            new SetInfo("tricera torso   ", "1EF4", "tricera         "),
        };

        private static readonly SetInfo[] SetInfoTulipArray =
        {
            new SetInfo("tulip table     ", "126C", "tulip           "),
            new SetInfo("tulip chair     ", "127C", "tulip           "),
        };

        private static readonly SetInfo[] SetInfoVaseArray =
        {
            new SetInfo("blue vase       ", "1278", "vase            "),
            new SetInfo("tea vase        ", "129C", "vase            "),
            new SetInfo("red vase        ", "12A0", "vase            "),
        };

        private static readonly SetInfo[] SetInfoWritingArray =
        {
            new SetInfo("writing desk    ", "1110", "writing         "),
            new SetInfo("globe           ", "112C", "writing         "),
            new SetInfo("writing chair   ", "1194", "writing         "),
        };

        private static readonly SetInfo[] UseableFurniturePoolArray =
        {
            new SetInfo("writing desk    ", "1110", "furniture       "),
            new SetInfo("blue wardrobe   ", "1008", "furniture       "),
            new SetInfo("office locker   ", "100C", "furniture       "),
            new SetInfo("regal armoire   ", "1014", "furniture       "),
            new SetInfo("cabana wardrobe ", "1018", "furniture       "),
            new SetInfo("cabin wardrobe  ", "101C", "furniture       "),
            new SetInfo("pear wardrobe   ", "1028", "furniture       "),
            new SetInfo("ranch wardrobe  ", "102C", "furniture       "),
            new SetInfo("blue cabinet    ", "1030", "furniture       "),
            new SetInfo("exotic wardrobe ", "1038", "furniture       "),
            new SetInfo("regal dresser   ", "1040", "furniture       "),
            new SetInfo("cabana dresser  ", "1044", "furniture       "),
            new SetInfo("lovely dresser  ", "104C", "furniture       "),
            new SetInfo("green dresser   ", "1054", "furniture       "),
            new SetInfo("pear dresser    ", "1058", "furniture       "),
            new SetInfo("blue bureau     ", "1064", "furniture       "),
            new SetInfo("modern dresser  ", "1068", "furniture       "),
            new SetInfo("exotic bureau   ", "106C", "furniture       "),
            new SetInfo("kiddie dresser  ", "1070", "furniture       "),
            new SetInfo("kiddie bureau   ", "1074", "furniture       "),
            new SetInfo("kiddie wardrobe ", "1078", "furniture       "),
            new SetInfo("froggy chair    ", "10A4", "furniture       "),
            new SetInfo("lily-pad table  ", "10A8", "furniture       "),
            new SetInfo("refrigerator    ", "10AC", "furniture       "),
            new SetInfo("red sofa        ", "10B8", "furniture       "),
            new SetInfo("red armchair    ", "10BC", "furniture       "),
            new SetInfo("stove           ", "10C4", "furniture       "),
            new SetInfo("cream sofa      ", "10C8", "furniture       "),
            new SetInfo("folk guitar     ", "10D8", "furniture       "),
            new SetInfo("country guitar  ", "10DC", "furniture       "),
            new SetInfo("rock guitar     ", "10E0", "furniture       "),
            new SetInfo("hinaningyo      ", "10E4", "furniture       "),
            new SetInfo("papa bear       ", "10E8", "furniture       "),
            new SetInfo("mama bear       ", "10EC", "furniture       "),
            new SetInfo("baby bear       ", "10F0", "furniture       "),
            new SetInfo("classic chair   ", "10F8", "furniture       "),
            new SetInfo("classic desk    ", "10FC", "furniture       "),
            new SetInfo("classic table   ", "1100", "furniture       "),
            new SetInfo("classic cabinet ", "1104", "furniture       "),
            new SetInfo("rocking chair   ", "1108", "furniture       "),
            new SetInfo("writing desk    ", "1110", "furniture       "),
            new SetInfo("keiko figurine  ", "1114", "furniture       "),
            new SetInfo("yuki figurine   ", "1118", "furniture       "),
            new SetInfo("yoko figurine   ", "111C", "furniture       "),
            new SetInfo("emi figurine    ", "1120", "furniture       "),
            new SetInfo("maki figurine   ", "1124", "furniture       "),
            new SetInfo("naomi figurine  ", "1128", "furniture       "),
            new SetInfo("globe           ", "112C", "furniture       "),
            new SetInfo("regal table     ", "1134", "furniture       "),
            new SetInfo("retro tv        ", "1138", "furniture       "),
            new SetInfo("eagle pole      ", "113C", "furniture       "),
            new SetInfo("raven pole      ", "1140", "furniture       "),
            new SetInfo("bear pole       ", "1144", "furniture       "),
            new SetInfo("taiko drum      ", "114C", "furniture       "),
            new SetInfo("space heater    ", "1150", "furniture       "),
            new SetInfo("retro stereo    ", "1154", "furniture       "),
            new SetInfo("classic sofa    ", "115C", "furniture       "),
            new SetInfo("lovely armchair ", "1164", "furniture       "),
            new SetInfo("lovely lamp     ", "116C", "furniture       "),
            new SetInfo("lovely chair    ", "1174", "furniture       "),
            new SetInfo("lovely bed      ", "1178", "furniture       "),
            new SetInfo("classic clock   ", "117C", "furniture       "),
            new SetInfo("cabana bed      ", "1180", "furniture       "),
            new SetInfo("green golf bag  ", "1184", "furniture       "),
            new SetInfo("white golf bag  ", "1188", "furniture       "),
            new SetInfo("blue golf bag   ", "118C", "furniture       "),
            new SetInfo("regal bookcase  ", "1190", "furniture       "),
            new SetInfo("writing chair   ", "1194", "furniture       "),
            new SetInfo("ranch couch     ", "1198", "furniture       "),
            new SetInfo("ranch armchair  ", "119C", "furniture       "),
            new SetInfo("ranch tea table ", "11A0", "furniture       "),
            new SetInfo("ranch bookcase  ", "11A8", "furniture       "),
            new SetInfo("ranch bed       ", "11B0", "furniture       "),
            new SetInfo("ranch table     ", "11B4", "furniture       "),
            new SetInfo("office desk     ", "11BC", "furniture       "),
            new SetInfo("vibraphone      ", "11C8", "furniture       "),
            new SetInfo("biwa lute       ", "11CC", "furniture       "),
            new SetInfo("conga drum      ", "11D0", "furniture       "),
            new SetInfo("extinguisher    ", "11D4", "furniture       "),
            new SetInfo("ruby econo-chair", "11D8", "furniture       "),
            new SetInfo("gold econo-chair", "11DC", "furniture       "),
            new SetInfo("jade econo-chair", "11E0", "furniture       "),
            new SetInfo("gold stereo     ", "11E4", "furniture       "),
            new SetInfo("folding chair   ", "11E8", "furniture       "),
            new SetInfo("lovely vanity   ", "11EC", "furniture       "),
            new SetInfo("birdcage        ", "11F0", "furniture       "),
            new SetInfo("tall cactus     ", "120C", "furniture       "),
            new SetInfo("round cactus    ", "1210", "furniture       "),
            new SetInfo("classic bed     ", "1214", "furniture       "),
            new SetInfo("wide-screen TV  ", "1218", "furniture       "),
            new SetInfo("lovely table    ", "121C", "furniture       "),
            new SetInfo("low lantern     ", "1228", "furniture       "),
            new SetInfo("tall lantern    ", "122C", "furniture       "),
            new SetInfo("pond lantern    ", "1230", "furniture       "),
            new SetInfo("office chair    ", "1234", "furniture       "),
            new SetInfo("cubby hole      ", "1238", "furniture       "),
            new SetInfo("letter cubby    ", "123C", "furniture       "),
            new SetInfo("science table   ", "124C", "furniture       "),
            new SetInfo("shrine lantern  ", "1254", "furniture       "),
            new SetInfo("barrel          ", "1258", "furniture       "),
            new SetInfo("keg             ", "125C", "furniture       "),
            new SetInfo("vaulting horse  ", "1260", "furniture       "),
            new SetInfo("glass-top table ", "1264", "furniture       "),
            new SetInfo("alarm clock     ", "1268", "furniture       "),
            new SetInfo("tulip table     ", "126C", "furniture       "),
            new SetInfo("daffodil table  ", "1270", "furniture       "),
            new SetInfo("iris table      ", "1274", "furniture       "),
            new SetInfo("blue vase       ", "1278", "furniture       "),
            new SetInfo("tulip chair     ", "127C", "furniture       "),
            new SetInfo("daffodil chair  ", "1280", "furniture       "),
            new SetInfo("iris chair      ", "1284", "furniture       "),
            new SetInfo("elephant slide  ", "1288", "furniture       "),
            new SetInfo("toilet          ", "128C", "furniture       "),
            new SetInfo("pine table      ", "1294", "furniture       "),
            new SetInfo("pine chair      ", "1298", "furniture       "),
            new SetInfo("tea vase        ", "129C", "furniture       "),
            new SetInfo("red vase        ", "12A0", "furniture       "),
            new SetInfo("sewing machine  ", "12A4", "furniture       "),
            new SetInfo("billiard table  ", "12A8", "furniture       "),
            new SetInfo("strange painting", "12D0", "furniture       "),
            new SetInfo("rare painting   ", "12D4", "furniture       "),
            new SetInfo("classic painting", "12D8", "furniture       "),
            new SetInfo("perfect painting", "12DC", "furniture       "),
            new SetInfo("fine painting   ", "12E0", "furniture       "),
            new SetInfo("worthy painting ", "12E4", "furniture       "),
            new SetInfo("pineapple bed   ", "12E8", "furniture       "),
            new SetInfo("orange chair    ", "12EC", "furniture       "),
            new SetInfo("lemon table     ", "12F4", "furniture       "),
            new SetInfo("apple tv        ", "12F8", "furniture       "),
            new SetInfo("table tennis    ", "12FC", "furniture       "),
            new SetInfo("harp            ", "1300", "furniture       "),
            new SetInfo("cabin clock     ", "1304", "furniture       "),
            new SetInfo("train set       ", "1308", "furniture       "),
            new SetInfo("water bird      ", "130C", "furniture       "),
            new SetInfo("wobbelina       ", "1310", "furniture       "),
            new SetInfo("slot machine    ", "1318", "furniture       "),
            new SetInfo("exotic bench    ", "131C", "furniture       "),
            new SetInfo("exotic chair    ", "1320", "furniture       "),
            new SetInfo("exotic chest    ", "1324", "furniture       "),
            new SetInfo("caladium        ", "132C", "furniture       "),
            new SetInfo("lady palm       ", "1330", "furniture       "),
            new SetInfo("exotic screen   ", "1334", "furniture       "),
            new SetInfo("exotic table    ", "1338", "furniture       "),
            new SetInfo("djimbe drum     ", "133C", "furniture       "),
            new SetInfo("modern bed      ", "1340", "furniture       "),
            new SetInfo("modern sofa     ", "1350", "furniture       "),
            new SetInfo("modern table    ", "1354", "furniture       "),
            new SetInfo("blue bed        ", "1358", "furniture       "),
            new SetInfo("blue bench      ", "135C", "furniture       "),
            new SetInfo("blue chair      ", "1360", "furniture       "),
            new SetInfo("blue bookcase   ", "1368", "furniture       "),
            new SetInfo("green bed       ", "1370", "furniture       "),
            new SetInfo("green bench     ", "1374", "furniture       "),
            new SetInfo("green chair     ", "1378", "furniture       "),
            new SetInfo("green counter   ", "1380", "furniture       "),
            new SetInfo("green lamp      ", "1384", "furniture       "),
            new SetInfo("green table     ", "1388", "furniture       "),
            new SetInfo("cabin bed       ", "138C", "furniture       "),
            new SetInfo("cabin couch     ", "1390", "furniture       "),
            new SetInfo("cabin armchair  ", "1394", "furniture       "),
            new SetInfo("cabin low table ", "139C", "furniture       "),
            new SetInfo("aloe            ", "13A0", "furniture       "),
            new SetInfo("bromeliaceae    ", "13A4", "furniture       "),
            new SetInfo("coconut palm    ", "13A8", "furniture       "),
            new SetInfo("snake plant     ", "13AC", "furniture       "),
            new SetInfo("rubber tree     ", "13B4", "furniture       "),
            new SetInfo("pothos          ", "13B8", "furniture       "),
            new SetInfo("fan palm        ", "13BC", "furniture       "),
            new SetInfo("grapefruit table", "13C0", "furniture       "),
            new SetInfo("lime chair      ", "13C4", "furniture       "),
            new SetInfo("weeping fig     ", "13C8", "furniture       "),
            new SetInfo("corn plant      ", "13CC", "furniture       "),
            new SetInfo("croton          ", "13D0", "furniture       "),
            new SetInfo("pachira         ", "13D4", "furniture       "),
            new SetInfo("cactus          ", "13D8", "furniture       "),
            new SetInfo("metronome       ", "13DC", "furniture       "),
            new SetInfo("pine bonsai     ", "13E4", "furniture       "),
            new SetInfo("mugho bonsai    ", "13E8", "furniture       "),
            new SetInfo("barber's pole   ", "13EC", "furniture       "),
            new SetInfo("ponderosa bonsai", "13F0", "furniture       "),
            new SetInfo("quince bonsai   ", "1404", "furniture       "),
            new SetInfo("azalea bonsai   ", "1408", "furniture       "),
            new SetInfo("jasmine bonsai  ", "140C", "furniture       "),
            new SetInfo("executive toy   ", "1410", "furniture       "),
            new SetInfo("traffic cone    ", "1414", "furniture       "),
            new SetInfo("orange cone     ", "141C", "furniture       "),
            new SetInfo("maple bonsai    ", "1424", "furniture       "),
            new SetInfo("hawthorne bonsai", "1428", "furniture       "),
            new SetInfo("holly bonsai    ", "142C", "furniture       "),
            new SetInfo("soda machine    ", "1440", "furniture       "),
            new SetInfo("manhole cover   ", "1444", "furniture       "),
            new SetInfo("green drum      ", "1450", "furniture       "),
            new SetInfo("iron frame      ", "145C", "furniture       "),
            new SetInfo("trash can       ", "1460", "furniture       "),
            new SetInfo("watermelon chair", "1468", "furniture       "),
            new SetInfo("melon chair     ", "146C", "furniture       "),
            new SetInfo("watermelon table", "1470", "furniture       "),
            new SetInfo("garbage can     ", "1478", "furniture       "),
            new SetInfo("trash bin       ", "147C", "furniture       "),
            new SetInfo("violin          ", "1480", "furniture       "),
            new SetInfo("cello           ", "1488", "furniture       "),
            new SetInfo("ebony piano     ", "148C", "furniture       "),
            new SetInfo("handcart        ", "1494", "furniture       "),
            new SetInfo("detour arrow    ", "14A4", "furniture       "),
            new SetInfo("garden stone    ", "14A8", "furniture       "),
            new SetInfo("standing stone  ", "14AC", "furniture       "),
            new SetInfo("spooky table    ", "14D8", "furniture       "),
            new SetInfo("lunar lander    ", "14DC", "furniture       "),
            new SetInfo("satellite       ", "14E0", "furniture       "),
            new SetInfo("mossy stone     ", "14E4", "furniture       "),
            new SetInfo("leaning stone   ", "14E8", "furniture       "),
            new SetInfo("dark stone      ", "14EC", "furniture       "),
            new SetInfo("flying saucer   ", "14F0", "furniture       "),
            new SetInfo("stone couple    ", "14F4", "furniture       "),
            new SetInfo("rocket          ", "14FC", "furniture       "),
            new SetInfo("spaceman sam    ", "1500", "furniture       "),
            new SetInfo("exotic bed      ", "150C", "furniture       "),
            new SetInfo("exotic end table", "1510", "furniture       "),
            new SetInfo("asteroid        ", "1514", "furniture       "),
            new SetInfo("cabana lamp     ", "1518", "furniture       "),
            new SetInfo("cabana table    ", "151C", "furniture       "),
            new SetInfo("scale           ", "1524", "furniture       "),
            new SetInfo("cabana screen   ", "1530", "furniture       "),
            new SetInfo("cabana vanity   ", "1534", "furniture       "),
            new SetInfo("cabana bookcase ", "153C", "furniture       "),
            new SetInfo("lunar rover     ", "1544", "furniture       "),
            new SetInfo("blue clock      ", "1554", "furniture       "),
            new SetInfo("mochi pestle    ", "1558", "furniture       "),
            new SetInfo("green desk      ", "1560", "furniture       "),
            new SetInfo("modern chair    ", "1568", "furniture       "),
            new SetInfo("space station   ", "1570", "furniture       "),
            new SetInfo("regal bed       ", "157C", "furniture       "),
            new SetInfo("space shuttle   ", "1580", "furniture       "),
            new SetInfo("regal vanity    ", "1584", "furniture       "),
            new SetInfo("regal sofa      ", "1588", "furniture       "),
            new SetInfo("regal lamp      ", "158C", "furniture       "),
            new SetInfo("cabin table     ", "1590", "furniture       "),
            new SetInfo("tea set         ", "159C", "furniture       "),
            new SetInfo("gerbera         ", "15A4", "furniture       "),
            new SetInfo("sunflower       ", "15A8", "furniture       "),
            new SetInfo("daffodil        ", "15AC", "furniture       "),
            new SetInfo("red boom box    ", "1E04", "furniture       "),
            new SetInfo("white boom box  ", "1E08", "furniture       "),
            new SetInfo("high-end stereo ", "1E0C", "furniture       "),
            new SetInfo("lovely stereo   ", "1E14", "furniture       "),
            new SetInfo("Jingle table    ", "1E30", "furniture       "),
            new SetInfo("phonograph      ", "1E38", "furniture       "),
            new SetInfo("dice stereo     ", "1E40", "furniture       "),
            new SetInfo("apple clock     ", "1E44", "furniture       "),
            new SetInfo("kitschy clock   ", "1E4C", "furniture       "),
            new SetInfo("antique clock   ", "1E50", "furniture       "),
            new SetInfo("reel-to-reel    ", "1E54", "furniture       "),
            new SetInfo("tape deck       ", "1E58", "furniture       "),
            new SetInfo("CD player       ", "1E5C", "furniture       "),
            new SetInfo("glow clock      ", "1E60", "furniture       "),
            new SetInfo("odd clock       ", "1E64", "furniture       "),
            new SetInfo("red clock       ", "1E68", "furniture       "),
            new SetInfo("cube clock      ", "1E6C", "furniture       "),
            new SetInfo("owl clock       ", "1E70", "furniture       "),
            new SetInfo("white pawn      ", "1EB4", "furniture       "),
            new SetInfo("black pawn      ", "1EB8", "furniture       "),
            new SetInfo("kiddie table    ", "1EC8", "furniture       "),
            new SetInfo("kiddie couch    ", "1ECC", "furniture       "),
            new SetInfo("kiddie chair    ", "1ED4", "furniture       "),
            new SetInfo("kiddie bookcase ", "1ED8", "furniture       "),
            new SetInfo("chalk board     ", "1EE4", "furniture       "),
            new SetInfo("tricera skull   ", "1EEC", "furniture       "),
            new SetInfo("tricera tail    ", "1EF0", "furniture       "),
            new SetInfo("tricera torso   ", "1EF4", "furniture       "),
            new SetInfo("T-rex skull     ", "1EF8", "furniture       "),
            new SetInfo("T-rex tail      ", "1EFC", "furniture       "),
            new SetInfo("T-rex torso     ", "1F00", "furniture       "),
            new SetInfo("apato skull     ", "1F04", "furniture       "),
            new SetInfo("apato tail      ", "1F08", "furniture       "),
            new SetInfo("apato torso     ", "1F0C", "furniture       "),
            new SetInfo("stego skull     ", "1F10", "furniture       "),
            new SetInfo("stego tail      ", "1F14", "furniture       "),
            new SetInfo("stego torso     ", "1F18", "furniture       "),
            new SetInfo("plesio skull    ", "1F28", "furniture       "),
            new SetInfo("plesio neck     ", "1F2C", "furniture       "),
            new SetInfo("plesio torso    ", "1F30", "furniture       "),
            new SetInfo("mammoth skull   ", "1F34", "furniture       "),
            new SetInfo("mammoth torso   ", "1F38", "furniture       "),
            new SetInfo("amber           ", "1F3C", "furniture       "),
            new SetInfo("dinosaur track  ", "1F40", "furniture       "),
            new SetInfo("ammonite        ", "1F44", "furniture       "),
            new SetInfo("dinosaur egg    ", "1F48", "furniture       "),
            new SetInfo("trilobite       ", "1F4C", "furniture       "),
            new SetInfo("modern lamp     ", "1F50", "furniture       "),
            new SetInfo("Snowman table   ", "1F58", "furniture       "),
            new SetInfo("Snowman bed     ", "1F5C", "furniture       "),
            new SetInfo("matryoshka      ", "1FB8", "furniture       "),
            new SetInfo("Mouth of Truth  ", "1FD8", "furniture       "),
            new SetInfo("red balloon     ", "1FF0", "furniture       "),
            new SetInfo("yellow balloon  ", "1FF4", "furniture       "),
            new SetInfo("blue balloon    ", "1FF8", "furniture       "),
            new SetInfo("green balloon   ", "1FFC", "furniture       "),
            new SetInfo("purple balloon  ", "3000", "furniture       "),
            new SetInfo("Bunny P. balloon", "3004", "furniture       "),
            new SetInfo("Bunny B. balloon", "3008", "furniture       "),
            new SetInfo("Bunny O. balloon", "300C", "furniture       "),
            new SetInfo("stone coin      ", "3018", "furniture       "),
            new SetInfo("mermaid statue  ", "301C", "furniture       "),
            new SetInfo("moon            ", "309C", "furniture       "),
            new SetInfo("grass model     ", "30E8", "furniture       "),
            new SetInfo("track model     ", "30EC", "furniture       "),
            new SetInfo("dirt model      ", "30F0", "furniture       "),
            new SetInfo("train car model ", "30F4", "furniture       "),
            new SetInfo("orange box      ", "30F8", "furniture       "),
            new SetInfo("fireplace       ", "31A0", "furniture       "),
            new SetInfo("wave breaker    ", "31C8", "furniture       "),
            new SetInfo("merge sign      ", "31EC", "furniture       "),
            new SetInfo("wet roadway sign", "31F4", "furniture       "),
            new SetInfo("detour sign     ", "31F8", "furniture       "),
            new SetInfo("men at work sign", "31FC", "furniture       "),
            new SetInfo("lefty desk      ", "3200", "furniture       "),
            new SetInfo("righty desk     ", "3204", "furniture       "),
            new SetInfo("school desk     ", "3208", "furniture       "),
            new SetInfo("flagman sign    ", "320C", "furniture       "),
            new SetInfo("jersey barrier  ", "3214", "furniture       "),
            new SetInfo("speed sign      ", "3218", "furniture       "),
            new SetInfo("teacher's desk  ", "3220", "furniture       "),
            new SetInfo("haz-mat barrel  ", "3224", "furniture       "),
            new SetInfo("saw horse       ", "322C", "furniture       "),
            new SetInfo("bug zapper      ", "3234", "furniture       "),
            new SetInfo("coffee machine  ", "323C", "furniture       "),
            new SetInfo("bird bath       ", "3240", "furniture       "),
            new SetInfo("barbecue        ", "3244", "furniture       "),
            new SetInfo("radiator        ", "3248", "furniture       "),
            new SetInfo("lawn chair      ", "324C", "furniture       "),
            new SetInfo("chess table     ", "3250", "furniture       "),
            new SetInfo("candy machine   ", "3254", "furniture       "),
            new SetInfo("backyard pool   ", "3258", "furniture       "),
            new SetInfo("jackhammer      ", "3260", "furniture       "),
            new SetInfo("tiki torch      ", "3264", "furniture       "),
            new SetInfo("birdhouse       ", "3268", "furniture       "),
            new SetInfo("potbelly stove  ", "326C", "furniture       "),
            new SetInfo("bus stop        ", "3270", "furniture       "),
            new SetInfo("flip-top desk   ", "3278", "furniture       "),
            new SetInfo("bird feeder     ", "3284", "furniture       "),
            new SetInfo("teacher's chair ", "3288", "furniture       "),
            new SetInfo("steam roller    ", "328C", "furniture       "),
            new SetInfo("hammock         ", "329C", "furniture       "),
            new SetInfo("tumbleweed      ", "32B0", "furniture       "),
            new SetInfo("storefront      ", "32D8", "furniture       "),
            new SetInfo("picnic table    ", "32DC", "furniture       "),
            new SetInfo("green pipe      ", "32F8", "furniture       "),
            new SetInfo("boxing barricade", "3338", "furniture       "),
            new SetInfo("red corner      ", "3340", "furniture       "),
            new SetInfo("blue corner     ", "3344", "furniture       "),
            new SetInfo("boxing mat      ", "3348", "furniture       "),
            new SetInfo("weight bench    ", "3358", "furniture       "),
            new SetInfo("bonfire         ", "3360", "furniture       "),
            new SetInfo("kayak           ", "3364", "furniture       "),
            new SetInfo("sprinkler       ", "3368", "furniture       "),
            new SetInfo("mountain bike   ", "33A8", "furniture       "),
            new SetInfo("sleeping bag    ", "33AC", "furniture       "),
        };

        private static readonly SetInfo[] CarpetsArray =
        {
            new SetInfo("plush carpet    ", "2600", "carpet          "),
            new SetInfo("classic carpet  ", "2601", "carpet          "),
            new SetInfo("checkered tile  ", "2602", "carpet          "),
            new SetInfo("old flooring    ", "2603", "carpet          "),
            new SetInfo("red tile        ", "2604", "carpet          "),
            new SetInfo("birch flooring  ", "2605", "carpet          "),
            new SetInfo("classroom floor ", "2606", "carpet          "),
            new SetInfo("lovely carpet   ", "2607", "carpet          "),
            new SetInfo("exotic rug      ", "2608", "carpet          "),
            new SetInfo("mossy carpet    ", "2609", "carpet          "),
            new SetInfo("18 mat tatami   ", "260A", "carpet          "),
            new SetInfo("8 mat tatami    ", "260B", "carpet          "),
            new SetInfo("citrus carpet   ", "260C", "carpet          "),
            new SetInfo("cabin rug       ", "260D", "carpet          "),
            new SetInfo("closed road     ", "260E", "carpet          "),
            new SetInfo("lunar surface   ", "260F", "carpet          "),
            new SetInfo("sand garden     ", "2610", "carpet          "),
            new SetInfo("spooky carpet   ", "2611", "carpet          "),
            new SetInfo("western desert  ", "2612", "carpet          "),
            new SetInfo("green rug       ", "2613", "carpet          "),
            new SetInfo("blue flooring   ", "2614", "carpet          "),
            new SetInfo("regal carpet    ", "2615", "carpet          "),
            new SetInfo("ranch flooring  ", "2616", "carpet          "),
            new SetInfo("modern tile     ", "2617", "carpet          "),
            new SetInfo("cabana flooring ", "2618", "carpet          "),
            new SetInfo("snowman carpet  ", "2619", "carpet          "),
            new SetInfo("backyard lawn   ", "261A", "carpet          "),
            new SetInfo("music room floor", "261B", "carpet          "),
            new SetInfo("plaza tile      ", "261C", "carpet          "),
            new SetInfo("kitchen tile    ", "261D", "carpet          "),
            new SetInfo("ornate rug      ", "261E", "carpet          "),
            new SetInfo("tatami floor    ", "261F", "carpet          "),
            new SetInfo("bamboo flooring ", "2620", "carpet          "),
            new SetInfo("kitchen flooring", "2621", "carpet          "),
            new SetInfo("charcoal tile   ", "2622", "carpet          "),
            new SetInfo("stone tile      ", "2623", "carpet          "),
            new SetInfo("imperial tile   ", "2624", "carpet          "),
            new SetInfo("opulent rug     ", "2625", "carpet          "),
            new SetInfo("slate flooring  ", "2626", "carpet          "),
            new SetInfo("ceramic tile    ", "2627", "carpet          "),
            new SetInfo("fancy carpet    ", "2628", "carpet          "),
            new SetInfo("cowhide rug     ", "2629", "carpet          "),
            new SetInfo("steel flooring  ", "262A", "carpet          "),
            new SetInfo("office flooring ", "262B", "carpet          "),
            new SetInfo("ancient tile    ", "262C", "carpet          "),
            new SetInfo("exquisite rug   ", "262D", "carpet          "),
            new SetInfo("sandlot         ", "262E", "carpet          "),
            new SetInfo("jingle carpet   ", "262F", "carpet          "),
            new SetInfo("daisy meadow    ", "2630", "carpet          "),
            new SetInfo("sidewalk        ", "2631", "carpet          "),
            new SetInfo("mosaic tile     ", "2632", "carpet          "),
            new SetInfo("parquet floor   ", "2633", "carpet          "),
            new SetInfo("basement floor  ", "2634", "carpet          "),
            new SetInfo("chessboard rug  ", "2635", "carpet          "),
            new SetInfo("kiddie carpet   ", "2636", "carpet          "),
            new SetInfo("shanty mat      ", "2637", "carpet          "),
            new SetInfo("concrete floor  ", "2638", "carpet          "),
            new SetInfo("saharah's desert", "2639", "carpet          "),
            new SetInfo("tartan rug      ", "263A", "carpet          "),
            new SetInfo("palace tile     ", "263B", "carpet          "),
            new SetInfo("tropical floor  ", "263C", "carpet          "),
            new SetInfo("playroom rug    ", "263D", "carpet          "),
            new SetInfo("kitschy tile    ", "263E", "carpet          "),
            new SetInfo("diner tile      ", "263F", "carpet          "),
            new SetInfo("block flooring  ", "2640", "carpet          "),
            new SetInfo("boxing ring mat ", "2641", "carpet          "),
            new SetInfo("harvest rug     ", "2642", "carpet          "),
        };

        private static readonly SetInfo[] WallpapersArray =
        {
            new SetInfo("classic wall    ", "2701", "wallpaper       "),
            new SetInfo("parlor wall     ", "2702", "wallpaper       "),
            new SetInfo("stone wall      ", "2703", "wallpaper       "),
            new SetInfo("blue-trim wall  ", "2704", "wallpaper       "),
            new SetInfo("plaster wall    ", "2705", "wallpaper       "),
            new SetInfo("classroom wall  ", "2706", "wallpaper       "),
            new SetInfo("lovely wall     ", "2707", "wallpaper       "),
            new SetInfo("exotic wall     ", "2708", "wallpaper       "),
            new SetInfo("mortar wall     ", "2709", "wallpaper       "),
            new SetInfo("gold screen wall", "270A", "wallpaper       "),
            new SetInfo("tea room wall   ", "270B", "wallpaper       "),
            new SetInfo("citrus wall     ", "270C", "wallpaper       "),
            new SetInfo("cabin wall      ", "270D", "wallpaper       "),
            new SetInfo("blue tarp       ", "270E", "wallpaper       "),
            new SetInfo("lunar horizon   ", "270F", "wallpaper       "),
            new SetInfo("garden wall     ", "2710", "wallpaper       "),
            new SetInfo("spooky wall     ", "2711", "wallpaper       "),
            new SetInfo("western vista   ", "2712", "wallpaper       "),
            new SetInfo("green wall      ", "2713", "wallpaper       "),
            new SetInfo("blue wall       ", "2714", "wallpaper       "),
            new SetInfo("regal wall      ", "2715", "wallpaper       "),
            new SetInfo("ranch wall      ", "2716", "wallpaper       "),
            new SetInfo("modern wall     ", "2717", "wallpaper       "),
            new SetInfo("cabana wall     ", "2718", "wallpaper       "),
            new SetInfo("snowman wall    ", "2719", "wallpaper       "),
            new SetInfo("backyard fence  ", "271A", "wallpaper       "),
            new SetInfo("music room wall ", "271B", "wallpaper       "),
            new SetInfo("plaza wall      ", "271C", "wallpaper       "),
            new SetInfo("lattice wall    ", "271D", "wallpaper       "),
            new SetInfo("ornate wall     ", "271E", "wallpaper       "),
            new SetInfo("modern screen   ", "271F", "wallpaper       "),
            new SetInfo("bamboo wall     ", "2720", "wallpaper       "),
            new SetInfo("kitchen wall    ", "2721", "wallpaper       "),
            new SetInfo("old brick wall  ", "2722", "wallpaper       "),
            new SetInfo("stately wall    ", "2723", "wallpaper       "),
            new SetInfo("imperial wall   ", "2724", "wallpaper       "),
            new SetInfo("manor wall      ", "2725", "wallpaper       "),
            new SetInfo("ivy wall        ", "2726", "wallpaper       "),
            new SetInfo("mod wall        ", "2727", "wallpaper       "),
            new SetInfo("rose wall       ", "2728", "wallpaper       "),
            new SetInfo("wood paneling   ", "2729", "wallpaper       "),
            new SetInfo("concrete wall   ", "272A", "wallpaper       "),
            new SetInfo("office wall     ", "272B", "wallpaper       "),
            new SetInfo("ancient wall    ", "272C", "wallpaper       "),
            new SetInfo("exquisite wall  ", "272D", "wallpaper       "),
            new SetInfo("sandlot wall    ", "272E", "wallpaper       "),
            new SetInfo("jingle wall     ", "272F", "wallpaper       "),
            new SetInfo("meadow vista    ", "2730", "wallpaper       "),
            new SetInfo("tree-lined wall ", "2731", "wallpaper       "),
            new SetInfo("mosaic wall     ", "2732", "wallpaper       "),
            new SetInfo("arched window   ", "2733", "wallpaper       "),
            new SetInfo("basement wall   ", "2734", "wallpaper       "),
            new SetInfo("backgammon wall ", "2735", "wallpaper       "),
            new SetInfo("kiddie wall     ", "2736", "wallpaper       "),
            new SetInfo("shanty wall     ", "2737", "wallpaper       "),
            new SetInfo("industrial wall ", "2738", "wallpaper       "),
            new SetInfo("desert vista    ", "2739", "wallpaper       "),
            new SetInfo("library wall    ", "273A", "wallpaper       "),
            new SetInfo("floral wall     ", "273B", "wallpaper       "),
            new SetInfo("tropical vista  ", "273C", "wallpaper       "),
            new SetInfo("playroom wall   ", "273D", "wallpaper       "),
            new SetInfo("kitschy wall    ", "273E", "wallpaper       "),
            new SetInfo("groovy wall     ", "273F", "wallpaper       "),
            new SetInfo("mushroom mural  ", "2740", "wallpaper       "),
            new SetInfo("ringside seating", "2741", "wallpaper       "),
            new SetInfo("harvest wall    ", "2742", "wallpaper       "),
            new SetInfo("harvest rug     ", "2642", "wallpaper       "),
        };

        private static readonly string[] AddressArray =
        {
            "0007148A",
            "00077AD2",
            "00077AD6",
            "00077AEA",
            "00077AEE",
            "0007C48A",
            "0007C492",
            "0007CD5E",
            "0007D8AE",
            "0007D8C6",
            "001620EE",
            "0016211A",
            "002D9BE0",
            "002D9BE2",
            "002D9BE4",
            "002D9BE6",
            "002D9BE8",
            "002D9BEA",
            "002D9BEC",
            "002D9BEE",
            "002D9BF0",
            "002D9BF2",
            "002D9BF4",
            "002D9BF6",
            "002E64BB",
            "002E64CB",
            "002E64DB",
            "002E64EB",
            "002EDA56",
            "002EDA58",
            "002EDA5A",
            "002EDA5E",
            "002EDA60",
            "002EDA62",
            "002EDA64",
            "002EDA66",
            "002EDA68",
            "002EDA6A",
            "002EDA6C",
            "002EDA70",
            "002EDA72",
            "002EDA74",
            "002EDA76",
            "002EDA78",
            "002EDA7A",
            "002EDA7C",
            "002EDA7E",
            "002EDA80",
            "002EDA82",
            "002EDA84",
            "002EDA88",
            "002EDA8A",
            "0031E770",
            "0031E772",
            "0031E774",
            "0031E776",
            "0031E778",
            "0031E77A",
            "0031E77C",
            "0031E77E",
            "0031E780",
            "0031E782",
            "0031E784",
            "0031E786",
            "009086A0",
            "009086A2",
            "009086A4",
            "009086A6",
            "009086A8",
            "009086AA",
            "009086AC",
            "009086AE",
            "009086B0",
            "009086B2",
            "009086B4",
            "009086B6",
            "009086B8",
            "009086BA",
            "009086BC",
            "009086BE",
            "009086C0",
            "009086C2",
            "00975560",
            "00975562",
            "00975564",
            "00975566",
            "00975568",
            "0097556A",
            "0097556C",
            "0097556E",
            "00975570",
            "00975572",
            "00975574",
            "00975576",
            "00975578",
            "0097557A",
            "0097557C",
            "0097557E",
            "00975580",
            "00975582",
            "00975584",
            "00975586",
            "00975588",
            "0097558A",
            "0097558C",
            "0097558E",
            "00975590",
            "00975592",
            "00975594",
            "00975596",
            "00975598",
            "0097559A",
            "0097559C",
            "0097559E",
            "009755A0",
            "009755A2",
            "009755A4",
            "009755A6",
            "009755A8",
            "009755AA",
            "009755AC",
            "009755AE",
            "009755B0",
            "009755B2",
            "009755B4",
            "009755B6",
            "009755B8",
            "009755BA",
            "009755BC",
            "009755BE",
            "009755C0",
            "009755C2",
            "009755C4",
            "009755C6",
            "009755C8",
            "009755CA",
            "009755CC",
            "009755CE",
            "009755D0",
            "009755D2",
            "009755D4",
            "009755D6",
            "009755D8",
            "009755DA",
            "009755DC",
            "009755DE",
            "009755E0",
            "009755E2",
            "009755E4",
            "009755E6",
            "009755E8",
            "009755EA",
            "009755EC",
            "009755EE",
            "009755F0",
            "009755F2",
            "009755F4",
            "009755F6",
            "009755F8",
            "009755FA",
            "009755FC",
            "009755FE",
            "00975600",
            "00975602",
            "00975604",
            "00975606",
            "00975608",
            "0097560A",
            "0097560C",
            "0097560E",
            "00975610",
            "00975612",
            "00975614",
            "00975616",
            "00975618",
            "0097561A",
            "0097561C",
            "0097561E",
            "00975620",
            "00975622",
            "00975624",
            "00975626",
            "00975628",
            "0097562A",
            "0097562C",
            "00975630",
            "00975632",
            "00975634",
            "00975636",
            "00975638",
            "0097563A",
            "0097563C",
            "0097563E",
            "00975640",
            "00975642",
            "00975644",
            "00975646",
            "00975648",
            "0097564A",
            "0097564C",
            "0097564E",
            "00975650",
            "00975652",
            "00975654",
            "00975656",
            "00975658",
            "0097565A",
            "0097565C",
            "0097565E",
            "00975660",
            "00975662",
            "00975664",
            "00975666",
            "00975668",
            "0097566A",
            "0097566C",
            "0097566E",
            "00975670",
            "00975672",
            "00975674",
            "00975676",
            "00975678",
            "0097567A",
            "0097567C",
            "0097567E",
            "00975680",
            "00975682",
            "00975684",
            "00975686",
            "00975688",
            "0097568A",
            "0097568C",
            "0097568E",
            "00975690",
            "00975692",
            "00975694",
            "00975696",
            "00975698",
            "0097569A",
            "0097569C",
            "0097569E",
            "009756A0",
            "009756A2",
            "009756A4",
            "009756A6",
            "009756A8",
            "009756AA",
            "009756AC",
            "009756AE",
            "009756B0",
            "009756B2",
            "009756B4",
            "009756B6",
            "009756B8",
            "009756BA",
            "009756BC",
            "009756BE",
            "009756C0",
            "009756C2",
            "009756C4",
            "009756C6",
            "009756C8",
            "009756CA",
            "009756CC",
            "009756CE",
            "009756D0",
            "009756D2",
            "009756D4",
            "009756D6",
            "009756D8",
            "009756DA",
            "009756DC",
            "009756DE",
            "009756E0",
            "009756E2",
            "009756E4",
            "009756E6",
            "009756E8",
            "009756EA",
            "009756EC",
            "009756EE",
            "009756F0",
            "009756F2",
            "009756F4",
            "009756F6",
            "009756F8",
            "009756FA",
            "009756FC",
            "009756FE",
            "00975704",
            "00975706",
            "00975708",
            "0097570A",
            "0097570C",
            "0097570E",
            "00975710",
            "00975712",
            "00975714",
            "00975716",
            "00975718",
            "0097571A",
            "0097571C",
            "0097571E",
            "00975720",
            "00975722",
            "00975724",
            "00975726",
            "00975728",
            "0097572A",
            "0097572C",
            "0097572E",
            "00975730",
            "00975732",
            "00975734",
            "00975736",
            "00975738",
            "0097573A",
            "0097573C",
            "0097573E",
            "00975740",
            "00975742",
            "00975744",
            "00975746",
            "00975748",
            "0097574A",
            "0097574C",
            "0097574E",
            "00975750",
            "00975752",
            "00975754",
            "00975756",
            "00975758",
            "0097575A",
            "0097575C",
            "0097575E",
            "00975760",
            "00975762",
            "00975764",
            "00975766",
            "00975768",
            "0097576A",
            "0097576C",
            "0097576E",
            "00975770",
            "00975772",
            "00975774",
            "00975776",
            "00975778",
            "0097577A",
            "0097577C",
            "0097577E",
            "00975780",
            "00975782",
            "00975784",
            "00975786",
            "00975788",
            "0097578A",
            "0097578C",
            "0097578E",
            "00975790",
            "00975792",
            "00975794",
            "00975796",
            "00975798",
            "0097579A",
            "0097579C",
            "0097579E",
            "009757A0",
            "009757A2",
            "009757A4",
            "009757A6",
            "009757A8",
            "009757AA",
            "009757AC",
            "009757AE",
            "009757B0",
            "009757B2",
            "009757B4",
            "009757B6",
            "009757B8",
            "009757BA",
            "009757BC",
            "009757BE",
            "009757C0",
            "009757C2",
            "009757C4",
            "009757C6",
            "009757C8",
            "009757CA",
            "009757CC",
            "009757CE",
            "009757D0",
            "009757D4",
            "009757D6",
            "009757D8",
            "009757DA",
            "009757DC",
            "009757DE",
            "009757E0",
            "009757E2",
            "009757E4",
            "009757E6",
            "009757E8",
            "009757EA",
            "009757EC",
            "009757EE",
            "009757F0",
            "009757F2",
            "009757F4",
            "009757F6",
            "009757F8",
            "009757FA",
            "009757FC",
            "009757FE",
            "00975800",
            "00975802",
            "00975804",
            "00975806",
            "00975808",
            "0097580A",
            "0097580C",
            "0097580E",
            "00975810",
            "00975812",
            "00975814",
            "00975816",
            "00975818",
            "0097581A",
            "0097581C",
            "0097581E",
            "00975820",
            "00975822",
            "00975824",
            "00975826",
            "00975828",
            "0097582A",
            "0097582C",
            "0097582E",
            "00975830",
            "00975832",
            "00975834",
            "00975836",
            "00975838",
            "0097583A",
            "0097583C",
            "0097583E",
            "00975840",
            "00975842",
            "00975844",
            "00975846",
            "00975848",
            "0097584A",
            "0097584C",
            "0097584E",
            "00975850",
            "00975852",
            "00975854",
            "00975856",
            "00975858",
            "00975868",
            "0097586A",
            "0097586C",
            "0097586E",
            "00975870",
            "00975872",
            "00975874",
            "00975876",
            "00975878",
            "0097587A",
            "0097587C",
            "0097587E",
            "00975880",
            "00975882",
            "00975884",
            "00975886",
            "00975888",
            "0097588A",
            "0097588C",
            "0097588E",
            "00975890",
            "00975892",
            "00975894",
            "00975896",
            "00975898",
            "0097589A",
            "0097589C",
            "0097589E",
            "009758A0",
            "009758A2",
            "009758A4",
            "009758A6",
            "009758A8",
            "009758AA",
            "009758AC",
            "009758AE",
            "009758B4",
            "009758B6",
            "009758B8",
            "009758BA",
            "009758BC",
            "009758BE",
            "009758C0",
            "009758C2",
            "009758C4",
            "009758C6",
            "009758D0",
            "009758D2",
            "009758D4",
            "009758D6",
            "009758D8",
            "009758DA",
            "009758DC",
            "009758DE",
            "009758E0",
            "009758E2",
            "00975900",
            "00975902",
            "00975908",
            "0097590A",
            "0097590C",
            "0097590E",
            "00975910",
            "00975912",
            "00975914",
            "00975916",
            "00975918",
            "0097591A",
            "0097591C",
            "0097591E",
            "00975920",
            "00975922",
            "00975924",
            "00975926",
            "00975928",
            "0097592A",
            "0097592C",
            "0097592E",
            "0097598C",
            "0097598E",
            "00975990",
            "00975992",
            "00975994",
            "00975996",
            "00975998",
            "0097599A",
            "0097599C",
            "009759C4",
            "009759C6",
            "009759C8",
            "009759CA",
            "009759CC",
            "009759CE",
            "009759D0",
            "009759D2",
            "009759D4",
            "009759D6",
            "00DA5710",
            "00DA5712",
            "00DA5714",
            "00DA5716",
            "00DA5718",
            "00DA571A",
            "00DA571C",
            "00DA571E",
            "00DA5720",
            "00DA5722",
            "00DA5724",
            "00DA5726",
            "00DA5728",
            "00DA572A",
            "00DA572C",
            "00DA572E",
            "00DA5730",
            "00DA5732",
        };

        private static readonly ModInfo[] ModInfoArray =
        {
            new ModInfo("Remove Fossil Identification    ", "2513", "000569AA", "fossil          "),
            new ModInfo("New Museum Model                ", "313C", "000569AA", "musem           "),
            new ModInfo("Remove Gyroids                  ", "2000", "0007A89A", "gyroid          "),
            new ModInfo("Remove Station Models           ", "0440", "0007BC56", "holiday         "),
            new ModInfo("Remove Flower Models            ", "0440", "0007C4C6", "holiday         "),
            new ModInfo("New Manor Model                 ", "15A0", "0016211A", "goal            "),
            new ModInfo("New Piggy Bank                  ", "3140", "002E64CB", "post            "),
            new ModInfo("New Mailbox                     ", "3144", "002E64DB", "post            "),
            new ModInfo("New Post Model                  ", "3148", "002E64EB", "post            "),
            //new ModInfo("Faster Running                  ", "2000", "0007A89A", "gyroid          "),
            //new ModInfo("First Bite Fishing              ", "2000", "0007A89A", "fish            "),
        };
    }
}
