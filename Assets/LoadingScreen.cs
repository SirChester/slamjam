using System.Collections;
using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
	[SerializeField] private GameObject _logo;
	[SerializeField] private GameObject _start;
	[SerializeField] private GameObject _main;

	private void Awake()
	{
		StartCoroutine(WaitForLogoEnd());
	}

	private IEnumerator WaitForLogoEnd()
	{
		yield return new WaitForSeconds(1.2f);
		Destroy(_logo);
		_start.SetActive(true);
		_main.SetActive(true);
	}
}