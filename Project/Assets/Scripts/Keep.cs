using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


[DefaultExecutionOrder(-400)]
public class Keep : MonoBehaviour
{
    const string GOB_NAME = "Game";

    Player player;
    public int playerHealth;
    public Weapon currentWeapon;
    public Bow currentBow;
    public int level = 1;

    #region Statics
    static Keep instance;
    public static Keep I
    {
        get
        {
            if (instance == null)
            {
                var gob = new GameObject(GOB_NAME);
                gob.AddComponent<Keep>();
            }
            return instance;
        }
    }
    #endregion

    [SerializeField] TMP_Text dmgTxtPrfb;
    [SerializeField] TMP_Text diedText;
    [SerializeField] TMP_Text levelText;

    GameObject canvas = null;

    public TMP_Text healthText;
    public TMP_Text weaponText;
    public TMP_Text bowText;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning($"There is another instance of {GetType().Name} already in the scene. Deleting this one!");
            DestroyImmediate(gameObject);
            return;
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
    }

    public void SetText(string textType, string weaponName = null, string bowName = null, int health = 0, int maxHealth = 0)
    {
        if (weaponText == null || healthText == null || bowText == null)
        {
            FindTexts();
        }

        if (textType == "Weapon")
        {
            weaponText.text = $"Weapon: {weaponName}";
        }

        else if (textType == "Health")
        {
            healthText.text = $"Health {health}/{maxHealth}";
        }

        else if (textType == "Bow")
        {
            bowText.text = $"Bow: {bowName}";
        }
    }

    public void FindTexts()
    {
        var texts = FindObjectsByType<TMP_Text>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        for (int i = 0; i < texts.Length; i++)
        {
            if (texts[i].name == "Health_Text")
            {
                healthText = texts[i];
            }
            else if (texts[i].name == "Weapon_Text")
            {
                weaponText = texts[i];
            }
            else if (texts[i].name == "Bow_Text")
            {
                bowText = texts[i];
            }
            else if (texts[i].name == "You Died Text")
                diedText = texts[i];
            else if (texts[i].name == "Level_Text")
            {
                levelText = texts[i];
            }
        }
    }

    public void NewDamageText(int damage, Transform transformPos, bool isPlayer = false)
    {
        if (!canvas)
        {
            canvas = GameObject.FindGameObjectWithTag("World Space Canvas");
        }

        var newText = Instantiate(dmgTxtPrfb);

        newText.text = $"{damage}";

        newText.transform.SetParent(canvas.transform);

        newText.transform.position = transformPos.position;

        if (isPlayer)
        {
            newText.color = Color.red + new Color(1, .1f, .3f, 1);
        }
    }

    private void Start()
    {
        if (healthText == null || weaponText == null)
        {
            FindTexts();
        }
        if (!canvas)
        {
            canvas = GameObject.FindGameObjectWithTag("World Space Canvas");
        }

        player = FindFirstObjectByType<Player>();

        if (level == 1)
        {
            playerHealth = player.maxHealth;
        }

        levelText.text = $"Level: {level}";
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindTexts();
        levelText.text = $"Level: {level}";
    }

    public void NewGame()
    {
        level = 1;

        playerHealth = player.maxHealth;
        SetText("Health", null, null, playerHealth, player.maxHealth);

        currentWeapon = WeaponGenerator.I.startingWeapon;
        currentBow = BowGenerator.I.startingBow;
        SetText("Weapon", WeaponGenerator.I.startingWeapon.myName);

        levelText.text = $"Level: {level}";
        gameOver = false;
    }

    public void LoadNextLevel(Door door)
    {
        level++;
        Scene current = SceneManager.GetActiveScene();
        SceneManager.LoadScene(current.buildIndex);
    }

    public void CreateItem(Transform pos)
    {
        var gob = new GameObject();

        gob.transform.position = pos.position + new Vector3(0, .25f, 0);

        gob.AddComponent<Item>();

        var itemInfo = gob.GetComponent<Item>();

        gob.name = itemInfo.name;

        var col = gob.AddComponent<BoxCollider2D>();
        col.isTrigger = true;

        gob.tag = "Pickup";

        var gobChild = new GameObject();
        gobChild.transform.SetParent(gob.transform);
        gobChild.transform.localPosition = Vector3.zero;

        var col1 = gobChild.AddComponent<BoxCollider2D>();
        col1.size /= 10;


        //var pCol = gob.AddComponent<PolygonCollider2D>();

        //pCol.

        var rb = gob.AddComponent<Rigidbody2D>();
        rb.mass = 5;

        var rand = Random.Range(0, 2);

        if (rand == 0)
        {
            itemInfo.type = "Weapon";
            var index = WeaponGenerator.I.names.Length - 1;
            var weaponData = WeaponGenerator.I.GetData(index);
            itemInfo.name = weaponData.myName;
        }
        else if (rand == 1)
        {
            itemInfo.type = "Bow";
            var index = BowGenerator.I.names.Length - 1;
            var bowData = BowGenerator.I.getData(index);
            itemInfo.name = bowData.myName;
        }
    }

    bool gameOver;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Scene current = SceneManager.GetActiveScene();
            SceneManager.LoadScene(current.buildIndex);
            NewGame();
        }
    }

    public void GameOver()
    {
        diedText.gameObject.SetActive(true);
        gameOver = true;
    }
}
