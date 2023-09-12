using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI ui;
	[SerializeField] private GameObject prefab;
	[SerializeField] private Sprite yay, nay;

	private int score = 0, lives = 3;
	private float spawnRate = 5, bubbleLifespan = 12, difficultyIncrease = 30, timer1 = 0, timer2 = 0;
	private const float xBound = 6.55f, yBound = 2.56f;
	// Common things typed while coding (or related things). Not language specific. Definitely missing some.
	private static readonly string[] common = {
		"public",
		"static",
		"void",
		"main",
		"string",
		"args",
		"()",
		"[]",
		"{}",
		"!",
		"#",
		"%",
		"&&",
		"*",
		"-",
		"_",
		"+",
		"=",
		"-",
		"+=",
		"-=",
		"*=",
		"/",
		"/=",
		"%=",
		"++",
		"--",
		"==",
		"!=",
		"||",
		"//",
		"/* */",
		"int",
		"float",
		"double",
		"array",
		"list",
		"set",
		"hashmap",
		"object",
		"private",
		"protected",
		"git",
		"commit",
		":",
		";",
		"\"",
		"\'",
		",",
		".",
		"<",
		">",
		"<=",
		">=",
		"[0]",
		"for",
		"while",
		"do",
		"switch",
		"case",
		"for (int i = 0; i < n; ++i)",
		"? :",
		"\\n",
		"\\t",
		"if",
		"else if",
		"else",
		"new",
		"return",
		"class"
	};
	private readonly List<Bubble> bubbles = new();
	private readonly HashSet<string> inUse = new();
	private string previousStuff = "";

	private IEnumerator Pop(GameObject bubble)
	{
		yield return new WaitForSeconds(1);
		Destroy(bubble);
	}

	private void Update()
	{
		timer1 += Time.deltaTime;
		timer2 += Time.deltaTime;
		string stuff = Input.inputString;
		if (stuff == previousStuff)
		{
			stuff = "";
		}
		previousStuff = stuff;
		if (timer1 > spawnRate)
		{
			timer1 = 0;
			GameObject obj = Instantiate(prefab);
			obj.transform.position = new Vector2(Random.Range(-xBound, xBound), Random.Range(-yBound, yBound));
			string s;
			do
			{
				s = common[Random.Range(0, common.Length)];
			} while (inUse.Contains(s) && inUse.Count < common.Length);
			inUse.Add(s);
			Bubble bubble = new();
			bubble.bubble = obj;
			bubble.life = bubbleLifespan;
			bubble.str = bubble.origin = s;
			bubble.text = bubble.bubble.GetComponentInChildren<TextMeshPro>();
			bubble.renderer = bubble.bubble.GetComponent<SpriteRenderer>();
			bubble.text.text = s;
			bubbles.Add(bubble);
		}
		for (int i = 0; i < bubbles.Count; ++i)
		{
			Bubble bubble = bubbles[i];
			bubble.life -= Time.deltaTime;
			if (bubble.life < 0)
			{
				bubble.renderer.sprite = nay;
				StartCoroutine(Pop(bubble.bubble));
				inUse.Remove(bubble.origin);
				bubbles.Remove(bubble);
				--lives;
				ui.text = $"Score: {score}    Lives: {lives}";
			}
			if (lives <= 0)
			{
				lives = 1000;
				string user = PlayerPrefs.GetString("currentUser");
				string data = PlayerPrefs.GetString("scores");
				List<string> parsed = new(data.Split(';'));
				if (parsed[0] == "")
				{
					parsed.RemoveAt(0);
				}
				Debug.Log("Data: " + data);
				string sortScore = $"{score}";
				while (sortScore.Length < 3)
				{
					sortScore = "0" + sortScore;
				}
				parsed.Add($"{sortScore} {user}");
				parsed.Sort();
				string scoreboard = "";
				foreach (string j in parsed)
				{
					scoreboard += j + ";";
				}
				scoreboard = scoreboard.Remove(scoreboard.Length - 1);
				PlayerPrefs.SetString("scores", scoreboard);
				SceneManager.LoadScene("Title");
			}
			foreach (char c in stuff)
			{
				if (bubble.str.StartsWith(c))
				{
					bubble.str = bubble.str.Remove(0, 1);
					bubble.text.text = bubble.str;
				}
			}
			if (bubble.str.Length == 0)
			{
				bubble.renderer.sprite = yay;
				StartCoroutine(Pop(bubble.bubble));
				inUse.Remove(bubble.origin);
				bubbles.Remove(bubble);
				++score;
				ui.text = $"Score: {score}    Lives: {lives}";
			}
		}
		if (timer2 > difficultyIncrease && spawnRate > 1)
		{
			timer2 = 0;
			--spawnRate;
			bubbleLifespan -= 2;
		}
	}

	private class Bubble
	{
		public GameObject bubble;
		public float life;
		public string str, origin;
		public TextMeshPro text;
		public SpriteRenderer renderer;
	}
}
