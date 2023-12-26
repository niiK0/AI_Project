using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int Coins { get; set; }

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
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
