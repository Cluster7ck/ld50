using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChameleonController : MonoBehaviour
{
    [SerializeField] private Transform tongueOrigin;
    [SerializeField] private LineRenderer tongue;

    public void Click(Vector3 worldPos)
    {
        Debug.Log("Chameleon click");
        tongue.SetPositions(new Vector3[]{tongueOrigin.position, worldPos});
    }
}
