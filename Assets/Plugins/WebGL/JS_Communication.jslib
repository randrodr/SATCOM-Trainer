var JS_Communication =
{
    SendDialogue:function(nodeID, showNextButton, fileName)
    {
        var jsText = Pointer_stringify(nodeID);
        var jsFileName = Pointer_stringify(fileName);
        SetDialogue(jsText, showNextButton, jsFileName);
    },

    SendTaskTitle:function(text)
    {
        var jsText = Pointer_stringify(text);
        setTaskTitle(jsText);
    },

    SendAudio:function(fileName)
    {
        var jsText = Pointer_stringify(fileName);
        setAudio(jsText);
    },

    SendHint:function(text)
    {
        var jsText = Pointer_stringify(text);
        SetHint(jsText);
    },

    SendFeedbackToPage:function(text)
    {
        var jsText = Pointer_stringify(text);
        SetFeedback(jsText);
    },

    ShowFeedback:function()
    {
        ShowFeedbackText();
    },

    SendPositionAndSize:function(triggerName, x, y, width, height)
    {
        var jsTriggerName = Pointer_stringify(triggerName);
        SetButton(jsTriggerName, x, y, width, height);
    },

    SendHover:function(text)
    {
        var hoverText = Pointer_stringify(text);
        SetHoverText(hoverText);
    },

    SendReady:function()
    {
        readyToStart();
    },

    SendTotalProgress:function(total)
    {
        setTotalProgress(total);
    },

    SendCorrect:function(triggerID)
    {
        var jsText = Pointer_stringify(triggerID);
        correctClickEvent(jsText);
    },

    ResetCameraButton:function()
    {
        showCameraResetButton();
    },

    SendNode:function(nodeID)
    {
        var jsText = Pointer_stringify(nodeID);
        sendNodeID(jsText);
    },

    EndReached:function()
    {
        endReached();
    },

    DebugAlert:function(text)
    {
        var jsText = Pointer_stringify(text);
        debugAlert(jsText);
    },

    AddToMaxScore:function(nodeID)
    {
        var jsText = Pointer_stringify(nodeID);
        addToMaxScore(jsText);
    }
}

mergeInto(LibraryManager.library, JS_Communication);