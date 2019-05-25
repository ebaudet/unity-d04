using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class profilUser : MonoBehaviour
{
	public int			rings = 0;
	public int			lifeLosed = 0;
	public List<Level>	levels = null;

	public Text			countLifeLosed;
	public Text			countRings;
	
	void Awake () {
		if (PlayerPrefs.GetInt("rings") != 0)
			rings = PlayerPrefs.GetInt("rings");
		if (PlayerPrefs.GetInt("lifeLosed") != 0)
			lifeLosed = PlayerPrefs.GetInt("lifeLosed");
		if (PlayerPrefs.GetString("levels") != null) {
			foreach (string level_string in PlayerPrefs.GetString("levels").Split(',')) {
				if (level_string == "")
					continue;
				Debug.Log("coucou: "+level_string);
				levels.Add(new Level(level_string));
			}
		}
	}

	private void Start() {
		SetCountLifeLosed();
		SetCountRings();
	}

	private void Update() {
		if (Input.GetKeyDown(KeyCode.R)) {
			rings += 1;
			SetCountRings();
			SetRings();
		}
		if (Input.GetKeyDown(KeyCode.L)) {
			lifeLosed += 1;
			SetLifeLosed();
			SetCountLifeLosed();
		}
		if (Input.GetKeyDown(KeyCode.Backspace)) {
			Reset();
			SetCountLifeLosed();
			SetCountRings();
		}
	}

	public void UpdateAll() {
		SetRings();
		SetLifeLosed();
		SetLevels();
	}
	
	public void Reset() {
		rings = 0;
		SetRings();
		lifeLosed = 0;
		SetLifeLosed();
		levels = null;
		SetLevels();
		Save();
	}

	public void UpdateLevel(int levelId, int bestScore) {
		addLevel(levelId, bestScore);
		SetLevels();
	}

	public void SetLevels() {
		string levelToStore = "";
		int count = 0;
		if (levels != null) {
			foreach (Level level in levels) {
				if (count > 0)
					string.Concat(levelToStore, ",");
				string.Concat(levelToStore, level.ToString());
				count++;
			}
		}
		PlayerPrefs.SetString("levels", levelToStore);
	}

	public void SetRings() {
		PlayerPrefs.SetInt("rings", rings);
	}

	public void SetLifeLosed() {
		PlayerPrefs.SetInt("lifeLosed", lifeLosed);
	}

	public void Save() {
		PlayerPrefs.Save();
	}

	public void SetCountLifeLosed() {
		countLifeLosed.text = lifeLosed.ToString();
	}

	public void SetCountRings() {
		countRings.text = rings.ToString();
	}

	/***************** PRIVATE *****************/

	private void OnDisable() {
		Save();
	}

	private void addLevel(int levelId, int bestScore) {
		foreach (Level level in levels) {
			if (level.levelId == levelId) {
				level.bestScore = bestScore;
				return;
			}
		}
		levels.Add(new Level(levelId, bestScore));
	}
}

public class Level {
	public int levelId;
	public int bestScore;

	public Level(int level_id, int best_score) {
		levelId = level_id;
		bestScore = best_score;
	}

	public Level(string level_string) {
		if (level_string == "")
			return;
		Debug.Log(level_string);
		Int32.TryParse(level_string.Split('_')[0], out levelId);
		Int32.TryParse(level_string.Split('_')[1], out bestScore);
	}

	override public string ToString() {
		return levelId.ToString() + "_" + bestScore.ToString();
	}
}
