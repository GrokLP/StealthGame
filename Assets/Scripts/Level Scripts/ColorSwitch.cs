using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSwitch : MonoBehaviour
{
    [SerializeField] ColorOfSwitch colorOfSwitch;

    enum ColorOfSwitch
    {
        WHITE,
        RED,
        BLUE,
        GREEN
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            switch (colorOfSwitch)
            {
                case ColorOfSwitch.WHITE:
                    ChangeColor.Instance.HandleColorChange(ChangeColor.PlayerColor.WHITE);
                    break;

                case ColorOfSwitch.RED:
                    ChangeColor.Instance.HandleColorChange(ChangeColor.PlayerColor.RED);
                    break;

                case ColorOfSwitch.BLUE:
                    ChangeColor.Instance.HandleColorChange(ChangeColor.PlayerColor.BLUE);
                    break;

                case ColorOfSwitch.GREEN:
                    ChangeColor.Instance.HandleColorChange(ChangeColor.PlayerColor.GREEN);
                    break;

                default:
                    break;
            }
        }
    }
}
