using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Ouvrier : MonoBehaviour
{
    void Start()
    {
        if (TableauRcords.instance.isInfini)
           Destroy(gameObject);
    }
}
