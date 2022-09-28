using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    //[SerializeField] private Player playersTurn;
    private int currentPlayerId;
    [SerializeField] private MapGenerator mapGenerator;
    [SerializeField] private List<GameObject> playerPrefabs;
    [SerializeField] private GameObject playerInfoPrefab;
    [SerializeField] private CameraFollow cameraFollow;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private Button rollButton;

    [HideInInspector] private Turn turn = Turn.none;
    [HideInInspector] private List<Player> players;

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

        for (int i = 0; i < 2; i++)
        {
            Player p = new Player();
            p.name = Random.Range(0, 100).ToString();
            p.playerColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
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
        Debug.Log("Zmiana na " + turn);
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
                SetPlayersUI();
                uiManager.RollButton(true);

                rollButton.onClick.RemoveAllListeners();
                rollButton.onClick.AddListener(()=>{
                    move = RollDice();
                    uiManager.RollButton(false,move);
                    SetTurn(Turn.move);
                });
                break;
            case Turn.move:
                StartCoroutine(MakeMove(currentPlayerId, move));
                break;
            case Turn.action:
                int blockId = players[currentPlayerId].currentAreaId;
                Area block = mapGenerator.mapBlocks[blockId].GetComponent<Area>();
                block.StepOn();
                break;
            case Turn.nextTurn:
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
            PlayerDirection(playerId);
            yield return new WaitForSeconds(0.5f);
        }
        SetTurn(Turn.action);
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
        bool plotHasOwner = plot.hasOwner();
        if (correct && plotHasOwner) { }//skip
            //DealDamage(currentPlayerId, plot.ownerId, plot.damage);
        else if(correct && !plotHasOwner)
            plot.SetOwner(currentPlayerId, players[currentPlayerId].playerColor);
        else if(!correct && plotHasOwner)
            DealDamage(plot.ownerId, currentPlayerId, plot.damage);
        else if(!correct && !plotHasOwner) { }//skip
    }

    public void DealDamage(int winningId, int loosingId,int amount)
    {
        players[winningId].hp += amount;
        players[loosingId].hp -= amount;
        SetPlayersUI();
    }


    void SetPlayersUI()
    {
        foreach(Player player in players)
        {
            player.playerInfo.GetComponentInChildren<TextMeshPro>().text = player.hp.ToString();
        }
    }

}
