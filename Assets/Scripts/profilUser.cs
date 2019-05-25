using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level {
	public int levelId;
	public int bestScore;

	public Level(int level_id, int best_score) {
		levelId = level_id;
		bestScore = best_score;
	}

	public Level(string level_string) {
		Int32.TryParse(level_string.Split('_')[0], out levelId);
		Int32.TryParse(level_string.Split('_')[1], out bestScore);
	}

	override public string ToString() {
		return levelId.ToString() + "_" + bestScore.ToString();
	}
}

public class profilUser : MonoBehaviour
{
	public int			rings = 0;
	public int			lifeLosed = 0;
	public List<Level>	levels = null;
	
	void Awake () {
		if (PlayerPrefs.GetInt("rings") != 0)
			rings = PlayerPrefs.GetInt("rings");
		if (PlayerPrefs.GetInt("lifeLosed") != 0)
			lifeLosed = PlayerPrefs.GetInt("lifeLosed");
		if (PlayerPrefs.GetString("levels") != null) {
			foreach (string level in PlayerPrefs.GetString("levels").Split(',')) {
				levels.Add(new Level(level));
			}
		}
	}

	private void OnDisable() {
		Save();
	}

	public void UpdateLevel(int levelId, int bestScore) {
		addLevel(levelId, bestScore);
		string levelToStore = "";
		int count = 0;
		foreach (Level level in levels) {
			if (count > 0)
				string.Concat(levelToStore, ",");
			string.Concat(levelToStore, level.ToString());
			count++;
		}
		PlayerPrefs.SetString("levels", levelToStore);
	}

	public void UpdateRings(int ring) {
		PlayerPrefs.SetInt("rings", ring);
	}

	public void UpdateLifeLosed(int lifeLosed) {
		PlayerPrefs.SetInt("lifeLosed", lifeLosed);
	}

	public void Save() {
		PlayerPrefs.Save();
	}

	/***************** PRIVATE *****************/
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
