using UnityEngine;
using UnityEngine.UI;
using static System.Math;

public class Roulette : MonoBehaviour
{
    private float Friction = 0.04F;
    private float Acceleration = 0.2F;
    private float StartingLocation = 97.98473F;

    public Rigidbody2D Spinner;
    private float speed = 0F;
    private bool SpeedUp = true;
    private bool Spinning;
    private string BetPlaced;

    [SerializeField] Button RedBut;
    [SerializeField] Button GreenBut;
    [SerializeField] Button BlueBut;

    //--------------------------------------------------------------------------
    void Start()
    {
        //ReSpin();
    }

    //--------------------------------------------------------------------------
    public void ReSpin()
    {
        Friction = Random.Range(0.035F,0.045F);
        Acceleration = 0.2F;
        SpeedUp = true;
        speed = 0F;
        Spinner.position = new Vector2(StartingLocation,2.229621F);
        Spinner.velocity = Vector2.left * speed;
        Spinning = true;
    }
    //--------------------------------------------------------------------------
    private void Update()
    {
        if(Spinning)
        {
            if(SpeedUp)
            {
                speed -= Acceleration;
                Spinner.velocity = Vector2.right * speed;
                if(speed < -40F)
                {
                    SpeedUp = false;
                }
            }
            else
            {
                if(speed < 0)
                {
                    speed += Friction;
                    Spinner.velocity = Vector2.right * speed;
                }
                else
                {
                    Spinner.velocity = Vector2.zero;
                    Spinning = false;
                    FindValue();
                }
            }
        }
    }

    //--------------------------------------------------------------------------
    private void FindValue()
    {
        float Position = Spinner.position[0];
        float Difference = StartingLocation - Position;
        string Winner = "";
        double MovedBy = (((Truncate(Difference / 0.56F)) % 9)+ 5 )%9;
        switch(MovedBy)
        {
            case 0: case 2: case 5: case 7:
                Debug.Log(MovedBy.ToString() + " Blue");
                Winner = "Blue";
                break;
            case 1: case 3: case 6: case 8:
                Debug.Log(MovedBy.ToString() + " Red");
                Winner = "Red";
                break;
            case 4:
                Debug.Log(MovedBy.ToString() + " Green");
                Winner = "Green";
                break;
        }
        if(Winner == BetPlaced)
        {
            Debug.Log("You Win!");
        }
        else
        {
            Debug.Log("You Suck!");
        }
        //ReSpin();
    }

    //--------------------------------------------------------------------------
    public void RedButton()
    {
        BetPlaced = "Red";
        DisableButtons();
        ReSpin();
        EnableButtons();
    }

    //--------------------------------------------------------------------------
    public void GreenButton()
    {
        BetPlaced = "Green";
        DisableButtons();
        ReSpin();
        EnableButtons();
    }

    //--------------------------------------------------------------------------
    public void BlueButton()
    {
        BetPlaced = "Blue";
        DisableButtons();
        ReSpin();
        EnableButtons();
    }

    //--------------------------------------------------------------------------
    void DisableButtons()
    {
        RedBut.enabled = false;
        GreenBut.enabled = false;
        BlueBut.enabled = false;
    }

    //--------------------------------------------------------------------------
    void EnableButtons()
    {
        RedBut.enabled = true;
        GreenBut.enabled = true;
        BlueBut.enabled = true;
    }
}
