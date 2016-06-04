module Youtubedl

open System
open Common
open Process

let launchYoutubedl = launchProcess youtubedlPath

let getFileName (url:Uri) =
    let (Result (code, lines)) = launchYoutubedl ["--get-filename"; url.ToString()]
    match code with
    | ExitCode 0 -> Some (lines |> String.concat "")
    | _ -> None

let download parameters (url:Uri) = 
    let (Result (code, _)) = launchYoutubedl <| List.append parameters [url.ToString()] 
    match code with
    | ExitCode 0 -> true
    | _ -> false

let downloadMp3 url = download ["-x"; "--audio-format=mp3"] url
//let downloadMp4 url = download ["--recode-video=mp4"] url
let downloadMp4 url = download ["--format=mp4"] url
