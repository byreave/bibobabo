using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField, Tooltip("Force to rotate the player.")]
    private float PlayerAngularForce = 100.0f;

    [Tooltip("Freeze the position and rotation by code.")]
    public bool bForceFreeze2D = true;

    private Rigidbody PlayerRB;
    private float OriPlayerMass = 100.0f;
    private float PlayerMass = 100.0f;
    // Start is called before the first frame update
    private float oriZ;
    void Start()
    {
        PlayerRB = GetComponent<Rigidbody>();
        oriZ = gameObject.transform.position.z;
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
        if(bForceFreeze2D && PlayerRB)
        {
            PlayerRB.velocity = new Vector3(PlayerRB.velocity.x, PlayerRB.velocity.y, 0.0f);
            PlayerRB.angularVelocity = new Vector3(0.0f, 0.0f, PlayerRB.angularVelocity.z);

            transform.position = new Vector3(transform.position.x, transform.position.y, oriZ);
            transform.rotation = Quaternion.Euler(0.0f, 0.0f, transform.rotation.eulerAngles.z);
        }
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
    public void UpdatePlayerMass(float _massScale)
    {
        PlayerMass = OriPlayerMass * _massScale;
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
