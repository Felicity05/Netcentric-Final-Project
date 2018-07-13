////////////////////////////////////////
/*
//      KodaGames - Unity Video Tutorials
//        http://www.kodagames.com
*/
//             ©2013 KodaGames 
// The Read me File Has The Link to the Videos
////////////////////////////////////////

#pragma strict
import System.Collections.Generic;

@script ExecuteInEditMode()

var deck : List.<GameObject>;//the list of objects that will refill the cards list when the cards list is empty
var cards : List.<GameObject> = new List.<GameObject>();//card objects in game list that will be refilled by the deck once empty

var dealRebetText : TextMesh;

var lastBet : int;
var placeBet : int = 0;//taken from the roll and placed on the table
var cash : int = 1000;//chip Total bank account
var cashText : TextMesh;
var yourBet : TextMesh;
var bounceYourBetText : GameObject;
var bounceCashText : GameObject;
var placeBetText : GameObject;

var dealersHand : int;
var playersHand : int;

var howManyAces : int;
var dealerAces : int;

var dpos1 : Transform;//card positions for the dealer
var dpos2 : Transform;//card positions for the dealer
var dpos3 : Transform;//card positions for the dealer
var dpos4 : Transform;//card positions for the dealer
var dpos5 : Transform;//card positions for the dealer
var dpos6 : Transform;//card positions for the dealer
var dpos7 : Transform;//card positions for the dealer

var blank : GameObject;//The blank card for the dealer

var pPos1 : Transform;//card positions for the player
var pPos2 : Transform;//card positions for the player
var posHit1 : Transform;//card positions for the player
var posHit2 : Transform;//card positions for the player
var posHit3 : Transform;//card positions for the player
var posHit4 : Transform;//card positions for the player
var posHit5 : Transform;//card positions for the player

var textBackingDealer : GameObject;
var textBackingPlayer : GameObject;
var playerTextMesh : TextMesh;//players points(cards)
var dealerTextMesh : TextMesh;//dealers points(cards)
var prefix = "";
var dPrefix = "";

var waitTime : float = 1.0;

var dealerFirstCard : GameObject;

var dealer2Card : GameObject;
var dealer3Card : GameObject;
var dealer4Card : GameObject;
var dealer5Card : GameObject;
var dealer6Card : GameObject;
var dealer7Card : GameObject;

var playerFirstCard : GameObject;
var playerSecondCard : GameObject;
var playersHitCard1 : GameObject;
var playersHitCard2 : GameObject;
var playersHitCard3 : GameObject;
var playersHitCard4 : GameObject;
var playersHitCard5 : GameObject;

var playerHit1 : boolean = false;///////////////////make it false later
var playerHit2 : boolean = false;
var playerHit3 : boolean = false;
var playerHit4 : boolean = false;
var playerHit5 : boolean = false;

var dh2 : boolean = true;
var dh3 : boolean = false;
var dh4 : boolean = false;
var dh5 : boolean = false;
var dh6 : boolean = false;
var dh7 : boolean = false;

var blankCard : GameObject;

var youWinText : GameObject;
var youWinCube : GameObject;
var youLoseText : GameObject;
var youLoseCube : GameObject;
var bustedText : GameObject;
var bustedCube : GameObject;
var pushText : GameObject;
var pushCube : GameObject;
var dBJText : GameObject;
var dBJCube : GameObject;
var pBJText : GameObject;
var pBJCube : GameObject;

var pokerSkin : GUISkin;

var keepGoing : boolean = true;
var showStandButton : boolean = false;
var showDoubleDown : boolean = false;
var showDealButton : boolean = true;
var showRebetButton : boolean = false;
var hideHitButtons : boolean = false;
var madeBet : boolean = false;

var doubleDown : GameObject;

////////////////////////////////////////////////////////////
//var splitCard1Pos : Transform;//splits
//var splitCard2Pos : Transform;//splits
////////////////////////////////////////////////////////////

function Start()
{
    ResetDeck();
}
function Update()
{
	if(dealersHand >= 17)
	{
		keepGoing = false;
	}	
	else
	{
		keepGoing = true;	
	}
	yourBet.text = placeBet.ToString();//get placebet amount add it to yourBet text
	cashText.text = cash.ToString();//bankroll
}
function OnGUI () 
{
	GUI.skin = pokerSkin;
	
	GUI.Label (Rect (190,2,40,40), "", "blankchip");
	
	GUI.Label (Rect (615,160,75,75), "", "chip5label");
	GUI.Label (Rect (615,235,75,75), "", "chip25label");
	GUI.Label (Rect (615,310,75,75), "", "chip50label");
	GUI.Label (Rect (615,385,75,75), "", "chip100label");
	GUI.Label (Rect (615,460,75,75), "", "chip500label");
	
	GUI.Label (Rect (555,520,55,55), "", "rebetlabel");
	GUI.Label (Rect (400,520,55,55), "", "hitlabel");
	GUI.Label (Rect (330,520,55,55), "", "standlabel");
	GUI.Label (Rect (260,520,55,55), "", "doublelabel");
	 //GUI.Label (Rect (220,520,55,55), "", "splitlabel");/////////////////////////////////Split
	 
	 /////////////////////////////////////////////////////////////////////////////////////Split
	 /*
	 if(showSplitButton == true)
	 {
	 	if (GUI.Button (Rect (220,600,60,60), "", "split"))
	 	{
	 		//splitbutton
	 		print("SPLIT*******");
	 		//reposition the 2 cards to split them
	 		playerFirstCard.transform.position = splitCard1Pos.position;
	 		playerSecondCard.transform.position = splitCard2Pos.position;
	 		
	 		//have to create another set of hit buttons
	 		//have to edit stand button and make a split active var for 2 stands or make 2 stand buttons? just for this
	 		//have to make 2 win buttons & lose buttons & blackjack buttons 2 of all 
	 		//need to compare dealers hand with splitHand 1 and splitHand 2
	 		//need to disable textMeshPlayer and Create another one or just recalculate this one
	 		//reposition text meshes 
	 	}
	 }
	 */
	 /////////////////////////////////////////////////////////////////////////////////////
	
	if(placeBet > 0)
	{
		if(showDealButton == true)
		{
			dealRebetText.text = "Deal";
			
			if (GUI.Button (Rect (555,520,55,55), "", "deal"))
			{
				DestroyAll();
				StartDealing();
				showDealButton = false;
				madeBet = true;//hide chips
				hideHitButtons = false;
				placeBetText.gameObject.active = false;
				lastBet = placeBet;//record the last bet
				
			}
		}
	}
	if(cash > 0 && cash >= lastBet)//cash > 0 (need money to bet) & cash > lastBet (need enough money to rebet)
	{ 
		if(showRebetButton == true)
		{
			dealRebetText.text = "Rebet";
		
			if (GUI.Button (Rect (555,520,55,55), "", "rebet"))
			{
				DestroyAll();
				StartDealing();
				hideHitButtons = false;
				madeBet = true;
				placeBetText.active = false;
				yourBet.gameObject.active = true;
				showRebetButton = false;
				//Disabled itween on the bounceYourBetText which is the gameObject "YourBetText" see bug video
				//if you click really fast this effect messes up the text so we just removed it from this object
				//see video about at the very end which eplains why this is removed (only this gameObject and chips).
				//iTweenEvent.GetEvent(bounceYourBetText, "spring").Play();
			
				if(placeBet == 0)
				{
					placeBet = lastBet;
					cash -= lastBet;//take cash for last bet to be rebet
				}
			
			}
		}
	}
	
	if(showStandButton == true)
	{
		if (GUI.Button (Rect (330,520,55,55), "", "stand"))
		{
			FinishDealersHand();
			showStandButton = false;
			showDoubleDown = false;
			hideHitButtons = true;		
		}
	}
	
	if(cash >= lastBet)//cash has to be greater than last bet to doubledown or you have a negative value
	{
		if(showDoubleDown == true)
		{
			if (GUI.Button (Rect (260,520,55,55), "", "double"))
			{
				hideHitButtons = true;
				showStandButton = false;
				showDoubleDown = false;
			
				var doubleCard : GameObject = DealCard();
				doubleCard.transform.position = posHit1.transform.position;
				doubleCard.name = "double";
				doubleDown = GameObject.Find("double");
				var double1 = doubleDown.GetComponent(WhatCard).cardNumber;
				
				placeBet += lastBet;//take lastbet value & add it to placeBet
				cash -= lastBet;//also take lastBet from cash
				
				if(double1 == 11)
				{
					howManyAces += 1;
					playersHand += 11;
				}
				else
				{
					playersHand += double1;
				}
				CheckSum();
			
				if(playersHand < 21)
				{
					FinishDealersHand();
				}
			}
		}
	}
	
	if(madeBet == false)
	{
		placeBetText.gameObject.active = true;
		
		if(cash >= 5)
		{
			if (GUI.Button(Rect(615,160,75,75),"", "chip5" ))
			{
				yourBet.gameObject.active = true;
				placeBet += 5;
       			cash -= 5;
       			//iTweenEvent.GetEvent(bounceYourBetText, "spring").Play();
       			showDealButton = true;
       			showRebetButton = false;
			}
    	}
    	if(cash >= 25)
    	{
			if (GUI.Button(Rect(615,235,75,75),"", "chip25" ))
			{
				yourBet.gameObject.active = true;
				placeBet += 25;
        		cash -= 25;
        		//iTweenEvent.GetEvent(bounceYourBetText, "spring").Play();
        		showDealButton = true;
        		showRebetButton = false;
        	}
     	}
		if(cash >= 50)
		{
			if (GUI.Button(Rect(615,310,75,75),"", "chip50" ))
			{
				yourBet.gameObject.active = true;
				placeBet += 50;
       			cash -= 50;
       			//iTweenEvent.GetEvent(bounceYourBetText, "spring").Play();
       			showDealButton = true;
       			showRebetButton = false;
       		}
		}
		if(cash >= 100)
		{
			if (GUI.Button(Rect(615,385,75,75),"", "chip100" ))
			{
				yourBet.gameObject.active = true;
				placeBet += 100;
       			cash -= 100;
       			//iTweenEvent.GetEvent(bounceYourBetText, "spring").Play();
       			showDealButton = true;
       			showRebetButton = false;
			}
		}
		if(cash >= 500)
		{
			if (GUI.Button(Rect(615,460,75,75),"", "chip500" ))
			{
				yourBet.gameObject.active = true;
				placeBet += 500;
       			cash -= 500;
       			//iTweenEvent.GetEvent(bounceYourBetText, "spring").Play();
       			showDealButton = true;
       			showRebetButton = false;
			}
		}
	}
		
	
	if(hideHitButtons == false)
	{
		if(playerHit1)
		{
			showStandButton = true;
			showDoubleDown = true;
		
			if (GUI.Button (Rect (400,520,55,55), "", "hit"))
			{
				var hitCard1 : GameObject = DealCard();
				hitCard1.transform.position = posHit1.transform.position;
				hitCard1.name = "hit1";
				playersHitCard1 = GameObject.Find("hit1");
				var hit1 = playersHitCard1.GetComponent(WhatCard).cardNumber;
				if(hit1 == 11)
				{
					howManyAces += 1;
					playersHand += 11;
				}
				else
				{
					playersHand += hit1;//other numbers 10, 9, 8, 7, etc...
				}
				CheckSum();
				playerHit1 = false;
				playerHit2 = true;	
			}
		}
		if(playerHit2)
		{
			if (GUI.Button (Rect (400,520,55,55), "", "hit"))
			{
				var hitCard2 : GameObject = DealCard();
				hitCard2.transform.position = posHit2.transform.position;
				hitCard2.name = "hit2";
				playersHitCard2 = GameObject.Find("hit2");
				var hit2 = playersHitCard2.GetComponent(WhatCard).cardNumber;
				if(hit2 == 11)
				{
					howManyAces += 1;
					playersHand += 11;
				}
				else
				{
					playersHand += hit2;
				}
				CheckSum();
			
				playerHit2 = false;
				playerHit3 = true;
			}
		}
		if(playerHit3)
		{
			if (GUI.Button (Rect (400,520,55,55), "", "hit"))
			{
				var hitCard3 : GameObject = DealCard();
				hitCard3.transform.position = posHit3.transform.position;
				hitCard3.name = "hit3";
				playersHitCard3 = GameObject.Find("hit3");
				var hit3 = playersHitCard3.GetComponent(WhatCard).cardNumber;
				if(hit3 == 11)
				{
					howManyAces += 1;
					playersHand += 11;
				}
				else
				{
					playersHand += hit3;
				}
				CheckSum();
			
				playerHit3 = false;
				playerHit4 = true;
			}
		}
		if(playerHit4)
		{
			if (GUI.Button (Rect (400,520,55,55), "", "hit"))
			{
				var hitCard4 : GameObject = DealCard();
				hitCard4.transform.position = posHit4.transform.position;
				hitCard4.name = "hit4";
				playersHitCard4 = GameObject.Find("hit4");
				var hit4 = playersHitCard4.GetComponent(WhatCard).cardNumber;
				if(hit4 == 11)
				{
					howManyAces += 1;
					playersHand += 11;
				}
				else
				{
					playersHand += hit4;
				}
				CheckSum();
				
				playerHit4 = false;
				playerHit5 = true;
			}
		}
		if(playerHit5)
		{
			if (GUI.Button (Rect (400,520,55,55), "", "hit"))
			{
				var hitCard5 : GameObject = DealCard();
				hitCard5.transform.position = posHit5.transform.position;
				hitCard5.name = "hit5";
				playersHitCard5 = GameObject.Find("hit5");
				var hit5 = playersHitCard5.GetComponent(WhatCard).cardNumber;
				if(hit5 == 11)
				{
					howManyAces += 1;
					playersHand += 11;
				}
				else
				{
					playersHand += hit5;
				}
				CheckSum();
			
				playerHit5 = false;
			}
		}
	}
}
function StartDealing()
{
	yield WaitForSeconds(waitTime);
	
	//Players First Card
	var newCard3 : GameObject = DealCard();
	newCard3.transform.position = pPos1.transform.position;
    newCard3.name = "playerFirst";
    playerFirstCard = GameObject.Find("playerFirst");
    var card1Value = playerFirstCard.GetComponent(WhatCard).cardNumber;
    
    playersHand += card1Value;
    
    yield WaitForSeconds(waitTime);
	
	//Dealers First Card
	var newCard1 : GameObject = DealCard();
	newCard1.transform.position = dpos1.transform.position;
	newCard1.name = "dealerFirst";//give it a name to find it and get the integer value
	dealerFirstCard = GameObject.Find("dealerFirst");//find the "first" card
	var dCard1 = dealerFirstCard.GetComponent(WhatCard).cardNumber;
	
	dealersHand += dCard1;
	
	yield WaitForSeconds(waitTime);
	
	//PlayersSecond Card
	var newCard4 : GameObject = DealCard();
	newCard4.transform.position = pPos2.transform.position;
    newCard4.name = "playerSecond";
    playerSecondCard = GameObject.Find("playerSecond");
    var card2Value = playerSecondCard.GetComponent(WhatCard).cardNumber;
    
    playersHand += card2Value;
    
    yield WaitForSeconds(waitTime);
    
    /////////////////////////////////////////////////////////////////////////////////////Split
    ////
    //Split if 1st and second card have the same value
    /*
    if(card1Value == card2Value)
    {
    	showSplitButton = true;
    	print("these 2 cards are the Same!");
    }
    if(card1Value != card2Value)
    {
    	showSplitButton = false;	
    }
	*/
	/////////////////////////////////////////////////////////////////////////////////////
    
    Blank();
    CheckSum();
	CheckDealerSum();
    
    //player
    playerTextMesh.text = prefix + playersHand;//3d text to show players score
    playerTextMesh.gameObject.active = true;//enable players score
    textBackingPlayer.gameObject.active = true;//enable the blue backing for the score
    
    //dealer
    dealerTextMesh.text = dPrefix + dealersHand;
    dealerTextMesh.gameObject.active = true;
    textBackingDealer.gameObject.active = true;
    
    playerHit1 = true;
}
function Blank()
{
	blankCard = Instantiate(blank, dpos2.transform.position, dpos2.transform.rotation);
	blankCard.name = "blank";	
}

function FinishDealersHand()
{
	Destroy(blankCard);
	
	if(dh2 == true && keepGoing == true)
	{
		//dealer 2nd Card
		var dealer2 : GameObject = DealCard();
		dealer2.transform.position = dpos2.transform.position;//pos1 dealer card
   		dealer2.name = "dealerSecond";//give it a name to find it and get the integer value 
    	dealer2Card = GameObject.Find("dealerSecond");//find the "first" card
    	var dCard2 = dealer2Card.GetComponent(WhatCard).cardNumber;
    	if(dCard2 == 11)
		{
			dealerAces += 1;
			dealersHand += 11;	
		}
		else
		{
			dealersHand += dCard2;	
		}
		CheckDealerSum();
    	dh3 = true;
		yield WaitForSeconds(waitTime);
	}
	if(dh3 == true && keepGoing == true)
	{
		//dealer 3rd Card
		var dealer3 : GameObject = DealCard();
		dealer3.transform.position = dpos3.transform.position;//pos1 dealer card
   		dealer3.name = "dealer3";//give it a name to find it and get the integer value 
    	dealer3Card = GameObject.Find("dealer3");//find the "first" card
    	var dCard3 = dealer3Card.GetComponent(WhatCard).cardNumber;
    	if(dCard3 == 11)
		{
			dealerAces += 1;
			dealersHand += 11;	
		}
		else
		{
			dealersHand += dCard3;	
		}
		
		CheckDealerSum();    	
		dh4 = true;
		yield WaitForSeconds(waitTime);
	}
	if(dh4 == true && keepGoing == true)
	{
		//dealer 4th Card
		var dealer4 : GameObject = DealCard();
		dealer4.transform.position = dpos4.transform.position;//pos1 dealer card
   		dealer4.name = "dealer4";//give it a name to find it and get the integer value 
    	dealer4Card = GameObject.Find("dealer4");//find the "first" card
    	var dCard4 = dealer4Card.GetComponent(WhatCard).cardNumber;
    	if(dCard4 == 11)
		{
			dealerAces += 1;
			dealersHand += 11;	
		}
		else
		{
			dealersHand += dCard4;	
		}
		
		CheckDealerSum();
    	
		dh5 = true;
		yield WaitForSeconds(waitTime);
	}
	if(dh5 == true && keepGoing == true)
	{
		//dealer 5th Card
		var dealer5 : GameObject = DealCard();
		dealer5.transform.position = dpos5.transform.position;//pos1 dealer card
   		dealer5.name = "dealer5";//give it a name to find it and get the integer value 
    	dealer5Card = GameObject.Find("dealer5");//find the "first" card
    	var dCard5 = dealer5Card.GetComponent(WhatCard).cardNumber;
    	if(dCard5 == 11)
		{
			dealerAces += 1;
			dealersHand += 11;	
		}
		else
		{
			dealersHand += dCard5;	
		}
		
		CheckDealerSum();
    	
		dh6 = true;
		yield WaitForSeconds(waitTime);
	}
	if(dh6 == true && keepGoing == true)
	{
		//dealer 6th Card
		var dealer6 : GameObject = DealCard();
		dealer6.transform.position = dpos6.transform.position;//pos1 dealer card
   		dealer6.name = "dealer6";//give it a name to find it and get the integer value 
    	dealer6Card = GameObject.Find("dealer6");//find the "first" card
    	var dCard6 = dealer6Card.GetComponent(WhatCard).cardNumber;
    	if(dCard6 == 11)
		{
			dealerAces += 1;
			dealersHand += 11;	
		}
		else
		{
			dealersHand += dCard6;	
		}
		
		CheckDealerSum();
    	
		dh7 = true;
		yield WaitForSeconds(waitTime);
	}
	if(dh7 == true && keepGoing == true)
	{
		//dealer 7th Card
		var dealer7 : GameObject = DealCard();
		dealer7.transform.position = dpos7.transform.position;//pos1 dealer card
   		dealer7.name = "dealer7";//give it a name to find it and get the integer value 
    	dealer7Card = GameObject.Find("dealer7");//find the "first" card
    	var dCard7 = dealer7Card.GetComponent(WhatCard).cardNumber;
    	if(dCard7 == 11)
		{
			dealerAces += 1;
			dealersHand += 11;	
		}
		else
		{
			dealersHand += dCard7;	
		}
		CheckDealerSum();
    
	}
	GetTotals();	
}
function GetTotals()
{
	if(dealersHand == playersHand)
	{
		Tie();//push	
	}
	if(dealersHand > playersHand && dealersHand < 21)
	{
		Lose();	
	}
	if(dealersHand < playersHand && dealersHand < 21)
	{
		Win();	
	}
}
function CheckSum()
{
	while(playersHand > 21 && howManyAces > 0)//1
	{
		howManyAces--;//0
		playersHand -= 10;//11 - 10 = 1
	}
	playerTextMesh.text = prefix + playersHand;
	
	if(playersHand > 21)
	{
		Bust();	
	}
	if(playersHand == 21)
	{
		PlayerBlackJack();	
	}	
}
function CheckDealerSum()
{
	while(dealersHand > 21 && dealerAces > 0)
	{
		dealerAces--;
		dealersHand -= 10;
	}
	dealerTextMesh.text = dPrefix + dealersHand;
	
	if(dealersHand > 21)
	{
		Win();	
	}
	if(dealersHand == 21)
	{
		DealerBlackJack();	
	}
}
function Bust()
{
	print("YOU BUSTED");
	bustedText.active = true;
	bustedCube.active = true;
	
	hideHitButtons = true;
	showStandButton = false;
	showDoubleDown = false;	
	madeBet = false;
	placeBet = 0;
	yourBet.gameObject.active = false;
	showRebetButton = true;
}
function Win()
{
	print("WINNER");
	youWinText.active = true;
	youWinCube.active = true;
	
	var winner = placeBet * 2;
	cash += winner;
	placeBet = 0;
	iTweenEvent.GetEvent(bounceCashText, "cash").Play();
	madeBet = false;
	yourBet.gameObject.active = false;
	showRebetButton = true;
}
function Lose()
{
	print("You Losttt!!!");
	youLoseText.active = true;
	youLoseCube.active = true;
	placeBet = 0;//already taken from bank account to goto placebet	
	madeBet = false;
	yourBet.gameObject.active = false;
	showRebetButton = true;
}
function Tie()//push
{
	print("It's a Push or Tie");
	pushText.active = true;
	pushCube.active = true;
	madeBet = false;
	yourBet.gameObject.active = false;
	placeBet = 0;//no cash exchange
	cash += lastBet;//put the money back into bank
	showRebetButton = true;
}
function DealerBlackJack()
{
	print("BLACKJACK");
	dBJText.active = true;
	dBJCube.active = true;
	placeBet = 0;
	madeBet = false;
	yourBet.gameObject.active = false;
	showRebetButton = true;
}
function PlayerBlackJack()
{
	print("YOU THE MAN 21 Blackjack");
	pBJText.active = true;
	pBJCube.active = true;
	
	hideHitButtons = true;
	showStandButton = false;
	showDoubleDown = false;	
	
	//blackjack pays 3 to 2 so 3 div by 2 = 1.5 //plus the cash on the table = 1 total is 2.5
	var jackpot = placeBet * 2.5;
	cash += jackpot;
	iTweenEvent.GetEvent(bounceCashText, "cash").Play();
	placeBet = 0;
	madeBet = false;
	yourBet.gameObject.active = false;
	showRebetButton = true;
}

function DealCard() : GameObject
{
	if(cards.Count == 0)
    {
    	ResetDeck();
    }
	
	var card : int = Random.Range(0, cards.Count);
	var go : GameObject = GameObject.Instantiate(cards[card]) as GameObject;
	cards.RemoveAt(card);
    return go;	
}
function ResetDeck()
{
	cards.Clear();
	cards.AddRange(deck);
}
function DestroyAll()
{
	if(playerFirstCard != null)
	{
		Destroy(playerFirstCard);	
	}
	if(playerSecondCard != null)
	{
		Destroy(playerSecondCard);	
	}
	if(playersHitCard1 != null)
	{
		Destroy(playersHitCard1);	
	}
	if(playersHitCard2 != null)
	{
		Destroy(playersHitCard2);	
	}
	if(playersHitCard3 != null)
	{
		Destroy(playersHitCard3);	
	}
	if(playersHitCard4 != null)
	{
		Destroy(playersHitCard4);	
	}
	if(playersHitCard5 != null)
	{
		Destroy(playersHitCard5);	
	}
	if(dealerFirstCard != null)
	{
		Destroy(dealerFirstCard);	
	}
	if(dealer2Card != null)
	{
		Destroy(dealer2Card);	
	}
	if(dealer3Card != null)
	{
		Destroy(dealer3Card);	
	}
	if(dealer4Card != null)
	{
		Destroy(dealer4Card);	
	}
	if(dealer5Card != null)
	{
		Destroy(dealer5Card);	
	}
	if(dealer6Card != null)
	{
		Destroy(dealer6Card);	
	}
	if(dealer7Card != null)
	{
		Destroy(dealer7Card);	
	}
	if(blankCard != null)
	{
		Destroy(blankCard);	
	}
	if(doubleDown != null)
	{
		Destroy(doubleDown);	
	}
	
	
	//deactivate points and backgrounds
	playerTextMesh.gameObject.active = false;
	textBackingPlayer.gameObject.active = false;
	dealerTextMesh.gameObject.active = false;//////////////////
	textBackingDealer.gameObject.active = false;
	
	youWinText.active = false;
	youWinCube.active = false;
	youLoseText.active = false;
	youLoseCube.active = false;
	bustedText.active = false;
	bustedCube.active = false;
	pushText.active = false;
	pushCube.active = false;
	dBJText.active = false;
	dBJCube.active = false;
	pBJText.active = false;
	pBJCube.active = false;
	
	playerHit2 = false;
	playerHit3 = false;
	playerHit4 = false;
	playerHit5 = false;
	
	dh3 = false;
	dh4 = false;
	dh5 = false;
	dh6 = false;
	dh7 = false;
	
	//starting over reset aces
	howManyAces = 0;
	dealerAces = 0;
		
	//reset hand score
	playersHand = 0;
	dealersHand = 0;	
}