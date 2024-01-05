using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public int health = 50;
    public Color flash;
    public SpriteRenderer sprite;
    private Color mainColor;

    public Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        mainColor = sprite.color;
        slider.maxValue = health;
        slider.value = health;
    }

    public void FlashColors()
    {
        sprite.color = flash;
        Invoke("ResetColor", 0.1f);
    }

    public void ResetColor()
    {
        sprite.color = mainColor;
    }

    public void TakeDamage()
    {
        health -= 10;
        slider.value -= 10;

        FlashColors();

        if (health <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetSceneAt(0).name, LoadSceneMode.Single);
        }
    }
}
