using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
	[SerializeField] private Image _playerHealth;
	[SerializeField] private Image _enemyHealth;
	[SerializeField] private Character _player;
	[SerializeField] private Character _enemy;
	[SerializeField] private Floor _playerFloor;
	[SerializeField] private Floor _enemyFloor;
	[SerializeField] private SpriteText _playerScoreLbl;
	[SerializeField] private SpriteText _enemyScoreLbl;
	[SerializeField] private int _damageByFloor;
	[SerializeField] private GameObject _startGameScreen;
	[SerializeField] private GameObject _resultsScreen;
	[SerializeField] private GameObject _roundScreen;
	[SerializeField] private Text _resultsLbl;
	[SerializeField] private Text _roundLbl;
	[SerializeField] private LootGenerator _generator;
	
	[SerializeField] private Text _animatioText;

	private bool _gameStarted;
	
	private int _playerScore;
	private int _enemyScore;
	private int _roundCount;

	private float _changeRoundAnimationTime = 2.0f;

	public bool GameStarted
	{
		get { return _gameStarted; }
		set
		{
			_gameStarted = value; 
			_generator.gameObject.SetActive(value);
		}
	}

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
			_playerScoreLbl.SetText("PALADIN: " + _playerScore.ToString());
		}
	}

	public int EnemyScore
	{
		get { return _enemyScore; }
		set
		{
			_enemyScore = value; 
			_enemyScoreLbl.SetText("BEAR: " + _enemyScore.ToString());
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
		GameStarted = false;
		_roundScreen.SetActive(true);
		_roundLbl.text = "ROUND " + _roundCount;
		yield return new WaitForSeconds(_changeRoundAnimationTime);
		_roundScreen.SetActive(false);
		GameStarted = true;
	}

	private IEnumerator GameOver()
	{
		_roundCount = 0;
		GameStarted = false;
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
		GameStarted = false;
	}

	private void Update()
	{
		if (!GameStarted)
		{
			return;
		}
		UpdatePlayerMovement();
		UpdatePlayerShooting();
		UpdateEnemyMovement();
		UpdateEnemyShooting();
		UpdateFloors();

//		_animatioText.text = "Animation state:" + _player.AmimationStatus;
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
			if (_player.CanShootByIndex(_player.ChargeIndex))
			{
				_player.ShootByIndex(_player.ChargeIndex);
			}
			_player.StopShootWithCharge();
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
			if (_enemy.CanShootByIndex(_enemy.ChargeIndex))
			{
				_enemy.ShootByIndex(_enemy.ChargeIndex);
			}
			_enemy.StopShootWithCharge();
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
