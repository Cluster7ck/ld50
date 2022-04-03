using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eye : MonoBehaviour
{
    [SerializeField] private Transform lookTarget;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(Input.mousePosition);
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, 90f, 0f);
    }
}
