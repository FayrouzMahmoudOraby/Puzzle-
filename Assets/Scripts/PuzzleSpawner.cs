using UnityEngine;
using System.Collections.Generic;

public class PuzzleSpawner : MonoBehaviour
{
    public GameObject redCubePrefab;
    public GameObject greenCubePrefab;
    public GameObject blueCubePrefab;

    public GameObject slotPrefab; // White slot
    public int numberOfCubes = 3;

    private Vector2 cubeXRange = new Vector2(-4.5f, -1f);
    private Vector2 slotXRange = new Vector2(1f, 4.5f);
    private Vector2 zRange = new Vector2(-4f, 4f);

    void Start()
    {
        SpawnCubes();
        SpawnSlots();
    }

    void SpawnCubes()
    {
        GameObject[] cubePrefabs = new GameObject[] { redCubePrefab, greenCubePrefab, blueCubePrefab };
        foreach (GameObject cubePrefab in cubePrefabs)
        {
            Vector3 pos = new Vector3(Random.Range(cubeXRange.x, cubeXRange.y), 0.5f, Random.Range(zRange.x, zRange.y));
            Instantiate(cubePrefab, pos, Quaternion.identity);
        }
    }

    void SpawnSlots()
    {
        string[] tags = new string[] { "Red", "Green", "Blue" };
        float spacing = 3f; // space between slots
        float startZ = -spacing; // center the slots across the Z axis

        for (int i = 0; i < tags.Length; i++)
        {
            Vector3 pos = new Vector3(
                Random.Range(slotXRange.x, slotXRange.y), 
                0.01f, 
                startZ + (i * spacing)
            );

            GameObject slot = Instantiate(slotPrefab, pos, Quaternion.identity);
            
            // Assign the Unity GameObject tag to slot
            slot.tag = tags[i];

            // Setup detector
            SlotDetector detector = slot.GetComponent<SlotDetector>();
            if (detector != null)
                detector.acceptedTag = tags[i];

            // Create clue text above the slot
            TextMesh clueText = new GameObject("Clue").AddComponent<TextMesh>();
            clueText.text = GetClue(tags[i]);
            clueText.fontSize = 48;
            clueText.characterSize = 0.2f;
            clueText.anchor = TextAnchor.MiddleCenter;
            clueText.transform.position = pos + new Vector3(0, 1f, 0);
        }
    }


    string GetClue(string tag)
    {
        switch (tag)
        {
            case "Red": return "Fiery Passion";
            case "Green": return "Nature’s Calm";
            case "Blue": return "Ocean Deep";
            default: return "???";
        }
    }
}
