using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
	[SerializeField] private Slider _playerHealth;
	[SerializeField] private Slider _enemyHealth;
	[SerializeField] private RectTransform _myPlace;
	[SerializeField] private RectTransform _enemyPlace;
	[SerializeField] private Player _player;
	[SerializeField] private Enemy _enemy;

	private void Awake()
	{
		_player.OnHpChanged += PlayerHpDidChange;
		_enemy.OnHpChanged += EnemyHpDidChange;
	}

	private void OnDestroy()
	{
		_player.OnHpChanged -= PlayerHpDidChange;
		_enemy.OnHpChanged -= EnemyHpDidChange;
	}

	private void EnemyHpDidChange()
	{
		_enemyHealth.value = _enemy.Hp;
	}
	
	private void PlayerHpDidChange()
	{
		_playerHealth.value = _enemy.Hp;
	}

	public void OnRockClicked(bool enemy)
	{
		if (enemy)
		{
			_enemy.ShootRock();
		}

		else
		{
			_player.ShootRock();
		}
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
		UpdatePlayerMovement();
		UpdatePlayerShooting();
		UpdateEnemyMovement();
		UpdateEnemyShooting();
	}

	private void UpdatePlayerMovement()
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
	
	private void UpdateEnemyMovement()
	{
		if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			_enemy.Up();
		}
		if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			_enemy.Left();
		}
		if (Input.GetKeyDown(KeyCode.DownArrow))
		{
			_enemy.Down();
		}
		if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			_enemy.Right();
		}
	}
	
	private void UpdatePlayerShooting()
	{
		if (Input.GetKeyDown(KeyCode.F))
		{
			OnRockClicked(false);
		}
		if (Input.GetKeyDown(KeyCode.G))
		{
			OnScissorClicked(false);
		}
		if (Input.GetKeyDown(KeyCode.H))
		{
			OnPaperClicked(false);
		}
	}
	
	private void UpdateEnemyShooting()
	{
		if (Input.GetKeyDown(KeyCode.I))
		{
			OnRockClicked(true);
		}
		if (Input.GetKeyDown(KeyCode.O))
		{
			OnScissorClicked(true);
		}
		if (Input.GetKeyDown(KeyCode.P))
		{
			OnPaperClicked(true);
		}
	}
}