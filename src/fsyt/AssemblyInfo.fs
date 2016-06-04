namespace System
open System.Reflection

[<assembly: AssemblyTitleAttribute("fsyt")>]
[<assembly: AssemblyProductAttribute("fsyt")>]
[<assembly: AssemblyDescriptionAttribute("Delivers mp3 from YouTube to OneDrive")>]
[<assembly: AssemblyVersionAttribute("1.0")>]
[<assembly: AssemblyFileVersionAttribute("1.0")>]
do ()

module internal AssemblyVersionInformation =
    let [<Literal>] Version = "1.0"
    let [<Literal>] InformationalVersion = "1.0"
