using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindfieldController : MonoBehaviour
{
    [Tooltip("How much force the wind field does to the player.")]
    public float WindfieldForce = 100.0f;
    [Tooltip("To which direction the wind blows. Default Y Axis")]
    public Vector3 WindDirection = new Vector3(0.0f, 1.0f, 0.0f);
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        GameObject PlayerObject = other.gameObject;
        if (PlayerObject.CompareTag("Player"))
        {
            Debug.Log("Player Enter");
            WindDirection.Normalize();
            CharacterMovement PlayerMovement = PlayerObject.GetComponent<CharacterMovement>();
            Rigidbody PlayerRb = PlayerObject.GetComponent<Rigidbody>();
            if (PlayerMovement && PlayerRb)
            {
                float PlayerMass = PlayerMovement.GetPlayerMass();
                PlayerRb.AddForce(WindDirection * WindfieldForce / PlayerMass, ForceMode.Acceleration);
            }
        }
    }
}
