using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextButtonBehaviour : StateMachineBehaviour
{
	[SerializeField] private string id;
	public string ID { get { return id; } }
	public TextButtonStyle Style { get; set; }
	private TextMeshProUGUI textMesh;
	private string originalText;
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		if (!textMesh)
		{
			textMesh = animator.GetComponent<TextMeshProUGUI>();
			if (!textMesh) return;
		}
		originalText = textMesh.text;
		textMesh.text = Style.Prefix + originalText + Style.Suffix;

		textMesh.color = Style.TextColor;
	}
	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (textMesh)
		{
			textMesh.text = originalText;
		}
	}
}
