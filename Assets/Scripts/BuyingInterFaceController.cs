using UnityEngine;

public class BuyingInterfaceController : MonoBehaviour
{

    private Canvas towerIcon;

    private void Start() {
        towerIcon = gameObject.GetComponent<Canvas>();
        towerIcon.enabled = false;
    }

    
}
