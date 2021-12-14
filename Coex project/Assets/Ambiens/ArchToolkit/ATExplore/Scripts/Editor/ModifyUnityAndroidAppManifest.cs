//https://stackoverflow.com/questions/43293173/use-custom-manifest-file-and-permission-in-unity?noredirect=1&lq=1

using System.IO;
using System.Text;
using System.Xml;
using UnityEditor.Android;

public class ModifyUnityAndroidAppManifest : IPostGenerateGradleAndroidProject
{

    public void OnPostGenerateGradleAndroidProject(string basePath)
    {
#if UNITY_ANDROID
        bool isQuest = UnityEditor.EditorPrefs.GetBool("IsOculusQuest", false);
        bool isPicoNeo = UnityEditor.EditorPrefs.GetBool("IsPicoNeo", false);

        if (isQuest)
        {
            var androidManifest = new AndroidManifest(GetManifestPath(basePath));

            androidManifest.SetHeadTracking();
#if AVR_OCULUS_QUEST_HAND_TRACKING
           // androidManifest.SetHandTracking();
#endif
            androidManifest.Save();
        }
        else if(isPicoNeo)
        {

        }

#endif
    }

    public int callbackOrder { get { return 1; } }

    private string _manifestFilePath;

    private string GetManifestPath(string basePath)
    {
        if (string.IsNullOrEmpty(_manifestFilePath))
        {
            var pathBuilder = new StringBuilder(basePath);
            pathBuilder.Append(Path.DirectorySeparatorChar).Append("src");
            pathBuilder.Append(Path.DirectorySeparatorChar).Append("main");
            pathBuilder.Append(Path.DirectorySeparatorChar).Append("AndroidManifest.xml");
            _manifestFilePath = pathBuilder.ToString();
        }
        return _manifestFilePath;
    }
}


internal class AndroidXmlDocument : XmlDocument
{
    private string m_Path;
    protected XmlNamespaceManager nsMgr;
    public readonly string AndroidXmlNamespace = "http://schemas.android.com/apk/res/android";
    public AndroidXmlDocument(string path)
    {
        m_Path = path;
        using (var reader = new XmlTextReader(m_Path))
        {
            reader.Read();
            Load(reader);
        }
        nsMgr = new XmlNamespaceManager(NameTable);
        nsMgr.AddNamespace("android", AndroidXmlNamespace);
    }

    public string Save()
    {
        return SaveAs(m_Path);
    }

    public string SaveAs(string path)
    {
        using (var writer = new XmlTextWriter(path, new UTF8Encoding(false)))
        {
            writer.Formatting = Formatting.Indented;
            Save(writer);
        }
        return path;
    }
}


internal class AndroidManifest : AndroidXmlDocument
{
    private readonly XmlElement ApplicationElement;

    public AndroidManifest(string path) : base(path)
    {
        ApplicationElement = SelectSingleNode("/manifest/application") as XmlElement;
    }

    private XmlAttribute CreateAndroidAttribute(string key, string value)
    {
        XmlAttribute attr = CreateAttribute("android", key, AndroidXmlNamespace);
        attr.Value = value;
        return attr;
    }
   

    internal XmlNode GetActivityWithLaunchIntent()
    {
        return SelectSingleNode("/manifest/application/activity[intent-filter/action/@android:name='android.intent.action.MAIN' and " +
                "intent-filter/category/@android:name='android.intent.category.LAUNCHER']", nsMgr);
    }

    internal void SetApplicationTheme(string appTheme)
    {
        ApplicationElement.Attributes.Append(CreateAndroidAttribute("theme", appTheme));
    }

    internal void SetStartingActivityName(string activityName)
    {
        GetActivityWithLaunchIntent().Attributes.Append(CreateAndroidAttribute("name", activityName));
    }


    internal void SetHardwareAcceleration()
    {
        GetActivityWithLaunchIntent().Attributes.Append(CreateAndroidAttribute("hardwareAccelerated", "true"));
    }

    internal void SetMicrophonePermission()
    {
        var manifest = SelectSingleNode("/manifest");
        XmlElement child = CreateElement("uses-permission");
        manifest.AppendChild(child);
        XmlAttribute newAttribute = CreateAndroidAttribute("name", "android.permission.RECORD_AUDIO");
        child.Attributes.Append(newAttribute);
    }
    internal void SetHeadTracking()
    {
        var manifest = SelectSingleNode("/manifest");
        XmlElement child = CreateElement("uses-feature");
        manifest.AppendChild(child);
        
        child.Attributes.Append(CreateAndroidAttribute("name", "android.hardware.vr.headtracking"));
        child.Attributes.Append(CreateAndroidAttribute("version", "1"));
        child.Attributes.Append(CreateAndroidAttribute("required", "true"));
    }
    internal void SetHeadTrackingPico()
    {
        var manifest = SelectSingleNode("/manifest");
        XmlElement child = CreateElement("uses-feature");
        manifest.AppendChild(child);
        /*
         <meta-data android:name=" pvr.app.type " android:value="vr"/>
        <meta-data android:name=" pvr.display.orientation " android:value="180"/>
         */
        child.Attributes.Append(CreateAndroidAttribute("name", "android.hardware.vr.headtracking"));
        child.Attributes.Append(CreateAndroidAttribute("version", "1"));
        child.Attributes.Append(CreateAndroidAttribute("required", "true"));
    }
    /*
     <uses-permission android:name="oculus.permission.handtracking" />
<uses-feature android:name="oculus.software.handtracking"
android:required="false" />
         
         */
    internal void SetHandTracking()
    {
        var manifest = SelectSingleNode("/manifest");
        XmlElement feature = CreateElement("uses-feature");
        manifest.AppendChild(feature);

        feature.Attributes.Append(CreateAndroidAttribute("name", "oculus.software.handtracking"));
        feature.Attributes.Append(CreateAndroidAttribute("version", "1"));
        feature.Attributes.Append(CreateAndroidAttribute("required", "false"));

        XmlElement permission = CreateElement("uses-permission");
        manifest.AppendChild(permission);

        permission.Attributes.Append(CreateAndroidAttribute("name", "oculus.permission.handtracking"));

    }
}