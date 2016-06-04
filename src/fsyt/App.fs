module App

open System
open System.IO
open Common
open Upload
open Youtubedl

type MediaType =
    | Audio
    | Video

type Parameters =
    { DropboxToken: string
      MediaType: MediaType
      Url: Uri }

let (|NonEmptyToken|_|) t =
    match isNullOrWhitespace t with
    | true -> None
    | false -> Some t

let (|MediaType|_|) (mt:string) =
    match mt.ToUpperInvariant() with
    | "-V" -> Some Video
    | "-A" -> Some Audio
    | _ -> None

let parseParameters (args:string[]) =
    match args with
    | [|NonEmptyToken token; MediaType mediaType; Url url|] -> Some (token, mediaType, url)
    | [|NonEmptyToken token; Url url|] -> Some (token, MediaType.Audio, url)
    | _ -> None

[<EntryPoint>]
let main args = 
    match parseParameters args with
    | Some (token, mediaType, url) ->
        printfn "Getting file name for URL: %A" <| url.ToString()
        
        let extension, download = 
            match mediaType with
            | Audio -> "mp3", downloadMp3
            | Video -> "mp4", downloadMp4

        let filename = 
            getFileName url 
            |> Option.map (replaceExtension extension)

        match filename with
        | Some fileName ->
            fileName |> printfn "Filename is: %A"

            printfn "Starting download from %A" url

            match download url with
            | true -> 
                printfn "File has been downloaded..."
                printfn "Starting upload to DropBox..."

                let filePath = workingDir </> fileName
                let remoteFilePath = "/Media/" + fileName;

                uploadToDropbox token filePath remoteFilePath 
                |> Async.RunSynchronously
                |> ignore

                printfn "Done with upload..."
            | false ->
                printfn "Failed to download file..."
        | None -> 
            printfn "No file name retreived...."
        0
    | None -> 
        printfn "Provided parameters are incorrect..."
        -1
