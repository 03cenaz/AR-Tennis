using UnityEngine;
using TMPro;

public class tennisBall : MonoBehaviour
{
    private int courtCollisionCount = 0;
    public TextMeshProUGUI scoreText;
    public AudioClip bounceSound; // SES KLİBİ
    private AudioSource audioSource;

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
        if (collision.gameObject.CompareTag("Court"))
        {
            courtCollisionCount++;
            Debug.Log("Ball Hit Court! " + courtCollisionCount);
            UpdateScoreText();

            if (bounceSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(bounceSound);
            }
        }
    }

     private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + courtCollisionCount;
        }
    }
}
