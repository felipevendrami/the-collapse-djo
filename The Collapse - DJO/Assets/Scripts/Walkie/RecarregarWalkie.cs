using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecarregarWalkie : MonoBehaviour,IPegavel
{
    public void Pegar(){
        GameManager.Instance.AddPilha();
    }
}
