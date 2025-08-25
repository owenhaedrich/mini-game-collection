using UnityEngine;

namespace MiniGameCollection.Games2024.Team00
{
    public class Obstacle : MonoBehaviour
    {
        [field: SerializeField] public Rigidbody Rigidbody { get; private set; }
        [field: SerializeField] public Collider Collider { get; private set; }
        [field: SerializeField] public float MinForce { get; private set; } = 10;
        [field: SerializeField] public float MaxForce { get; private set; } = 25;
        [field: SerializeField] public float MinTorque { get; private set; } = 20;
        [field: SerializeField] public float MaxTorque { get; private set; } = 40;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                // Enable gravity
                Rigidbody.useGravity = true;
                Rigidbody.isKinematic = false;
                Rigidbody.velocity = Vector3.zero;
                Rigidbody.MovePosition(transform.position + Vector3.up * 0.25f); // move up a bit to prevent hictching on greound
                // Enable collisions
                Collider.isTrigger = false;

                // Add random force
                float randomForceX = Random.Range(-MaxForce, MaxForce);
                float randomForceY = Random.Range(MinForce, MaxForce);
                float randomForceZ = Random.Range(MinForce, MaxForce);
                Vector3 randomForce = new Vector3 (randomForceX, randomForceY,randomForceZ);
                Vector3 randomTorque = Random.rotation * Vector3.forward * Random.Range(MinTorque, MaxTorque);
                Rigidbody.AddForce(randomForce * Rigidbody.mass, ForceMode.Impulse);
                Rigidbody.AddTorque(randomTorque * Rigidbody.mass, ForceMode.Impulse);

                // Remove script which acts as tag
                Destroy(this);
            }
        }

        private void Reset()
        {
            if (Rigidbody == null)
                Rigidbody = GetComponent<Rigidbody>();
            if (Collider == null)
                Collider = GetComponentInChildren<Collider>();
        }

    }
}
