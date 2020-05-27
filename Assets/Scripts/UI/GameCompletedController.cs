using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameCompletedController : MonoBehaviour
{
    public GameObject ListElementPrefab;
    public GameObject ResultsPanel;
    public GameObject ContentPanel;

    public void HomeClicked()
    {
        //Options controller is Singleton, so we know there will only be one to destroy
        Destroy(FindObjectOfType<OptionsController>().gameObject);

        //Load Menu again, with empty options so that player can input new ones
        SceneManager.LoadScene("Menu");
    }

    public void NextClicked()
    {
        //Reload the scene to run the new simulation on a newly generated map
        SceneManager.LoadScene("SimulationScene");
    }

    public void FinishClicked()
    {
        //Show results
        ResultsPanel.SetActive(true);
        foreach(RunResult result in ResultsHolder.runResults)
        {
            //Instantiate new list element for each run. List element knows how to setup itself
            GameObject listElement = Instantiate(ListElementPrefab, ContentPanel.transform);
            ResultsListElement element = listElement.GetComponent<ResultsListElement>();

            //Let the list element setup itself
            element.PopulateData(ResultsHolder.runResults.IndexOf(result), OptionsController.instance.MapSize, OptionsController.instance.NumberOfObstacles, result.algorithmNames, result.numberOfFieldsChecked, result.timeSpent);
        }
    }
}