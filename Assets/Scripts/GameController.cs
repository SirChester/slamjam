using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
	[SerializeField] private Slider _playerHealth;
	[SerializeField] private Slider _enemyHealth;
	[SerializeField] private Character _player;
	[SerializeField] private Character _enemy;
	[SerializeField] private Floor _playerFloor;
	[SerializeField] private Floor _enemyFloor;
	[SerializeField] private Text _playerScoreLbl;
	[SerializeField] private Text _enemyScoreLbl;

	private bool _playerMovementLocked;
	private bool _enemyMovementLocked;

	private int _playerScore = 0;
	private int _enemyScore = 0;

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
		if (_enemy.Hp <= 0)
		{
			_playerScore++;
			_playerScoreLbl.text = _playerScore.ToString();
			_enemy.Hp = 100;
			_player.Hp = 100;
		}
	}
	
	private void PlayerHpDidChange()
	{
		_playerHealth.value = _player.Hp;
		if (_player.Hp <= 0)
		{
			_enemyScore++;
			_enemyScoreLbl.text = _enemyScore.ToString();
			_enemy.Hp = 100;
			_player.Hp = 100;
		}
	}

	public void OnRockClicked(bool enemy)
	{
		if (enemy)
		{
			if (_enemy.CanShootByIndex(0))
			{
				_enemy.ShootByIndex(0);
			}
		}
		else
		{
			if (_player.CanShootByIndex(0))
			{
				_player.ShootByIndex(0);
			}
		}
	}

	public void OnScissorClicked(bool enemy)
	{
		if (enemy)
		{
			if (_enemy.CanShootByIndex(1))
			{
				_enemy.ShootByIndex(1);
			}
		}
		else
		{
			if (_player.CanShootByIndex(1))
			{
				_player.ShootByIndex(1);
			}
		}
	}

	public void OnPaperClicked(bool enemy)
	{
		if (enemy)
		{
			if (_enemy.CanShootByIndex(2))
			{
				_enemy.ShootByIndex(2);
			}
		}
		else
		{
			if (_player.CanShootByIndex(2))
			{
				_player.ShootByIndex(2);
			}
		}
	}

	private void Update()
	{
		UpdatePlayerMovement();
		UpdatePlayerShooting();
		UpdateEnemyMovement();
		UpdateEnemyShooting();
		UpdateFloors();
	}
	
	private void UpdatePlayerMovement()
	{
		if (!_player.CanMove)
		{
			return;
		}
		
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
		if (!_enemy.CanMove)
		{
			return;
		}
		
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
	
	private void UpdateFloors()
	{
		_playerFloor.damageByPlayer(_player.PositionOnBoard);
	}
}