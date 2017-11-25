using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
	[SerializeField] private Image _playerHealth;
	[SerializeField] private Image _enemyHealth;
	[SerializeField] private Slider _playerCharge;
	[SerializeField] private Slider _enemyCharge;
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

	private float _changeRoundAnimationTime = 2.0f;

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
		_enemyHealth.fillAmount = (float)_enemy.Hp / (float)_enemy.MaxHp;
		if (_enemy.Hp <= 0)
		{
			PlayerScore++;
			ResetMatch();
		}
	}
	
	private void PlayerHpDidChange()
	{
		_playerHealth.fillAmount = (float)_player.Hp / (float)_enemy.MaxHp;
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
		yield return new WaitForSeconds(_changeRoundAnimationTime);
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
		yield return new WaitForSeconds(_changeRoundAnimationTime);
		EnemyScore = 0;
		PlayerScore = 0;
		_resultsScreen.SetActive(false);
		_startGameScreen.SetActive(true);
		_player.HasInvulnerability = false;
		_enemy.HasInvulnerability = false;
		_gameStarted = false;
	}

	private void Update()
	{
		if (!_gameStarted)
		{
			return;
		}

		_playerCharge.value = _player.ChargeTime;
		_enemyCharge.value = _enemy.ChargeTime;
		
		UpdatePlayerMovement();
		UpdatePlayerShooting();
		UpdateEnemyMovement();
		UpdateEnemyShooting();
		UpdateFloors();
	}
	
	private void UpdatePlayerMovement()
	{
		if (!_player.CanMove || Input.GetKey(KeyCode.Space))
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
		if (!_enemy.CanMove || Input.GetKey(KeyCode.Return))
		{
			return;
		}
		
		if (Input.GetKey(KeyCode.I))
		{
			_enemy.Up();
		}
		if (Input.GetKey(KeyCode.J))
		{
			_enemy.Left();
		}
		if (Input.GetKey(KeyCode.K))
		{
			_enemy.Down();
		}
		if (Input.GetKey(KeyCode.L))
		{
			_enemy.Right();
		}
	}
	
	private void UpdatePlayerShooting()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			_player.StartShootWithCharge();
		}
		if (Input.GetKeyUp(KeyCode.Space))
		{
			if (_player.ChargeAvailable)
			{
				_player.ShootCharge();
			}
			else
			{
				_player.StopShootWithCharge();
				if (_player.CanShootByIndex(0))
				{
					_player.ShootByIndex(0);
				}
			}
		}
	}
	
	private void UpdateEnemyShooting()
	{
		if (Input.GetKeyDown(KeyCode.Return))
		{
			_enemy.StartShootWithCharge();
		}
		if (Input.GetKeyUp(KeyCode.Return))
		{
			if (_enemy.ChargeAvailable)
			{
				_enemy.ShootCharge();
			}
			else
			{
				_enemy.StopShootWithCharge();
				if (_enemy.CanShootByIndex(0))
				{
					_enemy.ShootByIndex(0);
				}
			}
		}
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
