using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GMScript : MonoBehaviour {

	public bool playerOneTurn;
	public GameObject boardBlock, gridLine;
	public Transform canvasParent;
	public int activeCount = 0;
	public float blockSize = 250f;
	public float boardOffset = 500f;
	public CanvasGroup victoryScreen, boardScreen;
	public Text victoryText;
	public float waitTime;
	public GameObject line;
	public float animSpeed = 2f;

	private GameObject copy;
	private GameObject lineHolder, gridHolder;
	private Text text;
	private GameObject textGOobj;
	private GameObject[] clone = new GameObject[9];
	private ButtonScript[] instance = new ButtonScript[9];
	private bool waited;
	private Animation anim;


	ButtonScript btnscrpt;

	void Awake ()
	{
		CreateBoard();
		playerOneTurn = true;
	}

	void Start ()
	{
		textGOobj = GameObject.FindWithTag("PlayerText");
		text = textGOobj.GetComponent<Text>();
		text.text = "Player 1";

		//Hide the Vicory Screen
		victoryScreen.alpha = 0f;
		victoryScreen.interactable = false;
		victoryScreen.blocksRaycasts = false;


		anim = line.GetComponent<Animation> ();
		anim ["DrawLeft"].speed = animSpeed;
	}

	void Update ()
	{
		
	}

	void DrawGrid (int i)
	{
		Vector3 gridPos = Vector3.zero;
		switch (i) {
			case 0:
				gridPos = new Vector3 (-blockSize/2,0 ,0) + Vector3.right * boardOffset;
				gridHolder=Instantiate (gridLine, gridPos, Quaternion.Euler(0,0,-90), canvasParent) as GameObject;
				gridHolder.transform.localPosition = gridPos;
				break;
			case 1:
				gridPos = new Vector3 (blockSize/2, 0 ,0) + Vector3.right * boardOffset;
				gridHolder=Instantiate (gridLine, gridPos, Quaternion.Euler(0,0,-90), canvasParent) as GameObject;
				gridHolder.transform.localPosition = gridPos;
				break;
			case 2:
				gridPos = new Vector3 (0, blockSize/2 ,0) + Vector3.right * boardOffset;
				gridHolder=Instantiate (gridLine, gridPos, Quaternion.Euler(0,0,0), canvasParent) as GameObject;
				gridHolder.transform.localPosition = gridPos;
				break;
			case 3:
				gridPos = new Vector3 (0, -blockSize/2 ,0) + Vector3.right * boardOffset;
				gridHolder=Instantiate (gridLine, gridPos, Quaternion.Euler(0,0,0), canvasParent) as GameObject;
				gridHolder.transform.localPosition = gridPos;
				break;
		}
	}



	void CreateBoard ()
	{
		Vector3 pos = Vector3.zero;
		for (int i = 0; i < 9; i++) {
			switch (i) {
			case 0:
				pos = new Vector3 (-blockSize, blockSize, 0) + Vector3.right * boardOffset;
				break;
			case 1:
				pos = new Vector3 (0, blockSize, 0) +	Vector3.right * boardOffset;
				break;
			case 2:
				pos = new Vector3 (blockSize, blockSize, 0) + Vector3.right * boardOffset;
				break;
			case 3:
				pos = new Vector3 (-blockSize, 0, 0) + Vector3.right * boardOffset;
				break;
			case 4:
				pos = new Vector3 (0, 0, 0) + Vector3.right * boardOffset;
				break;
			case 5:
				pos = new Vector3 (blockSize, 0, 0) +	Vector3.right * boardOffset;
				break;
			case 6:
				pos = new Vector3 (-blockSize, -blockSize, 0) + Vector3.right * boardOffset;
				break;
			case 7:
				pos = new Vector3 (0, -blockSize, 0) +	Vector3.right * boardOffset;
				break;
			case 8:
				pos = new Vector3 (blockSize, -blockSize, 0) + Vector3.right * boardOffset;
				break;
			}
			BoardBlockInfo (i, pos);
			if (i<4)
				DrawGrid(i);
		}
	}

	public void TurnSwitch ()
	{
		playerOneTurn = !playerOneTurn;
		if (playerOneTurn) text.text = "Player 1"; else text.text = "Player 2";
	}

	public void CheckVictory(bool pOneVictory)
	{
		Debug.Log("Checking Victory...");
			 if (instance[0].cellValue != ButtonScript.Value.none && 
			     instance[0].cellValue == instance[1].cellValue && 
				 instance[0].cellValue == instance[2].cellValue)
				 StartCoroutine(GameOver(false, pOneVictory, "hTop"));
		else if (instance[3].cellValue != ButtonScript.Value.none && 
				 instance[3].cellValue == instance[4].cellValue && 
				 instance[3].cellValue == instance[5].cellValue)
			     StartCoroutine(GameOver(false, pOneVictory, "hMid"));
		else if (instance[6].cellValue != ButtonScript.Value.none && 
				 instance[6].cellValue == instance[7].cellValue && 
				 instance[6].cellValue == instance[8].cellValue)
				 StartCoroutine(GameOver(false, pOneVictory,"hBot"));
		else if (instance[0].cellValue != ButtonScript.Value.none && 
				 instance[0].cellValue == instance[4].cellValue && 
				 instance[0].cellValue == instance[8].cellValue)
				 StartCoroutine(GameOver(false, pOneVictory, "dLR"));
		else if (instance[2].cellValue != ButtonScript.Value.none && 
				 instance[2].cellValue == instance[4].cellValue && 
				 instance[2].cellValue == instance[6].cellValue)
				 StartCoroutine(GameOver(false, pOneVictory, "dRL"));
		else if (instance[0].cellValue != ButtonScript.Value.none && 
				 instance[0].cellValue == instance[3].cellValue && 
				 instance[0].cellValue == instance[6].cellValue)
				 StartCoroutine(GameOver(false, pOneVictory, "vLeft"));
		else if (instance[1].cellValue != ButtonScript.Value.none && 
				 instance[1].cellValue == instance[4].cellValue && 
				 instance[1].cellValue == instance[7].cellValue)
				 StartCoroutine(GameOver(false, pOneVictory, "vMid"));
		else if (instance[2].cellValue != ButtonScript.Value.none && 
				 instance[2].cellValue == instance[5].cellValue && 
				 instance[2].cellValue == instance[8].cellValue)
				 StartCoroutine(GameOver(false, pOneVictory, "vRight"));
	}

	void BoardBlockInfo (int i, Vector3 pos)
	{
		clone[i] = Instantiate (boardBlock, pos, Quaternion.identity, canvasParent) as GameObject;
		instance[i] = clone[i].GetComponent<ButtonScript>();
		instance[i].index = i;
		clone[i].transform.localPosition = pos;
	}

	IEnumerator GameOver (bool tie, bool pOneVictory, string victoryIndex)
	{
		if (!tie) {
			boardScreen.interactable = false;
			boardScreen.blocksRaycasts = false;
			DrawLineAcross (victoryIndex);
			yield return new WaitForSeconds (waitTime);
			victoryScreen.alpha = 1f;
			victoryScreen.interactable = true;
			victoryScreen.blocksRaycasts = true;
			if (pOneVictory) {
				victoryText.text = ("Player 1 Wins!");
			} else if (!pOneVictory) {
				victoryText.text = ("Player 2 Wins!");
			}
		} else {
			boardScreen.interactable = false;
			boardScreen.blocksRaycasts = false;
			yield return new WaitForSeconds (waitTime);
			victoryScreen.alpha = 1f;
			victoryScreen.interactable = true;
			victoryScreen.blocksRaycasts = true;
			victoryText.text = ("It's a Tie!");
		}
	}

	public void Tie ()
	{
		StartCoroutine(GameOver(true, true, "null"));
	}

	public void ResetGame ()
	{
		victoryScreen.alpha = 0f;
		victoryScreen.interactable = false;
		victoryScreen.blocksRaycasts = false;
		boardScreen.interactable = true;
		boardScreen.blocksRaycasts = true;
		for (int i = 0; i < instance.Length; i++) {
			instance [i].cellValue = ButtonScript.Value.none;
			instance [i].GetComponent<Button> ().interactable = true;
		}
		playerOneTurn = true;
		ButtonScript.Clear();
		text.text = "Player 1";
		Destroy(lineHolder);
	}

	void DrawLineAcross (string victoryIndex)
	{
		if (victoryIndex != "tie") {
			switch (victoryIndex) {
			case "hTop":
				lineHolder = Instantiate (line, instance [1].transform.position, Quaternion.identity, instance [1].transform) as GameObject;
				break;
			case "hMid":
				lineHolder = Instantiate (line, instance [4].transform.position, Quaternion.identity, instance [4].transform) as GameObject;
				break;
			case "hBot":
				lineHolder = Instantiate (line, instance [7].transform.position, Quaternion.identity, instance [7].transform) as GameObject;
				break;
			case "dLR":
				lineHolder = Instantiate (line, instance [4].transform.position, Quaternion.Euler (0, 0, -45), instance [4].transform) as GameObject;
				break;
			case "dRL":
				lineHolder = Instantiate (line, instance [4].transform.position, Quaternion.Euler (0, 0, 180 + 45), instance [4].transform) as GameObject;
				break;
			case "vLeft":
				lineHolder = Instantiate (line, instance [3].transform.position, Quaternion.Euler (0, 0, -90), instance [1].transform) as GameObject;
				break;
			case "vMid":
				lineHolder = Instantiate (line, instance [4].transform.position, Quaternion.Euler (0, 0, -90), instance [1].transform) as GameObject;
				break;
			case "vRight":
				lineHolder = Instantiate (line, instance [5].transform.position, Quaternion.Euler (0, 0, -90), instance [1].transform) as GameObject;
				break;
			}
		}
	}
}
