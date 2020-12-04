using UnityEngine;

[RequireComponent(typeof(Collider2D),
                  typeof(SpriteRenderer))]
public abstract class ObstacleBase : MonoBehaviour
{
    protected virtual void Start()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == AgentTags.PLAYER_CAR_TAG)
        {
            EventManager.NotifyEvent(MissionEvents.PLAYER_BUMP);
        }
    }
}