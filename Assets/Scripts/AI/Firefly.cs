using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firefly : MonoBehaviour
{
	public GameObject marker;
	public GameObject othermarker;
	private Vector3[] checkpoints = new Vector3[4];
	float counter = 0f;
	Vector3 myPosition;
	Vector3 endPos;

	// Start is called before the first frame update
	void Start()
	{
		endPos = NewEndPos();
		GeneratePoints(transform.position, endPos);
	}

	// Update is called once per frame
	void Update()
	{
		counter += Time.deltaTime / 100f;
		GetBezier(out myPosition, checkpoints, counter);

		if (Vector3Equal(transform.position, checkpoints[3]))
		{
			// reached final checkpoint, find new position
			counter = 0.0f;
			endPos = NewEndPos();
			GeneratePoints(transform.position, endPos);
		}
		else
			transform.position = myPosition;
	}

	Vector3 NewEndPos()
	{
		Vector3 newPos = (Vector2)transform.position + Random.insideUnitCircle;
		Debug.Log("newPos is : " + newPos);
		return newPos;
    }

	void GeneratePoints(Vector3 start, Vector3 end)
	{
		checkpoints[0] = start;
		checkpoints[3] = end;

		float negative;
		Vector3 randOffset;

		// creating first checkpoint
		Vector3 one = start + (end - start) / 3.0f; // first third
		negative = Random.Range(0f, 1f);
		negative = (negative > 0.5f) ? 1f : -1f;
		randOffset = new Vector3(0f, Random.Range(0.5f, 1f) * negative);
		one += randOffset * Vector3.Distance(one, start);
		one.x = 0f;
		checkpoints[1] = one;

		// creating second checkpoint
		Vector3 two = end - (end - start) / 4.0f; // last quarter
		negative = Random.Range(0f, 1f);
		negative = (negative > 0.5f) ? 1f : -1f;
		randOffset = new Vector3(0f, Random.Range(0.5f, 1f) * negative);
		two += randOffset * Vector3.Distance(end, two);
		two.x = 0f;
		checkpoints[2] = two;

		Instantiate(marker, checkpoints[0], Quaternion.identity);
		Instantiate(marker, checkpoints[1], Quaternion.identity);
		Instantiate(marker, checkpoints[2], Quaternion.identity);
		Instantiate(othermarker, endPos, Quaternion.identity);
	}

	void GetBezier(out Vector3 pos, Vector3[] points, float time)
	{
		float tt = time * time;
		float ttt = time * tt;

		float u = 1f - time;
		float uu = u * u;
		float uuu = u * uu;

		pos = uuu * points[0];
		pos += 3f * uu * time * points[1];
		pos += 3f * u * tt * points[2];
		pos += ttt * points[3];
	}

	public bool Vector3Equal(Vector3 a, Vector3 b)
	{
		return Vector3.SqrMagnitude(a - b) < 0.01;
	}

}
