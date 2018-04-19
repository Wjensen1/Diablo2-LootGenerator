using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace CommandLine
{
	public class LootGenerator
	{
		public static LootGenerator instance = new LootGenerator();

		static string smallDataPath = "small_data\\";
		static string largeDataPath = "large_data\\";

		static string armorPath = "armor.txt";
		static string magicPrefixPath = "MagicPrefix.txt";
		static string magicSuffixPath = "MagicSuffix.txt";
		static string monsterStatsPath = "Monstats.txt";
		static string treasureClassPath = "TreasureClassEx.txt";

		public static string treasureClassKey = "TreasureClass";
		public static string nameKey = "name";
		public static string minACKey = "minac";
		public static string maxACKey = "maxac";
		public static string mod1CodeKEy = "mod1code";
		public static string mod1MinKey = "mod1min";
		public static string mod1MaxKey = "mod1max";

		public string[,] armors;
		public string[,] magicPrefixes;
		public string[,] magicSuffixes;
		public string[,] monsterStats;
		public string[,] treasureClasses;

		public Dictionary<string, int> armorDict = new Dictionary<string, int>();
		public Dictionary<string, int> magicPrefixDict = new Dictionary<string, int>();
		public Dictionary<string, int> magicSuffixDict = new Dictionary<string, int>();
		public Dictionary<string, int> monsterStatsDict = new Dictionary<string, int>();
		public Dictionary<string, int> treasureClassDict = new Dictionary<string, int>();

		Random r = new Random();


		//run on execute
		public static void Main()
		{
			//read in data from .txt files and store as arrays
			instance.GetData();

			bool generateLoot = true;
			while (generateLoot == true)
			{
				generateLoot = instance.YesOrNo("Generate Loot?");
				if (generateLoot == true)
				{
					instance.GenerateLoot();
				}
			}

			Console.WriteLine("END");
		}

		//fill data arrays and dictionaries from text files
		void GetData()
		{
			string path;
			string data;

			//fill each 2d array and corresponding dictionary
			//get path for file
			//get text data from file
			//convert data to array
			//create dictionary for data

			//get armor data
			path = smallDataPath + armorPath;
			data = System.IO.File.ReadAllText(path);
			armors = ConvertToStringArray(data,1);
			armorDict = GenerateDictionary(data, 0);
			//get magic prefix data
			path = smallDataPath + magicPrefixPath;
			data = System.IO.File.ReadAllText(path);
			magicPrefixes = ConvertToStringArray(data, 2);
			magicPrefixDict = GenerateDictionary(data, 1);
			//get magic suffix data
			path = smallDataPath + magicSuffixPath;
			data = System.IO.File.ReadAllText(path);
			magicSuffixes = ConvertToStringArray(data, 2);
			magicSuffixDict = GenerateDictionary(data, 1);
			//get monster stat data
			path = smallDataPath + monsterStatsPath;
			data = System.IO.File.ReadAllText(path);
			monsterStats = ConvertToStringArray(data, 2);
			monsterStatsDict = GenerateDictionary(data, 1);
			//get treasure class data
			path = smallDataPath + treasureClassPath;
			data = System.IO.File.ReadAllText(path);
			treasureClasses = ConvertToStringArray(data, 1);
			treasureClassDict = GenerateDictionary(data, 0);
		}

		//generate an Dictionary from the row at the given y index
		//dictionary will store the data in the cells as the keys and their indexes as the value
		Dictionary<string, int> GenerateDictionary(string input, int yIndex = 0, char lineBreak = '\r', char unitBreak = '\t')
		{
			//remove any extra characters from the string
			//string extra = "\r";
			//input = input.Replace(extra, "");

			Dictionary<string, int> output = new Dictionary<string, int>();

			string[] rows = input.Split(lineBreak);
			string[] dictRow = rows[yIndex].Split(unitBreak);

			for(int i = 0;i < dictRow.Length; i++)
			{
				output.Add(dictRow[i], i);
			}
			return output;
		}

		//converts the inputed string into an array
		string[,] ConvertToStringArray(string input, int initialYIndex = 0, char lineBreak = '\r', char unitBreak = '\t')
		{
			//romve any extra characters from the string
			//string extra = "\r";
			//input = input.Replace(extra, "");

			int top = initialYIndex - 1;
			if (top < 0)
			{
				top = 0;
			}

			string[] rows = input.Split(lineBreak);
			string[] topRow = rows[top].Split(unitBreak);
			int xLength = topRow.Length;
			int yLength = rows.Length - initialYIndex;
			string[,] output = new string[xLength,yLength];
			for (int y = 0; y < yLength; y++)
			{
				string[] splitRow = rows[y + initialYIndex].Split(unitBreak);
				for(int x = 0; x < xLength; x++)
				{
					output[x, y] = splitRow[x];
				}
			}
			return output;
		}

		bool YesOrNo(string question)
		{
			bool output = false;
			bool responded = false;
			while (responded == false)
			{
				Console.WriteLine(question + " [y/n]");
				string response = Console.ReadLine();
				if (response == "y" || response == "Y")
				{
					//return bool true
					output = true;
					responded = true;
				}
				else if (response == "n" || response == "N")
				{
					output = false;
					responded = true;
				}
				else
				{
					Console.WriteLine("INVALID RESPONSE");
				}
			}
			return output;
		}

		void GenerateLoot()
		{
			//pick random monster from monster.txt
			//extract monster's tc from monster.txt
			string monsterTC = PickMonster();
			//generate base item found in treasureClassEx.txt iwth inputed monster's tc
			//string itemName = GetRandomItem(monsterTC);
			Item item = GetRandomItem(monsterTC);
			//generate base stats found in armor.txt for base item
			item = GenerateItemStats(item);
			//randomly choose affix from MagicPrefix.txt/{MagicSuffix.txt

			//generate the stats for the chosen affix from MagicPrefix.txt/MagicSuffix.txt

			Console.WriteLine(item.data);
			Console.WriteLine();
		}

		//picks random moster, logs to console, outputs monster's treasureClass
		string PickMonster()
		{
			//pick random monster
			int index = r.Next(0,monsterStats.GetLength(1)-1);
			//print monster
			string name = monsterStats[monsterStatsDict[nameKey], index];
			Console.WriteLine();
			Console.WriteLine(name + " dropped:");
			Console.WriteLine("=====");

			//output monsters treasure class
			string monsterTC = monsterStats[monsterStatsDict[treasureClassKey], index];
			return monsterTC;
		}

		//returns tuple with itemTreasureClass and item
		Item GetRandomItem(string monsterTreasureClass)
		{
			string itemTreasureClass = "";
			string item = monsterTreasureClass;
			//loop until it returns an item instead of a treasureClass
			while (item.Substring(0, 3) == "tc:")
			{
				itemTreasureClass = item;
				int yIndex = GetYIndex(item, treasureClasses, treasureClassDict[treasureClassKey]);
				int xIndex = r.Next(1, treasureClasses.GetLength(0));
				item = treasureClasses[xIndex, yIndex];
			}
			Item output = new Item(item, itemTreasureClass);
			return output;
		}

		public static int GetYIndex(string inputName, string[,] data, int xIndex = 0)
		{
			int output = -1;

			for (int y = 0; y < data.GetLength(1); y++)
			{
				if (inputName == data[xIndex, y])
				{
					output = y;
					break;
				}
			}
			if (output == -1)
			{
				Console.WriteLine("Didn't find the treasureClass: " + inputName);
			}
			return output;
		}

		Item GenerateItemStats(Item inputItem)
		{
			Item output = null;
			switch (inputItem.treasureClass.Substring(0, 7))
			{ 
				case "tc:armo":
					output = new Armor(inputItem.name, inputItem.treasureClass);
					break;
				case "tc:weap":
					output = new Weapon(inputItem.name, inputItem.treasureClass);
					break;
			}
			return output;
		}

		string GenerateArmorStats(string itemName)
		{
			int itemIndex = GetYIndex(itemName, armors, armorDict[nameKey]);
			int minAC = Convert.ToInt32(armors[armorDict[minACKey], itemIndex]);
			int maxAC = Convert.ToInt32(armors[armorDict[maxACKey], itemIndex]);
			int ac = r.Next(minAC, maxAC);
			string output = "Defense: " + ac;
			return output;
		}
	}

	public class Item
	{
		public string prefix = "";
		public string suffix = "";
		public string name { get; private set; }
		public string treasureClass { get; private set; }
		public string data;

		//constructor
		public Item(string inputName, string inputTreasureClass)
		{
			name = inputName;
			treasureClass = inputTreasureClass;
		}
		
		void GenerateAffix()
		{

		}

		void SetData()
		{

		}
	}

	public class Weapon : Item
	{

		public Weapon(string inputName, string inputTreasureClass):base(inputName, inputTreasureClass)
		{
			//Generate base stats by name
			GenerateStats();
			SetData();
		}

		void GenerateStats()
		{

		}

		void SetData()
		{

		}
	}

	public class Armor : Item
	{
		public int ac { get; private set; }
		public int durability { get; private set; }

		public Armor(string inputName, string inputTreasureClass): base(inputName,inputTreasureClass)
		{
			//Generate base stats by name
			GenerateStats();
			SetData();
		}

		void GenerateStats()
		{
			int itemIndex = LootGenerator.GetYIndex(name, LootGenerator.instance.armors, LootGenerator.instance.armorDict[LootGenerator.nameKey]);
			int minAC = Convert.ToInt32(LootGenerator.instance.armors[LootGenerator.instance.armorDict[LootGenerator.minACKey], itemIndex]);
			int maxAC = Convert.ToInt32(LootGenerator.instance.armors[LootGenerator.instance.armorDict[LootGenerator.maxACKey], itemIndex]);
			Random r = new Random();
			ac = r.Next(minAC, maxAC);
		}

		void SetData()
		{
			data = prefix + name+ suffix + '\n' + "Defense: " + ac;
		}
	}
}
