using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
	[SerializeField] private Slider _health;
	[SerializeField] private RectTransform _room;
	
	public void OnRockClicked()
	{
		Instantiate(Resources.Load("Prefabs/Rock"), _room);
	}

	public void OnScissorClicked()
	{
	}

	public void OnPaperClicked()
	{
	}
}