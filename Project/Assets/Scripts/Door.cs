using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    [SerializeField] TMP_Text exitText;
    public bool isExit = false;
    bool done = false;

    Player player;

    private void Start()
    {
        player = FindFirstObjectByType<Player>();
        exitText.transform.rotation = Quaternion.identity;
    }

    private void Update()
    {
        if (exitText.enabled)
            if (Input.GetKeyDown(KeyCode.F))
            {
                NextFloor();
            }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isExit && collision.gameObject.CompareTag("Player") && !done)
        {
            exitText.enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        exitText.enabled = false;
    }

    public void NextFloor()
    {
        done = true;

        Keep.I.currentWeapon = player.currentWeapon;
        Keep.I.currentBow = player.currentBow;

        Keep.I.playerHealth = player.playerHealth;

        Scene current = SceneManager.GetActiveScene();

        SceneManager.LoadScene(current.buildIndex);

        Keep.I.LoadNextLevel(this);
    }

}
