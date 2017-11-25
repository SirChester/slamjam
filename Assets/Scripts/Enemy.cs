using UnityEngine;

public class Enemy : Character
{
	protected override void InitializePosition()
	{
		_positionOnBoard = new Vector2(2, 0);
	}

	protected override void SetPosition()
	{
		var pos = new Vector2(_positionOnBoard.x * 50.0f, _positionOnBoard.y * -50.0f);
		GetComponent<RectTransform>().anchoredPosition = pos;
	}

	public override void ShootRock()
	{
		var obj = Instantiate(Resources.Load("Prefabs/Rock"), gameObject.transform.parent) as GameObject;
		obj.transform.localPosition = new Vector2(gameObject.transform.localPosition.x - 55.0f,
			gameObject.transform.localPosition.y - 12.5f);
		obj.GetComponent<Bullet>().PushTo(false);
	}

	public override void ShootScissor()
	{
		var obj = Instantiate(Resources.Load("Prefabs/Scissor"), gameObject.transform.parent) as GameObject;
		obj.transform.localPosition = new Vector2(gameObject.transform.localPosition.x - 55.0f,
			gameObject.transform.localPosition.y - 12.5f);
		obj.GetComponent<Bullet>().PushTo(false);
	}

	public override void ShootPaper()
	{
		var obj = Instantiate(Resources.Load("Prefabs/Paper"), gameObject.transform.parent) as GameObject;
		obj.transform.localPosition = new Vector2(gameObject.transform.localPosition.x - 55.0f,
			gameObject.transform.localPosition.y - 12.5f);
		obj.GetComponent<Bullet>().PushTo(false);
	}
}