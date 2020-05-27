using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayButton : MonoBehaviour
{
    public Text mapSize;
    public Text numberOfObstacles;
    public Text startX;
    public Text startY;
    public Text endX;
    public Text endY;

    public void Play()
    {
        //Sanitize the input
        OptionsController.instance.MapSize = Mathf.Max(mapSize.text == "" ? OptionsController.instance.defaultMapSize : int.Parse(mapSize.text), 2);
        int MapSizeLocal = OptionsController.instance.MapSize;

        OptionsController.instance.XStart = Mathf.Clamp(startX.text == "" ? OptionsController.instance.defaultXStart : int.Parse(startX.text), 0, OptionsController.instance.MapSize - 1);
        OptionsController.instance.YStart = Mathf.Clamp(startY.text == "" ? OptionsController.instance.defaultYStart : int.Parse(startY.text), 0, OptionsController.instance.MapSize - 1);

        OptionsController.instance.XEnd = Mathf.Clamp(endX.text == "" ? OptionsController.instance.defaultXEnd : int.Parse(endX.text), 0, OptionsController.instance.MapSize - 1);
        OptionsController.instance.YEnd = Mathf.Clamp(endY.text == "" ? OptionsController.instance.defaultYEnd : int.Parse(endY.text), 0, OptionsController.instance.MapSize - 1);

        int distanceFromStartToEnd = PathfinderHelper.ManhattanDistance(OptionsController.instance.XStart, OptionsController.instance.YStart, OptionsController.instance.XEnd, OptionsController.instance.YEnd);
        OptionsController.instance.NumberOfObstacles = Mathf.Clamp(numberOfObstacles.text == "" ? OptionsController.instance.defaultNumberOfObstacles : int.Parse(numberOfObstacles.text), 0, MapSizeLocal * MapSizeLocal - distanceFromStartToEnd + 1);

        //load the simulation scene
        SceneManager.LoadScene("SimulationScene");
    }
}
