using UnityEngine.UI;
using System;

public class HelperFunctions {

    public static void PopulateSaveButtons(Button[] saveFiles)
    {
        for (int i = 0; i < saveFiles.Length - 1; i++)
        {
            string data = SaveManager.data.GetSaveData(i + 1);
            if (data.Equals("Empty"))
                saveFiles[i].GetComponentInChildren<Text>().text = data;
            else
            {
                string[] splitData = data.Split('\n');
                string name = splitData[0].Split(':')[1].Trim();
                string timePlayed = splitData[9].Split(':')[1].Trim();
                int secondsPlayed;
                Int32.TryParse(timePlayed, out secondsPlayed);
                string seconds;
                string minutes;
                string hours;
                if (secondsPlayed >= 60)
                {
                    int minutesPlayed = secondsPlayed / 60;

                    if (secondsPlayed % 60 == 0)
                        seconds = "00";
                    else
                    {
                        secondsPlayed = secondsPlayed - (minutesPlayed * 60);
                        if (secondsPlayed < 10)
                            seconds = "0" + secondsPlayed;
                        else
                            seconds = secondsPlayed.ToString();
                    }

                    if (minutesPlayed < 10)
                        minutes = "0" + minutesPlayed;
                    else
                        minutes = minutesPlayed.ToString();

                    if (minutesPlayed >= 60)
                    {
                        if (minutesPlayed < 600)
                            hours = "0" + minutesPlayed / 60;
                        else
                            hours = (minutesPlayed / 60).ToString();
                    }
                    else
                        hours = "00";
                }
                else
                {
                    minutes = "00";
                    hours = "00";
                    seconds = timePlayed;
                }
                saveFiles[i].GetComponentInChildren<Text>().text = name + " - " + hours + ":" + minutes + ":" + seconds;
            }
        }
    }
}
