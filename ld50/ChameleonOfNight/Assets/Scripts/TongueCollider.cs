using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class TongueCollider : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;
    SphereCollider sphereCollider;
    // Start is called before the first frame update
    void Start()
    {
        sphereCollider = GetComponent<SphereCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        var hitInsects = Physics.OverlapSphere(sphereCollider.transform.position, sphereCollider.radius, layerMask);
        if(hitInsects.Length > 0)
        {
            foreach(var insectCollider in hitInsects)
            {
                var enemy = insectCollider.GetComponent<Enemy>();
                // hit enemy
            }
        }
    }
}
