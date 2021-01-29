using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundColor : MonoBehaviour
{
    [SerializeField] Camera cam;

    [SerializeField] [ColorUsage(true, true)] Color green;
    [SerializeField] [ColorUsage(true, true)] Color red;
    [SerializeField] [ColorUsage(true, true)] Color blue;

    private void Start()
    {
        ChangeColor.Instance.OnPlayerColorChange.AddListener(HandleColorChange);
    }

    void HandleColorChange(ChangeColor.PlayerColor currentColor, ChangeColor.PlayerColor previousColor)
    {
        switch (currentColor)
        {
            case ChangeColor.PlayerColor.RED:
                cam.backgroundColor = red;
                break;

            case ChangeColor.PlayerColor.BLUE:
                cam.backgroundColor = blue;
                break;

            case ChangeColor.PlayerColor.GREEN:
                cam.backgroundColor = green;
                break;

            case ChangeColor.PlayerColor.WHITE:
                cam.backgroundColor = Color.grey;
                break;
            default:
                break;
        }
    }
}
