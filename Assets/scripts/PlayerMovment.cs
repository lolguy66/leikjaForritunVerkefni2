using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading;

public class PlayerMovment : MonoBehaviour
{
    //default values og breytur fyrir leikinn.
    public float speed = 20;
    public float sideways = 20;
    public float jump = 20;
    public static int count;
    public Text countText;
    Rigidbody rb; // rigidbody fyrir gott jump
    private bool grounded = true;
    
    // Fixed update fer alltaf í gang 60 sinnum á sekúndu
    void FixedUpdate()
    {
        //sný player ef axis horisontal er ekki 0og ég veit að það er létt að optimiza þennan kóða en ég ákvað að hafa if helvíti í staðinn.
        if (Input.GetAxisRaw("Horizontal") == 1)
        {
            transform.Rotate(new Vector3(0, 5, 0)); // beygja hægri
        }
        if (Input.GetAxisRaw("Horizontal") == -1)//snúa leikmanni
        {
            transform.Rotate(new Vector3(0, -5, 0)); // beygja vinstri
        }
        if (Input.GetKey(KeyCode.Space) && grounded)// hoppa P.S. þetta virkar varla en þetta gerir eitthvað (: P.P.S Ég lagaði þetta með rigidbody.
        {
            rb.AddForce(new Vector3(0.0f,2.0f,0.0f) * jump, ForceMode.Impulse);
        }
        if (transform.position.y <= -1) // ef leikmaður dettur af mappinu
        {
            Endurræsa();
        }
        if (Input.GetAxisRaw("Vertical") == 1)//áfram og ég nota kúl axis í staðinn furir Input.GetKey
        {
            transform.position += transform.forward * speed ;
        }
        if (Input.GetAxisRaw("Vertical") == -1)// til baka
        {
            transform.position += -transform.forward * speed;

        }
        if (Input.GetKey("e"))//hægri strafe, ég gerði strafe á q og e því að mér líkar betur við þannig movement system.
        {
            transform.position += transform.right * sideways;
        }
        if (Input.GetKey("q"))//vinstri
        {
            //hreyfir player um sideways í hvert skipti sem ýtt er á leftArrow
            transform.position += -transform.right * sideways;
        }

        if (transform.position.y<=-1) // ef leykmaður dettur af þá er hann endurræstur
        {
            Endurræsa();
        }
    }
   
     void OnCollisionExit(Collision colission) // þegar leikmaður hættir að snerta eitthvað
    {
        if (colission.collider.tag == "Ground") // ef það er jörðin
        {
            grounded = false; // þá snertir hann ekki jörðina
        }
    }
     void OnCollisionEnter(Collision collision) // þegar leikmaður snertir eitthvað
    {

        if (collision.collider.tag == "Ground") // sama og áðan nema öfugt
        {
            grounded = true;
        }
        
        if (collision.collider.tag == "SmallCoin") // ef player keyrir á object sem heitir x
        {
            collision.collider.gameObject.SetActive(false);  // þá hverfur x
            count = count + 1; // peningur fer upp/niður
            SetCountText(); // og það er kallað á þetta fall
        }
        if (collision.collider.tag == "BigCoin")
        {
            collision.collider.gameObject.SetActive(false);
            count = count + 5;
            SetCountText();
        }
        if (collision.collider.tag == "BadBox")
        {
            collision.collider.gameObject.SetActive(false);
            count = count -1;
            SetCountText();
        }
        if (collision.collider.tag == "Finish") // ef leikmaður snertir endapúnkt þá hverfur hann og leikmaðurfer í næstu senu.
        {
            collision.collider.gameObject.SetActive(false);
            nextScene();
        }
    }
    void SetCountText()  // þetta er fall
    {
        countText.text = "Stig: " + count.ToString(); // þetta breytir ui element og segjir hversu mörg stig leikmaður er með
        if (count < 0) // ef leikmaður tapar öllum lífum
        {
            this.enabled = false;//kemur í veg fyrir að playerinn geti hreyfst áfram eftir dauðan
            countText.text = "Þú tapaðir /:"; // breytir texta í tapa texta
            count = 0; // endurstillir pening

            StartCoroutine(Bida()); // lætur leikmann bíða
            
        }
        
    }
    void Awake()// þegar það er nýkominn ný sena
    {
        rb = GetComponent<Rigidbody>(); // sækja rigidbody, veit ekki hvort þetta þarf að vera hér en ákvað bara að gera það

        if (SceneManager.GetActiveScene().buildIndex != 0) // textin segjir vanalega tutorial textan en þetta breytir því
        {
            countText.text = "Stig: "; // ef manneskja tók ekki upp tutorial coin'ið þá lætur þetta standa stig
            SetCountText(); // kalla á setcounttext fallið
        }
        if (SceneManager.GetActiveScene().buildIndex == 3)  // ef þú ert á lokaborði
        {  // sýnaþennan texta í staðinn fyrir stig:
            countText.text = "Þú kláraðir leikinn og endaðir með " + count + " stig, farðu á takkan til þess að byrja aftur.";
        }
    }
    void nextScene()
    {
        if (SceneManager.GetActiveScene().buildIndex != 3)  // ef það er ekki lokasenan
        { 
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // sækja númer kvaða senan sem er í gangi er og kalla á næstu.
        }
        else// ef þú varst að klára leikinn
        {
            count = 0; // reset pening
            SceneManager.LoadScene(0); // fara aftur í tutorial
        }


    }
    IEnumerator Bida() // bíða fallið
    {
        yield return new WaitForSeconds(1); // bíða í eina sekúndu
        Endurræsa(); // kalla á fall
    }
   
    public void Endurræsa() // fall sem endurræsir
    {
        SceneManager.LoadScene(0); // load'a fyrstu senu.
    }

}
