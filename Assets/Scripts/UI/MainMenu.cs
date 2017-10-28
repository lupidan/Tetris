using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
	private const int MinWidth = 5;
	private const int DefaultWidth = 10;
	private const int MaxWidth = 100;
	private const int MinHeight = 5;
	private const int DefaultHeight = 20;
	private const int MaxHeight = 200;

	[SerializeField] private Text _gameTitleLabel;
	[SerializeField] private Text _problemsLabel;
	[SerializeField] private InputField _widthInputField;
	[SerializeField] private InputField _heightInputField;

	private RectTransform _widthInputFieldRectTransform;
	private RectTransform _heightInputFieldRectTransform;

	private Vector2 _gameTitleLabelOriginalAnchorPos;
	private Vector2 _widthInputFieldOriginalAnchorPos;
	private Vector2 _heightInputFieldOriginalAnchorPos;

	#region MonoBehaviour

	private void Start()
	{
		_gameTitleLabelOriginalAnchorPos = _gameTitleLabel.rectTransform.anchoredPosition;

		_widthInputFieldRectTransform = _widthInputField.transform as RectTransform;
		_widthInputFieldOriginalAnchorPos = _widthInputFieldRectTransform.anchoredPosition;

		_heightInputFieldRectTransform = _heightInputField.transform as RectTransform;
		_heightInputFieldOriginalAnchorPos = _heightInputFieldRectTransform.anchoredPosition;

		_problemsLabel.text = "";
		_widthInputField.text = DefaultWidth.ToString();
		_heightInputField.text = DefaultHeight.ToString();
	}

	#endregion

	#region Button Actions

	public void StartButtonWasSelected()
	{
		bool shakeWidthInputField = false;
		bool shakeHeightInputField = false;
		string gridConfigError = "";

		int width = DefaultWidth;
		if (!int.TryParse(_widthInputField.text, out width))
		{
			gridConfigError += "Please input a correct Width.\n";
			shakeWidthInputField = true;
		}

		int height = DefaultHeight;
		if (!int.TryParse(_heightInputField.text, out height))
		{
			gridConfigError += "Please input a correct Height.\n";
			shakeHeightInputField = true;
		}
			
		if (width > height)
		{
			gridConfigError += "Width can't be greater than height.\n";
			width = height;
			shakeWidthInputField = true;
		}

		if (width < MinWidth)
		{
			gridConfigError += string.Format("Width can't be smaller than {0}.\n", MinWidth);
			width = MinWidth;
			shakeWidthInputField = true;
		}
		else if (width > MaxWidth)
		{
			gridConfigError += string.Format("Width can't be greater than {0}.\n", MaxWidth);
			width = MaxWidth;
			shakeWidthInputField = true;
		}

		if (height < MinHeight)
		{
			gridConfigError += string.Format("Height can't be smaller than {0}.\n", MinHeight);
			width = MinHeight;
			shakeHeightInputField = true;
		}
		else if (height > MaxHeight)
		{
			gridConfigError += string.Format("Height can't be greater than {0}.\n", MaxHeight);
			width = MaxHeight;
			shakeHeightInputField = true;
		}		

		_problemsLabel.text = gridConfigError;
		if (gridConfigError.Length != 0)
		{
			if (shakeWidthInputField)
			{
				_widthInputFieldRectTransform.DOKill();
				_widthInputFieldRectTransform.anchoredPosition = _widthInputFieldOriginalAnchorPos;
				_widthInputFieldRectTransform.DOShakeAnchorPos(0.5f, 10.0f, vibrato: 100, fadeOut: false);
			}
				
			if (shakeHeightInputField)
			{
				_heightInputFieldRectTransform.DOKill();
				_heightInputFieldRectTransform.anchoredPosition = _heightInputFieldOriginalAnchorPos;
				_heightInputFieldRectTransform.DOShakeAnchorPos(0.5f, 10.0f, vibrato: 100, fadeOut: false);
			}
		}
		else
		{
			//DO START
		}
	}

	#endregion

}
