using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsPanel : MonoBehaviour
{
    public bool isOpen = false;

    private void Awake()
    {
        this.gameObject.SetActive(isOpen);
    }

    public void ToggleOpen()
    {
        isOpen = !isOpen;

        this.gameObject.SetActive(isOpen);
    }
}
