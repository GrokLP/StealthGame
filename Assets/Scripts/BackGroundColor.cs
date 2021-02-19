using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundColor : MonoBehaviour
{
    [SerializeField] Camera cam;

    [SerializeField] [ColorUsage(true, true)] Color green;
    [SerializeField] [ColorUsage(true, true)] Color red;
    [SerializeField] [ColorUsage(true, true)] Color blue;

    [SerializeField] GameObject distortionPlane;

    private void Start()
    {
        if(GameManager.Instance.CurrentGameState != GameManager.GameState.PREGAME)
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

        if (currentColor != previousColor)
        {
            AudioManager.Instance.PlaySound("ColorChangeWoosh");
            StartCoroutine(Distortion());
        }

    }

    IEnumerator Distortion()
    {
        distortionPlane.SetActive(true);

        yield return new WaitForSeconds(0.15f);

        distortionPlane.SetActive(false);
    }
}
