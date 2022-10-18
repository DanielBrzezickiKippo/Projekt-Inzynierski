using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using inzynierka;



public class MenuSystem : MonoBehaviour
{
    public static MenuSystem instance;
    public List<PlayerCell> playersList;
    [SerializeField] private GameObject playerCellPrefab;
    [SerializeField] private GameObject content;

    [SerializeField] private Button playBtn;
    [SerializeField] private Button addPlayerBtn;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
        instance = this;
        Setup();
    }



    // Update is called once per frame
    void Update()
    {
        
    }

    public void Setup()
    {
        playBtn.onClick.AddListener(() => { Play(); });
        addPlayerBtn.onClick.AddListener(() => { AddPlayerObject(); });
        AddPlayerObject();
    }

    public void AddPlayerObject()
    {
        GameObject obj = Instantiate(playerCellPrefab, content.transform);
        PlayerCell playerCell = obj.GetComponent<PlayerCell>();
        if (playersList.Count == 0)
            playerCell.Setup(this,()=> { }, false);
        playersList.Add(playerCell);

        playerCell.Setup(this,()=>RemovePlayerObject(playerCell));

        HandleAddPlayerButton();
        HandlePlayButton();
    }

    public void RemovePlayerObject(PlayerCell playerToDel)
    {
        playersList.Remove(playerToDel);
        Destroy(playerToDel.gameObject);

        HandleAddPlayerButton();
        HandlePlayButton();
    }

    public bool CanPlay()
    {
        if (playersList.Count == 0)
            return false;

        foreach(PlayerCell playerCell in playersList)
        {
            if (!playerCell.IsReadyToPlay())
                return false;
        }
        return true;
    }

    public void HandlePlayButton()
    {
        playBtn.interactable = CanPlay();
    }

    public void HandleAddPlayerButton()
    {
        if (playersList.Count >= 4)
            addPlayerBtn.interactable = false;
        else
            addPlayerBtn.interactable = true;

        HandlePlayButton();
    }


    public List<string> GetPlayers()
    {

        List<string> playersNames=new List<string>();
        foreach(PlayerCell playerCell in playersList)
        {
            playersNames.Add(playerCell.GetName());
        }
        return playersNames;
    }

    

    public void Play()
    {
        inzynierka.SceneManager.NextScene();
    }
}
