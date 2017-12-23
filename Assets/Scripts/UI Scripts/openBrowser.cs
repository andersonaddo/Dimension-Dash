using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class openBrowser : MonoBehaviour {

	public void link(string link)
    {
        Application.OpenURL(link);
    }
}
