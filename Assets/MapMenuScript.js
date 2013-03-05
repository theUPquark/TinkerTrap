var newSkin : GUISkin;
var mapTexture : Texture2D;

function theMapMenu() {
    //layout start
    GUI.BeginGroup(Rect(Screen.width / 2 - 200, 50, 400, 300));
   
    //boxes
    GUI.Box(Rect(0, 0, 400, 300), "");
    GUI.Box(Rect(96, 20, 200, 200), "");
    GUI.Box(Rect(96, 222, 200, 20), "Coastside Level");
   
    //map preview/icon
    GUI.Label(Rect(100, 20, 198, 198), mapTexture);
   
    //buttons
    if(GUI.Button(Rect(15, 250, 180, 40), "start level")) {
    Application.LoadLevel(1);
    }
    if(GUI.Button(Rect(205, 250, 180, 40), "go back")) {
    var script = GetComponent("MainMenuScript");
    script.enabled = true;
    var script2 = GetComponent("MapMenuScript");
    script2.enabled = false;
    }
   
    //layout end
    GUI.EndGroup();
}

function OnGUI () {
    //load GUI skin
    GUI.skin = newSkin;
   
    //execute theMapMenu function
    theMapMenu();
}