using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
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
	[SerializeField] private GameObject _playButton;
	
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

	private void OnEnable()
	{
		EventSystem.current.SetSelectedGameObject(_playButton);
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
			_playerScoreLbl.SetText("PALADIN: " + _playerScore);
		}
	}

	public int EnemyScore
	{
		get { return _enemyScore; }
		set
		{
			_enemyScore = value; 
			_enemyScoreLbl.SetText("CYBEAR: " + _enemyScore);
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
		if (!_player.CanMove || IsPlayerFireKey())
		{
			return;
		}
		
		if (IsPlayerUpKey())
		{
			_player.Up();
		}
		if (IsPlayerLeftKey())
		{
			_player.Left();
		}
		if (IsPlayerDownKey())
		{
			_player.Down();
		}
		if (IsPlayerRightKey())
		{
			_player.Right();
		}
	}

	private static bool IsPlayerRightKey()
	{
		return Input.GetKey(KeyCode.D) || Input.GetAxis("Horizontal") > 0.2;
	}

	private static bool IsPlayerLeftKey()
	{
		return Input.GetKey(KeyCode.A) || Input.GetAxis("Horizontal") < -0.2;
	}

	private static bool IsPlayerDownKey()
	{
		return Input.GetKey(KeyCode.S) || Input.GetAxis("Vertical") < -0.2;
	}

	private static bool IsPlayerUpKey()
	{
		return Input.GetKey(KeyCode.W) || Input.GetAxis("Vertical") > 0.2;
	}

	private static bool IsPlayerFireKey()
	{
		return Input.GetKey(KeyCode.Space)
		       || Input.GetKey(KeyCode.Joystick1Button1);
	}

	private void UpdateEnemyMovement()
	{
		if (!_enemy.CanMove || IsEnemyFireKey())
		{
			return;
		}
		
		if (IsEnemyUpKey())
		{
			_enemy.Up();
		}
		if (IsEnemyLeftKey())
		{
			_enemy.Left();
		}
		if (IsEnemyDownKey())
		{
			_enemy.Down();
		}
		if (IsEnemyRightKey())
		{
			_enemy.Right();
		}
	}

	private static bool IsEnemyDownKey()
	{
		return Input.GetKey(KeyCode.K) || Input.GetAxis("Vertical2") < -0.2;
	}

	private static bool IsEnemyUpKey()
	{
		return Input.GetKey(KeyCode.I) || Input.GetAxis("Vertical2") > 0.2;
	}

	private static bool IsEnemyLeftKey()
	{
		return Input.GetKey(KeyCode.J) || Input.GetAxis("Horizontal2") < -0.2;
	}

	private static bool IsEnemyRightKey()
	{
		return Input.GetKey(KeyCode.L) || Input.GetAxis("Horizontal2") > 0.2;
	}

	private static bool IsEnemyFireKey()
	{
		return Input.GetKey(KeyCode.Return)
		       || Input.GetKey(KeyCode.Joystick2Button1);
	}

	private void UpdatePlayerShooting()
	{
		if (IsPlayerFireDown())
		{
			_player.StartShootWithCharge();
		}
		if (IsPlayerFireUp())
		{
			if (_player.CanShootByIndex(_player.ChargeIndex))
			{
				_player.ShootByIndex(_player.ChargeIndex);
			}
			_player.StopShootWithCharge();
		}
	}

	private static bool IsPlayerFireUp()
	{
		return Input.GetKeyUp(KeyCode.Space)
		       || Input.GetKeyUp(KeyCode.Joystick1Button1);
	}

	private static bool IsPlayerFireDown()
	{
		return Input.GetKeyDown(KeyCode.Space)
		       || Input.GetKeyDown(KeyCode.Joystick1Button1);
	}

	private void UpdateEnemyShooting()
	{
		if (IsEnemyFireDown())
		{
			_enemy.StartShootWithCharge();
		}
		if (IsEnemyFireUp())
		{
			if (_enemy.CanShootByIndex(_enemy.ChargeIndex))
			{
				_enemy.ShootByIndex(_enemy.ChargeIndex);
			}
			_enemy.StopShootWithCharge();
		}
	}

	private static bool IsEnemyFireUp()
	{
		return Input.GetKeyUp(KeyCode.Return)
		       || Input.GetKeyUp(KeyCode.Joystick2Button1);
	}

	private static bool IsEnemyFireDown()
	{
		return Input.GetKeyDown(KeyCode.Return)
		       || Input.GetKeyDown(KeyCode.Joystick2Button1);
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
