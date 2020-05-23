using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiController : MonoBehaviour
{
    public Text CellInfo;
    public GameObject pointer;
    private GameObject pointerGO;

    private void Start()
    {
        pointerGO = Instantiate(pointer, MapController.instance.grid.transform);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cell = MapController.instance.grid.WorldToCell(mousePosition);
        CellInfo.text = "X = " + cell.x + " : " + "Y = " + cell.y;


        pointerGO.transform.position = MapController.CellToWorld(cell.x, cell.y);
    }
}
