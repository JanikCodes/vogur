using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNearbyEmitter : MonoBehaviour
{
    [Header("Configurations")]
    [SerializeField] private float range;
    [SerializeField] private LayerMask layer;
    [SerializeField] private float scanInterval = 1f;

    private HashSet<PlayerNearby> previousNearbyPlayers = new();

    private IEnumerator Start()
    {
        while (true)
        {
            HashSet<PlayerNearby> currentNearbyPlayers = new();

            Collider[] colliders = Physics.OverlapSphere(transform.position, range, layer);
            foreach (Collider collider in colliders)
            {
                PlayerNearby playerNearby = collider.GetComponent<PlayerNearby>();
                if (playerNearby != null)
                {
                    playerNearby.SetValue(true);
                    currentNearbyPlayers.Add(playerNearby);
                }
            }

            // Reset values for players not found in this scan
            foreach (PlayerNearby cachedPlayer in previousNearbyPlayers)
            {
                if (!currentNearbyPlayers.Contains(cachedPlayer))
                {
                    cachedPlayer.SetValue(false);
                }
            }

            // Update previous nearby players
            previousNearbyPlayers = currentNearbyPlayers;

            yield return new WaitForSeconds(scanInterval);
        }
    }
}
