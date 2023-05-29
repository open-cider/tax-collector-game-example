using UnityEngine;

public class CoinController : MonoBehaviour
{
    private static float _rotationSpeed = 90f;
    
    private GameController _gameControllerRef;
    private GameObject _coinGameObject;

    void Awake()
    {
        //Get reference to Game Controller
        _gameControllerRef = GameObject.FindGameObjectWithTag("GameController")
        .GetComponent<GameController>();
        

        //Get reference to gameobject child in position 0
        _coinGameObject = transform.GetChild(0).gameObject;
        
        
        //throw game if refs are not present
        if (_coinGameObject == null || _gameControllerRef == null)
        Debug.LogError("Object ref(s) not Found!!!");

        //Tilt coin for rotation
        _coinGameObject.transform.Rotate(35f, 0f, 0f);
    }


    void FixedUpdate() {
        Spin();
    }

    void OnTriggerEnter(Collider other) {
        //Confirm that collider is player
        if (other.CompareTag("Player")) {
            _gameControllerRef.IncrementCoinsCollected();
            Destroy(this.gameObject);
        } //Else ignore...
    }

    private void Spin() {
        _coinGameObject.transform.Rotate(Vector3.up * Time.deltaTime * _rotationSpeed, Space.World);
    }
}
