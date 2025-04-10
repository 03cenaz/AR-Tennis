using UnityEngine;
using TMPro;

public class tennisBall : MonoBehaviour
{
    public static int courtCollisionCount = 0;  // previous: private int courtCollisionCount = 0;
    private int ballCollisionCount = 0;
    public TextMeshProUGUI scoreText;
    public AudioClip bounceSound; // SES KLİBİ
    private AudioSource audioSource;

    // For launching new ball
    public GameObject ballPrefab;
    public Transform spawnPoint;  
    public float launchForce;

    private bool inside;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        UpdateScoreText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        // VOICE
        if (bounceSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(bounceSound);
        }

        // BALL HIT NUMBER
        ballCollisionCount ++;
        if (collision.gameObject.CompareTag("Court_Inner"))
        {
          //  courtCollisionCount++;
          //  ballCollisionCount ++;
            inside = true;
            Debug.Log("Ball Hit Court! " + courtCollisionCount);
        //    UpdateScoreText();

            LaunchNewBall(); // ➕ Yeni top fırlat
        } else {    // out of the court
            inside = false;
            string objName = collision.gameObject.name;
            string objTag = collision.gameObject.tag;
            Debug.Log("Ball Hit Outside! name: " + objName + " | Tag: " + objTag + " | ballCollisionCount: " + ballCollisionCount);
          //  courtCollisionCount = 0;
        }

        // UPDATE COURT COLLISION NUMBER
        if(ballCollisionCount == 1 && !inside){    // Ball first fall outside
            courtCollisionCount = 0;
        } else if(ballCollisionCount == 1 && inside){ // Ball first fall inside
            courtCollisionCount++;
        }

        UpdateScoreText();
        
    }

     private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + courtCollisionCount;
        }
    }

    private void LaunchNewBall()
    {
        if (ballPrefab != null && spawnPoint != null)
        {
            GameObject newBall = Instantiate(ballPrefab, spawnPoint.position, Quaternion.identity);
            Rigidbody rb = newBall.GetComponent<Rigidbody>();
            if (rb != null)
            {
                /*
                // Fırlatma yönü: spawnPoint'in ileri yönü
                // .forward → Z pozitif yönü (mavi ok)
                // .right → X pozitif yönü (kırmızı ok)
                // .up → Y pozitif yönü (yeşil ok)

                // ForceMode.Force → sürekli kuvvet uygular
                // ForceMode.Impulse → tek seferde anlık kuvvet
                // ForceMode.VelocityChange → kütleyi önemsemeden doğrudan hız ekler (çok sert olur)
                // ForceMode.Acceleration → ivme bazlı (kütleyle uyumlu
                */

                rb.AddForce(spawnPoint.right * launchForce, ForceMode.Impulse); // To send Ball straight
                rb.AddForce(spawnPoint.forward * Random.Range(-0.3f, 0.3f), ForceMode.Impulse); // To send ball between left and right side a little

                Debug.Log("NEW BALL");

            }
        }
    }
}
