using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu(menuName = "Levels/LevelsList")]

[System.Serializable]
public class Levels : ScriptableObject
{
	[SerializeField] private List<Level> levelsList;
	public List<Level> LevelsList
	{
		get => levelsList;
	}

}

[System.Serializable]
public class Level
{
	[SerializeField] private bool underConstructon = false;
	public bool UnderConstructon
	{
		get => underConstructon;
		set => underConstructon = value;
	}

	[SerializeField] private int levelNum;
	public int LevelNum
	{
		get => levelNum;
	}
	
	[SerializeField] private string levelSceneName;
	public string LevelSceneName
	{
		get => levelSceneName;
	}

	[SerializeField] private int levelCost = 10;
	public int LevelCost
	{
		get => levelCost;
		set => levelCost = value;
	}

	[SerializeField] private int fruitsMax = 50;
	public int FruitsMax
	{
		get => fruitsMax;
	}

	[SerializeField] private int fruitsCollected = 0;
	public int FruitsCollected
	{
		get => fruitsCollected;
		set
		{
			if (value > fruitsCollected)
            {
				fruitsCollected = value;
            }
		}
	}

	[SerializeField] private bool noDamadge = false;
	public bool NoDamadge
	{
		get => noDamadge;
		set
		{
			if (value)
            {
				noDamadge = value;
            }
		}
	}

	[SerializeField] private int secondsRecord = 60;
	public int SecondsRecord
	{
		get => secondsRecord;
	}
	
	[SerializeField] private int secondsSpend = 0;
	public int SecondsSpend
	{
		get => secondsSpend;
		set => secondsSpend = value;
	}
	

	[SerializeField] private bool[] coinEnabled;
	public bool[] CoinEnabled
	{
		get => coinEnabled;
		set => coinEnabled = value;
	}
	


}