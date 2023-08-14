using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextHealth : MonoBehaviour
{
    TextMeshProUGUI text;
    HealthComponent health;
    

    public void Awake()
    {
        StartCoroutine(GetHero());
        text = GetComponent<TextMeshProUGUI>();

    }

    public void Update()
    {    if (health == null) return;
        text.text = health.HP.ToString();
    }
    public IEnumerator GetHero()
    {
        Hero hero = FindObjectOfType<Hero>();
        while (hero == null)
        {
            hero = FindObjectOfType<Hero>();
            yield return new WaitForSeconds(0.3f);
        }
        health = hero.GetComponent<HealthComponent>();
    }
}
