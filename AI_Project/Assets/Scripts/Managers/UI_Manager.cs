using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_Manager : MonoBehaviour
{
    public static UI_Manager Instance;

    public TMP_Text coinsText;

    private void Awake()
    {
        Instance = this;
    }

    public void UpdateCoinsText(string newText)
    {
        coinsText.text = newText;
    }
}
