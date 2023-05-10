using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VacuumEffecter : MonoBehaviour
{
    public PlayerInputs playerInputs;
    public GameObject vacuumEffectObjPrefab;
    //public Vector3 startPos = new Vector3(0, 0, 3);
    public int effectAmount = 10;
    public AnimationCurve vacuumeEffectAnimationCurve;
    public AnimationCurve hyperVacuumeEffectAnimationCurve;


    private const float vacuumWidth = 0.05f;
    private const float hyperVacuumWidth = 0.3f;
    private List<GameObject> vacuumEffects = new List<GameObject>();

    private void Start()
    {
        //startPos = transform.forward * 3;
        //vacuumEffectObj.transform.position = startPos;
        //Init(effectAmount);
    }

    void Update()
    {
        if(vacuumEffects.Count <= 0 && playerInputs.vaccum)
        {
            Init(effectAmount);
        }
        Excute();
        
        
        //startPos = transform.position + transform.forward * 3;
        //Vector3 destinationDireciton = (transform.position - vacuumEffectObj.transform.position).normalized;
        //vacuumEffectObj.transform.position += destinationDireciton * 5 * Time.deltaTime;

        //if ((startPos - vacuumEffectObj.transform.position).sqrMagnitude > 2.8 * 2.8)
        //{
        //    vacuumEffectObj.GetComponent<TrailRenderer>().emitting = false;
        //    vacuumEffectObj.transform.position = startPos;
        //    vacuumEffectObj.GetComponent<TrailRenderer>().Clear();
        //    vacuumEffectObj.GetComponent<TrailRenderer>().emitting = true;
        //}

    }

    private void Init(int effectAmount)
    {
        for(int i=0; i<effectAmount; i++)
        {
            Vector3 startPos = transform.TransformPoint((Vector3.forward + Vector3.right * Random.Range(-1f, 1f) + Vector3.up * Random.Range(-0.3f, 0.3f)) * 3 * Random.Range(0.5f, 1f));
            var vacuumEffect = Instantiate(vacuumEffectObjPrefab, startPos, transform.rotation, transform);
            //vacuumEffect.GetComponent<TrailRenderer>().emitting = false;

            vacuumEffects.Add(vacuumEffect);
        }
    }

    private void Excute()
    {
        if(vacuumEffects.Count <= 0)
        {
            return;
        }

        foreach (var effect in vacuumEffects)
        {
              
            var trailRenderer = effect.GetComponent<TrailRenderer>();
            //trailRenderer.widthMultiplier
            if(trailRenderer.emitting)
            {
                Vector3 destinationDireciton = (transform.position - effect.transform.position).normalized;
                effect.transform.position += destinationDireciton * Random.Range(8f, 20f) * Time.deltaTime;
            }
            

            if ((transform.position - effect.transform.position).sqrMagnitude < 0.2 * 0.2 || (transform.TransformPoint(Vector3.forward) - effect.transform.position).sqrMagnitude >= (transform.TransformPoint(-Vector3.forward) - effect.transform.position).sqrMagnitude)
            {
                trailRenderer.emitting = false;
                if (playerInputs.vaccum || playerInputs.hyperVaccum)
                {
                    trailRenderer.widthCurve = playerInputs.vaccum? vacuumeEffectAnimationCurve : hyperVacuumeEffectAnimationCurve;
                    effect.transform.position = transform.TransformPoint((Vector3.forward + Vector3.right * Random.Range(-1f, 1f) + Vector3.up * Random.Range(-0.3f, 0.3f)) * 3 * Random.Range(0.5f, 1f));
                    trailRenderer.Clear();
                    trailRenderer.emitting = true;
                }
                
            }
        }
    }

}
