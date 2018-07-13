#pragma strict

// The Read me File Has The Link to the Videos

var barDisplay : float = 0;
var pos : Vector2 = new Vector2(20,40);
var size : Vector2 = new Vector2(60,20);
var progressBarEmpty : Texture2D;
var progressBarFull : Texture2D;
var howFast : float = 0.05;

var star : GameObject;
var delay1 : float = 1;
var delay2 : float = 0;

var bonusText : GameObject;

var levelNumber : TextMesh;
var levelName : TextMesh;
var level : int = 1;
var dealScript : Deal;//access the deal script then cash var to give $100 bonus

var pokerSkin : GUISkin;

//save to player prefs, so next time you play it continues where it was level (35)

function Update()
{
    // for this example, the bar display is linked to the current time,
    // however you would set this value based on your desired display
    // eg, the loading progress, the player's health, or whatever.
    barDisplay += Time.deltaTime * howFast;
        
    if (barDisplay >= 1.0)
    {
   		barDisplay = 0.0;
   		DisplayBonus();
   	}
   	
   	levelNumber.text = level.ToString();
   	
   	if(level == 1)
   	{
   		levelName.text = "Beginner    1/50";	
   	}
   	if(level == 2)
   	{
   		levelName.text = "Hit Man    50/100";	
   	}
   	if(level == 3)
   	{
   		levelName.text = "Marksman    100/500";	
   	}
   	if(level == 4)
   	{
   		levelName.text = "Money Maker    500/1000";	
   	}
   	if(level == 5)
   	{
   		levelName.text = "Professional    1000/5000";	
   	}
   	
}
function OnGUI()
{
	GUI.skin = pokerSkin;//Set the Chip Skins by Name
	
	if (GUI.Button(Rect(20,455,115,115),"", "post" ))
	{
		//post to wall pop-up code
		//print("POSTING TO WALL");
		print("GETTING MORE CHIPS");	
	}
	if (GUI.Button(Rect(20,340,115,115),"", "more" ))
	{
		//Get More Chips pop-up code
		//print("GETTING MORE CHIPS");
		print("POSTING TO WALL");	
	}
	
	
	// draw the background:
    GUI.BeginGroup (new Rect (pos.x, pos.y, size.x, size.y));
        GUI.Box (Rect (0,0, size.x, size.y),progressBarEmpty);

        // draw the filled-in part:
        GUI.BeginGroup (new Rect (0, 0, size.x * barDisplay, size.y));
            GUI.Box (Rect (0,0, size.x, size.y),progressBarFull);
        GUI.EndGroup ();

    GUI.EndGroup ();
} 
function DisplayBonus()
{
	level += 1;
	bonusText.gameObject.active = true;
	star.gameObject.active = true;
	dealScript.cash += 100;//$100 bonus
	yield WaitForSeconds(delay1);
	iTweenEvent.GetEvent(star, "bonus").Play();
	yield WaitForSeconds(delay2);
	star.gameObject.active = false;
	bonusText.gameObject.active = false;
}