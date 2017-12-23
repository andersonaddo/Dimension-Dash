using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class emailMe : MonoBehaviour {

    public void email()
    {
        //Email address to send the mail to
        string email = "lumberjackapps72@gmail.com";

        //Subject of the mail
        string subject = MyEscapeURL("Dimension Dash: Feedback/Suggestions");

        //Body of the mail which consists of Device Model and its Operating System
        string body = MyEscapeURL("Hey! We're glad to hear from you. Kindly just enter your message here and we'll be sure to get back to you soon.\n\n\n\n" +
         "________" +
         "\n\nPlease Do Not Modify This\n\n" +
         "Model: " + SystemInfo.deviceModel + "\n\n" +
            "OS: " + SystemInfo.operatingSystem + "\n\n" +
         "________");

        //Open the Default Mail App
        Application.OpenURL("mailto:" + email + "?subject=" + subject + "&body=" + body);
    }

    string MyEscapeURL(string url)
    {
        return WWW.EscapeURL(url).Replace("+", "%20");
    }
}
