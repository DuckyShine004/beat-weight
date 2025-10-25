using System.Collections;
using TMPro;
using UnityEngine;

/// <summary>
/// Applies a wave-like vertical animation to a TextMeshPro (TMP) text component,
/// giving each character an independent sine-based motion over time.
/// </summary>
[RequireComponent(typeof(TMP_Text))]
public class Wave : MonoBehaviour
{
    [Header("Wave attributes")]
    public float amplitude;
    public float speed;
    public float waveLength;

    private TMP_Text text;

    /// <summary>
    /// Caches the TMP_Text component when the object is first initialised.
    /// </summary>
    void Awake() => text = GetComponent<TMP_Text>();

    /// <summary>
    /// Starts the text wave animation coroutine when the component is enabled.
    /// </summary>
    void OnEnable() => StartCoroutine(Animate());

    /// <summary>
    /// Stops all running coroutines (the animation) when the component is disabled.
    /// </summary>
    void OnDisable() => StopAllCoroutines();

    /// <summary>
    /// Coroutine that continuously updates vertex positions of the text mesh
    /// to create a smooth sine wave motion across characters.
    /// </summary>
    IEnumerator Animate()
    {
        while (true)
        {
            text.ForceMeshUpdate();

            TMP_TextInfo textInfo = text.textInfo;

            for (int i = 0; i < textInfo.characterCount; ++i)
            {
                TMP_CharacterInfo characterInfo = textInfo.characterInfo[i];

                int materialReferenceIndex = characterInfo.materialReferenceIndex;
                int vertexIndex = characterInfo.vertexIndex;

                Vector3[] vertices = textInfo.meshInfo[materialReferenceIndex].vertices;

                float phase = (i / waveLength) + (Time.time * speed);
                float offsetY = Mathf.Sin(phase * Mathf.PI * 2.0f) * amplitude;

                Vector3 offset = new Vector3(0.0f, offsetY, 0.0f);

                for (int j = 0; j <= 3; ++j)
                {
                    vertices[vertexIndex + j] += offset;
                }
            }

            for (int materialIndex = 0; materialIndex < textInfo.meshInfo.Length; ++materialIndex)
            {
                TMP_MeshInfo meshInfo = textInfo.meshInfo[materialIndex];

                meshInfo.mesh.vertices = meshInfo.vertices;

                text.UpdateGeometry(meshInfo.mesh, materialIndex);
            }

            yield return null;
        }
    }
}
