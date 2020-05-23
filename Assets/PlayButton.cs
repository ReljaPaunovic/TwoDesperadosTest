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
        OptionsController.instance.MapSize = mapSize.text == "" ? OptionsController.instance.defaultMapSize : Mathf.Max(int.Parse(mapSize.text), 1);
        int MapSizeLocal = OptionsController.instance.MapSize;

        OptionsController.instance.XStart = startX.text == "" ? OptionsController.instance.defaultXStart : Mathf.Clamp(int.Parse(startX.text), 0, OptionsController.instance.MapSize - 1);
        OptionsController.instance.YStart = startY.text == "" ? OptionsController.instance.defaultYStart : Mathf.Clamp(int.Parse(startY.text), 0, OptionsController.instance.MapSize - 1);

        OptionsController.instance.XEnd = endX.text == "" ? OptionsController.instance.defaultXEnd : Mathf.Clamp(int.Parse(endX.text), 0, OptionsController.instance.MapSize - 1);
        OptionsController.instance.YEnd = endY.text == "" ? OptionsController.instance.defaultYEnd : Mathf.Clamp(int.Parse(endY.text), 0, OptionsController.instance.MapSize - 1);


        int distanceFromStartToEnd = PathfinderHelper.ManhattanDistance(OptionsController.instance.XStart, OptionsController.instance.YStart, OptionsController.instance.XEnd, OptionsController.instance.YEnd);
        OptionsController.instance.NumberOfObstacles = numberOfObstacles.text == "" ? OptionsController.instance.defaultNumberOfObstacles : Mathf.Clamp(int.Parse(numberOfObstacles.text), 0, MapSizeLocal * MapSizeLocal - distanceFromStartToEnd + 1);

        SceneManager.LoadScene("SimulationScene");
    }
}
