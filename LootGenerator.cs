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

		public Dictionary<string, int> armorDict;
		public Dictionary<string, int> magicPrefixDict;
		public Dictionary<string, int> magicSuffixDict;
		public Dictionary<string, int> monsterStatsDict;
		public Dictionary<string, int> treasureClassDict;


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
			monsterStats = ConvertToStringArray(data, 2);
			monsterStatsDict = GenerateDictionary(data, 1);
			//get treasure class data
			data = System.IO.File.ReadAllText(path);
			path = smallDataPath + treasureClassPath;
			treasureClasses = ConvertToStringArray(data, 1);
			treasureClassDict = GenerateDictionary(data, 0);
		}

		//generate an Dictionary from the row at the given y index
		//dictionary will store the data in the cells as the keys and their indexes as the value
		Dictionary<string, int> GenerateDictionary(string[,] input, int yIndex = 0, char lineBreak = '\n', char unitBreak = '\t')
		{
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
		string[,] ConvertToStringArray(string input, int initialYIndex = 0, char lineBreak = '\n', char unitBreak = '\t')
		{
			int top = initialYIndex - 1;
			if (top < 0)
			{
				top = 0;
			}

			string[] rows = input.Split(lineBreak);
			string[] topRow = rows[top];
			int xLength = topRow.Length;
			int yLength = rows.Length - initialYIndex;
			string[,] output = new string[xLength,yLength];

			for(int y = 0; y< yLength; y++)
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
			//generate base item found in treasureClassEx.txt iwth inputed monster's tc
			//generate base stats found in armor.txt for base item
			//randomly choose affix from MagicPrefix.txt/{MagicSuffix.txt
			//generate the stats for the chosen affix from MagicPrefix.txt/MagicSuffix.txt

			Console.WriteLine("Loot Generated");
		}
	}
}
