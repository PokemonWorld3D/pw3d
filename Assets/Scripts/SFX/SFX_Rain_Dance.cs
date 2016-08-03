using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class SFX_Rain_Dance : NetworkBehaviour
{
	void OnTriggerEnter(Collider col)
	{
		if(!isServer)
			return;

		if(col.gameObject.GetComponent<Pokemon>())
		{
			Pokemon pokemon = col.gameObject.GetComponent<Pokemon>();

			foreach(Move move in pokemon.KnownMoves)
			{
				if(move.moveType == PokemonType.WATER)
					move.PowerIncrease(0.5f);
				if(move.moveType == PokemonType.FIRE || move.name == "Solar Beam")
					move.PowerDecrease(0.5f);
			}
		}
	}
	void OnTriggerExit(Collider col)
	{
		if(!isServer)
			return;

		if(col.gameObject.GetComponent<Pokemon>())
		{
			Pokemon pokemon = col.gameObject.GetComponent<Pokemon>();

			foreach(Move move in pokemon.KnownMoves)
			{
				if(move.moveType == PokemonType.WATER)
					move.PowerDecrease(0.5f);
				if(move.moveType == PokemonType.FIRE || move.name == "Solar Beam")
					move.PowerIncrease(0.5f);
			}
		}
	}
}
