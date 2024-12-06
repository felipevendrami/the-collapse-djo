using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharController_Motor : MonoBehaviour {

	public float speed = 10.0f;
	public float sensitivity = 70.0f;
	public float WaterHeight = 15.5f;
	public CharacterController character;
	public GameObject cam;
	float moveFB, moveLR;
	float rotX, rotY;
	public bool webGLRightClickRotation = true;
	public float gravity = 40.0f;

    // Movimento Pulo
	public float alturaPulo = 100f;
	public Transform checaChao;
    public float raioEsfera = 0.4f;
    public LayerMask chaoMask;
    public bool estaNoChao;

	Vector3 velocidadeCai;

	// Movimento Agachar
    private Transform cameraTransform;
    private bool estahAbaixado = false;
    private bool levantarBloqueado;
    public float alturaLevantado, alturaAbaixado, posicaoCameraEmPe, posicaoCameraAbaixado;

	public int vida = 100;
	public GameObject sangueNaTela;


	void Start(){
		//LockCursor ();
		sangueNaTela.SetActive(false);
		character = GetComponent<CharacterController> ();
		cameraTransform = cam.transform;
		if (Application.isEditor) {
			webGLRightClickRotation = false;
			sensitivity = sensitivity * 1.5f;
		}
	}


	void CheckForWaterHeight(){
		if (transform.position.y < WaterHeight) {
			gravity = 0f;			
		} else {
			gravity = -9.8f;
		}
	}

	void Update(){
		moveFB = Input.GetAxis ("Horizontal") * speed;
		moveLR = Input.GetAxis ("Vertical") * speed;

		rotX = Input.GetAxis ("Mouse X") * sensitivity;
		rotY = Input.GetAxis ("Mouse Y") * sensitivity;

		CheckForWaterHeight ();

		Vector3 movement = new Vector3 (moveFB, gravity, moveLR);

		if (webGLRightClickRotation) {
			if (Input.GetKey (KeyCode.Mouse0)) {
				CameraRotation (cam, rotX, rotY);
			}
		} else if (!webGLRightClickRotation) {
			CameraRotation (cam, rotX, rotY);
		}

		// Para pulo
		// Cria uma esfera de raioEsfera na posicao do checaChao, batendo com a mascara do chao
        // se estah em contato com o chaoMask, entao retorna true
        estaNoChao = Physics.CheckSphere(checaChao.position, raioEsfera, chaoMask);

        if (!levantarBloqueado && estaNoChao && Input.GetButtonDown("Jump"))
        {
			Debug.Log("Pulou");
            velocidadeCai.y = Mathf.Sqrt(alturaPulo * -2f * gravity);
        }
        
        if (!estaNoChao)
        {
            velocidadeCai.y += gravity * Time.deltaTime;
        }

		ChecarBloqueioAbaixado();

		// Para agachar e lavantar
		if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            AgacharLevantar();
        }

		// Morte do player
		if(vida <= 0)
		{
			Debug.Log("Player morreu");
			speed = 0f;
			//SceneManager.LoadScene("GameOver");
		}

		movement = transform.rotation * movement;
		if (character.enabled){
			character.Move(movement * Time.deltaTime);
		}
	}

	void CameraRotation(GameObject cam, float rotX, float rotY){		
		transform.Rotate (0, rotX * Time.deltaTime, 0);
		cam.transform.Rotate (-rotY * Time.deltaTime, 0, 0);
	}

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(checaChao.position, raioEsfera);
    }

    private void AgacharLevantar()
    {
        if (levantarBloqueado || estaNoChao == false)
        {
            return;
        }
        
        estahAbaixado = !estahAbaixado;
        if (estahAbaixado)
        {
            character.height = alturaAbaixado;
            cameraTransform.localPosition = new Vector3(0, posicaoCameraAbaixado, 0);
        } else
        {
            character.height = alturaLevantado;
            cameraTransform.localPosition = new Vector3(0, posicaoCameraEmPe, 0);
        }
    }

	private void ChecarBloqueioAbaixado()
    {
        Debug.DrawRay(cameraTransform.position, Vector3.up * 1.1f, Color.red);
        RaycastHit hit;
        levantarBloqueado = Physics.Raycast(cameraTransform.position, Vector3.up, out hit, 1.1f);
    }

	// Dano causado pelo capanga
	void OnTriggerEnter(Collider collider)
	{
		
		if(collider.gameObject.tag=="maoCapanga")
		{
			vida -= 30;
			GameManager.Instance.DiminuiVidaJogador(30);
			sangueNaTela.SetActive(true);
			StartCoroutine("SaidaTelaSangue");
			Debug.Log("Personagem levou dano. Vida = " + vida);
		}
	}

	IEnumerator SaidaTelaSangue()
	{
		yield return new WaitForSeconds(4f);
		sangueNaTela.SetActive(false);
	}


	/*public void LevarDano(int dano)
	{
		vida -= dano;
		Debug.Log("Personagem levou dano. Vida = " + vida);
	}*/
}