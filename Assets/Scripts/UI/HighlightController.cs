using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighlightController : MonoBehaviour
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
        //Show the cell size to player when he hovers over the tile
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cell = MapController.instance.grid.WorldToCell(mousePosition);
        CellInfo.text = "<b>X = " + cell.x + " : " + "Y = " + cell.y + "</b>";

        pointerGO.transform.position = MapController.CellToWorld(cell.x, cell.y);
    }
}
