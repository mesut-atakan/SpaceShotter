using UnityEngine;



public class Collision : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Variables
        other.gameObject.TryGetComponent(out IInteraction _interaction);

        if (_interaction != null)
        {
            _interaction.Damage(other);
        }
    }
}