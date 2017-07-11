using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


public class MessageBox
{
    public static JavaScriptResult Show(string message, MessageType type = MessageType.Alert, bool modal = false, MessageAlignment layout = MessageAlignment.BottomRight, bool dismissQueue = false, int timeout = 4000)
    {
        string txt = "$.noty.closeAll(); noty({ text: \"" + message + "\", type: \"" + type.ToString().ToLower() + "\", layout: \"" + layout.ToString().ToLowerFirst() + "\", dismissQueue: " + dismissQueue.ToString().ToLower() + ", modal: " + modal.ToString().ToLower() + ", timeout: " + timeout.ToString().ToLower() + " });";
        return new JavaScriptResult() { Script = txt };
    }
}

public enum MessageType
{
    Success,
    Error,
    Information,
    Warning,
    Alert,
    Notification
}

public enum MessageAlignment
{
    Bottom,
    BottomCenter,
    BottomLeft,
    BottomRight,
    Center,
    CenterLeft,
    CenterRight,
    Inline,
    Top,
    TopCenter,
    TopLeft,
    TopRight
}