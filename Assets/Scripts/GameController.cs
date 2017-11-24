using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GameController : NetworkBehaviour
{
	[SerializeField] private Slider _health;
	[SerializeField] private RectTransform _room;
	
	public GameObject rockPrefab;
	
	public void OnRockClicked()
	{
		GameObject projectile = Instantiate(rockPrefab, _room);
		NetworkServer.Spawn(projectile);
		
	}

	public void OnScissorClicked()
	{
	}

	public void OnPaperClicked()
	{
	}
}