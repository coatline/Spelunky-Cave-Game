using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Fader : MonoBehaviour
{
    public enum State
    {
        idle,
        fadeIn,
        fadeOut
    }

    [SerializeField] float fadeSpeed = 3;

    public Image image = null;
    TMP_Text levelText = null;
    Color color;
    float alpha = 1;
    State state;
    string levelname;

    void Start()
    {
        state = State.fadeIn;

        levelText = transform.GetComponentInChildren<TMP_Text>();

        image = GetComponent<Image>();


        switch (Keep.I.level)
        {
            case 1:
            case 2:
            case 3:
                levelname = "Cave";
                break;
            case 4:
            case 5:
            case 6:
                levelname = "Temple";
                break;
            case 7:
            case 8:
            case 9:
                levelname = "Ruins";
                break;
            case 10: 
                levelname = "";
                break;
        }

        levelText.text = levelname + $"\n {Keep.I.level}";

    }

    void Update()
    {
        if (state == State.idle)
        {
            return;
        }

        alpha -= Time.deltaTime / fadeSpeed;

        if (state == State.fadeOut)
        {
            if (SceneManager.GetActiveScene().buildIndex == 0)
            {
                levelText.gameObject.SetActive(true);
                levelText.text = "Game Over";
            }

            if (image.color.a <= 1)
            {
                image.color = new Color(0, 0, 0, alpha);
            }
            else
            {
                Keep.I.NewGame();
                state = State.idle;
                levelText.gameObject.SetActive(false);
                Scene scene = SceneManager.GetActiveScene();
                SceneManager.LoadScene(scene.buildIndex);
            }
        }
        else
        {
            if (image.color.a >= 0)
            {
                image.color = new Color(0, 0, 0, alpha);
            }
            else if (levelText.enabled)
            {
                levelText.gameObject.SetActive(false);
                state = State.idle;
            }
        }

    }

    public void ChangeState(State thestate)
    {
        if (thestate == State.fadeIn)
        {
            fadeSpeed = 3;
        }
        else if (thestate == State.fadeOut)
        {
            fadeSpeed = -3;
        }

        state = thestate;
    }
}
