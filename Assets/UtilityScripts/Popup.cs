using UnityEngine;
 
public class Popup {
	static int popupListHash = "PopupList".GetHashCode();
 
	public static bool List (Rect position, ref bool showList, ref int listEntry, GUIContent buttonContent, GUIContent[] listContent,
							 GUIStyle listStyle) {
		return List(position, ref showList, ref listEntry, buttonContent, listContent, "button", "box", listStyle);
	}
 
	public static bool List (Rect position, ref bool showList, ref int listEntry, GUIContent buttonContent, GUIContent[] listContent,
							 GUIStyle buttonStyle, GUIStyle boxStyle, GUIStyle listStyle) {
		int controlID = GUIUtility.GetControlID(popupListHash, FocusType.Passive);
		bool done = false;
		switch (Event.current.GetTypeForControl(controlID)) {
			case EventType.mouseDown:
				if (position.Contains(Event.current.mousePosition)) {
					GUIUtility.hotControl = controlID;
					showList = true;
				}
				break;
			case EventType.mouseUp:
				if (showList) {
					done = true;
				}
				break;
		}
 
		GUI.Label(position, buttonContent, buttonStyle);
		if (showList) {
			if (listContent.Length > 0) {
				Rect listRect = new Rect(position.x, position.y, position.width, listStyle.CalcHeight(listContent[0], 1.0f)*listContent.Length);
				GUI.Box(listRect, "", boxStyle);
				listEntry = GUI.SelectionGrid(listRect, listEntry, listContent, 1, listStyle);
			} else {
				Rect listRect = new Rect(position.x, position.y, position.width, listStyle.CalcHeight(new GUIContent("None"), 1.0f));
				GUI.Box(listRect, "", boxStyle);
			}
		}
		if (done) {
			showList = false;
		}
		return done;
	}
}