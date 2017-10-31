using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonScript : MonoBehaviour {
	//publics
	public GameObject crossL, crossR, circle;
	public int index;
	public enum Value {none, circle, cross};
	public Value cellValue;
	public float speed = 10.0f;
	public float animSpeed = 2f;
	public bool draw;
	public AudioClip crossSFX, circleSFX;
	//statics
	public static int copyIndex = 0;
	private static GameObject[] copy = new GameObject[15];
	//privates
	[SerializeField]private bool localTurn;
	[SerializeField]private static int activeCount = 0;
	private GameObject playerGMturn = null;
	private Button btn = null;
	private GMScript gmScript;
	private Animation anim;
	private AudioSource aSrc;

	void Start ()
	{
		//Creating Button reference and listener for onClick
		btn = this.GetComponent<Button> ();
		btn.onClick.AddListener (Draw);

		//Creating a reference to the Master Script
		playerGMturn = GameObject.Find ("_GM");
		gmScript = playerGMturn.GetComponent<GMScript>();

		aSrc = GetComponent<AudioSource>();
		draw = false;
	}
	void Update ()
	{

	}

	void Draw ()
	{
		if (Input.GetMouseButtonUp (0)) {
			localTurn = gmScript.playerOneTurn;
			if (localTurn) {
				DrawSign ("cross");
				this.cellValue = Value.cross;
			} else {
				DrawSign ("circle");
				this.cellValue = Value.circle;
			}
			if (activeCount == 9) {
				gmScript.Tie();
			} else if (activeCount > 4)
				gmScript.CheckVictory (localTurn);
		}
	}

	void DrawSign (string sign)
	{
		Debug.Log(copyIndex);
		if (sign == "circle") {
			aSrc.PlayOneShot(circleSFX);
			MakeCopy(circle);
			UpdateTurnInfo();
		} else if (sign == "cross") {
			aSrc.PlayOneShot(crossSFX);
			StartCoroutine(CrossAnim());
			UpdateTurnInfo();
		}
	}

	IEnumerator CrossAnim ()
	{
		CrossAnim("left");
		yield return new WaitForSeconds(1f/animSpeed);
		CrossAnim("right");
	}

	void CrossAnim (string side)
	{
		if (side == "left") MakeCopy(crossL);
		else if (side == "right") MakeCopy(crossR);
	}

	void MakeCopy(GameObject sign)
	{
		copy[copyIndex] = Instantiate (sign, Vector3.zero, Quaternion.identity, this.transform) as GameObject;
		copy[copyIndex].transform.localPosition = Vector2.zero;
		anim = copy[copyIndex].GetComponent<Animation> ();
		anim ["DrawLeft"].speed = animSpeed;
		copyIndex++;
	}

	void UpdateTurnInfo ()
	{
		gmScript.TurnSwitch ();
		btn.interactable = false;
		activeCount++;
	}

	public static void Clear ()
	{
		for (int i = 0; i < copy.Length; i++) {
			Destroy(copy[i]);
			copy[i] = null;
		} 
		copyIndex = 0;
		activeCount = 0;
	}
}
