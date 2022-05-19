using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotatio : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Application.runInBackground = true;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation *= Quaternion.Euler(Vector3.up*10f);
    }
}
