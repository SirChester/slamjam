using System.Collections;
using UnityEngine;

public class LootGenerator : MonoBehaviour
{
	[SerializeField] private Vector2 _timeRange;

	private void OnEnable()
	{
		StartCoroutine(Generation());
	}

	private void OnDisable()
	{
		StopAllCoroutines();
	}

	private void MakeHealthKit()
	{
		var obj = Instantiate(Resources.Load("Prefabs/HealthKit")) as GameObject;
		obj.transform.position = gameObject.transform.position;
	}

	private IEnumerator Generation()
	{
		while (true)
		{
			yield return new WaitForSeconds(Random.Range(_timeRange.x, _timeRange.y));
//			MakeHealthKit();
		}
	}
}