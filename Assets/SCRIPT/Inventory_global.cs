using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System.IO;
public class Inventory_global : MonoBehaviour
{
    public static int runes;

    public int startrune = 400;
    public Text runesText;

    public void Start()
    {
        runes = startrune;
        runesText.text = runes.ToString();
    }

    public void addRune(int newRune)
    {
        if (runes + newRune < 0)
            runes = 0;
        else
        {
            runes += newRune;
            runesText.text = runes.ToString();
        }

    }
}
