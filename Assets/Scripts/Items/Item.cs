using UnityEngine;
using System.Collections;

[System.Serializable]
public class Item {
	
	public string name;
	public string description;
	public int cost;
	public int worth;
	public Items_Types itemType;
	public Item_Types type;
	
	public Item(string _name, string _description, int _cost, int _worth, Items_Types _itemType, Item_Types _type)
	{
		name = _name;
		description = _description;
		cost = _cost;
		worth = _worth;
		itemType = _itemType;
		type = _type;
	}
	
	public Item()
	{
		
	}
	
}
public enum Item_Types{ AESTHETIC, BERRYANDAPRICORN, ITEM, MEDICINE, OTHER }
public enum Items_Types{ NONE, ESCAPE, EVOLUTION_STONE, EXCHANGEABLE, FLUTE, FOSSIL, HELD, LEGENDARY_ARTIFACT, REPEL, SHARD, VALUABLE }
