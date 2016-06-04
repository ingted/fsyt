namespace Common

open System.Reflection
open System.IO
open System

[<AutoOpen>]
module Operations = 
    let (</>) fst snd = Path.Combine(fst, snd)

[<AutoOpen>]
module ActivePatterns = 
    let (|Url|_|) s = 
        try 
            Some (Uri(s)) 
        with _ -> 
            None

[<AutoOpen>]
module Paths =
    let private appDir = 
        Assembly.GetExecutingAssembly().Location 
        |> Path.GetDirectoryName 
        |> Path.GetFullPath

    let workingDir = Directory.GetCurrentDirectory() |> Path.GetFullPath
    let youtubedlPath = appDir </> "tools" </> "youtube-dl.exe" 

[<AutoOpen>]
module Utils =
    let replaceExtension extension fileName =
        [fileName |> Path.GetFileNameWithoutExtension; extension]
        |> String.concat "."

    let isNullOrWhitespace (s:string) =
        System.String.IsNullOrWhiteSpace s


