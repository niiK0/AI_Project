using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int Coins { get; set; }

    public GameObject boss;

    private void Awake()
    {
        if (Instance != this) Destroy(gameObject);

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void CoinPickup()
    {
        Coins++;
        UI_Manager.Instance.UpdateCoinsText(Coins.ToString());
        if(Coins == 27)
        {
            boss.SetActive(true);
            UI_Manager.Instance.EnableBossUI();
        }
    }
}
