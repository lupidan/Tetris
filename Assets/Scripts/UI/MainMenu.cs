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

	[SerializeField] private Text GameTitleLabel;
	[SerializeField] private Text GridSizeProblemsLabel;
	[SerializeField] private InputField GridWidthInputField;
	[SerializeField] private InputField GridHeightInputField;

	public void StartButtonWasSelected()
	{
		bool shakeWidthInputField = false;
		bool shakeHeightInputField = false;
		string gridConfigError = "";
		int width = DefaultWidth;
		if (!int.TryParse(GridWidthInputField.text, out width))
		{
			gridConfigError += "Please input a correct Width.\n";
			shakeWidthInputField = true;
		}

		int height = DefaultHeight;
		if (!int.TryParse(GridHeightInputField.text, out height))
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

		GridSizeProblemsLabel.text = gridConfigError;
		if (gridConfigError.Length != 0)
		{
			if (shakeWidthInputField)
				GridWidthInputField.transform.DOShakePosition(0.5f, 10.0f, 100, 90, false);

			if (shakeHeightInputField)
				GridHeightInputField.transform.DOShakePosition(0.5f, 10.0f, 100, 90, false);
		}
		else
		{
			//DO START
		}
	}
}
