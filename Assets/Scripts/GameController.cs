using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
	[SerializeField] private Slider _playerHealth;
	[SerializeField] private Slider _enemyHealth;
	[SerializeField] private Player _player;
	[SerializeField] private Enemy _enemy;
	[SerializeField] private Floor _playerFloor;
	[SerializeField] private Floor _enemyFloor;

	private bool _playerMovementLocked;
	private bool _enemyMovementLocked;

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
		if (enemy)
		{
			_enemy.ShootScissor();
		}

		else
		{
			_player.ShootScissor();
		}
	}

	public void OnPaperClicked(bool enemy)
	{
		if (enemy)
		{
			_enemy.ShootPaper();
		}
		else
		{
			_player.ShootPaper();
		}
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
		if (_playerMovementLocked)
		{
			return;
		}
		
		if (Input.GetKeyDown(KeyCode.W))
		{
			_player.Up();
			StartCoroutine(LockPlayerMovement());
		}
		if (Input.GetKeyDown(KeyCode.A))
		{
			_player.Left();
			StartCoroutine(LockPlayerMovement());
		}
		if (Input.GetKeyDown(KeyCode.S))
		{
			_player.Down();
			StartCoroutine(LockPlayerMovement());
		}
		if (Input.GetKeyDown(KeyCode.D))
		{
			_player.Right();
			StartCoroutine(LockPlayerMovement());
		}
	}
	
	private void UpdateEnemyMovement()
	{
		if (_enemyMovementLocked)
		{
			return;
		}
		
		if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			_enemy.Up();
			StartCoroutine(LockEnemyMovement());
		}
		if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			_enemy.Left();
			StartCoroutine(LockEnemyMovement());
		}
		if (Input.GetKeyDown(KeyCode.DownArrow))
		{
			_enemy.Down();
			StartCoroutine(LockEnemyMovement());
		}
		if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			_enemy.Right();
			StartCoroutine(LockEnemyMovement());
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

	private IEnumerator LockPlayerMovement()
	{
		_playerMovementLocked = true;
		yield return new WaitForSeconds(1.0f);
		_playerMovementLocked = false;
	}
	
	private IEnumerator LockEnemyMovement()
	{
		_enemyMovementLocked = true;
		yield return new WaitForSeconds(1.0f);
		_enemyMovementLocked = false;
	}
}