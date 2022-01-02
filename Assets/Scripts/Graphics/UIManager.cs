using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region Attributes
    static TextMeshProUGUI timeText;
    static TMP_Dropdown dropDown;
    static float timeElapsed = 0;
    static GameObject UICoin;
    static Transform PickedupCoins;
    #endregion
    // Start is called before the first frame update
    void Awake()
    {
        timeText = GameObject.FindGameObjectWithTag("TimeText").GetComponent<TextMeshProUGUI>();
        dropDown = GameObject.FindGameObjectWithTag("HeuDropdown").GetComponent<TMP_Dropdown>();
        PickedupCoins = GameObject.FindGameObjectWithTag("PickedupCoins").transform;
        UICoin = Resources.Load<GameObject>("Prefabs/UICoin");
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameSolver.IsSolving)
        {
            if(timeElapsed != 0)
                timeText.color = Color.green;
            timeElapsed = 0;
            return;
        }
        timeElapsed += Time.deltaTime;
        timeText.text = string.Format("{0:0.00}", timeElapsed);
    }

    public void PauseUnpause()
    {
        if (GameSolver.IsSolving)
            return;
        if(Time.timeScale != 0)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }

    public void StartSolver()
    {
        timeText.color = Color.yellow;
        GameManager.Solve();
    }

    static public int DropdownValue
    {
        get { return dropDown.value; }
    }

    static public void PickupCoin(GameObject obj)
    {
        Color color = obj.GetComponent<SpriteRenderer>().color;
        GameObject coin = Instantiate(UICoin);
        coin.GetComponent<Image>().color = color;
        coin.name = obj.name;
        coin.transform.SetParent(PickedupCoins);
    }
    static public void DeliverCoin(GameObject obj)
    {
        Destroy(PickedupCoins.transform.Find(obj.name).gameObject);
    }


}
