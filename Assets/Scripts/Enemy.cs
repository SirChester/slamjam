using UnityEngine;

public class Enemy : Character
{
	protected override void InitializePosition()
	{
		PositionOnBoard = new Vector2(2, 0);
	}

	protected override void SetPosition()
	{
		var pos = new Vector2(PositionOnBoard.x * 50.0f, PositionOnBoard.y * -50.0f);
		GetComponent<RectTransform>().anchoredPosition = pos;
		base.SetPosition();
	}

	public override void ShootRock()
	{
		var obj = Instantiate(Resources.Load("Prefabs/Rock")) as GameObject;
		obj.transform.localPosition = new Vector2(gameObject.transform.localPosition.x - 55.0f,
			gameObject.transform.localPosition.y - 12.5f);
		obj.GetComponent<Bullet>().PushTo(false);
	}

	public override void ShootScissor()
	{
		var obj = Instantiate(Resources.Load("Prefabs/Scissor")) as GameObject;
		obj.transform.localPosition = new Vector2(gameObject.transform.localPosition.x - 55.0f,
			gameObject.transform.localPosition.y - 12.5f);
		obj.GetComponent<Bullet>().PushTo(false);
	}

	public override void ShootPaper()
	{
		var obj = Instantiate(Resources.Load("Prefabs/Paper")) as GameObject;
		obj.transform.localPosition = new Vector2(gameObject.transform.localPosition.x - 55.0f,
			gameObject.transform.localPosition.y - 12.5f);
		obj.GetComponent<Bullet>().PushTo(false);
	}
}