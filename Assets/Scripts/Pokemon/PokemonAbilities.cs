using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PokemonAbilities : MonoBehaviour
{
	[SerializeField] private Abilities ability, hiddenAbility;

	private enum Abilities { NONE, Adaptability, Aerilate, Aftermath, Air_Lock, Analytic, Anger_Point, Anticipation, Arena_Trap, Aroma_Veil, Aura_Break, Bad_Dreams, Battle_Armor,
		Big_Pecks, Blaze, Bulletproof };
	private Dictionary<Abilities, Action> AbilityFSM = new Dictionary<Abilities, Action>();
	private PokemonComponents components;

	void Awake()
	{
		components = GetComponent<PokemonComponents>();
	}
	void Update()
	{
		
	}

	private void Adaptability()
	{
		
	}
	private void Aerialte()
	{
		foreach(Move move in components.pokemon.KnownMoves)
		{
			if(move.moveType == PokemonType.NORMAL)
			{
				move.moveType = PokemonType.FLYING;
				move.curPower = (int)((float)move.curPower * 1.30f);
			}
		}
	}
	private void Aftermath()
	{
		
	}
	private void Air_Lock()
	{
		
	}
}
