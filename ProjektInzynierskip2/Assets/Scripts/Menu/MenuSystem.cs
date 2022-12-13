using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using inzynierka;
using TMPro;
using System.Linq;


public class MenuSystem : MonoBehaviour
{
    public static MenuSystem instance;
    public List<PlayerCell> playersList;
    [SerializeField] private GameObject playerCellPrefab;
    [SerializeField] private GameObject content;

    [SerializeField] private Button playBtn;
    [SerializeField] private Button addPlayerBtn;


    Dictionary<string, int> difficulties;
    [SerializeField] private List<Button> difficultiesButtons;
    [SerializeField] private GridLayoutGroup gridLayoutGroup;


    // Start is called before the first frame update
    void Start()
    {
        difficulties=new Dictionary<string, int> { ["£atwy"] = 0, ["Œredni"]=1,["Trudny"]=2 };

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

        SetDifficulty(0);

        gridLayoutGroup.cellSize = new Vector2(playBtn.GetComponent<RectTransform>().rect.width, gridLayoutGroup.cellSize.y);

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


    int currIndex=0;
    public void SetDifficulty(int index)
    {

        string difficultyName = difficulties.ElementAt(index).Key;
        int difficultyId = difficulties.ElementAt(index).Value;

        //selectDifficultyText.text = difficultyName;

        PlayerPrefs.SetInt("difficulty", difficultyId);

        for(int i = 0; i < difficultiesButtons.Count; i++)
        {
            difficultiesButtons[i].interactable = i == index ? false : true;
        }
    }


}
