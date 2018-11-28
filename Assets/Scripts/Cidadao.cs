using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cidadao : MonoBehaviour {
    [Header("Valores estáticos")]
    public string nomeDoCidadao;
    public float movementSpeed;
    public float jumpSpeed;
    public float rotationSpeed;
    public bool moverUsandoCharacterController;

    [Header("Valores dinâmicos")]
    public Vector3 velocidadeDeQueda;
    public Biarticulado biarticulado;
    public Vector3 rbVelocity;

    [Header("Condições")]
    public bool estaNoChao;

    private CharacterController cc;
    private Rigidbody rb;

    // Use this for initialization
    void Start () {
        cc = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
        if (moverUsandoCharacterController)
            Destroy(rb);
        else
            Destroy(cc);
    }
	
	// Update is called once per frame
	void Update () {
        ChecarPosicionamento();
        Movimentar();
        Pular();
        Rotacionar();

        rbVelocity = rb.velocity;
    }

    public void ChecarPosicionamento()
    {
        estaNoChao = Physics.Raycast(transform.position, -transform.up, 0.1F);
    }

    public void Movimentar()
    {
        Vector3 velocity = new Vector3();
        velocity.z = Input.GetAxis("Vertical");
        velocity.x = Input.GetAxis("Horizontal");
        velocity = velocity.normalized * movementSpeed;
        velocity = transform.TransformDirection(velocity);

        if (moverUsandoCharacterController)
        {
            cc.Move(velocity * Time.deltaTime);
        }
        else
        {
            rb.velocity = velocity;
            if (biarticulado)
                rb.velocity += biarticulado.GetRigidBody().velocity;
        }
        //else
            //rb.velocity = velocity;// * Time.deltaTime;
            //rb.AddForce(velocity * Time.deltaTime, ForceMode.VelocityChange);
    }

    public void Pular()
    {
        if (estaNoChao)
            velocidadeDeQueda.y = Input.GetAxis("Jump") * jumpSpeed;
        else
            velocidadeDeQueda += Physics.gravity * Time.deltaTime;

        if (moverUsandoCharacterController)
            cc.Move(velocidadeDeQueda * Time.deltaTime);
        else
            rb.velocity += velocidadeDeQueda;// * Time.deltaTime;
            //rb.AddForce(velocidadeDeQueda * Time.deltaTime, ForceMode.VelocityChange);
    }

    public void Rotacionar()
    {
        Vector3 velocity = new Vector3();
        velocity.y = Input.GetAxis("Mouse X");
        velocity *= rotationSpeed;
        transform.Rotate(velocity * Time.deltaTime);
    }
}
