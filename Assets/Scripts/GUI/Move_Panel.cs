using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Move_Panel : MonoBehaviour
{
	public RectTransform scrollPanel;
	public List<GameObject> MoveIcons;
	public RectTransform center;
	public int iconDistance, activeMoveIndex;
	public GameObject movePrefab;

	[HideInInspector] public Pokemon pokemon;

	public void Setup()
	{
		for(int i = 0; i < pokemon.KnownMoves.Count; i++)
		{
			GameObject moveIcon = Instantiate(movePrefab) as GameObject;

			moveIcon.transform.SetParent(scrollPanel.transform);

			int x = i * iconDistance;
			Vector2 position = new Vector2(x, 2.0f);

			moveIcon.GetComponent<RectTransform>().anchoredPosition = position;
			moveIcon.GetComponent<Move_Icon>().SetupIcon(pokemon.KnownMoves[i]);

			MoveIcons.Add(moveIcon);
		}
	}
	public void Clear()
	{
		foreach(GameObject g in MoveIcons)
			Destroy(g);

		MoveIcons.Clear();
	}
	public void AddMove()
	{
		
	}
	public void UpdateActiveMove(int activeMoveIndex)
	{
		this.activeMoveIndex = activeMoveIndex;
		SnapToIcon(activeMoveIndex * -iconDistance);
	}
		
	private void SnapToIcon(int position)
	{
		Vector2 newPosition = new Vector2(position, scrollPanel.anchoredPosition.y);

		scrollPanel.anchoredPosition = newPosition;
	}
}
