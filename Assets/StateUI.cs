using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateUI : MonoBehaviour {
    StateController controller;
    public State[] statesToShow;
    public Sprite[] correspondingImages;
    private SpriteRenderer sprite;
    private State previousState;
    private State remainInState;
    private Color startColor;
    // Use this for initialization

    void Awake()
    {
        controller = transform.root.GetComponent<StateController>();
        sprite = GetComponent<SpriteRenderer>();
        sprite.enabled = true;
        remainInState = controller.remainState;
        startColor = sprite.color;
        sprite.color = Color.clear;
    }

    void OnValidate()
    {
    }
	
	// Update is called once per frame
	void Update () {
        transform.rotation = Quaternion.LookRotation(Camera.main.transform.position - transform.position);
    }

    public void ShowState()
    {
        for(int i = 0; i < statesToShow.Length; i++)
        {
            if (controller.currentState == statesToShow[i] && statesToShow[i] != previousState||
                controller.currentState == statesToShow[i] && statesToShow[i].uiIsRepeatable)
            {
               sprite.color = startColor;
               sprite.sprite = correspondingImages[i];
               
               StartCoroutine(DisableSpriteAfteryield(2f));
            }
            previousState = controller.currentState;

        }
        
    }

    IEnumerator DisableSpriteAfteryield(float t)
    {
        yield return new WaitForSeconds(t);
        StartCoroutine(FadeSprite(0.5f));
    }

    IEnumerator FadeSprite(float duration)
    {
        float start = Time.time;
        Color color = sprite.color;
        while (Time.time <= start + duration)
        {
            
            color.a = 1f - Mathf.Clamp01((Time.time - start) / duration);
            sprite.color = color;
            yield return new WaitForEndOfFrame();
        }
        sprite.color = Color.clear;
    }
}
