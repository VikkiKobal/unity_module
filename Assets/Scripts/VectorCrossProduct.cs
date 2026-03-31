using UnityEngine;

public class Vector3D
{
    public float X, Y, Z;

    public Vector3D(float x, float y, float z)
    {
        X = x; Y = y; Z = z;
    }

    public Vector3D CrossProduct(Vector3D b)
    {
        return new Vector3D(
            Y * b.Z - Z * b.Y,   // i
            Z * b.X - X * b.Z,   // j
            X * b.Y - Y * b.X    // k
        );
    }

    public override string ToString()
        => $"({X}, {Y}, {Z})";
}

public class VectorCrossProduct : MonoBehaviour
{
    private string resultText = "";

    void Start()
    {
        var a = new Vector3D(1, -5, 7);
        var b = new Vector3D(2,  0, -6);

        Vector3D cross = a.CrossProduct(b);

        resultText =
            $"a = {a}\n" +
            $"b = {b}\n" +
            $"a × b = {cross}";

        Debug.Log("=== Векторний добуток ===");
        Debug.Log($"a = {a}");
        Debug.Log($"b = {b}");
        Debug.Log($"a × b = {cross}");
    }

    void OnGUI()
    {
        GUI.Box(new Rect(10, 110, 280, 100), "Векторний добуток");
        GUI.Label(new Rect(20, 132, 260, 22), $"a = (1, -5, 7)");
        GUI.Label(new Rect(20, 154, 260, 22), $"b = (2, 0, -6)");

        GUI.color = Color.yellow;
        GUI.Label(new Rect(20, 176, 260, 22), resultText.Split('\n')[2]); // a × b рядок
        GUI.color = Color.white;
    }
}
