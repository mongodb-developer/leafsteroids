using UnityEngine;

public class GhostFrightened : GhostBehavior
{
    public SpriteRenderer body;
    public SpriteRenderer eyes;
    public SpriteRenderer blue;
    public SpriteRenderer white;

    private bool HasBeenEaten { get; set; }

    private void OnEnable()
    {
        blue!.GetComponent<AnimatedSprite>()!.Restart();
        Ghost!.Movement!.speedMultiplier = 0.5f;
        HasBeenEaten = false;
    }

    private void OnDisable()
    {
        Ghost!.Movement!.speedMultiplier = 1f;
        HasBeenEaten = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision!.gameObject!.layer == LayerMask.NameToLayer("Pacman"))
            if (enabled)
                Eaten();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var node = other!.GetComponent<Node>();

        if (!enabled) return;
        
        var direction = Vector2.zero;
        var maxDistance = float.MinValue;

        // Find the available direction that moves farthest from pacman
        foreach (var availableDirection in node!.AvailableDirections!)
        {
            // If the distance in this direction is greater than the current
            // max distance then this direction becomes the new farthest
            var newPosition = transform.position + new Vector3(availableDirection.x, availableDirection.y);
            var distance = (Ghost!.target!.position - newPosition).sqrMagnitude;

            if (distance > maxDistance)
            {
                direction = availableDirection;
                maxDistance = distance;
            }
        }

        Ghost!.Movement!.SetDirection(direction);
    }

    public override void Enable(float enabledDuration)
    {
        base.Enable(enabledDuration);

        body!.enabled = false;
        eyes!.enabled = false;
        blue!.enabled = true;
        white!.enabled = false;

        Invoke(nameof(Flash), enabledDuration / 2f);
    }

    public override void Disable()
    {
        base.Disable();

        body!.enabled = true;
        eyes!.enabled = true;
        blue!.enabled = false;
        white!.enabled = false;
    }

    private void Eaten()
    {
        HasBeenEaten = true;
        Ghost!.SetPosition(Ghost.Home!.inside!.position);
        Ghost.Home.Enable(duration);

        body!.enabled = false;
        eyes!.enabled = true;
        blue!.enabled = false;
        white!.enabled = false;
    }

    private void Flash()
    {
        if (!HasBeenEaten)
        {
            blue!.enabled = false;
            white!.enabled = true;
            white.GetComponent<AnimatedSprite>()!.Restart();
        }
    }
}