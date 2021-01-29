using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shapes;

public class ColorLaserGuard : MonoBehaviour
{
    MeshCollider meshCollider;
    MeshRenderer meshRenderer;
    LineRenderer lineRenderer;
    Disc colorDisc;

    enum EnemyColor
    {
        NONE,
        WHITE,
        RED,
        BLUE,
        GREEN
    }

    [SerializeField] EnemyColor enemyColor;
    string enemyColorString;

    bool isActive = true;

    public bool IsActive
    {
        get { return isActive; }
    }

    private void Start()
    {
        meshCollider = GetComponentInChildren<MeshCollider>();
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        lineRenderer = GetComponentInChildren<LineRenderer>();
        colorDisc = GetComponentInChildren<Disc>();

        ChangeColor.Instance.OnPlayerColorChange.AddListener(OnColorChange);
        enemyColorString = enemyColor.ToString();
        if (enemyColor != EnemyColor.WHITE)
        {
            if (enemyColor == EnemyColor.NONE)
                return;

            //gameObject.SetActive(false);
            meshCollider.enabled = false;
            meshRenderer.enabled = false;
            lineRenderer.enabled = false;
            colorDisc.enabled = false;
            isActive = false;
        }
    }

    void OnColorChange(ChangeColor.PlayerColor currentColor, ChangeColor.PlayerColor previousColor)
    {
        string currentColorString = currentColor.ToString();

        if(enemyColor == EnemyColor.NONE)
        {
            return;
        }

        else if (enemyColorString == currentColorString)
        {
            //gameObject.SetActive(true);
            meshCollider.enabled = true;
            meshRenderer.enabled = true;
            lineRenderer.enabled = true;
            colorDisc.enabled = true;
            isActive = true;

        }

        else if (enemyColorString != currentColorString)
        {
            //gameObject.SetActive(false);
            meshCollider.enabled = false;
            meshRenderer.enabled = false;
            lineRenderer.enabled = false;
            colorDisc.enabled = false;
            isActive = false;
        }

    }
}
