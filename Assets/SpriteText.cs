using UnityEngine;

public class SpriteText : MonoBehaviour
{
	private TextMesh _textMesh;

	private void Awake()
	{
		_textMesh = GetComponent<TextMesh>();
	}

	private void Start()
	{
		var parent = transform.parent;
		var parentRenderer = parent.GetComponent<Renderer>();
		var locRenderer = GetComponent<Renderer>();
		locRenderer.sortingLayerID = parentRenderer.sortingLayerID + 5;
		locRenderer.sortingOrder = parentRenderer.sortingOrder + 5;
	}

	public void SetText(string str)
	{
		var parent = transform.parent;
		var spriteTransform = parent.transform;
		var pos = spriteTransform.position;
		_textMesh.text = string.Format(str, pos.x, pos.y);
	}
}