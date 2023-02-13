using System;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class AnimatedSprite : MonoBehaviour
    {
        public Sprite[] sprites = Array.Empty<Sprite>();
        public float animationTime = 0.25f;
        public bool loop = true;
        public SpriteRenderer SpriteRenderer { get; private set; }
        private int AnimationFrame { get; set; }

        private void Awake()
        {
            SpriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            InvokeRepeating(nameof(Advance), animationTime, animationTime);
        }

        private void Advance()
        {
            if (!SpriteRenderer!.enabled) return;

            AnimationFrame++;

            if (AnimationFrame >= sprites!.Length && loop) AnimationFrame = 0;

            if (AnimationFrame >= 0 && AnimationFrame < sprites.Length) SpriteRenderer.sprite = sprites[AnimationFrame];
        }

        public void Restart()
        {
            AnimationFrame = -1;

            Advance();
        }
    }
}