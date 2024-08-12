using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public static class SaveSystem 
{
    //static readonly string to contain location of save folder
    private static readonly string SAVE_FOLDER = Application.dataPath + "/Saves/";
    public static void Init()
    {
        //test if save folder exists
        if (!Directory.Exists(SAVE_FOLDER))
        {
            //Create new save folder if not exist
            Directory.CreateDirectory(SAVE_FOLDER);
        }
    }

    public static void Save(string saveString) //create new save file in save folder
    {
        int saveNumber = 1;
        //count number 
        while (File.Exists(SAVE_FOLDER + "save_" + saveNumber + ".txt"))
        {
            saveNumber++;
        }
        //write most rescent save file
        File.WriteAllText(SAVE_FOLDER + "save_" + saveNumber + ".txt", saveString);
        //Debug.Log("Saved in" + SAVE_FOLDER + "save_" + saveNumber + ".txt");
    }

    public static string Load()
    {
        //Create a directory info (allows us to use directory information very conveniently)
        DirectoryInfo directoryInfo = new DirectoryInfo(SAVE_FOLDER);

        //get list of all our save files   
        FileInfo[] saveFiles = directoryInfo.GetFiles("*.txt");
        FileInfo mostRecentFile = null;

        //cycle through the list to find the most recent save file
        foreach (FileInfo fileInfo in saveFiles)
        {
            if (mostRecentFile == null) //if no recent, current is most recent
                mostRecentFile = fileInfo;
            else if (fileInfo.LastWriteTime > mostRecentFile.LastWriteTime) //if there is more recent, exchange and remove older
            {
                FileInfo temp = null;
                temp = mostRecentFile;
                mostRecentFile = fileInfo;
                File.Delete(temp.FullName);
            }
            else //if there is an older file, remove 
                File.Delete(fileInfo.FullName);
        }

        //if there is a most recent, then return the string
        if (mostRecentFile != null)
        {
            string saveString = File.ReadAllText(mostRecentFile.FullName);
            return saveString;
        }
        else
        {
            return null;
        }
    }
}
