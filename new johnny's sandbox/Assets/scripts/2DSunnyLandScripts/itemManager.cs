using UnityEngine;

public class itemManager : MonoBehaviour
{
    GameManager gameManager;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    public void GetItem()
    {
        Destroy(this.gameObject);
        gameManager.AddScore(100);
    }
    public void GetGem()
    {
        Destroy(this.gameObject);
    }
}
