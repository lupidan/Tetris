using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Tetris
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private Text _gameTitleLabel;
        [SerializeField] private Text _problemsLabel;
        [SerializeField] private InputField _widthInputField;
        [SerializeField] private InputField _heightInputField;

        private RectTransform _widthInputFieldRectTransform;
        private RectTransform _heightInputFieldRectTransform;

        private Vector2 _gameTitleLabelOriginalAnchorPos;
        private Vector2 _widthInputFieldOriginalAnchorPos;
        private Vector2 _heightInputFieldOriginalAnchorPos;

        private GameController _gameController;

        #region MonoBehaviour

        private void Awake()
        {
            _gameTitleLabelOriginalAnchorPos = _gameTitleLabel.rectTransform.anchoredPosition;

            _widthInputFieldRectTransform = _widthInputField.transform as RectTransform;
            _widthInputFieldOriginalAnchorPos = _widthInputFieldRectTransform.anchoredPosition;

            _heightInputFieldRectTransform = _heightInputField.transform as RectTransform;
            _heightInputFieldOriginalAnchorPos = _heightInputFieldRectTransform.anchoredPosition;

            _problemsLabel.text = "";
            _widthInputField.text = PlayfieldConstants.DefaultWidth.ToString();
            _heightInputField.text = PlayfieldConstants.DefaultHeight.ToString();
        }

        private void OnEnable()
        {
            _gameTitleLabel.rectTransform.DOKill();
            _gameTitleLabel.rectTransform.anchoredPosition = _gameTitleLabelOriginalAnchorPos;

            _gameTitleLabel.rectTransform.DOAnchorPosY(_gameTitleLabelOriginalAnchorPos.y - 30.0f, 1.0f)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutQuad);
        }

        #endregion

        #region Public methods

        public void Initialize(GameController gameController)
        {
            _gameController = gameController;
        }

        #endregion

        #region Button Actions

        public void StartButtonWasSelected()
        {
            bool shakeWidthInputField = false;
            bool shakeHeightInputField = false;
            string gridConfigError = "";

            int width = PlayfieldConstants.DefaultWidth;
            if (!int.TryParse(_widthInputField.text, out width))
            {
                gridConfigError += "Please input a correct Width.\n";
                shakeWidthInputField = true;
            }

            int height = PlayfieldConstants.DefaultHeight;
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

            if (width < PlayfieldConstants.MinWidth)
            {
                gridConfigError += string.Format("Width can't be smaller than {0}.\n", PlayfieldConstants.MinWidth);
                width = PlayfieldConstants.MinWidth;
                shakeWidthInputField = true;
            }
            else if (width > PlayfieldConstants.MaxWidth)
            {
                gridConfigError += string.Format("Width can't be greater than {0}.\n", PlayfieldConstants.MaxWidth);
                width = PlayfieldConstants.MaxWidth;
                shakeWidthInputField = true;
            }

            if (height < PlayfieldConstants.MinHeight)
            {
                gridConfigError += string.Format("Height can't be smaller than {0}.\n", PlayfieldConstants.MinHeight);
                width = PlayfieldConstants.MinHeight;
                shakeHeightInputField = true;
            }
            else if (height > PlayfieldConstants.MaxHeight)
            {
                gridConfigError += string.Format("Height can't be greater than {0}.\n", PlayfieldConstants.MaxHeight);
                width = PlayfieldConstants.MaxHeight;
                shakeHeightInputField = true;
            }		

            _problemsLabel.text = gridConfigError;
            if (gridConfigError.Length != 0)
            {
                if (shakeWidthInputField)
                {
                    _widthInputFieldRectTransform.DOKill();
                    _widthInputFieldRectTransform.anchoredPosition = _widthInputFieldOriginalAnchorPos;
                    _widthInputFieldRectTransform.DOShakeAnchorPos(0.2f, 5.0f, vibrato: 100, fadeOut: false);
                }
                    
                if (shakeHeightInputField)
                {
                    _heightInputFieldRectTransform.DOKill();
                    _heightInputFieldRectTransform.anchoredPosition = _heightInputFieldOriginalAnchorPos;
                    _heightInputFieldRectTransform.DOShakeAnchorPos(0.2f, 5.0f, vibrato: 100, fadeOut: false);
                }
            }
            else
            {
                _gameController.StartGame(width, height);
            }
        }

        #endregion
    }
}
