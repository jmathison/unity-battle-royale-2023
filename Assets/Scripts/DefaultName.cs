using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DefaultName : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.GetComponent<TMP_InputField>().text = System.Environment.MachineName;
    }
}
