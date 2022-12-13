using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using inzynierka;
using DG.Tweening;

[System.Serializable]
public class Player
{
    public string name;
    public int characterId;
    public List<Area> properties;
    public Color playerColor;
    [HideInInspector] public int hp;
    [HideInInspector] public GameObject player;
    [HideInInspector] public GameObject playerInfo;
    [HideInInspector] public int currentAreaId=0;
    [HideInInspector] public bool isInPrison = false;
    public int rightAnswers = 0;
    public int wrongAnswers = 0;
}

public enum Turn { 
    none,
    dice,
    move,
    action,
    nextTurn
}

public class GameManager : MonoBehaviour
{
    //ONLY TO TEST
    [SerializeField] private TMP_InputField rollInput;


    //[SerializeField] private Player playersTurn;
    private int currentPlayerId;
    [SerializeField] public MapGenerator mapGenerator;
    [SerializeField] private List<GameObject> playerPrefabs;
    [SerializeField] private GameObject playerInfoPrefab;
    [SerializeField] private CameraFollow cameraFollow;
    [SerializeField] private UIManager uiManager;


    [HideInInspector] private Turn turn = Turn.none;
    [SerializeField] private List<Player> players;

    [SerializeField] private GameObject coin;
    // Start is called before the first frame update
    void Start()
    {
        LoadPlayers(GetData());
        CreatePlayers();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    List<Player> GetData()
    {
        List<string> playersNames = MenuSystem.instance.GetPlayers();
        List<Player> data = new List<Player>();

        for (int i = 0; i <playersNames.Count; i++)
        {
            Player p = new Player();
            p.name = playersNames[i];//Random.Range(0, 100).ToString();
            p.playerColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
            p.properties = new List<Area>();
            data.Add(p);
        }

        Destroy(MenuSystem.instance.gameObject);
        return data;
    }

    void LoadPlayers(List<Player> players)
    {
        this.players = players;
    }

    void CreatePlayers()
    {
        foreach(Player p in players)
        {
            //CreateCharacter
            p.player = Instantiate(playerPrefabs[p.characterId]);
            p.hp = 5;

            p.playerInfo = Instantiate(playerInfoPrefab,p.player.transform);
            Vector3 playerPos = p.player.transform.localPosition;
            p.playerInfo.transform.localPosition = new Vector3(playerPos.x, playerPos.y + 0.6f, playerPos.z);
        }
        SetNewTarget(Random.Range(0, players.Count));
        SetTurn(Turn.dice);

    }

    int move = 0;

    public void SetTurn(Turn turn)
    {
        this.turn = turn;
        //Debug.Log("Zmiana na " + turn);
        HandleTurn();
    }

    void SetNewTarget(int playerId)
    {
        currentPlayerId = playerId;
        uiManager.SetCurrentPlayer(players[currentPlayerId]);
        cameraFollow.SetTarget(players[currentPlayerId].player.transform);
    }

    void HandleTurn()
    {
        switch (turn)
        {
            case Turn.dice:
                if (players[currentPlayerId].hp <= 0)
                    SetTurn(Turn.nextTurn);

                SetPlayersUI();
                if (!players[currentPlayerId].isInPrison)
                {
                    uiManager.RollButton(true);

                    uiManager.rollButton.onClick.RemoveAllListeners();
                    uiManager.rollButton.onClick.AddListener(() =>
                    {
                        move = RollDice();
                        uiManager.RollButton(false, move);
                        SetTurn(Turn.move);
                    });
                }
                else
                {
                    mapGenerator.mapBlocks[players[currentPlayerId].currentAreaId].GetComponent<Area>().StepOn();
                }
                break;
            case Turn.move:
                StartCoroutine(MakeMove(currentPlayerId, move, Turn.action));
                break;
            case Turn.action:
                int blockId = players[currentPlayerId].currentAreaId;
                Area block = mapGenerator.mapBlocks[blockId].GetComponent<Area>();
                block.StepOn();
                break;
            case Turn.nextTurn:
                CheckWinPlayers();

                int id = currentPlayerId;
                id++;

                if (id > players.Count-1)
                    currentPlayerId = 0;
                else
                    currentPlayerId = id;

                SetNewTarget(currentPlayerId);
                SetPlayersUI();
                SetTurn(Turn.dice);
                break;
            case Turn.none:
                break;

        }
    }

    void CheckWinPlayers()
    {
        int playersWithNoHp=0;
        Player winner = null;
        foreach(Player p in players)
        {
            if (p.hp <= 0)
                playersWithNoHp++;
            else
                winner = p;

        }

        if (playersWithNoHp + 1 == players.Count && players.Count > 1)
        {
            SetTurn(Turn.none);
            Debug.Log($"Winner: {winner?.name}");
            StartCoroutine(uiManager.SendMessage($"Gracz {winner?.name} wygra³.",3f));
            Invoke("LoadFirstScene",3f);
        }

    }

    void LoadFirstScene()
    {
        inzynierka.SceneManager.ResetScene();
    }

    public int RollDice()
    {
        if (rollInput.text != "")
            return int.Parse(rollInput.text);
        if (turn == Turn.dice)
            return Random.Range(2, 13);

        return 0;
    }

    IEnumerator MakeMove(int playerId, int move,Turn turn)
    {
        for (int i = 0; i < move; i++)
        {
            players[playerId].currentAreaId++;
            if (players[playerId].currentAreaId > mapGenerator.mapBlocks.Count - 1)
            {
                players[playerId].currentAreaId = 0;
                PlayerGoThroughStart();
            }
            players[playerId].player.transform.position = mapGenerator.mapBlocks[players[playerId].currentAreaId].transform.position;
            PlayerDirection(playerId);
            yield return new WaitForSeconds(0.15f);
        }
        SetTurn(turn);
    }

    void PlayerDirection(int playerId)
    {

        float rotation;
        if (players[playerId].currentAreaId >= 8 && players[playerId].currentAreaId < 16)
            rotation = 90f;
        else if (players[playerId].currentAreaId >= 16 && players[playerId].currentAreaId < 24)
            rotation = 180f;
        else if (players[playerId].currentAreaId >= 24 && players[playerId].currentAreaId < 32)
            rotation = 270f;
        else
            rotation = 0f;

        players[playerId].player.transform.localRotation = Quaternion.Euler(new Vector3(0f, rotation, 0f));
    }


    public void CheckPlayerAnswer(Question question, string answer, Plot plot)
    {
        bool correct = question.isCorrect(answer);

        if (correct)
            players[currentPlayerId].rightAnswers++;
        else
            players[currentPlayerId].wrongAnswers++;

        bool plotHasOwner = plot.hasOwner();
        if (!IsPlayerInPrison())
        {
            if (correct && plotHasOwner) { }//skip
                                            //DealDamage(currentPlayerId, plot.ownerId, plot.damage);
            else if (correct && !plotHasOwner)
                plot.SetOwner(players[currentPlayerId],currentPlayerId);
            else if (!correct && plotHasOwner)
                DealDamage(plot.ownerId, currentPlayerId, plot.damage);
            else if (!correct && !plotHasOwner) { }//skip
        }
        else
        {
            if (correct)
            {
                players[currentPlayerId].isInPrison = false;
                StartCoroutine(uiManager.End(uiManager.propertyUI, 1.5f, Turn.dice));
                return;
            }
        }

        if(correct && question.category == "wiezienie")
            StartCoroutine(uiManager.End(uiManager.propertyUI, 1.5f, Turn.dice,UIManager.TextOnEndTurn.rewarded));
        else if (correct)
            StartCoroutine(uiManager.End(uiManager.propertyUI, 1.5f, Turn.nextTurn, UIManager.TextOnEndTurn.rewarded));
        else
            StartCoroutine(uiManager.ExplainToPlayer(plot,question.explanation,question.question, 1.5f));
    }

    private List<PlayerInfo> playerInfosToShowAnimation;

    public void DealDamage(int winningId, int loosingId,int amount)
    {
        playerInfosToShowAnimation = new List<PlayerInfo>();
        players[winningId].hp += amount;
        players[winningId].playerInfo.GetComponent<PlayerInfo>().ShowAnimation($"+{amount}",Color.green);
        players[loosingId].hp -= amount;
        players[loosingId].playerInfo.GetComponent<PlayerInfo>().ShowAnimation($"-{amount}",Color.red);

        playerInfosToShowAnimation.Add(players[winningId].playerInfo.GetComponent<PlayerInfo>());
        playerInfosToShowAnimation.Add(players[loosingId].playerInfo.GetComponent<PlayerInfo>());

        SetPlayersUI();
    }
    public float strength=1f;
    public void ShowPlayerInfoAnimations()
    {
        if(playerInfosToShowAnimation!=null && playerInfosToShowAnimation.Count > 0)
        {
            foreach(PlayerInfo pi in playerInfosToShowAnimation)
            {
                pi.PlayAnimation();
            }

            GameObject coin = Instantiate(this.coin, playerInfosToShowAnimation[1].coin.position,playerInfosToShowAnimation[1].coin.rotation);

            float time = 0.65f;
            coin.transform.DOJump(playerInfosToShowAnimation[0].dealHp.transform.position,strength,1, time);

            Destroy(coin, time);

            playerInfosToShowAnimation.Clear();
        }
    }

    public void DealDamage(Player player,int amount)
    {
        player.hp -= amount;
    }


    public void PlayerGoThroughStart()
    {
        players[currentPlayerId].hp++;
        StartCoroutine(uiManager.SendMessage($"Gracz {players[currentPlayerId].name} zyskuje monetkê", 1.5f));
        SetPlayersUI();
    }

    public void PlayerGoToPrison()
    {
        players[currentPlayerId].isInPrison = true;
        StartCoroutine(uiManager.SendMessage($"Gracz {players[currentPlayerId].name} trafia do wiêzienia", 1.5f));
        SetTurn(Turn.nextTurn);
    }

    public bool IsPlayerInPrison()
    {
        return players[currentPlayerId].isInPrison;
    }

    public void SetPlayersUI()
    {
        foreach(Player player in players)
        {
            player.playerInfo.GetComponentInChildren<TextMeshPro>().text = player.hp.ToString();
        }
    }

    public int GetCurrentPlayerTurnId()
    {
        return currentPlayerId;
    }


    public Player GetCurrentPlayer()
    {
        return players[currentPlayerId];
    }

    public List<Player> GetPlayers()
    {
        return players;
    }

    public List<Player> GetPlayersWithoutCurrentId()
    {
        List<Player> pls = new List<Player>();
        for(int i = 0; i < players.Count; i++)
        {
            if (i != currentPlayerId)
                pls.Add(players[i]);
        }
        return pls;
    }

    public List<Area> GetPlotsWherePlayerCanTeleport()
    {
        List<Area> areaList = new List<Area>();

        foreach(GameObject areaObject in mapGenerator.mapBlocks)
        {
            Plot plot = areaObject.GetComponent<Plot>();
            if (plot.type == AreaType.ToBuy && (!plot.hasOwner() || plot.ownerId == currentPlayerId))
                areaList.Add(plot);
        }
        
        return areaList;
    }

    public int CountDistance(int areaId, int destinationAreaId)
    {
        int distance = (mapGenerator.mapBlocks.Count + destinationAreaId) -areaId;
        return distance;
    }

    public void ChooseDoubleDamage(Area area)
    {
        area.damage = area.damage * 2;
        area.SetBlock();
        StartCoroutine(uiManager.End(uiManager.selectUI, 0f, Turn.nextTurn));
    }

    public void ChooseTeleport(Area area)
    {
        move = CountDistance(players[currentPlayerId].currentAreaId, area.areaId);
        StartCoroutine(uiManager.End(uiManager.selectUI, 0f, Turn.move));
    }

    public void RemovePlotFromPlayer(int ownerId, Area areaToDelete)
    {
        //areaToDelete.GetComponent<Plot>().ClearOwner();
        players[ownerId].properties.Remove(areaToDelete);
    }
}
