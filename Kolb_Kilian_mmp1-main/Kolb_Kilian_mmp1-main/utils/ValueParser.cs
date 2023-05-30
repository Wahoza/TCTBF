using System;
using System.IO;

public static class ValueParser
{
    public static int ParseGameState(int linePosition){
        //line Position: 0 == Gamestate (0 == Firstload, 2 == Shopping State, 3 == Finished, but Freeplay)
        //               1 == Money currently avadible to Player
        //               2 == Upgrade of Time before movement stops
        //               3 == Upgrade of Vision
        //               4 == Upgrade of Bait (or weight tbd)

        int returnValue;
        bool succes;

        string filePath = "ParsedData/Parsed.txt";
        try{
            string[] lines = File.ReadAllLines(filePath);
            succes = int.TryParse(lines[linePosition], out returnValue);
            if(succes){
                return returnValue;
            }else{
                lines[linePosition] = "0";
                File.WriteAllLines(filePath, lines);
                return 0;
            }
        }catch(Exception e){
            Console.WriteLine(e);
            string[] lines = new string[]{"0", "0", "0", "0", "0"};
            File.WriteAllLines(filePath, lines);
            return 0;
        }
        
    }
}