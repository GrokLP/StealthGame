using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColor : Singleton<ChangeColor>
{
    public Events.PlayerColorState OnPlayerColorChange;

    [SerializeField] ThrowObject throwObjectScript;
    [SerializeField] BetterJump betterJumpScript;
    [SerializeField] PushObject pushObjectScript;
    [SerializeField] PushChild pushChildScript;

    [SerializeField] [ColorUsage(true, true)] Color green;
    [SerializeField] [ColorUsage(true, true)] Color red;
    [SerializeField] [ColorUsage(true, true)] Color blue;

    [SerializeField] Material playerShader;
    [SerializeField] BoxCollider boxCollider;
    [SerializeField] SphereCollider sphereCollider;
    [SerializeField] Rigidbody rb;

    PlayerColor currentPlayerColor;
    public PlayerColor CurrentPlayerColor
    {
        get { return currentPlayerColor; }
        private set { currentPlayerColor = value; }
    }

    public enum PlayerColor
    {
        WHITE,
        RED,
        BLUE,
        GREEN
    }

    private void Start()
    {
        UpdateColor(PlayerColor.WHITE);
        boxCollider = GetComponent<BoxCollider>();
        sphereCollider = GetComponent<SphereCollider>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // will eventually remove this
        if (Input.GetKeyDown(KeyCode.R))
            UpdateColor(PlayerColor.RED);
        else if (Input.GetKeyDown(KeyCode.B))
            UpdateColor(PlayerColor.BLUE);
        else if (Input.GetKeyDown(KeyCode.G))
            UpdateColor(PlayerColor.GREEN);
        else if (Input.GetKeyDown(KeyCode.T))
            UpdateColor(PlayerColor.WHITE);
    }

    public void HandleColorChange(PlayerColor playerColor)
    {
        UpdateColor(playerColor);
    }

    void UpdateColor(PlayerColor playerColor)
    {
        PlayerColor previousPlayerColor = currentPlayerColor;
        currentPlayerColor = playerColor;

        switch (currentPlayerColor)
        {
            case PlayerColor.WHITE:
                throwObjectScript.IsActive = false;
                betterJumpScript.IsActive = false;
                pushObjectScript.IsActive = false;
                playerShader.SetColor("Color_7FBCE1A5", Color.white);
                boxCollider.enabled = true;
                sphereCollider.enabled = false;
                break;

            case PlayerColor.RED:
                throwObjectScript.IsActive = true;
                playerShader.SetColor("Color_7FBCE1A5", red);
                boxCollider.enabled = true;
                sphereCollider.enabled = false;
                break;

            case PlayerColor.BLUE:
                betterJumpScript.IsActive = true;
                playerShader.SetColor("Color_7FBCE1A5", blue);
                boxCollider.enabled = false;
                sphereCollider.enabled = true;
                rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
                break;

            case PlayerColor.GREEN:
                pushChildScript.IsActive = true;
                pushObjectScript.IsActive = true;
                playerShader.SetColor("Color_7FBCE1A5", green);
                boxCollider.enabled = true;
                sphereCollider.enabled = false;
                break;

            default:
                break;
        }

        switch (previousPlayerColor)
        {
            case PlayerColor.RED:
                if(currentPlayerColor != PlayerColor.RED)
                    throwObjectScript.IsActive = false;
                break;

            case PlayerColor.BLUE:
                if (currentPlayerColor != PlayerColor.BLUE)
                    betterJumpScript.IsActive = false;
                break;

            case PlayerColor.GREEN:
                if (currentPlayerColor != PlayerColor.GREEN)
                    pushChildScript.IsActive = false;
                pushObjectScript.IsActive = false;
                break;

            default:
                break;
        }

        OnPlayerColorChange.Invoke(currentPlayerColor, previousPlayerColor);
    }
}
