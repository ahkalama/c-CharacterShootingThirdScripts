using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VfxCreator : MonoBehaviour
{
    [SerializeField] private GameObject vfxPrefab;
    [SerializeField] private int poolSize = 20;
    [SerializeField] private float destroyDelay = 2f;

    private Queue<GameObject> vfxPool = new Queue<GameObject>();

    private void Awake()
    {
        InitializePool();
    }

    private void InitializePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject vfx = Instantiate(vfxPrefab);
            vfx.SetActive(false);
            vfxPool.Enqueue(vfx);
        }
    }

    public void TriggerVFX(Vector3 position)
    {
        if (vfxPool.Count > 0)
        {
            GameObject vfx = vfxPool.Dequeue();
            vfx.transform.position = position;
            vfx.SetActive(true);
            StartCoroutine(DeactivateVFX(vfx));
        }
        else
        {
            Debug.LogWarning("No available VFX objects in the pool!");
        }
    }

    private IEnumerator DeactivateVFX(GameObject vfx)
    {
        yield return new WaitForSeconds(destroyDelay);
        vfx.SetActive(false);
        vfxPool.Enqueue(vfx);
    }
}
