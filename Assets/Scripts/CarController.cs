using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class CarController : MonoBehaviour
{
    public Collider Coll;
    public NavMeshAgent NavMeshAgent;
    public Action OnKill;
    internal bool isDied { get; private set; }
    [SerializeField] Rigidbody rb;
    [SerializeField] float speed = 10, distanceFromBall, ballMovementSmoothness, ballHitForce = 10;
    [SerializeField] WrackingBall wrackingBall;
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] Animator carAnim;
    [SerializeField] ParticleSystem smokeVFX;
    [SerializeField] Sprite[] goodEmojiesArray, badEmojiesArray;
    [SerializeField] Image Emoji_Image;
    [SerializeField] float showingEmojiDuration = 3;
    [SerializeField] Renderer bodyRenderer , characterRenderer;
    Coroutine showingEmojiCoroutine;
    bool isMoving = false;
    WaitForFixedUpdate fixedUpdate;
    bool isSuperPower;
    private void OnEnable()
    {
        wrackingBall.OnHitCar += OnHitCar;
    }
    
    void ShowEmoji(bool goodEmoji)
    {
        float duration = showingEmojiDuration;
        if (showingEmojiCoroutine!=null)
        {
            StopCoroutine(showingEmojiCoroutine);
        }
        showingEmojiCoroutine = StartCoroutine(Showing());
        IEnumerator Showing()
        {
            Emoji_Image.transform.localScale = Vector3.zero;
            Emoji_Image.gameObject.SetActive(true);
            Emoji_Image.sprite = goodEmoji ? goodEmojiesArray[UnityEngine.Random.Range(0, goodEmojiesArray.Length)] :
                badEmojiesArray[UnityEngine.Random.Range(0, badEmojiesArray.Length)];
            while (duration > 0)
            {
                Emoji_Image.transform.localScale = Vector3.Lerp(Emoji_Image.transform.localScale, Vector3.one, Time.fixedDeltaTime * 20);
                Emoji_Image.transform.LookAt(InGameManager.Instance.cam.transform.position);
                duration -= Time.fixedDeltaTime;
                yield return fixedUpdate;
            }

            while (Emoji_Image.transform.localScale.x > 0)
            {
                Emoji_Image.transform.localScale = Vector3.Lerp(Emoji_Image.transform.localScale, Vector3.zero, Time.fixedDeltaTime * 20);
                yield return fixedUpdate;
            }
            Emoji_Image.gameObject.SetActive(false);
        }
    }
    
    private void OnDisable()
    {
        wrackingBall.OnHitCar -= OnHitCar;
    }

    private void OnHitCar(CarController car)
    {
        if (isMoving)
        {
            ShowEmoji(true);
            Vector3 hitDir = car.transform.position - wrackingBall.transform.position;
            hitDir.Normalize();
            hitDir.y = 1;
            car.Kill(hitDir);
        }
    }
    private void Start()
    {
        lineRenderer.positionCount = 2;
        wrackingBall.SetCar(this);
        NavMeshAgent.speed = 0;
        NavMeshAgent.angularSpeed = 0;
        fixedUpdate = new WaitForFixedUpdate();
        Emoji_Image.gameObject.SetActive(false);
        var ms = bodyRenderer.materials;
        foreach (var m in ms)
        {
            m.color = new Color(UnityEngine.Random.Range(0.0f, 1.0f), UnityEngine.Random.Range(0.0f, 1.0f), UnityEngine.Random.Range(0.0f, 1.0f), 1);
            bodyRenderer.material = m;
        }

        ms = characterRenderer.materials;
        foreach (var m in ms)
        {
            m.color = new Color(UnityEngine.Random.Range(0.0f, 1.0f), UnityEngine.Random.Range(0.0f, 1.0f), UnityEngine.Random.Range(0.0f, 1.0f), 1);
            bodyRenderer.material = m;
        }

    }
    public void Move(Vector3 dir)
    {
        if (!isDied && InGameManager.Instance.isFighting)
        {
            smokeVFX.Emit(10);
            isMoving = true;
            Vector3 localDir = transform.InverseTransformDirection(dir);
            if (localDir.x > .05f)
            {
                if (carAnim.GetBool("Center"))
                {
                    carAnim.SetBool("Center", false);
                    carAnim.speed = 0;
                }
                else
                {
                    carAnim.Play("SteerR", 0, Mathf.InverseLerp(0, 1, localDir.x));
                }
            }
            else if (localDir.x < -.05f)
            {
                if (carAnim.GetBool("Center"))
                {
                    carAnim.SetBool("Center", false);
                    carAnim.speed = 0;
                }
                else
                {
                    carAnim.Play("SteerL", 0, Mathf.InverseLerp(0, -1, localDir.x));
                }
            }
            else
            {
                carAnim.SetBool("Center", true);
                carAnim.speed = 1;
            }
            Quaternion targetRot = Quaternion.LookRotation(dir.normalized);
            rb.MoveRotation(Quaternion.Lerp(rb.rotation, targetRot, speed * Time.fixedDeltaTime * .015f));
            rb.velocity = transform.forward * speed * Time.fixedDeltaTime;
            var pos = rb.position;
            pos.y = 0;
            rb.position = pos;
        }
        else
        {
            Stop();
        }
    }
    private void FixedUpdate()
    {
        UpdateBallPos();
    }
    private void UpdateBallPos()
    {
        if (isDied)
        {
            return;
        }
        if (isSuperPower)
        {
            Quaternion q = Quaternion.AngleAxis(Time.time*.5f,  Vector3.up);
            Vector3 targetBallPos = q * (wrackingBall.rb.transform.position - transform.position).normalized *5+ transform.position;
            if (targetBallPos.y > 2)
            {
                targetBallPos.y = 2;
            }
            else if (targetBallPos.y < 1)
            {
                targetBallPos.y = 1;
            }
            wrackingBall.rb.MovePosition(targetBallPos);
        }
        else
        {
            Vector3 targetBallPos = transform.TransformPoint(0, 0, -distanceFromBall);
            float t = Time.fixedDeltaTime * ballMovementSmoothness;
            targetBallPos.y = wrackingBall.rb.position.y;
            if (targetBallPos.y > 2)
            {
                targetBallPos.y = 2;
            }
            else if (targetBallPos.y < 1)
            {
                targetBallPos.y = 1;
            }
            wrackingBall.rb.position = Vector3.Lerp(wrackingBall.rb.position, targetBallPos, t);
        }

    }
    private void LateUpdate()
    {
        UpdateLine();
    }
    private void UpdateLine()
    {
        if (isDied)
        {
            return;
        }
        lineRenderer.SetPosition(0, lineRenderer.transform.position);
        lineRenderer.SetPosition(1, wrackingBall.transform.position);
    }

    internal void Stop()
    {
        isMoving = false;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

    }
    public void Kill(Vector3 dir)
    {
        if (!isDied)
        {
            ShowEmoji(false);
            isDied = true;
            NavMeshAgent.enabled = false;
            Destroy(NavMeshAgent);
            isMoving = false;
            wrackingBall.OnHitCar -= OnHitCar;
            lineRenderer.gameObject.SetActive(false);
            rb.constraints = RigidbodyConstraints.None;
            rb.mass = 5f;
            rb.angularDrag = .0005f;
            rb.velocity = dir * ballHitForce;
            Destroy(gameObject, 2);
            OnKill?.Invoke();
            InGameManager.Instance.PlayerDied();
        }
    }
}
