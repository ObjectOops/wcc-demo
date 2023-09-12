using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Scoreboard : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI scoreboard;
	[SerializeField] private TMP_InputField input;

	private void Start()
	{
		scoreboard.text = "Press Enter to Play\n\n";
		string data = PlayerPrefs.GetString("scores", null);
		Debug.Log("Data: " + data);
		if (data == null || data.Length == 0)
		{
			scoreboard.text += "Be the first!";
			return;
		}
		string[] parsed = data.Split(';');
		for (int j = 1, i = parsed.Length - 1; i >= parsed.Length - 9 && i >= 0; --i, ++j)
		{
			scoreboard.text += j + ". " + parsed[i] + "\n";
		}
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Return))
		{
			PlayerPrefs.SetString("currentUser", input.text);
			Debug.Log("Playing as: " + input.text);
			SceneManager.LoadScene("MainScene");
		}
	}
}
