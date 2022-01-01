using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placeholder : MonoBehaviour
{
    public void Close()
    {
        GetComponent<Animator>().SetTrigger("close");
    }
}
