using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class navStickManager : MonoBehaviour
{
    public Material mat1, mat2;

    public MeshRenderer meshRenderer;

    public float forceMultiplier = 10f;
    public Text debugText;
    public Text debugDirectionText;
    public Text sampleText;
    private Rigidbody rb;
    public Transform centerObject;  // 基準となるオブジェクト

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        UpdateDebugText("Ready...");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("tenzi"))
        {
            meshRenderer.material = mat2;
            UpdateDebugText("Hit detected with tenzi object.");

            float boundsPower = 1.0f;
            Vector3 hitPos = collision.contacts[0].point;

            // 衝突ベクトルの計算 (杖の下端を基準にする)
            Vector3 boundVec = collision.contacts[0].point - transform.position;

            // UIにデバッグ情報を出力
            string debugInfo = $"Collision Point: {collision.contacts[0].point}\nCalculated Vector: {boundVec}";
            UpdateDebugText(debugInfo);

            // 法線ベクトルを考慮した力の計算
            Vector3 forceDir = collision.contacts[0].normal * boundsPower;

            // x, z 成分で方向を計算する
            Vector2 direction = new Vector2(forceDir.x, forceDir.z).normalized;

            // 方向の分類
            Vector2 assignedDirection = AssignDirectionTo8Directions(direction);

            // 分割された方向に基づいて力を加える
            if (rb != null)
            {
                Vector3 appliedForce = new Vector3(assignedDirection.x, forceDir.y, assignedDirection.y) * forceMultiplier;
                rb.AddForce(appliedForce, ForceMode.Impulse);
                UpdateDebugText($"Applied Bounce Force: {appliedForce}");
            }
        }
    }


    private Vector2 AssignDirectionTo8Directions(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (angle < 0) angle += 360;

        string directionText = "";
        sampleText.text = $"Direction Vector: {direction}";

        if (angle >= 337.5f || angle < 22.5f)
        {
            directionText = "右 (1, 0)";
            UpdateSampleText(directionText);
            return new Vector2(1, 0);  // 右
        }
        else if (angle >= 22.5f && angle < 67.5f)
        {
            directionText = "右上 (1, 1)";
            UpdateSampleText(directionText);
            return new Vector2(1, 1);  // 右上
        }
        else if (angle >= 67.5f && angle < 112.5f)
        {
            directionText = "上 (0, 1)";
            UpdateSampleText(directionText);
            return new Vector2(0, 1);  // 上
        }
        else if (angle >= 112.5f && angle < 157.5f)
        {
            directionText = "左上 (-1, 1)";
            UpdateSampleText(directionText);
            return new Vector2(-1, 1);  // 左上
        }
        else if (angle >= 157.5f && angle < 202.5f)
        {
            directionText = "左 (-1, 0)";
            UpdateSampleText(directionText);
            return new Vector2(-1, 0);  // 左
        }
        else if (angle >= 202.5f && angle < 247.5f)
        {
            directionText = "左下 (-1, -1)";
            UpdateSampleText(directionText);
            return new Vector2(-1, -1);  // 左下
        }
        else if (angle >= 247.5f && angle < 292.5f)
        {
            directionText = "下 (0, -1)";
            UpdateSampleText(directionText);
            return new Vector2(0, -1);  // 下
        }
        else
        {
            directionText = "右下 (1, -1)";
            UpdateSampleText(directionText);
            return new Vector2(1, -1);  // 右下
        }
    }

// UIテキストを更新するメソッド
    void UpdateSampleText(string message)
    {
        if (debugDirectionText != null)
        {
            debugDirectionText.text = message;
        }
    }



    private void OnCollisionExit(Collision collision)
    {
        Debug.Log("OnCollisionExit triggered");  // ここでログを出力
        if (collision.gameObject.CompareTag("tenzi"))
        {
            Debug.Log("Exit");
            meshRenderer.material = mat1;
            
            UpdateDebugText("Exit detected with tenzi object.");
        }
    }
    void UpdateDebugText(string message)
    {
        if (debugText != null)
        {
            debugText.text = message;
        }
    }
    
}
