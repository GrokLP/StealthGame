using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisibility : MonoBehaviour
{
    ChangeColor.PlayerColor currentPlayerColor;
    PlayerController playerController;
    bool isBlack;

    [SerializeField] Material playerShader;

    private void Start()
    {
        ChangeColor.Instance.OnPlayerColorChange.AddListener(HandleColorChange);
        playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        ChangeColorToBlack();
    }

    void HandleColorChange(ChangeColor.PlayerColor currentColor, ChangeColor.PlayerColor previousColor)
    {
        currentPlayerColor = currentColor;
    }

    public bool IsVisible() 
    {
        if (currentPlayerColor == ChangeColor.PlayerColor.WHITE && playerController.IsMoving == false && isBlack)
            return false;
        else
            return true;
    }

    void ChangeColorToBlack()
    {
        if(currentPlayerColor == ChangeColor.PlayerColor.WHITE && Input.GetButton("Hide"))
        {
            isBlack = true;
            playerShader.SetColor("Color_7FBCE1A5", Color.black);
        }
        if(currentPlayerColor == ChangeColor.PlayerColor.WHITE && Input.GetButtonUp("Hide"))
        {
            isBlack = false;
            playerShader.SetColor("Color_7FBCE1A5", Color.white);
        }
    }
}
