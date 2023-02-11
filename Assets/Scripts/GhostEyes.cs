using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class GhostEyes : MonoBehaviour
{
    public Sprite up;
    public Sprite down;
    public Sprite left;
    public Sprite right;

    private SpriteRenderer SpriteRenderer { get; set; }
    private Movement Movement { get; set; }

    private void Awake()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
        Movement = GetComponentInParent<Movement>();
    }

    private void Update()
    {
        if (Movement!.Direction == Vector2.up)
            SpriteRenderer!.sprite = up;
        else if (Movement.Direction == Vector2.down)
            SpriteRenderer!.sprite = down;
        else if (Movement.Direction == Vector2.left)
            SpriteRenderer!.sprite = left;
        else if (Movement.Direction == Vector2.right) SpriteRenderer!.sprite = right;
    }
}