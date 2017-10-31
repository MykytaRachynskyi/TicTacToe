using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneLoad : MonoBehaviour {

	void Start ()
	{
	}

	public void LoadMenu ()
	{
		SceneManager.LoadScene("menu");
	}

	public void StartGame ()
	{
		SceneManager.LoadScene("game");
	}

}
