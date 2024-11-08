using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CanvasScript : MonoBehaviour
{
    [SerializeField] private Image[] images;
    // Start is called before the first frame update
    void Start()
    {
        PlayerScript.onChangingInventory += SwitchInventory;
    }

    // Update is called once per frame
    void Update()
    {

    }
    void SwitchInventory(int pos)
    {
        print(pos);
        for (int i = 0; i < images.Count(); i++)
        {
            if (i == pos)
                images[i].color = Color.yellow;
            else
                images[i].color = Color.white;
        }
    }
}
