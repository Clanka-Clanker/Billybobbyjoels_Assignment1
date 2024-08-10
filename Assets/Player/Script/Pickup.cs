using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public enum PickupType
    {
        Life,
        PowerupJump,
        PowerupSpeed,
        Score
    }

    [SerializeField] private PickupType type;

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Player"))
    //    {
    //        Destroy(gameObject);
    //        // Perform actions when the player collects the object
    //        // Add score, play a sound, or activate a power-up
    //        //Collect();
    //    }
    //}
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            Playah pc = collider.GetComponent<Playah>();

            switch (type)
            {
                case PickupType.Life:
                    pc.lives++;
                    break;
                case PickupType.PowerupJump:
                case PickupType.PowerupSpeed:
                    pc.PowerupValueChange(type);
                    break;
                case PickupType.Score:
                    break;

            }
            Destroy(gameObject);
        }
    }

}
