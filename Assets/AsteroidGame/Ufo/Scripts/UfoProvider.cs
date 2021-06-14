using System.Collections;
using UnityEngine;

public class UfoProvider : MonoBehaviour
{
    [SerializeField]
    private Ufo ufo;
    [SerializeField]
    private float minSapwnDelay = 20f;
    [SerializeField]
    private float maxSapwnDelay = 40f;

    private IEnumerator coroutineSpawnUfoDelay;

    public void Init()
    {
        coroutineSpawnUfoDelay = SpawnUfoDelay();
        StartCoroutine(coroutineSpawnUfoDelay);
    }

    private void StartSpawnUfoDelay()
    {
        ufo.onExplode -= StartSpawnUfoDelay;
        Init();
    }

    private IEnumerator SpawnUfoDelay()
    {
        float deleyRatio = Random.value;
        float deley = Mathf.Lerp(minSapwnDelay, maxSapwnDelay, deleyRatio);
        Debug.Log("Ufo spawn delay " + deley);
        yield return new WaitForSeconds(deley);
        SpawnUfo();
    }
    
    private void SpawnUfo()
    {
        ufo.onExplode += StartSpawnUfoDelay;
        ufo.Init();
    }

    public void StopSpawn()
    {
        StopCoroutine(coroutineSpawnUfoDelay);
    }

    public void DeleteUfo()
    {
        ufo.gameObject.SetActive(false);
    }
}
