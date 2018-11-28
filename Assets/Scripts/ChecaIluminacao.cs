using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChecaIluminacao : MonoBehaviour {
    public bool usarComoDiurno;
    public bool usarComoNoturno;
    public Light luz;
    public GerenciadorDeCena gerenciador;

    // Use this for initialization
    void Start () {
        luz = GetComponent<Light>();
        gerenciador = FindObjectOfType<GerenciadorDeCena>();
        if (!gerenciador)
            Debug.LogWarning("Não foi encontrado um Gerenciador De Cena. A iluminação dinâmica do cenário não ocorrerá.");
    }
	
	// Update is called once per frame
	void Update () {
        if (gerenciador)
        {
            if (gerenciador.usarLuzNoturna)
            {
                if (usarComoNoturno || !usarComoDiurno)
                    luz.enabled = true;
                else
                    luz.enabled = false;
            }
            else
            {
                if (!usarComoNoturno || usarComoDiurno)
                    luz.enabled = true;
                else
                    luz.enabled = false;
            }
        }
    }
}
