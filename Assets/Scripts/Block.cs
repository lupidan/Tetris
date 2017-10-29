using UnityEngine;

namespace Tetris
{
    public class Block : MonoBehaviour
    {
        public Color Color
        {
            get { return _spriteRenderer.color; }
            set { _spriteRenderer.color = value; }
        }
        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        }
    }
}
