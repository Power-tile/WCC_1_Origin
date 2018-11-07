using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveMaterial : MonoBehaviour {
    public void GiveMaterialFunction() {
        int cnt = 0;
        Material material = Material.Model2.Load<Material>("2333-RGBA");
        while (cnt < this.gameObject.transform.GetChildCount()) {
            Transform list = this.gameObject.transform.GetChild(cnt ++);
            list.GetComponent<Renderer>().material = material;
        }
    }
}
