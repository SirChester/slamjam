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
	[SerializeField] private int _damageByFloor;
	[SerializeField] private GameObject _startGameScreen;
	[SerializeField] private GameObject _resultsScreen;
	[SerializeField] private GameObject _roundScreen;
	[SerializeField] private Text _resultsLbl;
	[SerializeField] private Text _roundLbl;

	private bool _gameStarted;
	
	private int _playerScore;
	private int _enemyScore;
	private int _roundCount;

	private void Awake()
	{
		_player.OnHpChanged += PlayerHpDidChange;
		_enemy.OnHpChanged += EnemyHpDidChange;

		_playerHealth.maxValue = _player.MaxHp;
		_enemyHealth.maxValue = _enemy.MaxHp;
	}

	private void OnDestroy()
	{
		_player.OnHpChanged -= PlayerHpDidChange;
		_enemy.OnHpChanged -= EnemyHpDidChange;
	}

	public int PlayerScore
	{
		get { return _playerScore; }
		set
		{
			_playerScore = value; 
			_playerScoreLbl.text = _playerScore.ToString();
		}
	}

	public int EnemyScore
	{
		get { return _enemyScore; }
		set
		{
			_enemyScore = value; 
			_enemyScoreLbl.text = _enemyScore.ToString();
		}
	}

	private void EnemyHpDidChange()
	{
		_enemyHealth.value = _enemy.Hp;
		if (_enemy.Hp <= 0)
		{
			PlayerScore++;
			ResetMatch();
		}
	}
	
	private void PlayerHpDidChange()
	{
		_playerHealth.value = _player.Hp;
		if (_player.Hp <= 0)
		{
			EnemyScore++;
			ResetMatch();
		}
	}

	private void ResetMatch()
	{
		_enemy.ResetChar();
		_player.ResetChar();
		_playerFloor.Reset();
		_enemyFloor.Reset();
		if (_playerScore == 3 || _enemyScore == 3)
		{
			StartCoroutine(GameOver());
		}
		else
		{
			StartCoroutine(ChangeRound());
		}
	}

	public void StartGame()
	{
		_startGameScreen.SetActive(false);
		StartCoroutine(ChangeRound());
	}

	private IEnumerator ChangeRound()
	{
		_roundCount++;
		_gameStarted = false;
		_roundScreen.SetActive(true);
		_roundLbl.text = "ROUND " + _roundCount;
		yield return new WaitForSeconds(2.0f);
		_roundScreen.SetActive(false);
		_gameStarted = true;
	}

	private IEnumerator GameOver()
	{
		_roundCount = 0;
		_gameStarted = false;
		_resultsScreen.SetActive(true);
		_player.HasInvulnerability = true;
		_enemy.HasInvulnerability = true;
		_resultsLbl.text = _playerScore == 3 ? "PALADIN WINS" : "BEAR WINS";
		yield return new WaitForSeconds(2.0f);
		EnemyScore = 0;
		PlayerScore = 0;
		_resultsScreen.SetActive(false);
		_startGameScreen.SetActive(true);
		_player.HasInvulnerability = false;
		_enemy.HasInvulnerability = false;
		_gameStarted = false;
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
		if (!_gameStarted)
		{
			return;
		}
		
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
		
		if (Input.GetKey(KeyCode.W))
		{
			_player.Up();
		}
		if (Input.GetKey(KeyCode.A))
		{
			_player.Left();
		}
		if (Input.GetKey(KeyCode.S))
		{
			_player.Down();
		}
		if (Input.GetKey(KeyCode.D))
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
		
		if (Input.GetKey(KeyCode.UpArrow))
		{
			_enemy.Up();
		}
		if (Input.GetKey(KeyCode.LeftArrow))
		{
			_enemy.Left();
		}
		if (Input.GetKey(KeyCode.DownArrow))
		{
			_enemy.Down();
		}
		if (Input.GetKey(KeyCode.RightArrow))
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
			_player.StartShootByIndexWithCharge(1);
		}
		if (Input.GetKeyUp(KeyCode.G))
		{
			_player.StopShootByIndexWithCharge(1);
		}
//		if (Input.GetKeyDown(KeyCode.H))
//		{
//			OnPaperClicked(false);
//		}
	}
	
	private void UpdateEnemyShooting()
	{
		if (Input.GetKeyDown(KeyCode.I))
		{
			OnRockClicked(true);
		}
		if (Input.GetKeyDown(KeyCode.O))
		{
			_enemy.StartShootByIndexWithCharge(1);
		}
		if (Input.GetKeyUp(KeyCode.O))
		{
			_enemy.StopShootByIndexWithCharge(1);
		}
//		if (Input.GetKeyDown(KeyCode.P))
//		{
//			OnPaperClicked(true);
//		}
	}
	
	private void UpdateFloors()
	{
		var needToDamagePlayer = _playerFloor.damageByPlayer(_player.PositionOnBoard);
		if (needToDamagePlayer)
		{
			_player.MakeDamage(_damageByFloor);
		}
		var needToDamageEnemy = _enemyFloor.damageByPlayer(_enemy.PositionOnBoard);
		if (needToDamageEnemy)
		{
			_enemy.MakeDamage(_damageByFloor);
		}
	}
}
