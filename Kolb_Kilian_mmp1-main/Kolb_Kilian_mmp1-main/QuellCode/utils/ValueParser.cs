using System;
using System.IO;

public static class ValueParser
{
    public static int ParseGameState(int linePosition){
        //line Position: 0 == Gamestate (0 == Firstload, 2 == Shopping State, 3 == Finished, but Freeplay)
        //               1 == Money currently avadible to Player
        //               2 == Value of the Last Fish (for cool animation)
        //               3 == Upgrade of Vision
        //               4 == Upgrade of Bait (or weight tbd)
        //               5 == Upgrade of Time before movement stops
        //               6 == Weight (increased downward accell);
        //           !!!NOTE: ALWAYS RESET UPGRADE STATS TO 1!!!
        string filePath = "Utils/ParsedData/ParsedGame.txt";

        return Read(filePath, linePosition);
    }

    public static void WriteGamestate(int location, int value){
        string filePath = "Utils/ParsedData/ParsedGame.txt";
        Write(filePath, location, value);
    }

    public static void ResetGameState(){
        string filePath = "Utils/ParsedData/ParsedGame.txt";
        string[] resetvalues = {"0","0","0","1","1","1","1"};
        File.WriteAllLines(filePath, resetvalues);
    }
    public static int ParseSettings(int location){
        //line Position: 0 == Music State
        //               1 == Sound State

        string filePath = "Utils/ParsedData/ParsedSettings.txt";
        return Read(filePath, location);
    }
    public static void WriteSettings(int location, int value){
        string filePath = "Utils/ParsedData/ParsedSettings.txt";
        Write(filePath, location, value);
    }
    private static void Write(string filePath, int location, int value){

        try{

           string[] lines = File.ReadAllLines(filePath);
            if(lines.Count() == 0){
                lines = new string[]{"0", "0", "1", "1", "1", "1"};
                File.WriteAllLines(filePath, lines);
            }
            if(location<lines.Count()){
                lines[location] = ""+value;
                File.WriteAllLines(filePath, lines);
            }else{
                Console.WriteLine("error Parsing, tried at " + location);
            }
        }catch(Exception e){
            Console.WriteLine(e);
        }
    }

    private static int Read(string filePath, int location){

        int returnValue;
        bool succes;

        try{
            string[] lines = File.ReadAllLines(filePath);
            if(lines.Count() == 0){
                lines = new string[]{"0", "0", "0", "1", "1", "1", "1"};
                File.WriteAllLines(filePath, lines);
                return 0;
            }
            if(location >= lines.Count()){
                return 0;
            }
            succes = int.TryParse(lines[location], out returnValue);
            if(succes){
                return returnValue;
            }else{
                lines[location] = "0";
                File.WriteAllLines(filePath, lines);
                return 0;
            }
        }catch(Exception e){
            Console.WriteLine(e);
            //string[] lines = new string[]{"0", "0", "0", "1", "1", "1", "1"};
            //File.WriteAllLines(filePath, lines);
            return 0;
        }
    }
}