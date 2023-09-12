using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Scoreboard : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreboard;

    private void Start()
    {
        scoreboard.text = "Click to Play\n\n";
        string data = PlayerPrefs.GetString("scores", null);
        if (data == null || data.Length == 0)
        {
            scoreboard.text += "Be the first!";
            return;
        }
        string[] parsed = data.Split(';');
        for (int i = 0; i < parsed.Length && i < 10; ++i)
        {
            scoreboard.text += i + ". " + parsed[i] + "\n";
        }
    }

    private void OnMouseDown()
    {
        SceneManager.LoadScene("MainScene");
    }
}
