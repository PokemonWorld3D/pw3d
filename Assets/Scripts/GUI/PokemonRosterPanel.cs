using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PokemonRosterPanel : MonoBehaviour
{
	[SerializeField] private Image[] EmptyPokeBalls, Pokemon, Selected;
	[SerializeField] private Color occupiedPokeBall;
	[SerializeField] private HUD hud;

	private int index = 0;

	public void Setup()
	{
		for(int i = 0; i < hud.trainer.PokemonRoster.Count; i++)
		{
			EmptyPokeBalls[i].color = occupiedPokeBall;
			Pokemon[i].sprite = Resources.Load<Sprite>("Pokemon Mini Sprites/" + hud.trainer.PokemonRoster[i].pokemonName);
			Pokemon[i].enabled = true;
		}

		index = index;
		UpdateSelectedPokemon(index);
	}
	public void UpdateSelectedPokemon(int index)
	{
		Selected[this.index].enabled = false;
		this.index = index;
		Selected[this.index].enabled = true;
	}
}
