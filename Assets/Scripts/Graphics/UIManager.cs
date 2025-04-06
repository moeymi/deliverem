using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject m_SolverGroup;
    [SerializeField] private TextMeshProUGUI m_TimeText;
    [SerializeField] TMP_Dropdown m_DropDown;
    [SerializeField] GameObject m_UICoin;
    [SerializeField] Transform m_PickedUpCoins;
    
    private static int m_Heuristic = 0;
    
    private Dictionary<Color, GameObject> m_CoinMap = new Dictionary<Color, GameObject>();
    float m_TimeElapsed = 0;
    
    public static UnityEvent<Color> PickedUpCoinEvent = new();
    public static UnityEvent<Color> DeliverCoinEvent = new();
    
    void Awake()
    {
        m_DropDown.onValueChanged.AddListener(delegate
        {
            m_Heuristic = m_DropDown.value;
        });
        
        PickedUpCoinEvent.AddListener(PickupCoin);
    }

    void Update()
    {
        if (!GameSolver.IsSolving)
        {
            if(m_TimeElapsed != 0)
                m_TimeText.color = Color.green;
            m_TimeElapsed = 0;
            return;
        }
        
        m_TimeElapsed += Time.deltaTime;
        m_TimeText.text = string.Format("{0:0.00}", m_TimeElapsed);
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
        m_TimeText.color = Color.yellow;
        GameManager.Solve();
        m_SolverGroup.SetActive(false);
    }

    static public int HeuristicValue => m_Heuristic; 

    private void PickupCoin(Color color)
    {
        GameObject coin = Instantiate(m_UICoin, m_PickedUpCoins);
        coin.GetComponent<Image>().color = color;
        m_CoinMap.Add(color, coin);
    }

    private void DeliverCoin(Color color)
    {
        Destroy(m_CoinMap[color].gameObject);
    }


}
