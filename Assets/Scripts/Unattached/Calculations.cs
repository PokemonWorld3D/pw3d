using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public static class Calculations
{
	public static float[,] StatModifier
	{
		get
		{
			if(statModifiers == null)
				InitializeSMArray();
			return statModifiers;
		}
	}
	public static float[,] TypeToTypeDamageRatios
	{
		get
		{
			if(typeToTypeDamageRatios == null)
				InitializeTDArray();
			return typeToTypeDamageRatios;
		}
	}
	
	
	private static float[,] statModifiers;
	private static float[,] typeToTypeDamageRatios;
	
	private static void InitializeSMArray()
	{
		#region statModifiers
		statModifiers = new float[25,25];
		statModifiers[(int)PokemonNature.ADAMANT, (int)StatType.ATTACK] = 1.1f;
		statModifiers[(int)PokemonNature.ADAMANT, (int)StatType.DEFENSE] = 1.0f;
		statModifiers [(int)PokemonNature.ADAMANT, (int)StatType.SPECIALATTACK] = 0.9f;
		statModifiers[(int)PokemonNature.ADAMANT, (int)StatType.SPECIALDEFENSE] = 1.0f;
		statModifiers[(int)PokemonNature.ADAMANT, (int)StatType.SPEED] = 1.0f;
		
		statModifiers[(int)PokemonNature.BASHFUL, (int)StatType.ATTACK] = 1.0f;
		statModifiers[(int)PokemonNature.BASHFUL, (int)StatType.DEFENSE] = 1.0f;
		statModifiers[(int)PokemonNature.BASHFUL, (int)StatType.SPECIALATTACK] = 1.0f;
		statModifiers[(int)PokemonNature.BASHFUL, (int)StatType.SPECIALDEFENSE] = 1.0f;
		statModifiers[(int)PokemonNature.BASHFUL, (int)StatType.SPEED] = 1.0f;
		
		statModifiers [(int)PokemonNature.BOLD, (int)StatType.ATTACK] = 0.9f;
		statModifiers[(int)PokemonNature.BOLD, (int)StatType.DEFENSE] = 1.1f;
		statModifiers[(int)PokemonNature.BOLD, (int)StatType.SPECIALATTACK] = 1.0f;
		statModifiers[(int)PokemonNature.BOLD, (int)StatType.SPECIALDEFENSE] = 1.0f;
		statModifiers[(int)PokemonNature.BOLD, (int)StatType.SPEED] = 1.0f;
		
		statModifiers[(int)PokemonNature.BRAVE, (int)StatType.ATTACK] = 1.1f;
		statModifiers[(int)PokemonNature.BRAVE, (int)StatType.DEFENSE] = 1.0f;
		statModifiers[(int)PokemonNature.BRAVE, (int)StatType.SPECIALATTACK] = 1.0f;
		statModifiers[(int)PokemonNature.BRAVE, (int)StatType.SPECIALDEFENSE] = 1.0f;
		statModifiers [(int)PokemonNature.BRAVE, (int)StatType.SPEED] = 0.9f;
		
		statModifiers [(int)PokemonNature.CALM, (int)StatType.ATTACK] = 0.9f;
		statModifiers[(int)PokemonNature.CALM, (int)StatType.DEFENSE] = 1.0f;
		statModifiers[(int)PokemonNature.CALM, (int)StatType.SPECIALATTACK] = 1.0f;
		statModifiers[(int)PokemonNature.CALM, (int)StatType.SPECIALDEFENSE] = 1.1f;
		statModifiers[(int)PokemonNature.CALM, (int)StatType.SPEED] = 1.0f;
		
		statModifiers[(int)PokemonNature.CAREFUL, (int)StatType.ATTACK] = 1.0f;
		statModifiers[(int)PokemonNature.CAREFUL, (int)StatType.DEFENSE] = 1.0f;
		statModifiers [(int)PokemonNature.CAREFUL, (int)StatType.SPECIALATTACK] = 0.9f;
		statModifiers[(int)PokemonNature.CAREFUL, (int)StatType.SPECIALDEFENSE] = 1.1f;
		statModifiers[(int)PokemonNature.CAREFUL, (int)StatType.SPEED] = 1.0f;
		
		statModifiers[(int)PokemonNature.DOCILE, (int)StatType.ATTACK] = 1.0f;
		statModifiers[(int)PokemonNature.DOCILE, (int)StatType.DEFENSE] = 1.0f;
		statModifiers[(int)PokemonNature.DOCILE, (int)StatType.SPECIALATTACK] = 1.0f;
		statModifiers[(int)PokemonNature.DOCILE, (int)StatType.SPECIALDEFENSE] = 1.0f;
		statModifiers[(int)PokemonNature.DOCILE, (int)StatType.SPEED] = 1.0f;
		
		statModifiers[(int)PokemonNature.GENTLE, (int)StatType.ATTACK] = 1.0f;
		statModifiers [(int)PokemonNature.GENTLE, (int)StatType.DEFENSE] = 0.9f;
		statModifiers[(int)PokemonNature.GENTLE, (int)StatType.SPECIALATTACK] = 1.0f;
		statModifiers[(int)PokemonNature.GENTLE, (int)StatType.SPECIALDEFENSE] = 1.1f;
		statModifiers[(int)PokemonNature.GENTLE, (int)StatType.SPEED] = 1.0f;
		
		statModifiers[(int)PokemonNature.HARDY, (int)StatType.ATTACK] = 1.0f;
		statModifiers[(int)PokemonNature.HARDY, (int)StatType.DEFENSE] = 1.0f;
		statModifiers[(int)PokemonNature.HARDY, (int)StatType.SPECIALATTACK] = 1.0f;
		statModifiers[(int)PokemonNature.HARDY, (int)StatType.SPECIALDEFENSE] = 1.0f;
		statModifiers[(int)PokemonNature.HARDY, (int)StatType.SPEED] = 1.0f;
		
		statModifiers[(int)PokemonNature.HASTY, (int)StatType.ATTACK] = 1.0f;
		statModifiers [(int)PokemonNature.HASTY, (int)StatType.DEFENSE] = 0.9f;
		statModifiers[(int)PokemonNature.HASTY, (int)StatType.SPECIALATTACK] = 1.0f;
		statModifiers[(int)PokemonNature.HASTY, (int)StatType.SPECIALDEFENSE] = 1.0f;
		statModifiers[(int)PokemonNature.HASTY, (int)StatType.SPEED] = 1.1f;
		
		statModifiers[(int)PokemonNature.IMPISH, (int)StatType.ATTACK] = 1.0f;
		statModifiers[(int)PokemonNature.IMPISH, (int)StatType.DEFENSE] = 1.1f;
		statModifiers [(int)PokemonNature.IMPISH, (int)StatType.SPECIALATTACK] = 0.9f;
		statModifiers[(int)PokemonNature.IMPISH, (int)StatType.SPECIALDEFENSE] = 1.0f;
		statModifiers[(int)PokemonNature.IMPISH, (int)StatType.SPEED] = 1.0f;
		
		statModifiers[(int)PokemonNature.JOLLY, (int)StatType.ATTACK] = 1.0f;
		statModifiers[(int)PokemonNature.JOLLY, (int)StatType.DEFENSE] = 1.0f;
		statModifiers [(int)PokemonNature.JOLLY, (int)StatType.SPECIALATTACK] = 0.9f;
		statModifiers[(int)PokemonNature.JOLLY, (int)StatType.SPECIALDEFENSE] = 1.0f;
		statModifiers[(int)PokemonNature.JOLLY, (int)StatType.SPEED] = 1.1f;
		
		statModifiers[(int)PokemonNature.LAX, (int)StatType.ATTACK] = 1.0f;
		statModifiers[(int)PokemonNature.LAX, (int)StatType.DEFENSE] = 1.1f;
		statModifiers[(int)PokemonNature.LAX, (int)StatType.SPECIALATTACK] = 1.0f;
		statModifiers [(int)PokemonNature.LAX, (int)StatType.SPECIALDEFENSE] = 0.9f;
		statModifiers[(int)PokemonNature.LAX, (int)StatType.SPEED] = 1.0f;
		
		statModifiers[(int)PokemonNature.LONELY, (int)StatType.ATTACK] = 1.1f;
		statModifiers [(int)PokemonNature.LONELY, (int)StatType.DEFENSE] = 0.9f;
		statModifiers[(int)PokemonNature.LONELY, (int)StatType.SPECIALATTACK] = 1.0f;
		statModifiers[(int)PokemonNature.LONELY, (int)StatType.SPECIALDEFENSE] = 1.0f;
		statModifiers[(int)PokemonNature.LONELY, (int)StatType.SPEED] = 1.0f;
		
		statModifiers[(int)PokemonNature.MILD, (int)StatType.ATTACK] = 1.0f;
		statModifiers [(int)PokemonNature.MILD, (int)StatType.DEFENSE] = 0.9f;
		statModifiers[(int)PokemonNature.MILD, (int)StatType.SPECIALATTACK] = 1.1f;
		statModifiers[(int)PokemonNature.MILD, (int)StatType.SPECIALDEFENSE] = 1.0f;
		statModifiers[(int)PokemonNature.MILD, (int)StatType.SPEED] = 1.0f;
		
		statModifiers [(int)PokemonNature.MODEST, (int)StatType.ATTACK] = 0.9f;
		statModifiers[(int)PokemonNature.MODEST, (int)StatType.DEFENSE] = 1.0f;
		statModifiers[(int)PokemonNature.MODEST, (int)StatType.SPECIALATTACK] = 1.1f;
		statModifiers[(int)PokemonNature.MODEST, (int)StatType.SPECIALDEFENSE] = 1.0f;
		statModifiers[(int)PokemonNature.MODEST, (int)StatType.SPEED] = 1.0f;
		
		statModifiers[(int)PokemonNature.NAIVE, (int)StatType.ATTACK] = 1.0f;
		statModifiers[(int)PokemonNature.NAIVE, (int)StatType.DEFENSE] = 1.0f;
		statModifiers[(int)PokemonNature.NAIVE, (int)StatType.SPECIALATTACK] = 1.0f;
		statModifiers [(int)PokemonNature.NAIVE, (int)StatType.SPECIALDEFENSE] = 0.9f;
		statModifiers[(int)PokemonNature.NAIVE, (int)StatType.SPEED] = 1.1f;
		
		statModifiers[(int)PokemonNature.NAUGHTY, (int)StatType.ATTACK] = 1.1f;
		statModifiers[(int)PokemonNature.NAUGHTY, (int)StatType.DEFENSE] = 1.0f;
		statModifiers[(int)PokemonNature.NAUGHTY, (int)StatType.SPECIALATTACK] = 1.0f;
		statModifiers [(int)PokemonNature.NAUGHTY, (int)StatType.SPECIALDEFENSE] = 0.9f;
		statModifiers[(int)PokemonNature.NAUGHTY, (int)StatType.SPEED] = 1.0f;
		
		statModifiers[(int)PokemonNature.QUIET, (int)StatType.ATTACK] = 1.0f;
		statModifiers[(int)PokemonNature.QUIET, (int)StatType.DEFENSE] = 1.0f;
		statModifiers[(int)PokemonNature.QUIET, (int)StatType.SPECIALATTACK] = 1.1f;
		statModifiers[(int)PokemonNature.QUIET, (int)StatType.SPECIALDEFENSE] = 1.0f;
		statModifiers [(int)PokemonNature.QUIET, (int)StatType.SPEED] = 0.9f;
		
		statModifiers[(int)PokemonNature.QUIRKY, (int)StatType.ATTACK] = 1.0f;
		statModifiers[(int)PokemonNature.QUIRKY, (int)StatType.DEFENSE] = 1.0f;
		statModifiers[(int)PokemonNature.QUIRKY, (int)StatType.SPECIALATTACK] = 1.0f;
		statModifiers[(int)PokemonNature.QUIRKY, (int)StatType.SPECIALDEFENSE] = 1.0f;
		statModifiers[(int)PokemonNature.QUIRKY, (int)StatType.SPEED] = 1.0f;
		
		statModifiers[(int)PokemonNature.RASH, (int)StatType.ATTACK] = 1.0f;
		statModifiers[(int)PokemonNature.RASH, (int)StatType.DEFENSE] = 1.0f;
		statModifiers[(int)PokemonNature.RASH, (int)StatType.SPECIALATTACK] = 1.1f;
		statModifiers [(int)PokemonNature.RASH, (int)StatType.SPECIALDEFENSE] = 0.9f;
		statModifiers[(int)PokemonNature.RASH, (int)StatType.SPEED] = 1.0f;
		
		statModifiers[(int)PokemonNature.RELAXED, (int)StatType.ATTACK] = 1.0f;
		statModifiers[(int)PokemonNature.RELAXED, (int)StatType.DEFENSE] = 1.1f;
		statModifiers[(int)PokemonNature.RELAXED, (int)StatType.SPECIALATTACK] = 1.0f;
		statModifiers[(int)PokemonNature.RELAXED, (int)StatType.SPECIALDEFENSE] = 1.0f;
		statModifiers [(int)PokemonNature.RELAXED, (int)StatType.SPEED] = 0.9f;
		
		statModifiers[(int)PokemonNature.SASSY, (int)StatType.ATTACK] = 1.0f;
		statModifiers[(int)PokemonNature.SASSY, (int)StatType.DEFENSE] = 1.0f;
		statModifiers[(int)PokemonNature.SASSY, (int)StatType.SPECIALATTACK] = 1.0f;
		statModifiers[(int)PokemonNature.SASSY, (int)StatType.SPECIALDEFENSE] = 1.1f;
		statModifiers [(int)PokemonNature.SASSY, (int)StatType.SPEED] = 0.9f;
		
		statModifiers[(int)PokemonNature.SERIOUS, (int)StatType.ATTACK] = 1.0f;
		statModifiers[(int)PokemonNature.SERIOUS, (int)StatType.DEFENSE] = 1.0f;
		statModifiers[(int)PokemonNature.SERIOUS, (int)StatType.SPECIALATTACK] = 1.0f;
		statModifiers[(int)PokemonNature.SERIOUS, (int)StatType.SPECIALDEFENSE] = 1.0f;
		statModifiers[(int)PokemonNature.SERIOUS, (int)StatType.SPEED] = 1.0f;
		
		statModifiers [(int)PokemonNature.TIMID, (int)StatType.ATTACK] = 0.9f;
		statModifiers[(int)PokemonNature.TIMID, (int)StatType.DEFENSE] = 1.0f;
		statModifiers[(int)PokemonNature.TIMID, (int)StatType.SPECIALATTACK] = 1.0f;
		statModifiers[(int)PokemonNature.TIMID, (int)StatType.SPECIALDEFENSE] = 1.0f;
		statModifiers[(int)PokemonNature.TIMID, (int)StatType.SPEED] = 1.1f;
		#endregion
	}
	private static void InitializeTDArray()
	{
		#region typeToTypeDamageRatios
		typeToTypeDamageRatios = new float[19,19];
		typeToTypeDamageRatios[(int)PokemonType.BUG, (int)PokemonType.BUG] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.BUG, (int)PokemonType.DARK] = 2.0f;
		typeToTypeDamageRatios[(int)PokemonType.BUG, (int)PokemonType.DRAGON] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.BUG, (int)PokemonType.ELECTRIC] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.BUG, (int)PokemonType.FAIRY] = 0.5f;
		typeToTypeDamageRatios[(int)PokemonType.BUG, (int)PokemonType.FIGHTING] = 0.5f;
		typeToTypeDamageRatios[(int)PokemonType.BUG, (int)PokemonType.FIRE] = 0.5f;
		typeToTypeDamageRatios[(int)PokemonType.BUG, (int)PokemonType.FLYING] = 0.5f;
		typeToTypeDamageRatios[(int)PokemonType.BUG, (int)PokemonType.GHOST] = 0.5f;
		typeToTypeDamageRatios[(int)PokemonType.BUG, (int)PokemonType.GRASS] = 2.0f;
		typeToTypeDamageRatios[(int)PokemonType.BUG, (int)PokemonType.GROUND] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.BUG, (int)PokemonType.ICE] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.BUG, (int)PokemonType.NONE] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.BUG, (int)PokemonType.NORMAL] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.BUG, (int)PokemonType.POISON] = 0.5f;
		typeToTypeDamageRatios[(int)PokemonType.BUG, (int)PokemonType.PSYCHIC] = 2.0f;
		typeToTypeDamageRatios[(int)PokemonType.BUG, (int)PokemonType.ROCK] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.BUG, (int)PokemonType.STEEL] = 0.5f;
		typeToTypeDamageRatios[(int)PokemonType.BUG, (int)PokemonType.WATER] = 1.0f;
		
		typeToTypeDamageRatios[(int)PokemonType.DARK, (int)PokemonType.BUG] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.DARK, (int)PokemonType.DARK] = 0.5f;
		typeToTypeDamageRatios[(int)PokemonType.DARK, (int)PokemonType.DRAGON] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.DARK, (int)PokemonType.ELECTRIC] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.DARK, (int)PokemonType.FAIRY] = 0.5f;
		typeToTypeDamageRatios[(int)PokemonType.DARK, (int)PokemonType.FIGHTING] = 0.5f;
		typeToTypeDamageRatios[(int)PokemonType.DARK, (int)PokemonType.FIRE] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.DARK, (int)PokemonType.FLYING] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.DARK, (int)PokemonType.GHOST] = 2.0f;
		typeToTypeDamageRatios[(int)PokemonType.DARK, (int)PokemonType.GRASS] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.DARK, (int)PokemonType.GROUND] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.DARK, (int)PokemonType.ICE] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.DARK, (int)PokemonType.NONE] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.DARK, (int)PokemonType.NORMAL] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.DARK, (int)PokemonType.POISON] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.DARK, (int)PokemonType.PSYCHIC] = 2.0f;
		typeToTypeDamageRatios[(int)PokemonType.DARK, (int)PokemonType.ROCK] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.DARK, (int)PokemonType.STEEL] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.DARK, (int)PokemonType.WATER] = 1.0f;
		
		typeToTypeDamageRatios[(int)PokemonType.DRAGON, (int)PokemonType.BUG] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.DRAGON, (int)PokemonType.DARK] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.DRAGON, (int)PokemonType.DRAGON] = 0.0f;
		typeToTypeDamageRatios[(int)PokemonType.DRAGON, (int)PokemonType.ELECTRIC] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.DRAGON, (int)PokemonType.FAIRY] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.DRAGON, (int)PokemonType.FIGHTING] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.DRAGON, (int)PokemonType.FIRE] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.DRAGON, (int)PokemonType.FLYING] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.DRAGON, (int)PokemonType.GHOST] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.DRAGON, (int)PokemonType.GRASS] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.DRAGON, (int)PokemonType.GROUND] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.DRAGON, (int)PokemonType.ICE] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.DRAGON, (int)PokemonType.NONE] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.DRAGON, (int)PokemonType.NORMAL] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.DRAGON, (int)PokemonType.POISON] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.DRAGON, (int)PokemonType.PSYCHIC] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.DRAGON, (int)PokemonType.ROCK] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.DRAGON, (int)PokemonType.STEEL] = 0.5f;
		typeToTypeDamageRatios[(int)PokemonType.DRAGON, (int)PokemonType.WATER] = 1.0f;
		
		typeToTypeDamageRatios[(int)PokemonType.ELECTRIC, (int)PokemonType.BUG] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.ELECTRIC, (int)PokemonType.DARK] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.ELECTRIC, (int)PokemonType.DRAGON] = 0.5f;
		typeToTypeDamageRatios[(int)PokemonType.ELECTRIC, (int)PokemonType.ELECTRIC] = 0.5f;
		typeToTypeDamageRatios[(int)PokemonType.ELECTRIC, (int)PokemonType.FAIRY] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.ELECTRIC, (int)PokemonType.FIGHTING] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.ELECTRIC, (int)PokemonType.FIRE] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.ELECTRIC, (int)PokemonType.FLYING] = 2.0f;
		typeToTypeDamageRatios[(int)PokemonType.ELECTRIC, (int)PokemonType.GHOST] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.ELECTRIC, (int)PokemonType.GRASS] = 0.5f;
		typeToTypeDamageRatios[(int)PokemonType.ELECTRIC, (int)PokemonType.GROUND] = 0.0f;
		typeToTypeDamageRatios[(int)PokemonType.ELECTRIC, (int)PokemonType.ICE] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.ELECTRIC, (int)PokemonType.NONE] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.ELECTRIC, (int)PokemonType.NORMAL] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.ELECTRIC, (int)PokemonType.POISON] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.ELECTRIC, (int)PokemonType.PSYCHIC] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.ELECTRIC, (int)PokemonType.ROCK] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.ELECTRIC, (int)PokemonType.STEEL] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.ELECTRIC, (int)PokemonType.WATER] = 2.0f;
		
		typeToTypeDamageRatios[(int)PokemonType.FAIRY, (int)PokemonType.BUG] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.FAIRY, (int)PokemonType.DARK] = 2.0f;
		typeToTypeDamageRatios[(int)PokemonType.FAIRY, (int)PokemonType.DRAGON] = 2.0f;
		typeToTypeDamageRatios[(int)PokemonType.FAIRY, (int)PokemonType.ELECTRIC] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.FAIRY, (int)PokemonType.FAIRY] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.FAIRY, (int)PokemonType.FIGHTING] = 2.0f;
		typeToTypeDamageRatios[(int)PokemonType.FAIRY, (int)PokemonType.FIRE] = 0.5f;
		typeToTypeDamageRatios[(int)PokemonType.FAIRY, (int)PokemonType.FLYING] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.FAIRY, (int)PokemonType.GHOST] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.FAIRY, (int)PokemonType.GRASS] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.FAIRY, (int)PokemonType.GROUND] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.FAIRY, (int)PokemonType.ICE] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.FAIRY, (int)PokemonType.NONE] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.FAIRY, (int)PokemonType.NORMAL] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.FAIRY, (int)PokemonType.POISON] = 0.5f;
		typeToTypeDamageRatios[(int)PokemonType.FAIRY, (int)PokemonType.PSYCHIC] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.FAIRY, (int)PokemonType.ROCK] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.FAIRY, (int)PokemonType.STEEL] = 0.5f;
		typeToTypeDamageRatios[(int)PokemonType.FAIRY, (int)PokemonType.WATER] = 1.0f;
		
		typeToTypeDamageRatios[(int)PokemonType.FIGHTING, (int)PokemonType.BUG] = 0.5f;
		typeToTypeDamageRatios[(int)PokemonType.FIGHTING, (int)PokemonType.DARK] = 2.0f;
		typeToTypeDamageRatios[(int)PokemonType.FIGHTING, (int)PokemonType.DRAGON] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.FIGHTING, (int)PokemonType.ELECTRIC] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.FIGHTING, (int)PokemonType.FAIRY] = 0.5f;
		typeToTypeDamageRatios[(int)PokemonType.FIGHTING, (int)PokemonType.FIGHTING] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.FIGHTING, (int)PokemonType.FIRE] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.FIGHTING, (int)PokemonType.FLYING] = 0.5f;
		typeToTypeDamageRatios[(int)PokemonType.FIGHTING, (int)PokemonType.GHOST] = 0.0f;
		typeToTypeDamageRatios[(int)PokemonType.FIGHTING, (int)PokemonType.GRASS] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.FIGHTING, (int)PokemonType.GROUND] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.FIGHTING, (int)PokemonType.ICE] = 2.0f;
		typeToTypeDamageRatios[(int)PokemonType.FIGHTING, (int)PokemonType.NONE] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.FIGHTING, (int)PokemonType.NORMAL] = 2.0f;
		typeToTypeDamageRatios[(int)PokemonType.FIGHTING, (int)PokemonType.POISON] = 0.5f;
		typeToTypeDamageRatios[(int)PokemonType.FIGHTING, (int)PokemonType.PSYCHIC] = 0.5f;
		typeToTypeDamageRatios[(int)PokemonType.FIGHTING, (int)PokemonType.ROCK] = 2.0f;
		typeToTypeDamageRatios[(int)PokemonType.FIGHTING, (int)PokemonType.STEEL] = 2.0f;
		typeToTypeDamageRatios[(int)PokemonType.FIGHTING, (int)PokemonType.WATER] = 1.0f;
		
		typeToTypeDamageRatios[(int)PokemonType.FIRE, (int)PokemonType.BUG] = 2.0f;
		typeToTypeDamageRatios[(int)PokemonType.FIRE, (int)PokemonType.DARK] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.FIRE, (int)PokemonType.DRAGON] = 0.5f;
		typeToTypeDamageRatios[(int)PokemonType.FIRE, (int)PokemonType.ELECTRIC] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.FIRE, (int)PokemonType.FAIRY] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.FIRE, (int)PokemonType.FIGHTING] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.FIRE, (int)PokemonType.FIRE] = 0.5f;
		typeToTypeDamageRatios[(int)PokemonType.FIRE, (int)PokemonType.FLYING] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.FIRE, (int)PokemonType.GHOST] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.FIRE, (int)PokemonType.GRASS] = 2.0f;
		typeToTypeDamageRatios[(int)PokemonType.FIRE, (int)PokemonType.GROUND] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.FIRE, (int)PokemonType.ICE] = 2.0f;
		typeToTypeDamageRatios[(int)PokemonType.FIRE, (int)PokemonType.NONE] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.FIRE, (int)PokemonType.NORMAL] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.FIRE, (int)PokemonType.POISON] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.FIRE, (int)PokemonType.PSYCHIC] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.FIRE, (int)PokemonType.ROCK] = 0.5f;
		typeToTypeDamageRatios[(int)PokemonType.FIRE, (int)PokemonType.STEEL] = 2.0f;
		typeToTypeDamageRatios[(int)PokemonType.FIRE, (int)PokemonType.WATER] = 0.5f;
		
		typeToTypeDamageRatios[(int)PokemonType.FLYING, (int)PokemonType.BUG] = 2.0f;
		typeToTypeDamageRatios[(int)PokemonType.FLYING, (int)PokemonType.DARK] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.FLYING, (int)PokemonType.DRAGON] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.FLYING, (int)PokemonType.ELECTRIC] = 0.5f;
		typeToTypeDamageRatios[(int)PokemonType.FLYING, (int)PokemonType.FAIRY] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.FLYING, (int)PokemonType.FIGHTING] = 2.0f;
		typeToTypeDamageRatios[(int)PokemonType.FLYING, (int)PokemonType.FIRE] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.FLYING, (int)PokemonType.FLYING] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.FLYING, (int)PokemonType.GHOST] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.FLYING, (int)PokemonType.GRASS] = 2.0f;
		typeToTypeDamageRatios[(int)PokemonType.FLYING, (int)PokemonType.GROUND] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.FLYING, (int)PokemonType.ICE] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.FLYING, (int)PokemonType.NONE] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.FLYING, (int)PokemonType.NORMAL] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.FLYING, (int)PokemonType.POISON] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.FLYING, (int)PokemonType.PSYCHIC] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.FLYING, (int)PokemonType.ROCK] = 0.5f;
		typeToTypeDamageRatios[(int)PokemonType.FLYING, (int)PokemonType.STEEL] = 0.5f;
		typeToTypeDamageRatios[(int)PokemonType.FLYING, (int)PokemonType.WATER] = 1.0f;
		
		typeToTypeDamageRatios[(int)PokemonType.GHOST, (int)PokemonType.BUG] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.GHOST, (int)PokemonType.DARK] = 0.5f;
		typeToTypeDamageRatios[(int)PokemonType.GHOST, (int)PokemonType.DRAGON] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.GHOST, (int)PokemonType.ELECTRIC] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.GHOST, (int)PokemonType.FAIRY] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.GHOST, (int)PokemonType.FIGHTING] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.GHOST, (int)PokemonType.FIRE] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.GHOST, (int)PokemonType.FLYING] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.GHOST, (int)PokemonType.GHOST] = 2.0f;
		typeToTypeDamageRatios[(int)PokemonType.GHOST, (int)PokemonType.GRASS] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.GHOST, (int)PokemonType.GROUND] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.GHOST, (int)PokemonType.ICE] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.GHOST, (int)PokemonType.NONE] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.GHOST, (int)PokemonType.NORMAL] = 0.0f;
		typeToTypeDamageRatios[(int)PokemonType.GHOST, (int)PokemonType.POISON] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.GHOST, (int)PokemonType.PSYCHIC] = 2.0f;
		typeToTypeDamageRatios[(int)PokemonType.GHOST, (int)PokemonType.ROCK] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.GHOST, (int)PokemonType.STEEL] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.GHOST, (int)PokemonType.WATER] = 1.0f;
		
		typeToTypeDamageRatios[(int)PokemonType.GRASS, (int)PokemonType.BUG] = 0.5f;
		typeToTypeDamageRatios[(int)PokemonType.GRASS, (int)PokemonType.DARK] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.GRASS, (int)PokemonType.DRAGON] = 0.5f;
		typeToTypeDamageRatios[(int)PokemonType.GRASS, (int)PokemonType.ELECTRIC] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.GRASS, (int)PokemonType.FAIRY] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.GRASS, (int)PokemonType.FIGHTING] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.GRASS, (int)PokemonType.FIRE] = 0.5f;
		typeToTypeDamageRatios[(int)PokemonType.GRASS, (int)PokemonType.FLYING] = 0.5f;
		typeToTypeDamageRatios[(int)PokemonType.GRASS, (int)PokemonType.GHOST] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.GRASS, (int)PokemonType.GRASS] = 0.5f;
		typeToTypeDamageRatios[(int)PokemonType.GRASS, (int)PokemonType.GROUND] = 2.0f;
		typeToTypeDamageRatios[(int)PokemonType.GRASS, (int)PokemonType.ICE] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.GRASS, (int)PokemonType.NONE] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.GRASS, (int)PokemonType.NORMAL] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.GRASS, (int)PokemonType.POISON] = 0.5f;
		typeToTypeDamageRatios[(int)PokemonType.GRASS, (int)PokemonType.PSYCHIC] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.GRASS, (int)PokemonType.ROCK] = 2.0f;
		typeToTypeDamageRatios[(int)PokemonType.GRASS, (int)PokemonType.STEEL] = 0.5f;
		typeToTypeDamageRatios[(int)PokemonType.GRASS, (int)PokemonType.WATER] = 2.0f;
		
		typeToTypeDamageRatios[(int)PokemonType.GROUND, (int)PokemonType.BUG] = 0.5f;
		typeToTypeDamageRatios[(int)PokemonType.GROUND, (int)PokemonType.DARK] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.GROUND, (int)PokemonType.DRAGON] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.GROUND, (int)PokemonType.ELECTRIC] = 2.0f;
		typeToTypeDamageRatios[(int)PokemonType.GROUND, (int)PokemonType.FAIRY] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.GROUND, (int)PokemonType.FIGHTING] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.GROUND, (int)PokemonType.FIRE] = 2.0f;
		typeToTypeDamageRatios[(int)PokemonType.GROUND, (int)PokemonType.FLYING] = 0.0f;
		typeToTypeDamageRatios[(int)PokemonType.GROUND, (int)PokemonType.GHOST] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.GROUND, (int)PokemonType.GRASS] = 0.5f;
		typeToTypeDamageRatios[(int)PokemonType.GROUND, (int)PokemonType.GROUND] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.GROUND, (int)PokemonType.ICE] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.GROUND, (int)PokemonType.NONE] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.GROUND, (int)PokemonType.NORMAL] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.GROUND, (int)PokemonType.POISON] = 2.0f;
		typeToTypeDamageRatios[(int)PokemonType.GROUND, (int)PokemonType.PSYCHIC] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.GROUND, (int)PokemonType.ROCK] = 2.0f;
		typeToTypeDamageRatios[(int)PokemonType.GROUND, (int)PokemonType.STEEL] = 2.0f;
		typeToTypeDamageRatios[(int)PokemonType.GROUND, (int)PokemonType.WATER] = 1.0f;
		
		typeToTypeDamageRatios[(int)PokemonType.ICE, (int)PokemonType.BUG] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.ICE, (int)PokemonType.DARK] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.ICE, (int)PokemonType.DRAGON] = 2.0f;
		typeToTypeDamageRatios[(int)PokemonType.ICE, (int)PokemonType.ELECTRIC] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.ICE, (int)PokemonType.FAIRY] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.ICE, (int)PokemonType.FIGHTING] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.ICE, (int)PokemonType.FIRE] = 0.5f;
		typeToTypeDamageRatios[(int)PokemonType.ICE, (int)PokemonType.FLYING] = 2.0f;
		typeToTypeDamageRatios[(int)PokemonType.ICE, (int)PokemonType.GHOST] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.ICE, (int)PokemonType.GRASS] = 2.0f;
		typeToTypeDamageRatios[(int)PokemonType.ICE, (int)PokemonType.GROUND] = 2.0f;
		typeToTypeDamageRatios[(int)PokemonType.ICE, (int)PokemonType.ICE] = 0.5f;
		typeToTypeDamageRatios[(int)PokemonType.ICE, (int)PokemonType.NONE] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.ICE, (int)PokemonType.NORMAL] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.ICE, (int)PokemonType.POISON] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.ICE, (int)PokemonType.PSYCHIC] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.ICE, (int)PokemonType.ROCK] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.ICE, (int)PokemonType.STEEL] = 0.5f;
		typeToTypeDamageRatios[(int)PokemonType.ICE, (int)PokemonType.WATER] = 0.5f;
		
		typeToTypeDamageRatios[(int)PokemonType.NONE, (int)PokemonType.BUG] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.NONE, (int)PokemonType.DARK] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.NONE, (int)PokemonType.DRAGON] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.NONE, (int)PokemonType.ELECTRIC] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.NONE, (int)PokemonType.FAIRY] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.NONE, (int)PokemonType.FIGHTING] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.NONE, (int)PokemonType.FIRE] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.NONE, (int)PokemonType.FLYING] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.NONE, (int)PokemonType.GHOST] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.NONE, (int)PokemonType.GRASS] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.NONE, (int)PokemonType.GROUND] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.NONE, (int)PokemonType.ICE] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.NONE, (int)PokemonType.NONE] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.NONE, (int)PokemonType.NORMAL] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.NONE, (int)PokemonType.POISON] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.NONE, (int)PokemonType.PSYCHIC] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.NONE, (int)PokemonType.ROCK] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.NONE, (int)PokemonType.STEEL] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.NONE, (int)PokemonType.WATER] = 1.0f;
		
		typeToTypeDamageRatios[(int)PokemonType.NORMAL, (int)PokemonType.BUG] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.NORMAL, (int)PokemonType.DARK] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.NORMAL, (int)PokemonType.DRAGON] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.NORMAL, (int)PokemonType.ELECTRIC] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.NORMAL, (int)PokemonType.FAIRY] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.NORMAL, (int)PokemonType.FIGHTING] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.NORMAL, (int)PokemonType.FIRE] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.NORMAL, (int)PokemonType.FLYING] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.NORMAL, (int)PokemonType.GHOST] = 0.0f;
		typeToTypeDamageRatios[(int)PokemonType.NORMAL, (int)PokemonType.GRASS] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.NORMAL, (int)PokemonType.GROUND] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.NORMAL, (int)PokemonType.ICE] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.NORMAL, (int)PokemonType.NONE] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.NORMAL, (int)PokemonType.NORMAL] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.NORMAL, (int)PokemonType.POISON] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.NORMAL, (int)PokemonType.PSYCHIC] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.NORMAL, (int)PokemonType.ROCK] = 0.5f;
		typeToTypeDamageRatios[(int)PokemonType.NORMAL, (int)PokemonType.STEEL] = 0.5f;
		typeToTypeDamageRatios[(int)PokemonType.NORMAL, (int)PokemonType.WATER] = 1.0f;
		
		typeToTypeDamageRatios[(int)PokemonType.POISON, (int)PokemonType.BUG] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.POISON, (int)PokemonType.DARK] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.POISON, (int)PokemonType.DRAGON] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.POISON, (int)PokemonType.ELECTRIC] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.POISON, (int)PokemonType.FAIRY] = 2.0f;
		typeToTypeDamageRatios[(int)PokemonType.POISON, (int)PokemonType.FIGHTING] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.POISON, (int)PokemonType.FIRE] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.POISON, (int)PokemonType.FLYING] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.POISON, (int)PokemonType.GHOST] = 0.5f;
		typeToTypeDamageRatios[(int)PokemonType.POISON, (int)PokemonType.GRASS] = 2.0f;
		typeToTypeDamageRatios[(int)PokemonType.POISON, (int)PokemonType.GROUND] = 0.5f;
		typeToTypeDamageRatios[(int)PokemonType.POISON, (int)PokemonType.ICE] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.POISON, (int)PokemonType.NONE] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.POISON, (int)PokemonType.NORMAL] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.POISON, (int)PokemonType.POISON] = 0.5f;
		typeToTypeDamageRatios[(int)PokemonType.POISON, (int)PokemonType.PSYCHIC] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.POISON, (int)PokemonType.ROCK] = 0.5f;
		typeToTypeDamageRatios[(int)PokemonType.POISON, (int)PokemonType.STEEL] = 0.0f;
		typeToTypeDamageRatios[(int)PokemonType.POISON, (int)PokemonType.WATER] = 1.0f;
		
		typeToTypeDamageRatios[(int)PokemonType.PSYCHIC, (int)PokemonType.BUG] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.PSYCHIC, (int)PokemonType.DARK] = 0.0f;
		typeToTypeDamageRatios[(int)PokemonType.PSYCHIC, (int)PokemonType.DRAGON] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.PSYCHIC, (int)PokemonType.ELECTRIC] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.PSYCHIC, (int)PokemonType.FAIRY] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.PSYCHIC, (int)PokemonType.FIGHTING] = 2.0f;
		typeToTypeDamageRatios[(int)PokemonType.PSYCHIC, (int)PokemonType.FIRE] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.PSYCHIC, (int)PokemonType.FLYING] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.PSYCHIC, (int)PokemonType.GHOST] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.PSYCHIC, (int)PokemonType.GRASS] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.PSYCHIC, (int)PokemonType.GROUND] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.PSYCHIC, (int)PokemonType.ICE] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.PSYCHIC, (int)PokemonType.NONE] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.PSYCHIC, (int)PokemonType.NORMAL] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.PSYCHIC, (int)PokemonType.POISON] = 2.0f;
		typeToTypeDamageRatios[(int)PokemonType.PSYCHIC, (int)PokemonType.PSYCHIC] = 0.5f;
		typeToTypeDamageRatios[(int)PokemonType.PSYCHIC, (int)PokemonType.ROCK] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.PSYCHIC, (int)PokemonType.STEEL] = 0.5f;
		typeToTypeDamageRatios[(int)PokemonType.PSYCHIC, (int)PokemonType.WATER] = 1.0f;
		
		typeToTypeDamageRatios[(int)PokemonType.ROCK, (int)PokemonType.BUG] = 2.0f;
		typeToTypeDamageRatios[(int)PokemonType.ROCK, (int)PokemonType.DARK] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.ROCK, (int)PokemonType.DRAGON] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.ROCK, (int)PokemonType.ELECTRIC] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.ROCK, (int)PokemonType.FAIRY] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.ROCK, (int)PokemonType.FIGHTING] = 0.5f;
		typeToTypeDamageRatios[(int)PokemonType.ROCK, (int)PokemonType.FIRE] = 2.0f;
		typeToTypeDamageRatios[(int)PokemonType.ROCK, (int)PokemonType.FLYING] = 2.0f;
		typeToTypeDamageRatios[(int)PokemonType.ROCK, (int)PokemonType.GHOST] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.ROCK, (int)PokemonType.GRASS] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.ROCK, (int)PokemonType.GROUND] = 0.5f;
		typeToTypeDamageRatios[(int)PokemonType.ROCK, (int)PokemonType.ICE] = 2.0f;
		typeToTypeDamageRatios[(int)PokemonType.ROCK, (int)PokemonType.NONE] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.ROCK, (int)PokemonType.NORMAL] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.ROCK, (int)PokemonType.POISON] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.ROCK, (int)PokemonType.PSYCHIC] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.ROCK, (int)PokemonType.ROCK] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.ROCK, (int)PokemonType.STEEL] = 0.5f;
		typeToTypeDamageRatios[(int)PokemonType.ROCK, (int)PokemonType.WATER] = 1.0f;
		
		typeToTypeDamageRatios[(int)PokemonType.STEEL, (int)PokemonType.BUG] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.STEEL, (int)PokemonType.DARK] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.STEEL, (int)PokemonType.DRAGON] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.STEEL, (int)PokemonType.ELECTRIC] = 0.5f;
		typeToTypeDamageRatios[(int)PokemonType.STEEL, (int)PokemonType.FAIRY] = 2.0f;
		typeToTypeDamageRatios[(int)PokemonType.STEEL, (int)PokemonType.FIGHTING] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.STEEL, (int)PokemonType.FIRE] = 0.5f;
		typeToTypeDamageRatios[(int)PokemonType.STEEL, (int)PokemonType.FLYING] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.STEEL, (int)PokemonType.GHOST] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.STEEL, (int)PokemonType.GRASS] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.STEEL, (int)PokemonType.GROUND] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.STEEL, (int)PokemonType.ICE] = 2.0f;
		typeToTypeDamageRatios[(int)PokemonType.STEEL, (int)PokemonType.NONE] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.STEEL, (int)PokemonType.NORMAL] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.STEEL, (int)PokemonType.POISON] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.STEEL, (int)PokemonType.PSYCHIC] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.STEEL, (int)PokemonType.ROCK] = 2.0f;
		typeToTypeDamageRatios[(int)PokemonType.STEEL, (int)PokemonType.STEEL] = 0.5f;
		typeToTypeDamageRatios[(int)PokemonType.STEEL, (int)PokemonType.WATER] = 0.5f;
		
		typeToTypeDamageRatios[(int)PokemonType.WATER, (int)PokemonType.BUG] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.WATER, (int)PokemonType.DARK] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.WATER, (int)PokemonType.DRAGON] = 0.5f;
		typeToTypeDamageRatios[(int)PokemonType.WATER, (int)PokemonType.ELECTRIC] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.WATER, (int)PokemonType.FAIRY] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.WATER, (int)PokemonType.FIGHTING] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.WATER, (int)PokemonType.FIRE] = 2.0f;
		typeToTypeDamageRatios[(int)PokemonType.WATER, (int)PokemonType.FLYING] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.WATER, (int)PokemonType.GHOST] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.WATER, (int)PokemonType.GRASS] = 0.5f;
		typeToTypeDamageRatios[(int)PokemonType.WATER, (int)PokemonType.GROUND] = 2.0f;
		typeToTypeDamageRatios[(int)PokemonType.WATER, (int)PokemonType.ICE] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.WATER, (int)PokemonType.NONE] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.WATER, (int)PokemonType.NORMAL] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.WATER, (int)PokemonType.POISON] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.WATER, (int)PokemonType.PSYCHIC] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.WATER, (int)PokemonType.ROCK] = 2.0f;
		typeToTypeDamageRatios[(int)PokemonType.WATER, (int)PokemonType.STEEL] = 1.0f;
		typeToTypeDamageRatios[(int)PokemonType.WATER, (int)PokemonType.WATER] = 0.5f;
		#endregion
	}

	#region Stats
	public static int CalculateHP(int _baseHP, int _level, int _iv, int _ev)
	{
		return (int)((((_iv + (2 * _baseHP) + (_ev / 4) + 100) * _level) / 100) + 10);
	}
	public static int CalculatePP(int _basePP, int _level)
	{
		return (int)((((2 * _basePP) + 100) * _level) / 100);
	}
	public static int CalculateStat(int _baseStat, int _level, int _iv, int _ev, PokemonNature _nature, StatType _statType)
	{
		float statModifier = DetermineStatModifier(_nature, _statType);
		return (int)(((((_iv + (2 * _baseStat) + (_ev / 4)) * _level) / 100) + 5) * statModifier);
	}
	private static float DetermineStatModifier(PokemonNature _nature, StatType _stat)
	{
		return (float) (StatModifier[(int)_nature, (int)_stat]);
	}
	#endregion

	#region EXP
	public static int CalculateCurrentXP(int _level, PokemonLevelRate _lvlrate)
	{
		if(_lvlrate == PokemonLevelRate.ERRATIC)
		{
			return CalculateErraticCurrentXP(_level);
		}
		if(_lvlrate == PokemonLevelRate.FAST)
		{
			return CalculateFastCurrentXP(_level);
		}
		if(_lvlrate == PokemonLevelRate.FLUCTUATING)
		{
			return CalculateFluctuatingCurrentXP(_level);
		}
		if(_lvlrate == PokemonLevelRate.MEDIUM_FAST)
		{
			return CalculateMediumFastCurrentXP(_level);
		}
		if(_lvlrate == PokemonLevelRate.MEDIUM_SLOW)
		{
			return CalculateMediumSlowCurrentXP(_level);
		}
		if(_lvlrate == PokemonLevelRate.SLOW)
		{
			return CalculateSlowCurrentXP(_level);
		}
		return 0;
	}
	public static int CalculateRequiredXP(int _level, PokemonLevelRate _lvlrate)
	{
		if(_lvlrate == PokemonLevelRate.ERRATIC)
		{
			return CalculateErraticRequiredXP(_level);
		}
		if(_lvlrate == PokemonLevelRate.FAST)
		{
			return CalculateFastRequiredXP(_level);
		}
		if(_lvlrate == PokemonLevelRate.FLUCTUATING)
		{
			return CalculateFluctuatingRequiredXP(_level);
		}
		if(_lvlrate == PokemonLevelRate.MEDIUM_FAST)
		{
			return CalculateMediumFastRequiredXP(_level);
		}
		if(_lvlrate == PokemonLevelRate.MEDIUM_SLOW)
		{
			return CalculateMediumSlowRequiredXP(_level);
		}
		if(_lvlrate == PokemonLevelRate.SLOW)
		{
			return CalculateSlowRequiredXP(_level);
		}
		return 0;
	}
	
	private static int CalculateErraticCurrentXP(int _level)
	{
		if(_level < 50)
		{
			return (int)((Mathf.Pow(_level, 3.0f) * (100 - _level)) / 50);
		}
		else if(_level >= 50 && _level < 68)
		{
			return (int)((Mathf.Pow(_level, 3.0f) * (150 - _level)) / 100);
		}
		else if(_level >= 68 && _level < 98)
		{
			return (int)((Mathf.Pow(_level, 3.0f) * (1911 - (10 * _level)) / 3) / 500);
		}
		else
		{
			return (int)((Mathf.Pow(_level, 3.0f) * (160 - _level)) / 100);
		}
	}
	private static int CalculateErraticRequiredXP(int _level)
	{
		if(_level < 50)
		{
			return (int)((Mathf.Pow((_level + 1), 3.0f) * (100 - (_level + 1))) / 50);
		}
		else if(_level >= 50 && _level < 68)
		{
			return (int)((Mathf.Pow((_level + 1), 3.0f) * (150 - (_level + 1))) / 100);
		}
		else if(_level >= 68 && _level < 98)
		{
			return (int)((Mathf.Pow((_level + 1), 3.0f) * (1911 - (10 * (_level + 1))) / 3) / 500);
		}
		else
		{
			return (int)((Mathf.Pow((_level + 1), 3.0f) * (160 - (_level + 1))) / 100);
		}
	}
	private static int CalculateFastCurrentXP(int _level)
	{
		return (int)((4 * Mathf.Pow(_level, 3.0f)) / 5);
	}	
	private static int CalculateFastRequiredXP(int _level)
	{
		return (int)((4 * Mathf.Pow((_level +1), 3.0f)) / 5);
	}
	private static int CalculateMediumFastCurrentXP(int _level)
	{
		return (int)(Mathf.Pow(_level, 3.0f));
	}	
	private static int CalculateMediumFastRequiredXP(int _level)
	{
		return (int)(Mathf.Pow((_level + 1), 3.0f));
	}	
	private static int CalculateMediumSlowCurrentXP(int _level)
	{	
		return (int)(1.2 * Mathf.Pow(_level, 3.0f) - 15 * Mathf.Pow(_level, 2.0f) + 100 * _level - 140);
	}	
	private static int CalculateMediumSlowRequiredXP(int _level)
	{	
		return (int)(1.2 * Mathf.Pow((_level + 1), 3.0f) - 15 * Mathf.Pow((_level + 1), 2.0f) + 100 * (_level + 1) - 140);
	}	
	private static int CalculateSlowCurrentXP(int _level)
	{
		return (int)((5 * Mathf.Pow(_level, 3.0f)) / 4);
	}	
	private static int CalculateSlowRequiredXP(int _level)
	{
		return (int)((5 * Mathf.Pow((_level + 1), 3.0f)) / 4);
	}	
	private static int CalculateFluctuatingCurrentXP(int _level)
	{
		if(_level < 15)
		{
			return (int)(Mathf.Pow(_level, 3.0f) * ((((_level + 1) / 3) + 24) / 50));
		}
		else if(_level >= 15 && _level < 36)
		{
			return (int)(Mathf.Pow(_level, 3.0f) * (((_level + 14) / 50)));
		}
		else
		{
			return (int)(Mathf.Pow(_level, 3.0f) * ((_level / 2) + 32) / 50);
		}
	}	
	private static int CalculateFluctuatingRequiredXP(int _level)
	{
		if(_level < 15)
		{
			return (int)(Mathf.Pow((_level + 1), 3.0f) * (((((_level + 1) + 1) / 3)) + 24) / 50);
		}
		else if(_level >= 15 && _level < 36)
		{
			return (int)(Mathf.Pow((_level + 1), 3.0f) * ((((_level + 1) + 14) / 50)));
		}
		else
		{
			return (int)(Mathf.Pow((_level + 1), 3.0f) * ((((_level + 1) / 2) + 32) / 50));
		}
	}
	#endregion

	#region Damage
	private static int CalculateAttackDamage(PokemonComponents attackerComponents, PokemonComponents targetComponents, Move moveUsed, bool crit)
	{
		return (int)((((2 * attackerComponents.pokemon.level + 10) / (float)250) * ((float)attackerComponents.stats.curATK /
			(float)targetComponents.stats.curDEF) * moveUsed.curPower + 2) * SetModifier(moveUsed.moveType, attackerComponents.pokemon,
				targetComponents.pokemon, crit));
	}
	private static int CalculateSpecialAttackDamage(PokemonComponents attackerComponents, PokemonComponents targetComponents, Move moveUsed, bool crit)
	{
		return (int)((((2 * attackerComponents.pokemon.level + 10) / (float)250) * ((float)attackerComponents.stats.curSPATK /
			(float)targetComponents.stats.curSPDEF) * moveUsed.curPower + 2) * SetModifier(moveUsed.moveType, attackerComponents.pokemon,
				targetComponents.pokemon, crit));
	}
	private static float SetModifier(PokemonType moveType, Pokemon attacker, Pokemon target, bool critHit)
	{
		//Other is dependant on equipped items, abilities, and field advantages.

		float crit = 0.0f;

		if(critHit)
			crit = 1.5f;
		else
			crit = 1.0f;

		return (DetermineSTAB(moveType, attacker) * DetermineTypeEffectiveness(moveType, target) * crit * /*other*/ Random.Range(0.85f, 1.0f));
	}
	private static float DetermineSTAB(PokemonType moveType, Pokemon user)
	{
		float stab1 = 1.0f;
		float stab2 = 1.0f;

		if(moveType == user.typeOne)
		{
			if(user.abilityOne == "Adaptability" || user.abilityTwo == "Adaptability")
				stab1 = 2.0f;
			else
				stab1 = 1.5f;
		}

		if(moveType == user.typeTwo)
		{
			if(user.abilityOne == "Adaptability" || user.abilityTwo == "Adaptability")
				stab2 = 2.0f;
			else
				stab2 = 1.5f;
		}

		return stab1 * stab2;
	}
	private static bool DetermineCritical(int attackerBaseSPD, bool moveHasHighCritChance)
	{
		float chance = 0.0f;

		if(moveHasHighCritChance)
			chance = (attackerBaseSPD / 64);
		else
			chance = (attackerBaseSPD / 512);

		float random = Random.Range(1, 101);

		if(random <= chance)
			return  true;
		else
			return false;
	}
	private static float DetermineTypeEffectiveness(PokemonType moveType, Pokemon target)
	{
		
		float te1 = (float)(TypeToTypeDamageRatios[(int)moveType, (int)target.typeOne]);
		float te2 = (float)(TypeToTypeDamageRatios[(int)moveType, (int)target.typeTwo]);

		return te1 * te2;
	}
	public static void DealDamage(Pokemon attacker, Pokemon target, Move moveUsed)
	{
		int damage;
		bool critical = DetermineCritical(attacker.baseSPD, moveUsed.hasHighCritRate);

		if(moveUsed.category == MoveCategory.PHYSICAL)
			damage = CalculateAttackDamage(attacker.components, target.components, moveUsed, critical);
		else if(moveUsed.category == MoveCategory.SPECIAL)
			damage = CalculateAttackDamage(attacker.components, target.components, moveUsed, critical);
		else
			damage = 0;

		target.components.hpPP.AdjCurHP(-damage, critical);

		NetworkServer.SendToAll((short)Messages.MessageTypes.BATTLE_MESSAGE, Messages.DamageMessage(attacker, target, moveUsed, damage, critical));
	}
	#endregion

	public static int AddExperience(bool _faintedIsCaptured, bool _winningIsFromTrade, int _faintedBaseEXP, bool _winningHasLuckyEgg, int _faintedLevel, int _winningLevel,
	                                int _winningEvolveLevel)
	{
		//		float f = 1.2 if the pkmn calculating exp for has 2 or more affection hearts or 1 if less than 2 affection hearts
		
		float a = 1.0f;
		if(!_faintedIsCaptured)
		{
			a = 1.0f;
		}
		
		float t = 1.0f;
		if(_winningIsFromTrade)
		{
			t = 1.5f;
		}
		
		int baseEXP = _faintedBaseEXP;
		
		float e= 1.0f;
		if(_winningHasLuckyEgg)
		{
			e = 1.5f; 
		}
		
		int level = _faintedLevel;
		
		float v = 1.0f;
		if(_winningLevel > _winningEvolveLevel)
		{
			v = 1.2f;
		}
		
		int s = 1;
		
		return (int)Mathf.Abs(a * t * baseEXP * e * level * /*f * */v) / (7 * s);
	}

	public static bool AttemptCapture(Pokemon _pokemon, PokemonConditions _pokemonConditions, Poke_Ball_Types _pokeBallType)
	{
		float ballBonus = 1.0f, statusBonus;
		int modifiedCatchRate;
		PokemonHPPP hp = _pokemon.gameObject.GetComponent<PokemonHPPP>();

		if(_pokeBallType == Poke_Ball_Types.POKEBALL)
		{
			ballBonus = 1.0f;
		}
		else if(_pokeBallType == Poke_Ball_Types.GREATBALL)
		{
			ballBonus = 1.5f;
		}
		else if(_pokeBallType == Poke_Ball_Types.ULTRABALL)
		{
			ballBonus = 2f;
		}
		else if(_pokeBallType == Poke_Ball_Types.MASTERBALL)
		{
			ballBonus = 255f;
		}
		statusBonus = CaptureStatusBonus(_pokemonConditions);
		modifiedCatchRate = (int)(((3 * hp.maxHP - 2 * hp.curHP) * _pokemon.captureRate * ballBonus) / (3 * hp.maxHP) * statusBonus);
		int i = Random.Range(0, 255);
		if(i <= modifiedCatchRate)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
	private static float CaptureStatusBonus(PokemonConditions _pokemonConditions)
	{
		if(_pokemonConditions.sleeping)
		{
			return 2f;
		}
		else if(_pokemonConditions.burned)
		{
			return 1.5f;
		}
		else if(_pokemonConditions.frozen)
		{
			return 2f;
		}
		else if(_pokemonConditions.paralyzed)
		{
			return 1.5f;
		}
		else if(_pokemonConditions.poisoned)
		{
			return 1.5f;
		}
		else
		{
			return 1f;
		}
	}
}
public enum StatType
{
	HITPOINTS, POWERPOINTS, ATTACK, DEFENSE, SPECIALATTACK, SPECIALDEFENSE, SPEED
}
