using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float interactDistance = 3f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    void Interact()
    {
        Ray ray = new Ray(transform.position + Vector3.up * 0.5f, transform.forward);

        Debug.DrawRay(ray.origin, ray.direction * interactDistance, Color.red, 2f);

        if (!Physics.Raycast(ray, out RaycastHit hit, interactDistance))
        {
            Debug.Log("Aucune station devant moi");
            return;
        }

        Debug.Log("Objet touché : " + hit.collider.gameObject.name);

        Station station = hit.collider.GetComponent<Station>();

        if (station == null)
        {
            Debug.Log("Objet devant moi mais ce n'est pas une station");
            return;
        }

        Debug.Log("Station devant moi : " + station.stationName);
    }
}