using System;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class AnimatedSprite : MonoBehaviour
{
    public SpriteRenderer SpriteRenderer { get; private set; }
    public Sprite[] sprites = Array.Empty<Sprite>();
    public float animationTime = 0.25f;
    private int AnimationFrame { get; set; }
    public bool loop = true;

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
        if (!SpriteRenderer!.enabled)
        {
            return;
        }

        AnimationFrame++;

        if (AnimationFrame >= sprites!.Length && loop)
        {
            AnimationFrame = 0;
        }

        if (AnimationFrame >= 0 && AnimationFrame < sprites.Length)
        {
            SpriteRenderer.sprite = sprites[AnimationFrame];
        }
    }

    public void Restart()
    {
        AnimationFrame = -1;

        Advance();
    }
}