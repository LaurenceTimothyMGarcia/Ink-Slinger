using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class healthBar : MonoBehaviour
{
    public Animator animator;

    public Slider healthSlider;
    public Slider easeHealthSlider;
    public float maxHealth = 100f;

    private float health;
    private float lerpSpeed = 0.05f;
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            KillEntity();
        }

        if (healthSlider.value != health)
        {
            healthSlider.value = health;
        }

        if (healthSlider.value != easeHealthSlider.value)
        {
            easeHealthSlider.value = Mathf.Lerp(easeHealthSlider.value, health, lerpSpeed);
        }


    }

    public void takeDamage(float damage)
    {
        if (health > 0)
        {
            animator.SetTrigger("Hurt");
            health -= damage;
        }
    }

    private void KillEntity()
    {
        Destroy(this.gameObject);
    }
}
