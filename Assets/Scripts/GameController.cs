using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
	[SerializeField] private Slider _health;
	[SerializeField] private RectTransform _myPlace;
	[SerializeField] private RectTransform _enemyPlace;
	[SerializeField] private Player _player;

	public void OnRockClicked(bool enemy)
	{
		var obj = Instantiate(Resources.Load("Prefabs/Rock"), enemy ? _enemyPlace : _myPlace) as GameObject;
		obj.GetComponent<Bullet>().PushTo(!enemy);
	}

	public void OnScissorClicked(bool enemy)
	{
		var obj = Instantiate(Resources.Load("Prefabs/Scissor"), enemy ? _enemyPlace : _myPlace) as GameObject;
		obj.GetComponent<Bullet>().PushTo(!enemy);
	}

	public void OnPaperClicked(bool enemy)
	{
		var obj = Instantiate(Resources.Load("Prefabs/Paper"), enemy ? _enemyPlace : _myPlace) as GameObject;
		obj.GetComponent<Bullet>().PushTo(!enemy);
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.W))
		{
			_player.Up();
		}
		if (Input.GetKeyDown(KeyCode.A))
		{
			_player.Left();
		}
		if (Input.GetKeyDown(KeyCode.S))
		{
			_player.Down();
		}
		if (Input.GetKeyDown(KeyCode.D))
		{
			_player.Right();
		}
	}
}