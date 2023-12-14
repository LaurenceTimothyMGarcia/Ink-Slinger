using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class inkBar : MonoBehaviour
{
    // Start is called before the first frame update
    public Slider inkSlider;
    public Slider easeInkSlider;
    public float maxInk = 100f;
    public float ink;
    private float lerpSpeed = 0.05f;
    // Start is called before the first frame update
    void Start()
    {
        ink = maxInk;
    }

    // Update is called once per frame
    void Update()
    {
        if (inkSlider.value != ink)
        {
            inkSlider.value = ink;
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            takeDamage(10);
        }

        if (inkSlider.value != easeInkSlider.value)
        {
            easeInkSlider.value = Mathf.Lerp(easeInkSlider.value, ink, lerpSpeed);
        }


    }

    public void takeDamage(float damage)
    {
        ink -= damage;
    }
}
