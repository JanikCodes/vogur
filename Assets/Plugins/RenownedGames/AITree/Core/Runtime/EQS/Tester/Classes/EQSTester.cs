using UnityEngine;

namespace RenownedGames.AITree.EQS
{
    [ExecuteInEditMode]
    public class EQSTester : MonoBehaviour
    {
        [SerializeField]
        private EnvironmentQuery environmentQuery;

        [SerializeField]
        private bool visualizeBounds = true;

        [SerializeField]
        private bool showScore;

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (environmentQuery != null)
            {
                environmentQuery.SetQuerier(transform);
                environmentQuery.Visualize(visualizeBounds, showScore);
            }
        }
#endif
    }
}