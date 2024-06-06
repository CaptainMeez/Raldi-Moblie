using UnityEngine;

public class ReticleManager : MonoBehaviour
{
    public GameObject MouseCursor;
    public GameObject HammerCursor;
    public GameObject GunCursor;
    public Transform playerTransform;

    private void Start()
    {
        GameControllerScript.current = FindObjectOfType<GameControllerScript>();
    }

    private void Update()
    {
        // RONS CODE FOR THIS IS GOD AWFUL AND I ONLY MADE IT SLIGHTLY LESS GOD AWFUL
        Ray ray = Camera.main.ScreenPointToRay(new Vector3((float) (Screen.width / 2), (float) (Screen.height / 2), 0.0f));
        RaycastHit hitInfo;
        
        // Normal Cursor
        bool mouseCondition = 
        (Physics.Raycast(ray, out hitInfo) && hitInfo.collider.tag == "Door" && Vector3.Distance(playerTransform.position, hitInfo.transform.position) <= 15) ||
        (Physics.Raycast(ray, out hitInfo) && hitInfo.collider.tag == "Item" && Vector3.Distance(playerTransform.position, hitInfo.transform.position) <= 10) ||
        (Physics.Raycast(ray, out hitInfo) && hitInfo.collider.tag == "Notebook" && Vector3.Distance(playerTransform.position, hitInfo.transform.position) <= 10) ||
        (Physics.Raycast(ray, out hitInfo) && hitInfo.collider.tag == "Toilet" && Vector3.Distance(playerTransform.position, hitInfo.transform.position) <= 15) ||
        (Physics.Raycast(ray, out hitInfo) && hitInfo.collider.tag == "Sink" && Vector3.Distance(playerTransform.position, hitInfo.transform.position) <= 15) ||
        (Physics.Raycast(ray, out hitInfo) && hitInfo.collider.gameObject.name.ToLower() == "ishaan" && Vector3.Distance(playerTransform.position, hitInfo.transform.position) <= 15) ||
        (Physics.Raycast(ray, out hitInfo) && hitInfo.collider.tag == "PMarker" && Vector3.Distance(playerTransform.position, hitInfo.transform.position) <= 10);

        // Hammer Cursor
        bool hammerCondition = GameControllerScript.current.item[GameControllerScript.current.itemSelected] == 16 && 
        (
            (Physics.Raycast(ray, out hitInfo) && hitInfo.collider.tag == "HammerableWindow" && Vector3.Distance(playerTransform.position, hitInfo.transform.position) <= 10) ||
            (Physics.Raycast(ray, out hitInfo) && hitInfo.collider.tag == "BladderHammerable" && Vector3.Distance(playerTransform.position, hitInfo.transform.position) <= 10) ||
            (Physics.Raycast(ray, out hitInfo) && hitInfo.collider.tag == "Door" && Vector3.Distance(playerTransform.position, hitInfo.transform.position) <= 15 & hitInfo.collider.gameObject.name.ToLower().Contains("jail")) || 
            (Physics.Raycast(ray, out hitInfo) && hitInfo.collider.tag == "NPC" && Vector3.Distance(playerTransform.position, hitInfo.transform.position) <= 15 & hitInfo.collider.gameObject.name.ToLower().Contains("1st prize"))
        );

        // Gun Cursor
        bool gunCondition = GameControllerScript.current.item[GameControllerScript.current.itemSelected] == 9 &&
        (
            (Physics.Raycast(ray, out hitInfo) && hitInfo.collider.tag == "NPC" && Vector3.Distance(playerTransform.position, hitInfo.transform.position) <= 50 & hitInfo.collider.gameObject.name.ToLower().Contains("mrbeast") && GameControllerScript.current.mrBeast.beasting) ||
            (GameControllerScript.current.player.jumpRope || GameControllerScript.current.player.dt_jumpRope)
        );

        if (mouseCondition) MouseCursor.SetActive(true);
        else MouseCursor.SetActive(false);

        if (hammerCondition) HammerCursor.SetActive(true);
        else HammerCursor.SetActive(false);
        
        if (gunCondition) GunCursor.SetActive(true);
        else GunCursor.SetActive(false);
    }
}