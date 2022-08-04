using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    public bool isExit = false;
    bool done = false;
    public int level;

    Player player;

    private void Start()
    {
        player = FindObjectOfType<Player>();
        level = Keep.I.level;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Input.GetKeyDown(KeyCode.F) && isExit && collision.gameObject.CompareTag("Player") && !done)
        {
            if (!done)
            {
                NextFloor();
            }
        }
    }

    public void NextFloor()
    {
        done = true;

        Keep.I.currentWeapon = player.currentWeapon;
        Keep.I.currentBow = player.currentBow;

        Keep.I.playerHealth = player.playerHealth;

        Scene current = SceneManager.GetActiveScene();

        SceneManager.LoadScene(current.buildIndex);

        Keep.I.LevelWasLoaded(this);
    }

}
