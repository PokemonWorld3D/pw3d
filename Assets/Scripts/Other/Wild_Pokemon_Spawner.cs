using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Wild_Pokemon_Spawner : NetworkBehaviour
{
	public GameObject pokemonPrefab;
	public float mapSize;
	public int spawnThisMany = 10, minLevel = 1, maxLevel = 100;
	
	private Vector3 spawnPoint;
	
	void Start()
	{
		if(isServer)
			SpawnPokemon(pokemonPrefab, spawnThisMany);
	}
	
	private void SpawnPokemon(GameObject _pokemon, int _numberToSpawn)
	{
		for(int numberSpawned = 0; numberSpawned < _numberToSpawn; numberSpawned++)
		{
			spawnPoint = new Vector3(Random.Range(0, mapSize), Random.Range(0, mapSize), Random.Range(0, mapSize));
//			spawnPoint.y = TerrainHeight(spawnPoint);
			spawnPoint.y = Terrain.activeTerrain.SampleHeight(spawnPoint);
			if(!IsInvalidSpawnPoint(spawnPoint))
			{
				NavMeshHit closestHit;
				if(NavMesh.SamplePosition(spawnPoint, out closestHit, 500, 1))
				{
					spawnPoint = closestHit.position;
				}
				else
				{
					Debug.Log("...");
				}
				
				Quaternion wayToFace = Quaternion.Euler(0, Random.Range(0, 360), 0);
				GameObject wildPokemon = Instantiate(_pokemon, spawnPoint, wayToFace) as GameObject;

				wildPokemon.GetComponent<Pokemon>().level = Random.Range(minLevel, maxLevel);
				wildPokemon.GetComponent<Pokemon>().SetupPokemonFirstTime();
				wildPokemon.GetComponent<PokemonAI>().enabled = true;
				
				NetworkServer.Spawn(wildPokemon);

//				wildPokemon.GetComponent<Network_Movement>().isOwner = true;
			}
		}
	}
	private bool IsInvalidSpawnPoint(Vector3 spawnPoint)
	{
		if(spawnPoint.y == Mathf.Infinity)
			return true;
		else
			return false;
	}
//	private float TerrainHeight(Vector3 spawnPoint)
//	{
//		Ray rayUp = new Ray(spawnPoint, Vector3.up);
//		Ray rayDown = new Ray(spawnPoint, Vector3.down);
//		RaycastHit hitPoint;
//		if(Physics.Raycast(rayUp, out hitPoint, Mathf.Infinity))
//		{
//			return hitPoint.point.y;
//		}
//		else if(Physics.Raycast(rayDown, out hitPoint, Mathf.Infinity))
//		{
//			return hitPoint.point.y;
//		}
//		else
//		{
//			return Mathf.Infinity;
//		}
//	}
}
