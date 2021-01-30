using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField, Tooltip("Force to rotate the player.")]
    private float PlayerAngularForce = 100.0f;

    private Rigidbody PlayerRB;
    private float PlayerMass = 100.0f;
    // Start is called before the first frame update
    void Start()
    {
        PlayerRB = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // Move the player
        float MovementInput = Input.GetAxis("Horizontal");
        PlayerMove(-MovementInput);
    }

    void FixedUpdate()
    {
        
    }

    void PlayerMove(float inputVal)
    {
        if(PlayerRB)
        {
            PlayerRB.AddTorque(Vector3.forward * PlayerAngularForce / PlayerMass * inputVal, ForceMode.Acceleration);
            // Drag is handled by the physics system itself...
        }
    }

    // Update the player mass, which decides the player's physics interactions.
    public void UpdatePlayerMass(float inPlayerMass)
    {
        PlayerMass = inPlayerMass;
    }

    public float GetPlayerMass()
    {
        return PlayerMass;
    }

    public float GetPlayerAngularForce()
    {
        return PlayerAngularForce;
    }

    public void SetPlayerAngularForce(float _playerAngularForce)
    {
        PlayerAngularForce = _playerAngularForce;
    }
}
