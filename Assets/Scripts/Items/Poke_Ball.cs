using UnityEngine;
using System.Collections;

[System.Serializable]
public class Poke_Ball : Item
{
	
	public Sprite icon;
	public GameObject model;
	public float catchRate;
	public Poke_Ball_Types pokeBallType;
	
	public Poke_Ball(string _name, string _description, float _catchRate, int _cost, int _worth, Poke_Ball_Types _pokeBallType, Item_Types _type)
	{
		name = _name;
		description = _description;
		icon = Resources.Load<Sprite>("Sprites/PokeBalls/" + name);
		catchRate = _catchRate;
		cost = _cost;
		worth = _worth;
		pokeBallType = _pokeBallType;
		type = _type;
	}
	
	public Poke_Ball()
	{
		
	}
}

public enum Poke_Ball_Types{ CHERISHBALL, DIVEBALL, DREAMBALL, DUSKBALL, FASTBALL, FRIENDBALL, GREATBALL, HEALBALL, HEAVYBALL, LEVELBALL, LOVEBALL, LUREBALL, LUXURYBALL, MASTERBALL,
								MOONBALL, NESTBALL, NETBALL, PARKBALL, POKEBALL, PREMIERBALL, QUICKBALL, REPEATBALL, SAFARIBALL, SPORTBALL, TIMERBALL, ULTRABALL }