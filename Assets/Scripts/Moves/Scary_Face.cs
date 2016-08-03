using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class Scary_Face : Move
{
	public GameObject scaryFace;
	public Image face;
	public Debuff speedDown = new Debuff
	{
		type = Debuff.Types.SpdDown,
		percentage = 0.50f,
		duration = 10.0f
	};
	
	public void Reset()
	{
		ResetMoveData("Scary Face", "The user frightens the target with a scary face to harshly lower its speed temporarily.", false, false, false, false, false, PokemonType.NORMAL,
			MoveCategory.STATUS, 0.0f, 0);
	}
	public void StartScaryFace()
	{
		face.CrossFadeAlpha(1.0f, 0.0f, false);
		scaryFace.SetActive(true);
		components.audioS.PlayOneShot(soundEffect);
	}
	public void ScaryFaceEffect()
	{
		if(!isServer)
			return;

		Collider[] cols = Physics.OverlapSphere(gameObject.transform.position, range);
		
		for(int i = 0; i < cols.Length; i++)
		{
			if(cols[i].gameObject == components.pokemon.enemy)
			{
				PokemonComponents targetComponents = cols[i].GetComponent<PokemonComponents>();

				if(!targetComponents.buffsDebuffs.Debuffs.Contains(speedDown))
				{
					targetComponents.buffsDebuffs.Debuffs.Add(speedDown);
					targetComponents.stats.AdjSpeed(speedDown.percentage);

					NetworkServer.SendToAll((short)Messages.MessageTypes.BATTLE_MESSAGE,
						Messages.StatModMessage(targetComponents.pokemon, this, StatType.SPEED, false));
				}
			}
		}
	}
}