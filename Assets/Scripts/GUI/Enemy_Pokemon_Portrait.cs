using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Enemy_Pokemon_Portrait : MonoBehaviour
{
	public Text enemyInfo;
	public Image enemyHPBar, enemyAvatar;

	private Pokemon enemyPokemon;
	private PokemonHPPP enemyHPPP;
	
	void Update()
	{
		HandleTargetGUI();

		if(enemyPokemon == null || enemyPokemon.Equals(null))
			RemoveTargetPokemon();
	}
	
	public void SetTargetPokemon(GameObject pokemon)
	{
		enemyPokemon = pokemon.GetComponent<Pokemon>();
		enemyHPPP = pokemon.GetComponent<PokemonHPPP>();
		gameObject.SetActive(true);
		enabled = true;
	}
	public void RemoveTargetPokemon()
	{
		gameObject.SetActive(false);
		enemyPokemon = null;
		enemyHPPP = null;
		enabled = false;
	}
	public void HandleTargetGUI()
	{
		if(enemyPokemon.nickName != "")
			enemyInfo.text = "Level " + enemyPokemon.level + " " + enemyPokemon.nickName;
		else
			enemyInfo.text = "Level " + enemyPokemon.level + " " + enemyPokemon.pokemonName;

		enemyAvatar.sprite = enemyPokemon.avatar;
		enemyHPBar.fillAmount = Mathf.MoveTowards(enemyHPBar.fillAmount, ((float)enemyHPPP.curHP / (float)enemyHPPP.curMaxHP), 0.5f);

		if(enemyHPPP.curHP > enemyHPPP.curMaxHP / 2)
			enemyHPBar.color = new Color32((byte)CalculateValue(enemyHPPP.curHP, enemyHPPP.curMaxHP / 2, enemyHPPP.curMaxHP, 255, 0), 255, 0, 255);
		else
			enemyHPBar.color = new Color32(255, (byte)CalculateValue(enemyHPPP.curHP, 0, enemyHPPP.curMaxHP / 2, 0, 255), 0 , 255);
	}
	private float CalculateValue(float curValue, float minValue, float maxValue, float minXPos, float maxXPos)
	{
		return (curValue - minValue) * (maxXPos - minXPos) / (maxValue - minValue) + minXPos;
	}
}
