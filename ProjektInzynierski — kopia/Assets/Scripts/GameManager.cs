using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Player
{
    public string name;
    public int characterId;
    public List<Area> properties;
    [HideInInspector] public int hp;
    [HideInInspector] public GameObject player;
    [HideInInspector] public int currentAreaId=0;
}

public enum Turn { 
    none,
    dice,
    action,
    nextTurn
}

public class GameManager : MonoBehaviour
{
    //[SerializeField] private Player playersTurn;
    private int currentPlayerId;
    [SerializeField] private MapGenerator mapGenerator;
    [SerializeField] private List<GameObject> playerPrefabs;
    [SerializeField] private List<Player> players;

    [SerializeField] private CameraFollow cameraFollow;
    [SerializeField] private Button rollButton;

    [SerializeField] private Turn turn = Turn.none;

    // Start is called before the first frame update
    void Start()
    {
        LoadPlayers(TestData());
        CreatePlayers();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    List<Player> TestData()
    {
        List<Player> data = new List<Player>();

        for (int i = 0; i < 3; i++)
        {
            Player p = new Player();
            p.name = Random.Range(0, 100).ToString();
            data.Add(p);
        }
        
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
            p.hp = 20;
        }
        ////////
        SetNewTarget(Random.Range(0, players.Count));
        SetTurn(Turn.dice);

    }

    int move = 0;

    void SetTurn(Turn turn)
    {
        this.turn = turn;
        Debug.Log("Zmiana na " + turn);
        HandleTurn();
    }

    void SetNewTarget(int playerId)
    {
        currentPlayerId = playerId;
        cameraFollow.SetTarget(players[currentPlayerId].player.transform);
    }

    void HandleTurn()
    {
        switch (turn)
        {
            case Turn.dice:
                if (!rollButton.gameObject.activeSelf)
                    rollButton.gameObject.SetActive(true);

                rollButton.onClick.RemoveAllListeners();
                rollButton.onClick.AddListener(()=>{
                    move = RollDice();
                    rollButton.gameObject.SetActive(false);
                    SetTurn(Turn.action);
                });
                break;
            case Turn.action:
                StartCoroutine(MakeMove(currentPlayerId, move));
                //SetTurn(Turn.nextTurn);
                break;
            case Turn.nextTurn:
                int id = currentPlayerId;
                id++;

                Debug.Log("b: " + (id - 1) + " n: " + id);
                if (id > players.Count-1)
                    currentPlayerId = 0;
                else
                    currentPlayerId = id;

                SetNewTarget(currentPlayerId);
                SetTurn(Turn.dice);
                break;
            case Turn.none:
                break;

        }
    }

    public int RollDice()
    {
        if (turn == Turn.dice)
            return Random.Range(2, 13);

        return 0;
    }

    IEnumerator MakeMove(int playerId, int move)
    {
        for (int i = 0; i < move; i++)
        {
            players[playerId].currentAreaId++;
            if (players[playerId].currentAreaId > mapGenerator.mapBlocks.Count-1)
                players[playerId].currentAreaId = 0;
            players[playerId].player.transform.position = mapGenerator.mapBlocks[players[playerId].currentAreaId].transform.position;
            yield return new WaitForSeconds(0.5f);
        }
        SetTurn(Turn.nextTurn);
    }




}
