using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

[System.Serializable]
public class Move : NetworkBehaviour
{
	[HideInInspector][SyncVar(hook="SyncRotation")] public Quaternion rotation;

	[HideInInspector] public string moveName, description;
	[HideInInspector] public bool hasRecoil, hasHighCritRate, causesFlinch, makesContact, isDisabled, critHit, isActiveMove;
	[HideInInspector] public PokemonType moveType = PokemonType.NONE;
	[HideInInspector] public MoveCategory category = MoveCategory.PHYSICAL;
	[HideInInspector] public float recoilDamage;
	[HideInInspector] public int pkmnLevel, pkmnATK, pkmnSPATK, pkmnBaseSpeed, curPower, defaultPower;
	[HideInInspector] public PokemonType pkmnTypeOne, pkmnTypeTwo;
	public float coolDownTime, coolDownTimer, range;

	public int levelLearned, ppCost;
	public Sprite icon;
	public PokemonComponents components { get; private set; }

	[SerializeField] protected AudioClip soundEffect;

	void Awake()
	{
		components = GetComponent<PokemonComponents>();
	}
	void Update()
	{
		if(coolDownTimer > 0.0f)
		{
			coolDownTimer -= Time.deltaTime;

			if(coolDownTimer < 0.0f)
			{
				coolDownTimer = 0.0f;

				if(!isActiveMove)
					enabled = false;
			}
		}
	}

	public void PowerIncrease(float modifier)
	{
		curPower = (int)((float)curPower + ((float)defaultPower * modifier));
	}
	public void PowerDecrease(float modifier)
	{
		curPower = (int)((float)curPower - ((float)defaultPower * modifier));
	}
	public void ResetMoveData(string moveName, string description, bool hasRecoil, bool hasHighCritRate, bool causesFlinch, bool makesContact, bool isDisabled, PokemonType moveType,
		MoveCategory category, float recoilDamage, int power)
	{
		this.moveName = moveName;
		this.description = description;
		this.hasRecoil = hasRecoil;
		this.hasHighCritRate = hasHighCritRate;
		this.causesFlinch = causesFlinch;
		this.makesContact = makesContact;
		this.isDisabled = isDisabled;
		this.moveType = moveType;
		this.category = category;
		this.recoilDamage = recoilDamage;
		this.curPower = power;
		this.defaultPower = power;
	}
	public void ResetMoveValues()
	{
		pkmnLevel = components.pokemon.level;
		pkmnATK = components.stats.curATK;
		pkmnSPATK = components.stats.curSPATK;
		pkmnBaseSpeed = components.pokemon.baseSPD;
		pkmnTypeOne = components.pokemon.typeOne;
		pkmnTypeTwo = components.pokemon.typeTwo;
		critHit = false;
	}

	private void SyncRotation(Quaternion rotation)
	{
		this.rotation = rotation;
	}
}
public enum MoveCategory { PHYSICAL, SPECIAL, STATUS }
