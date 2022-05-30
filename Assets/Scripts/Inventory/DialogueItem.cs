using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu(menuName = "InventoryObject/DialogueItem")]

public class DialogueItem : ScriptableObject
{
	[SerializeField] private List<Replic> replics;
	public List<Replic> Replics
	{
		get => replics;
	}
}

[System.Serializable]
public class Replic
{
	[SerializeField] private bool isPlayerReplic;
	public bool IsPlayerReplic
	{
		get => isPlayerReplic;
	}
	
	[SerializeField] private string characterName;
	public string CharacterName
	{
		get => characterName;
	}
	
	[SerializeField] private Sprite characterPortrait;
	public Sprite CharacterPortrait
	{
		get => characterPortrait;
	}

	[TextArea(5, 10)]
	[SerializeField] private string replicText;
	public string ReplicText
	{
		get => replicText;
	}

	[SerializeField] private bool isAcceptBttnEnabled;
	public bool IsAcceptBttnEnabled
	{
		get => isAcceptBttnEnabled;
	}
	
	[SerializeField] private bool isDeclineBttnEnabled;
	public bool IsDeclineBttnEnabled
	{
		get => isDeclineBttnEnabled;
	}
	
	[SerializeField] private bool isDeclineBttnFinishDialogue;
	public bool IsDeclineBttnFinishDialogue
	{
		get => isDeclineBttnFinishDialogue;
	}

	[SerializeField] private string acceptBttnText;
	public string AcceptBttnText
	{
		get => acceptBttnText;
	}
	[SerializeField] private string declineBttnText;
	public string DeclineBttnText
	{
		get => declineBttnText;
	}



}