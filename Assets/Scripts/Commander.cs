using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Commander : MonoBehaviour
{
    public static Commander Instance { get; private set; }
    
    public enum Direction
    {
        Left, Right
    }

    public Bird player;
    public GameObject LeftWall;
    public List<Transform> LeftSpikes;
    public GameObject RightWall;
    public List<Transform> RightSpikes;
    public int NumberOfSpikes = 10;
    public float SpikePositionOffset = -0.82f;
    public int spikesPerSwap = 3;
    

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        LeftSpikes = SpawnInitialSpikes(LeftWall.transform);
        RightSpikes = SpawnInitialSpikes(RightWall.transform);
    }

    private void Update() {
        if (player.start == false && Input.GetKeyDown(KeyCode.Mouse0))
        {
            Play();
        }
    }

    public void Play()
    {
        player.start = true;
        GenerateSpikes(Direction.Right);
    }

    public void Lose()
    {
        player.Reset();
    }

    public void GenerateSpikes(Direction dir)
    {
        if (dir == Direction.Right) GenerateSpikes(RightSpikes);
        else GenerateSpikes(LeftSpikes);
    }

    private void GenerateSpikes(List<Transform> spikes)
    {
        foreach (Transform child in RightSpikes) child.gameObject.SetActive(false);
        foreach (Transform child in LeftSpikes) child.gameObject.SetActive(false);

        // Shuffle
        for (int i = 0; i < spikes.Count; i++) {
            Transform temp = spikes[i];
            int randomIndex = Random.Range(i, spikes.Count);
            spikes[i] = spikes[randomIndex];
            spikes[randomIndex] = temp;
        }

        for (int i = 0; i < spikesPerSwap; i++)
        {
            spikes[i].gameObject.SetActive(true);
        }
    }

    public List<Transform> SpawnInitialSpikes(Transform wall)
    {
        List<Transform> children = new List<Transform>();

        Transform firstChild = wall.GetChild(0);

        Vector3 previousPosition = new Vector3(firstChild.position.x, firstChild.position.y, firstChild.position.z);

        for (int i = 0; i < NumberOfSpikes; i++)
        {
        
            Vector3 newPosition = new Vector3(previousPosition.x, previousPosition.y + SpikePositionOffset, previousPosition.z);
            GameObject newSpike = Instantiate(firstChild.gameObject, newPosition, firstChild.rotation, wall);
            newSpike.SetActive(false);
            previousPosition = newPosition;
            children.Add(newSpike.transform);
        }

        firstChild.gameObject.SetActive(false);
        children.Add(firstChild);

        return children;
    }
}
