using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ResultsListElement : MonoBehaviour
{
    public GameObject BasicInfo;
    public Text runNumberText;
    public Text BoardSizeText;
    public Text ObstacleNumberText;

    public void PopulateData(int runNumber, int mapSize, int numberOfObstacles, List<string> names, List<int> numFields, List<float> time)
    {
        runNumberText.text = runNumber.ToString();
        BoardSizeText.text = mapSize.ToString();
        ObstacleNumberText.text = numberOfObstacles.ToString();

        foreach(string name in names)
        {
            Instantiate(BasicInfo, transform);

            GameObject runInfoGO = Instantiate(BasicInfo, transform);
            RunInfo runInfo = runInfoGO.GetComponent<RunInfo>();

            runInfo.algorithmName.text = name;
            runInfo.numberOfTilesChecked.text = numFields[names.IndexOf(name)].ToString();

            //Display time with 5 decimal places
            runInfo.time.text = time[names.IndexOf(name)].ToString("f5");

        }
    }
}
