using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static System.Int32;
using static System.Text.RegularExpressions.Regex;

public class Blackjack : MonoBehaviour
{
    public GameObject explore;

    [Header("Player Cards:")]
    [SerializeField] SpriteRenderer PlayerCard0;
    [SerializeField] SpriteRenderer PlayerCard1;
    [SerializeField] SpriteRenderer PlayerCard2;
    [SerializeField] SpriteRenderer PlayerCard3;
    [SerializeField] SpriteRenderer PlayerCard4;
    [SerializeField] SpriteRenderer PlayerCard5;
    [SerializeField] SpriteRenderer PlayerCard6;
    [SerializeField] SpriteRenderer PlayerCard7;

    [Header("Actual Card Sprites:")]
    [SerializeField] Sprite[] DiamondArray;
    [SerializeField] Sprite[] ClubsArray;
    [SerializeField] Sprite[] HeartsArray;
    [SerializeField] Sprite[] SpadesArray;

    [Header("Dealer Cards:")]
    [SerializeField] SpriteRenderer DealerCard0;
    [SerializeField] SpriteRenderer DealerCard1;
    [SerializeField] SpriteRenderer DealerCard2;
    [SerializeField] SpriteRenderer DealerCard3;
    [SerializeField] SpriteRenderer DealerCard4;
    [SerializeField] SpriteRenderer DealerCard5;
    [SerializeField] SpriteRenderer DealerCard6;
    [SerializeField] SpriteRenderer DealerCard7;
    List<SpriteRenderer> DealerCardSprites;

    [Header("Labels:")]
    [SerializeField] Text PlayerLabel;
    [SerializeField] Text DealerLabel;
    [SerializeField] Text OutputLabel;

    [Header("Buttons:")]
    [SerializeField] Button Hit;
    [SerializeField] Button Stay;

    // Arrays for actual deck
    readonly private string[] cards = {"2","3","4","5","6","7","8","9","T","J","Q","K","A"};
    readonly private string[] symbols = {"♥","♦","♧","♤"};
    private List<string> deck = new List<string>();

    //Players and dealers in play hands
    private List<string> DealerCards = new List<string>();
    private List<string> PlayerCards = new List<string>();

    //Player evaluation variables
    private int PlayerEvaluation;
    private bool Soft;

    //Dealer evaluation variables
    private int DealerEvaluation;
    private bool DealerSoft;

    //Counts cards played so you can add the next card to each hands
    private int CardCounter = 0;

    private List<SpriteRenderer> CardSprites;

    //--------------------------------------------------------------------------
    private void Start()
    {
        explore = GameObject.Find("Main Camera");

        //Puts all card reneders in a list to update them with the cards throughout the game
        CardSprites = new List<SpriteRenderer> { PlayerCard0, PlayerCard1, PlayerCard2, PlayerCard3, PlayerCard4, PlayerCard5, PlayerCard6, PlayerCard7};
        DealerCardSprites = new List<SpriteRenderer> { DealerCard0, DealerCard1, DealerCard2, DealerCard3, DealerCard4, DealerCard5, DealerCard6, DealerCard7};
        
        ///Making deck
        for(int i = 0; i < 4; i++)
        {
            for(int j = 0; j < 13; j++ )
            {
                string card = cards[j] + symbols[i];
                deck.Add(card);
            }
        }
        
        Shuffle();
        
        //Assigning first cards
        PlayerCards.Add(deck[0]);
        DealerCards.Add(deck[1]);
        PlayerCards.Add(deck[2]);
        DealerCards.Add(deck[3]);
        CardCounter += 4;

        UpdatePlayerLabels();
        EvaluatePlayer();
        ShowDealerStarterCards();
        EnableButtons();
    }

    //--------------------------------------------------------------------------
    private void EvaluatePlayer()
    {
        int Evaluation = 0;
        //All cards that are equal to ten points
        List<string> TenValues = new List<string>()
        {"J", "Q", "K" ,"T"};

        //Loops through deck and calculates the deck value
        foreach(string card in PlayerCards)
        {
            string CardComponent = Replace(card, "[^0-9a-zA-Z]+", "");
            if (TenValues.Contains(CardComponent))
            {
                Evaluation += 10;
            }
            else if(CardComponent == "A")
            {
                Evaluation += 1;
                Soft = true;
            }
            else
            {
                int Number = Parse(CardComponent);
                Evaluation += Number;
            }
                  
        }
        //Now checks if the player has a soft hand meaning he has an ace
        //If he has an ace the hand is worth the value + 10 if its under 21
        PlayerEvaluation = Evaluation;
        string Output = "";
        if (Soft)
        {
            if(Evaluation + 10 == 21)
            {
                //Soft 21 is instant 21 as that would be something like Ace Jack which is black jack
                Evaluation = 21;
            }
            else if (Evaluation + 10 > 21)
            {
                //If Soft hand is above 21 so for example an Ace , 6 and 6 making the soft 23 which is invalid
                //The only valid value is 17 making the Ace just a normal 11
                Output = $"{Evaluation}";
                Soft = false;
            }
            else
            {
                //Both soft values are acceptable so its prints NormalValue/SoftValue
                Output = $"{Evaluation}/{Evaluation + 10}";
            }
        }
        else
        {
            //No Soft so evaluation is constant
            Output = Evaluation.ToString();
        }

        if(Evaluation == 21)
        {
            //21 instant win
            OutputLabel.text = "Blackjack Player Wins";
            GameEnded(true);
        }
        else if(Evaluation > 21)
        {
            //Bust so instant lose
            OutputLabel.text = "Player Busts Dealer Wins";
            GameEnded(false);
        }

        PlayerLabel.text += "\n" + Output;
    }

    //--------------------------------------------------------------------------
    private void ShowDealerStarterCards()
    {
        //This function shows the cards on the screen
        string Card = DealerCards[1];
        char Value = Card[0];
        char Symbol = Card[1];
        var ActualValue = Value switch
        {
            'A' => 1,
            'T' => 10,
            'J' => 11,
            'Q' => 12,
            'K' => 13,
            _ => Value - '0',
        };
        switch (Symbol)
        {
            case '♥':
                DealerCardSprites[1].sprite = HeartsArray[ActualValue];
                break;
            case '♦':
                DealerCardSprites[1].sprite = DiamondArray[ActualValue];
                break;
            case '♧':
                DealerCardSprites[1].sprite = ClubsArray[ActualValue];
                break;
            case '♤':
                DealerCardSprites[1].sprite = SpadesArray[ActualValue];
                break;
        }
        //Back facing card as first card is hidden from player
        DealerCardSprites[0].sprite  = ClubsArray[0];
    }

    //--------------------------------------------------------------------------
    private void EvaluateDealer()
    {
        int Evaluation = 0;
        //All cards that are equal to ten points
        List<string> TenValues = new List<string>()
        {"J", "Q", "K" ,"T"};

        //Loops through deck and calculates the deck value
        foreach(string card in DealerCards)
        {
            string CardComponent = Replace(card, "[^0-9a-zA-Z]+", "");
            if (TenValues.Contains(CardComponent))
            {
                Evaluation += 10;
            }
            else if(CardComponent == "A")
            {
                Evaluation += 1;
                DealerSoft = true;
            }
            else
            {
                int Number = Parse(CardComponent);
                Evaluation += Number;
            }
        }
        DealerEvaluation = Evaluation;
        string Output = "";
        if (DealerSoft)
        {
            if(Evaluation + 10 == 21)
            {
                DealerEvaluation = 21;
                DealerSoft = false;
                
            }
            else if (Evaluation + 10 > 21)
            {
                Output = $"{Evaluation}";
                DealerSoft = false;
            }
            else if (Evaluation + 10 > 17)
            {
                DealerEvaluation = Evaluation + 10;
                DealerSoft = false;
            }
            else
            {
                Output = $"{Evaluation}/{Evaluation + 10}";
            }
        }
        else
        {
            Output = Evaluation.ToString();
        }
        DealerLabel.text += "\n" + Output;
        
    }

    //--------------------------------------------------------------------------
    private void UpdateDealerLabels()
    {
        DealerLabel.text = $"Dealer Cards: \n {string.Join(", ", DealerCards)}";
        int Count = 0;
        foreach (string Card in DealerCards)
        {
            char Value = Card[0];
            char Symbol = Card[1];
            var ActualValue = Value switch
            {
                'A' => 1,
                'T' => 10,
                'J' => 11,
                'Q' => 12,
                'K' => 13,
                _ => Value - '0',
            };
            switch (Symbol)
            {
                case '♥':
                    DealerCardSprites[Count].sprite = HeartsArray[ActualValue];
                    break;
                case '♦':
                    DealerCardSprites[Count].sprite = DiamondArray[ActualValue];
                    break;
                case '♧':
                    DealerCardSprites[Count].sprite = ClubsArray[ActualValue];
                    break;
                case '♤':
                    DealerCardSprites[Count].sprite = SpadesArray[ActualValue];
                    break;
            }
            Count++;
        }
    }

    //--------------------------------------------------------------------------
    private void UpdatePlayerLabels()
    {
        PlayerLabel.text = $"Player Cards: \n {string.Join(", ", PlayerCards)}";
        int Count = 0;
        foreach(string Card in PlayerCards)
        {
            char Value = Card[0];
            char Symbol = Card[1];
            var ActualValue = Value switch
            {
                'A' => 1,
                'T' => 10,
                'J' => 11,
                'Q' => 12,
                'K' => 13,
                _ => Value - '0',
            };
            switch (Symbol)
            {
                case '♥':
                    CardSprites[Count].sprite = HeartsArray[ActualValue];
                    break;
                case '♦':
                    CardSprites[Count].sprite = DiamondArray[ActualValue];
                    break;
                case '♧':
                    CardSprites[Count].sprite = ClubsArray[ActualValue];
                    break;
                case '♤':
                    CardSprites[Count].sprite = SpadesArray[ActualValue];
                    break;
            }
            Count ++; 
        }
    }

    //--------------------------------------------------------------------------
    private void Shuffle() 
    {
        for (int i = 0; i < deck.Count; i++)
        {
            string temp = deck[i];
            int randomIndex = Random.Range(i, deck.Count);
            deck[i] = deck[randomIndex];
            deck[randomIndex] = temp;
        }
    }

    //--------------------------------------------------------------------------
    private void DisableButtons()
    {
        Hit.interactable = false;
        Stay.interactable = false;
    }

    //--------------------------------------------------------------------------
    private void EnableButtons()
    {
        Hit.interactable = true;
        Stay.interactable = true;
    }

    //--------------------------------------------------------------------------
    private IEnumerator DealersTurn()
    {
        bool Playing = true;
        UpdateDealerLabels();
        while(Playing)
        {
            UpdateDealerLabels();
            EvaluateDealer();
            yield return new WaitForSeconds(0.1F);
            
            //Dealer hits till 16 and stands on 17
            if (DealerEvaluation > 16)
            {
                Playing = false;
            }
            else
            {
                DealerCards.Add(deck[CardCounter]);
                CardCounter ++;
            }   
        }
        FinishGame();
    }

    //--------------------------------------------------------------------------
    public void HitButton()
    {
        PlayerCards.Add(deck[CardCounter]);
        CardCounter ++;
        UpdatePlayerLabels();
        EvaluatePlayer();
    }

    //--------------------------------------------------------------------------
    public void StayButton()
    {
        if(Soft){
            PlayerEvaluation += 10;
        }
        DisableButtons();
        StartCoroutine(DealersTurn());
    }

    //--------------------------------------------------------------------------
    private void FinishGame()
    {
        DealerLabel.text = $"Dealer Cards: \n {string.Join(", ", DealerCards)} \n{DealerEvaluation}";
        PlayerLabel.text = $"Player Cards: \n {string.Join(", ", PlayerCards)} \n{PlayerEvaluation}";
        if (DealerEvaluation > 21)
        {
            OutputLabel.text = "Dealer Busts Player Wins";
            GameEnded(true);
        }
        else if (DealerEvaluation > PlayerEvaluation)
        {
            OutputLabel.text = "Dealer Wins";
            GameEnded(false);
        }
        else if (PlayerEvaluation > DealerEvaluation)
        {
            OutputLabel.text = "Player Wins";
            GameEnded(true);
        }
        else if (PlayerEvaluation == DealerEvaluation)
        {
            OutputLabel.text = "Push";
            // DRAW
            GameEnded(true);

        }
    }

    //--------------------------------------------------------------------------
    private void GameEnded(bool won)
    {
        GameManager.Game.BlackjackResults(won);
        GameObject.Find("Hit").SetActive(false);
        GameObject.Find("Stay").SetActive(false);
        Instantiate(GameManager.Game.exitEncounterPrefab, GameObject.Find("Canvas").transform);
    }
}

