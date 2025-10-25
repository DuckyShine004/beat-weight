using UnityEngine;

/// <summary>
/// Represents the final endpoint trigger for cars.
/// When a car enters this trigger zone, the car is destroyed,
/// indicating it has reached the end of its path.
/// </summary>
public class EndPoint : MonoBehaviour
{
    /// <summary>
    /// Called automatically by Unity when another collider enters this trigger zone.
    /// If the entering object has the tag "Car", it will be destroyed.
    /// </summary>
    /// <param name="other">The collider of the object that entered the trigger.</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            Destroy(other.gameObject);
        }
    }
}
