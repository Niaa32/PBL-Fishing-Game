using UnityEngine;
using System.Collections;

public class SandalMovement : MonoBehaviour
{
    public float moveSpeed = 2.0f; // Kecepatan pergerakan sandal
    public float randomMovementDuration = 10f; // Durasi pergerakan acak (dalam detik)
    public float snagDuration = 3f; // Durasi ketika sandal menyangkut pada kail (dalam detik)
    public Transform rodPosition; // Posisi titik kail
    public float maxDistanceFromRod = 5f; // Jarak maksimum dari kail sebelum mulai bergerak ke kail

    private bool isSnagged = false; // Flag untuk memeriksa apakah sandal menyangkut
    private bool isFreeMovement = false; // Flag untuk memeriksa pergerakan bebas
    private Vector3 randomTargetPosition; // Posisi target acak untuk pergerakan bebas

    void Start()
    {
        StartCoroutine(MoveSandal());
    }

    void Update()
    {
        if (isSnagged)
        {
            // Jika sandal menyangkut, berhenti bergerak selama durasi snag
            return;
        }

        // Pergerakan acak (durasi tertentu)
        if (isFreeMovement)
        {
            MoveFreely(); // Memanggil fungsi untuk pergerakan bebas
        }
        else
        {
            MoveToRod(); // Menggerakkan sandal ke kail
        }
    }

    // Fungsi untuk memulai pergerakan sandal dengan durasi yang berbeda
    IEnumerator MoveSandal()
    {
        // Pergerakan acak pertama selama 15 detik
        yield return StartCoroutine(MoveRandomly(15f));

        // Pergerakan acak kedua selama 30 detik
        yield return StartCoroutine(MoveRandomly(30f));

        // Pergerakan acak ketiga selama 10 detik
        yield return StartCoroutine(MoveRandomly(10f));

        // Setelah selesai pergerakan acak, sandal akan menuju ke titik kail
        isFreeMovement = false;

        // Tunggu untuk pergerakan bebas lagi
        yield return new WaitForSeconds(3f);
        isFreeMovement = true;

        // Mengulang pergerakan bebas
        StartCoroutine(MoveSandal());
    }

    // Fungsi untuk bergerak secara acak
    IEnumerator MoveRandomly(float duration)
    {
        float timer = 0f;
        isFreeMovement = true;

        while (timer < duration)
        {
            // Setel target acak untuk pergerakan
            randomTargetPosition = new Vector3(transform.position.x + Random.Range(-5f, 5f), transform.position.y, transform.position.z + Random.Range(-5f, 5f));

            // Pergerakan ke target acak
            transform.position = Vector3.MoveTowards(transform.position, randomTargetPosition, moveSpeed * Time.deltaTime);

            // Timer untuk durasi acak
            timer += Time.deltaTime;

            yield return null;
        }

        // Setelah durasi pergerakan acak selesai, kembali ke gerakan normal menuju kail
        isFreeMovement = false;
    }

    // Fungsi untuk bergerak menuju titik kail
    void MoveToRod()
    {
        // Jika posisi sandal sudah cukup dekat dengan kail
        if (Vector3.Distance(transform.position, rodPosition.position) > maxDistanceFromRod)
        {
            transform.position = Vector3.MoveTowards(transform.position, rodPosition.position, moveSpeed * Time.deltaTime);
        }
        else
        {
            // Jika sudah dekat, sandal menyangkut pada kail dan mengganggu perikanan
            StartCoroutine(SnagSandal());
        }
    }

    // Fungsi untuk menangani keadaan sandal yang menyangkut pada kail
    IEnumerator SnagSandal()
    {
        isSnagged = true;
        Debug.Log("Sandal menyangkut pada kail dan mengganggu!");

        // Hentikan pergerakan selama 3 detik
        yield return new WaitForSeconds(snagDuration);

        isSnagged = false;

        // Kembali bergerak bebas setelah 3 detik
        isFreeMovement = true;
    }

    // Fungsi untuk pergerakan bebas sandal
    void MoveFreely()
    {
        // Jika sandal bergerak bebas, pergerakan acak dilakukan di sini
        transform.position = Vector3.MoveTowards(transform.position, randomTargetPosition, moveSpeed * Time.deltaTime);
    }
}
