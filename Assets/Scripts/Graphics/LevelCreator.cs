using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelCreator : MonoBehaviour
{
    [SerializeField] private TMP_InputField m_InputField;

    public void CreateLevel()
    {
        GameManager.CreateFromText(m_InputField.text);
        SceneManager.LoadScene(1);
    }
}
