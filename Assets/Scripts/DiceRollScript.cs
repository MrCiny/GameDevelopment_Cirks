using UnityEngine;
using UnityEngine.UIElements;

public class DiceRollScript : MonoBehaviour
{
    Rigidbody body;
    Vector3 pos;
    [SerializeField]private float maxRadForceVal, startRollingForce;
    float forceX, forceY, forceZ;
    public string diceFaceNum;
    public bool isLanded = false;
    public bool firstThrow = false;
    // Start is called before the first frame update
    void Awake()
    {
        Initialize(0);
        PlayerPrefs.SetInt("playerTurn", 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (body !=  null)
        {
            bool isPaused = PlayerPrefs.GetInt("isPaused") == 1 ? true : false;
            bool isVictory = PlayerPrefs.GetInt("isVictory") == 1 ? true : false;
            if (Input.GetMouseButton(0) && isLanded && !isPaused && !isVictory || Input.GetMouseButton(0) && !firstThrow && !isPaused && !isVictory)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if(hit.collider != null && hit.collider.gameObject == this.gameObject) {
                        if (!firstThrow)
                            firstThrow = true;

                        RollDice();
                    }
                }
            }
        }
    }

    public void Initialize(int node)
    {
        if (node == 0)
        {
            body = GetComponent<Rigidbody>();
            pos = transform.position;
        }
        else if (node == 1)
        {
            transform.position = pos;
        }
        firstThrow = false;
        isLanded = false;
        body.isKinematic = true;
        transform.rotation = new Quaternion(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360), 0);
    }

    private void RollDice()
    {
        body.isKinematic = false;
        forceX = Random.Range(0, maxRadForceVal);
        forceY = Random.Range(0, maxRadForceVal);
        forceZ = Random.Range(0, maxRadForceVal);
        body.AddForce(Vector3.up * Random.Range(800, startRollingForce));
        body.AddTorque(forceX, forceY, forceZ);
    }
}
