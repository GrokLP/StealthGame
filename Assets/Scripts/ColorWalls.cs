using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorWalls : MonoBehaviour
{
    BoxCollider boxCollider;
    MeshRenderer meshRenderer;
    
    enum WallColor
    {
        WHITE,
        RED,
        BLUE,
        GREEN
    }
    [SerializeField] WallColor wallColor;
    string wallColorString;

    private void Awake()
    {

    }

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        meshRenderer = GetComponent<MeshRenderer>();
        
        ChangeColor.Instance.OnPlayerColorChange.AddListener(OnColorChange);
        wallColorString = wallColor.ToString();
        if(wallColor != WallColor.WHITE)
        {
            //gameObject.SetActive(false);
            boxCollider.enabled = false;
            meshRenderer.enabled = false;
        }
    }

    void OnColorChange(ChangeColor.PlayerColor currentColor, ChangeColor.PlayerColor previousColor)
    {
        string currentColorString = currentColor.ToString();
        
        if(wallColorString == currentColorString)
        {
            //gameObject.SetActive(true);
            boxCollider.enabled = true;
            meshRenderer.enabled = true;

        }

        else if(wallColorString != currentColorString)
        {
            //gameObject.SetActive(false);
            boxCollider.enabled = false;
            meshRenderer.enabled = false;
        }

    }
}
