module Upload

open System
open System.IO
open Dropbox.Api
open Dropbox.Api.Files

let private chunkSize = 256 * 1024

let uploadToDropbox token filePath (remoteFilePath:string) = 
    async { 
        use client = new DropboxClient(token)
        use fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read)
        let fileSize = fileStream.Length
        let chunksCount = float fileSize / float chunkSize |> Math.Ceiling |> int
        let commitInfo = remoteFilePath |> CommitInfo
        let buffer = Array.zeroCreate<byte> chunkSize
        let mutable sessionId = ""

        for chunkNumber in 1 .. chunksCount do
            let readCount = fileStream.Read(buffer, 0, chunkSize)
            use readDataStream = new MemoryStream(buffer, 0, readCount)
            match chunkNumber with
            | 1 ->
                let! session = client.Files.UploadSessionStartAsync(body = readDataStream) |> Async.AwaitTask
                sessionId <- session.SessionId
            | _ ->
                let cursor = UploadSessionCursor(sessionId, uint64 (chunkNumber - 1) * uint64 chunkSize)
                match chunkNumber with
                | i when i = chunksCount -> 
                    do! 
                        client.Files.UploadSessionFinishAsync(cursor, commitInfo, readDataStream) 
                        |> Async.AwaitTask 
                        |> Async.Ignore
                | _ -> 
                    do!
                        client.Files.UploadSessionAppendV2Async(cursor, body = readDataStream) 
                        |> Async.AwaitTask 
                        |> Async.Ignore
    }
