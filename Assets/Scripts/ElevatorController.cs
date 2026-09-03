using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ElevatorController : MonoBehaviour
{
    bool isPlayerInside = false;
    public float timer;
    float resetTimer;

    public Transform mraz1;
    public Transform mraz2;
    public float speedMraz;
    void Start()
    {
        mraz1.localScale = new Vector3(mraz1.localScale.x, mraz1.localScale.y, 0.3f);
        mraz2.localScale = new Vector3(mraz2.localScale.x, mraz2.localScale.y, 0.3f);
    }
    void Update()
    {
        if (isPlayerInside)
        {
            resetTimer += Time.deltaTime;
            if (resetTimer >= timer)
                ElevatorActivated();
        }
    }
    void ElevatorActivated()
    {
        float newZ = Mathf.MoveTowards(mraz1.localScale.z, 1.287185f, speedMraz * Time.deltaTime);
        mraz1.localScale = new Vector3(mraz1.localScale.x, mraz1.localScale.y, newZ);
        mraz2.localScale = new Vector3(mraz2.localScale.x, mraz2.localScale.y, newZ);

        StartCoroutine(NextLevel());
    }
    IEnumerator NextLevel()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(0);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            isPlayerInside = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
            resetTimer = 0;
        }
    }
    
}
