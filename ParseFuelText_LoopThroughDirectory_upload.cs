//
//  Title: ParseFuelText.cs
//  Author: Michael Harris (City of Bloomington, MN)
//  Date: 01/18/2018
//  Summary: A program to parse FuelMaster Plus fuel transactions ('2500 Plus Classic Match' format)
//    for importing into FleetFocus ('Automated Fuel Ticket' screen). The script essentially adds a space
//    character before the 8-digit equipment ID so that Fleet Focus is able to parse this text and apply
//    the fuel transactions to the proper equipment.
// 

using System;
using System.Text;
using System.IO;
using System.Collections.Generic;

namespace ParseFuelText
{
    class ParseFuelText
    {
        static void Main() 
        {
			int counter = 0;
			int firstSplit = 0;
			string line;
			string newText;
			List<string> outputTextList = new List<string>();

			// Update these if the server changes or when the new year starts
			string basePath = "Y:";
			string year = "test2018";
			
			try
			{
				string fullPath = basePath + "\\" + year + "\\";
				string outputName;
				
				string[] textFilesList = Directory.GetFiles(fullPath, "*.txt");
				foreach (string name in textFilesList)
				{
					StreamReader file = new StreamReader(name);
					outputTextList.Clear();
					outputName = fullPath + "Processed\\proc_" + Path.GetFileName(name);
					
					// Check to see if this file has been processed previously
					// If so, skip all processing
					if ( File.Exists(outputName) == true )
					{
						System.Console.WriteLine(Path.GetFileName(outputName) + (" Not processed (already exists)"));
						continue;
					}
					
					// Read the file and display it line by line
					while((line = file.ReadLine()) != null )
					{
						newText = line;
						firstSplit = 0;
						
						// Split text at N character, add's a space character
						StringBuilder sb = new StringBuilder();
						for (int i = 0; i < newText.Length; i++) 
						{
							if ( i % 25 == 0 && i != 0 )
							{
								if ( firstSplit == 0 )
								{
									sb.Append(' ');
									firstSplit = 1;
								}
							}
							sb.Append(newText[i]);
						}
						outputTextList.Add(sb.ToString());
						counter++;
					}
					System.Console.WriteLine(Path.GetFileName(outputName) + " Processed");
					
					// Write to output text file
					using (StreamWriter outputText = new StreamWriter(outputName))
					{
						for ( int j = 0; j < outputTextList.Count; j++) 
						{
							outputText.WriteLine(outputTextList[j]);
						}
					}
					file.Close();
				}
				// Suspend the screen.
				System.Console.WriteLine("\n***Press ENTER to exit.***");
				System.Console.ReadLine();
			}
			
			catch ( DirectoryNotFoundException dirEx )
			{
				System.Console.WriteLine(basePath + " does not exist.  Please have IS map your Y: drive.\n" + 
				System.Console.WriteLine(dirEx);
				System.Console.WriteLine("\n***Press ENTER to exit.***");
				System.Console.ReadLine();
			}
			
			catch ( Exception ex )
			{
				System.Console.WriteLine("Error: Please see Michael Harris to correct this error.\n\n");
				System.Console.WriteLine(ex);
			}
			
        }
    }
}